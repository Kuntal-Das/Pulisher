using Publisher.Logic.Model;
using System.Diagnostics;

namespace Publisher.Logic
{
    internal class DeploymentManager : IDeploymentManager
    {
        private Helpers.Publisher _p;

        public Configuration Configuration { get; set; }

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
                catch (Exception ex)
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

        public async Task DeployAsync(string releaseDiirPath, string version = null, bool needToRollback = false, bool removeOldFile = false, IProgress<string> progressReport = null)
        {
            await Task.Run(() => Deploy(releaseDiirPath, version, needToRollback, removeOldFile, progressReport));
        }

        public void Launch()
        {
            var mainExeFullPath = Path.Combine(Configuration.LocalToolDirFullPath, Configuration.MainExeName);
            if (File.Exists(mainExeFullPath))
                Process.Start(mainExeFullPath);
        }


        public Task<List<IDeployedFile>> CheckForUpdateAsync()
        {
            throw new NotImplementedException();
        }

        public void CloseCurrentAndLaunchUpdated(List<IDeployedFile> notUpdatedFiles)
        {
            throw new NotImplementedException();
        }

        public Task<List<IDeployedFile>> UpdateAsync(List<IDeployedFile> deployedFiles, bool canFlush = false, IProgress<string> statusProgress = null, IProgress<int> percentageProgress = null)
        {
            throw new NotImplementedException();
        }
    }
}
