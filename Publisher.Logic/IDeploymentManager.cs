using Publisher.Logic.Model;

namespace Publisher.Logic
{
    public interface IDeploymentManager
    {
        public Configuration Configuration { get; internal set; }
        void Deploy(string releaseDiirPath, string version = null, bool needToRollback = false, bool removeOldFile = false, IProgress<string> progressReport = null);
        Task DeployAsync(string releaseDiirPath, string version = null, bool needToRollback = false, bool removeOldFile = false, IProgress<string> progressReport = null);

        Task<List<IDeployedFile>> CheckForUpdateAsync();
        Task<List<IDeployedFile>> UpdateAsync(List<IDeployedFile> deployedFiles, bool canFlush = false, IProgress<string> statusProgress = null, IProgress<int> percentageProgress = null);
        void CloseCurrentAndLaunchUpdated(List<IDeployedFile> notUpdatedFiles);
        void Launch();
    }
}
