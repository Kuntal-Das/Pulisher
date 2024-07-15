
using Newtonsoft.Json;
using Publisher.Logic.Model;
using Publisher.Logic.Util;
using System.Text;

namespace Publisher.Logic.Helpers
{
    internal class Publisher
    {
        private string GetPublishFilePath(string publishPath)
        {
            return Path.Combine(publishPath, AppVars.PublishJsonFileName);
        }

        internal string Publish(string publishPath, string releasePath, bool isRollback, bool flushOld = false, string version = null)
        {
            var releaseDirInfo = new DirectoryInfo(releasePath);
            if (releaseDirInfo!.Exists)
            {
                throw new DirectoryNotFoundException(releaseDirInfo.FullName);
            }

            var newPublishPath = Path.Combine(publishPath, version);
            var deployedFiles = CopyFileAndCreateCsvData(releaseDirInfo, newPublishPath, isRollback, flushOld);

            var settings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All };
            var json = JsonConvert.SerializeObject(deployedFiles, settings);

            WriteToFile(publishPath, json);

            var outPut = new StringBuilder();
            outPut.AppendLine("Published to:");
            outPut.AppendLine("\t" + newPublishPath);
            outPut.AppendLine("With csv data in:");
            outPut.AppendLine("\t" + GetPublishFilePath(newPublishPath));

            return outPut.ToString();
        }

        private void WriteToFile(string publishPath, string csvData)
        {
            using (var fs = new FileStream(GetPublishFilePath(publishPath), FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
            using (var sw = new StreamWriter(fs))
                sw.Write(csvData);
        }

        private List<DeployedFile> CopyFileAndCreateCsvData(DirectoryInfo sourceDirInfo, string destination, bool isRollback = false, bool flushOld = false, string relativeDest = ".")
        {
            List<DeployedFile> csvData = new();

            var files = sourceDirInfo.GetFiles();
            Directory.CreateDirectory(destination);

            foreach (var file in files)
            {
                var destinationPath = Path.Combine(destination, file.Name);
                file.CopyTo(destinationPath, true);
                csvData.Add(new DeployedFile(file.Name, destinationPath, relativeDest, file.LastWriteTimeUtc, isRollback, flushOld));
            }

            var dirInfo = sourceDirInfo.GetDirectories();
            foreach (var subDirInfo in dirInfo)
            {
                var subCsvData = CopyFileAndCreateCsvData(subDirInfo, Path.Combine(destination, subDirInfo.Name), isRollback, flushOld, Path.Combine(relativeDest, subDirInfo.Name));
                csvData.AddRange(subCsvData);
            }

            return csvData;
        }
    }
}
