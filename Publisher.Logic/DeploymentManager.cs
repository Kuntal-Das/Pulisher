using Publisher.Logic.Helpers;
using Publisher.Logic.Model;
using System.Diagnostics;
using System.Text;

namespace Publisher.Logic
{
    internal class DeploymentManager : IDeploymentManager
    {
        internal DeploymentManager()
        {
            _p = new Helpers.Publisher();
            _u = new Updater();
        }

        private Helpers.Publisher _p;
        private Updater _u;

        public DeploymentConfiguration Configuration { get; set; }

        public void Deploy(string releaseDirPath, string version = "0.0.0.0", bool needToRollback = false, bool removeOldFile = false, IProgress<string>? progress = null)
        {
            _p ??= new Helpers.Publisher();
            if (string.IsNullOrEmpty(version))
            {
                version = GetVersion(releaseDirPath, Configuration.MainExeName);
            }

            foreach (var site in Configuration.PublishPathBySite.Keys)
            {
                progress?.Report($"Started deploying to: {site}");

                try
                {
                    _p.Publish(Configuration.PublishPathBySite[site], releaseDirPath, needToRollback, removeOldFile, version);
                }
                catch
                {
                    progress?.Report($"Deployment to {site} was unsuccessful");
                }
                progress?.Report($"Deployed to {site} successfully");
            }
        }

        private string GetVersion(string releaseDirPath, string mainExeName)
        {
            var mainExeFullPath = Path.Combine(releaseDirPath, mainExeName);
            string version = "0.0.0.0";
            if (File.Exists(mainExeFullPath))
            {
                var verInfo = FileVersionInfo.GetVersionInfo(mainExeFullPath);
                if (!string.IsNullOrEmpty(verInfo?.FileVersion))
                    version = verInfo.FileVersion;
            }
            return version;
        }

        public async Task DeployAsync(string releaseDiirPath, string version = "0.0.0.0", bool needToRollback = false, bool removeOldFile = false, IProgress<string>? progressReport = null)
        {
            await Task.Run(() => Deploy(releaseDiirPath, version, needToRollback, removeOldFile, progressReport));
        }

        public void Launch()
        {
            var mainExeFullPath = Path.Combine(Configuration.LocalToolDirFullPath, Configuration.MainExeName);
            if (File.Exists(mainExeFullPath))
                Process.Start(mainExeFullPath);
        }


        public async Task<List<IDeployedFile>> CheckForUpdateAsync()
        {
            _u ??= new Updater();

            foreach (var site in Configuration.PublishPathBySite.Keys)
            {
                try
                {
                    var deployedFiles = await _u.GetFilesfromJsonAsync(Configuration.PublishPathBySite[site]);

                    if (deployedFiles.Count > 0)
                    {
                        var filteredDeployedFiles = deployedFiles.FindAll(d => d.IsUpdateNeeded(Configuration.LocalToolDirFullPath));
                        return filteredDeployedFiles;
                    }
                }
                catch { }
            }
            return Enumerable.Empty<IDeployedFile>().ToList();
        }

        public async Task<List<IDeployedFile>> UpdateAsync(List<IDeployedFile> updateFiles, bool canFlush = false, IProgress<string>? statusProgress = null, IProgress<int>? percentageProgress = null)
        {
            _u ??= new Updater();
            if (updateFiles.Any(u => u.FlushOld))
            {
                if (canFlush)
                {
                    _u.Flush(Configuration.LocalToolDirFullPath);
                }
                else
                {
                    return updateFiles;
                }
            }
            var unableToUpdateFiles = new List<IDeployedFile>();
            for (var d = 0; d < updateFiles.Count; d++)
            {
                if (updateFiles[d].IsUpdateNeeded(Configuration.LocalToolDirFullPath))
                {
                    try
                    {
                        statusProgress?.Report($"Downloading: " + Path.Combine(updateFiles[d].RelativePath, updateFiles[d].FileName));
                        await updateFiles[d].DownloadAsync(Configuration.LocalToolDirFullPath);
                        percentageProgress?.Report((d * 100) / updateFiles.Count);
                    }
                    catch
                    {
                        await updateFiles[d].DownloadAsync(Path.Combine(Configuration.LocalToolDirFullPath, DeploymentConfiguration.tempDownloadPath));
                        unableToUpdateFiles.Add(updateFiles[d]);
                    }
                }
            }
            return unableToUpdateFiles;
        }

        public void CloseCurrentAndLaunchUpdated(List<IDeployedFile> notUpdatedFiles)
        {
            var sb = new StringBuilder($"TASKKILL /F /IM {Configuration.MainExeName}");

            var localDirInfo = new DirectoryInfo(Configuration.LocalToolDirFullPath);
            sb.Append($" $cd {localDirInfo.FullName}");

            if (notUpdatedFiles.Any(f => f.FlushOld))
            {
                var files = localDirInfo.GetFiles();
                foreach (var f in files)
                    sb.Append($" & del /F /Q {f.Name}");

                var dirs = localDirInfo.GetDirectories();
                foreach (var d in dirs)
                {
                    if (d.Name == DeploymentConfiguration.tempDownloadPath) continue;
                    sb.Append($" $ RMDIR /S /Q {d.Name}");
                }
            }
            sb.Append($" & XCOPY \"{Path.Combine(localDirInfo.FullName, DeploymentConfiguration.tempDownloadPath)}\" \"{localDirInfo.FullName}\" /H /I /C /K /E /R /Y");
            sb.Append($" & RMDIR /S /Q {DeploymentConfiguration.tempDownloadPath}");
            sb.Append($" & {Configuration.MainExeName}");
            sb.Append(" & EXIT");

            var pInfo = new ProcessStartInfo("cmd.exe", "/C " + sb.ToString())
            {
                WindowStyle = ProcessWindowStyle.Hidden
            };

            Process.Start(pInfo);
        }
    }
}