
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
    }
}