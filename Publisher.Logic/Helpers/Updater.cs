using Newtonsoft.Json;
using Publisher.Logic.Model;
using Publisher.Logic.Util;

namespace Publisher.Logic.Helpers
{
    internal class Updater
    {
        internal async Task<List<IDeployedFile>> GetFilesfromJsonAsync(string publishPath)
        {
            var taskUser = _GetFilesFromJsonAsync(Path.Combine(publishPath, Environment.UserName), AppVars.PublishJsonFileName);
            var taskGlobal = _GetFilesFromJsonAsync(publishPath, AppVars.PublishJsonFileName);
            await Task.WhenAll(taskUser, taskGlobal);
            if (taskUser.Result.Count > 0)
            {
                return taskUser.Result;
            }
            return taskGlobal.Result;
        }

        private async Task<List<IDeployedFile>> _GetFilesFromJsonAsync(string publishPath, string publishJsonFileName)
        {
            List<IDeployedFile> deployedFiles = new List<IDeployedFile>();

            try
            {
                var jsonFilePath = Path.Combine(publishPath, publishJsonFileName);
                if (!File.Exists(jsonFilePath)) return deployedFiles;

                using FileStream fs = new(jsonFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                using StreamReader sr = new StreamReader(fs);
                var json = await sr.ReadToEndAsync();
                var settings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All };
                var tempDf = JsonConvert.DeserializeObject<List<IDeployedFile>>(json, settings);
                if (tempDf is not null)
                    deployedFiles.AddRange(tempDf);
            }
            catch (Exception ex) { }

            return deployedFiles;
        }

        private bool isFlushed = false;
        internal void Flush(string localToolDirFullPath)
        {
            if (!isFlushed)
            {
                var projectLocalDirInfo = new DirectoryInfo(localToolDirFullPath);
                if (projectLocalDirInfo.Exists)
                    projectLocalDirInfo.Delete(true);
                isFlushed = true;
            }
        }
    }
}
