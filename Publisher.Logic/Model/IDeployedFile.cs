

namespace Publisher.Logic.Model
{
    public interface IDeployedFile
    {
        string FileName { get; }
        bool FlushOld { get; }
        string HostedPath { get; }
        bool IsRollingBack { get; }
        DateTime LastWriteTimeUtc { get; }
        string RelativePath { get; }

        Task DownloadAsync(string toolLocalDirPath);
        bool IsUpdateNeeded(string toolLocalDirPath);
    }
}