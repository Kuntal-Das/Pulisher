
using Publisher.Logic.Model;
using Publisher.Logic.Util;
using System.Text;

namespace Publisher.Logic.Helpers
{
    internal class Publisher
    {
        private string GetPublishFilePath(string publishPath, bool isRollback)
        {
            if (isRollback)
            {
                return Path.Combine(publishPath, AppVars.RollbackCsvFileName);
            }
            return Path.Combine(publishPath, AppVars.PublishCsvFileName);
        }

        internal string Publish(string publishPath, string releasePath, bool isRollback, bool flushOld = false, string version = null)
        {
            var releaseDirInfo = new DirectoryInfo(releasePath);
            if (releaseDirInfo!.Exists)
            {
                throw new DirectoryNotFoundException(releaseDirInfo.FullName);
            }

            var newPublishPath = Path.Combine(publishPath, version);
            var csvdata = CopyFileAndCreateCsvData(releaseDirInfo, newPublishPath);

            WriteToFile(publishPath, csvdata.ToString(), isRollback);

            var outPut = new StringBuilder();
            outPut.AppendLine("Published to:");
            outPut.AppendLine("\t" + newPublishPath);
            outPut.AppendLine("With csv data in:");
            outPut.AppendLine("\t" + GetPublishFilePath(newPublishPath, isRollback));

            var flushFileinfo = new FileInfo(Path.Combine(publishPath, "flush"));
            if (flushOld && !flushFileinfo.Exists)
            {
                while (!flushFileinfo.Exists)
                {
                    try { flushFileinfo.Create(); } catch { }
                }
                outPut.AppendLine("flush file created in");
                outPut.AppendLine("\t" + flushFileinfo.FullName);
            }
            else
            {
                while (flushFileinfo.Exists)
                {
                    try { flushFileinfo.Delete(); } catch { }
                }
            }
            return outPut.ToString();
        }

        private void WriteToFile(string publishPath, string csvData, bool isRollback)
        {
            using (var fs = new FileStream(GetPublishFilePath(publishPath, isRollback), FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
            using (var sw = new StreamWriter(fs))
                sw.Write(csvData);
        }

        private List<DeployedFile> CopyFileAndCreateCsvData(DirectoryInfo sourceDirInfo, string destination, string relativeDest = ".")
        {
            List<DeployedFile> csvData = new();

            var files = sourceDirInfo.GetFiles();
            Directory.CreateDirectory(destination);

            foreach (var file in files)
            {
                var destinationPath = Path.Combine(destination, file.Name);
                file.CopyTo(destinationPath, true);
                csvData.Add(new DeployedFile(file.Name, destinationPath, relativeDest, file.LastWriteTimeUtc, false, false));
            }

            var dirInfo = sourceDirInfo.GetDirectories();
            foreach (var subDirInfo in dirInfo)
            {
                var subCsvData = CopyFileAndCreateCsvData(subDirInfo, Path.Combine(destination, subDirInfo.Name), Path.Combine(relativeDest, subDirInfo.Name));
                csvData.AddRange(subCsvData);
            }

            return csvData;
        }
    }
}
