namespace Publisher.Logic.Model
{
    internal class DeployedFile : IDeployedFile
    {
        private static readonly int _bufferSize = 1024 * 1024;

        public DeployedFile(string fileName, string hostedPath, string relativePath, DateTime lastWriteTimeUtc, bool isRollingBack, bool flushOld)
        {
            FileName = fileName;
            HostedPath = hostedPath;
            RelativePath = relativePath;
            LastWriteTimeUtc = lastWriteTimeUtc;
            IsRollingBack = isRollingBack;
            FlushOld = flushOld;
        }

        public string FileName { get; }
        public string HostedPath { get; }
        public string RelativePath { get; }
        public DateTime LastWriteTimeUtc { get; }
        public bool IsRollingBack { get; }
        public bool FlushOld { get; internal set; }

        public async Task DownloadAsync(string toolLocalDirPath)
        {
            var localDir = new DirectoryInfo(RelativePath == "." ? toolLocalDirPath : Path.Combine(toolLocalDirPath, RelativePath));
            if (!localDir.Exists) localDir.Create();

            var localFilePath = Path.Combine(localDir.FullName, FileName);

            using (FileStream localFs = new FileStream(localFilePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            using (FileStream hostedFs = new FileStream(HostedPath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            {
                localFs.SetLength(hostedFs.Length);

                int byteRead = -1;
                byte[] bytes = new byte[_bufferSize];
                while ((byteRead = await hostedFs.ReadAsync(bytes, 0, bytes.Length)) > 0)
                {
                    await localFs.WriteAsync(bytes, 0, bytes.Length);
                }
            }
            File.SetLastWriteTimeUtc(localFilePath, LastWriteTimeUtc);
        }

        public bool IsUpdateNeeded(string toolLocalDirPath)
        {
            if (FlushOld) return true;
            var localDir = new DirectoryInfo(RelativePath == "." ? toolLocalDirPath : Path.Combine(toolLocalDirPath, RelativePath));
            var locaFile = new FileInfo(Path.Combine(localDir.FullName, FileName));
            if (!locaFile.Exists) return true;

            if (IsRollingBack || FlushOld)
            {
                if (locaFile.LastAccessTimeUtc == LastWriteTimeUtc) return false;
                return true;
            }
            if (locaFile.LastWriteTimeUtc < LastWriteTimeUtc) return true;
            return false;
        }
    }
}
