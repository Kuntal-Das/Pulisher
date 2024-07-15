using Publisher.Logic.Model;

namespace Publisher.Logic
{
    internal class DeploymentManager : IDeploymentManager
    {
        public Configuration Configuration { get; set; }

        public Task<List<IDeployedFile>> CheckForUpdateAsync()
        {
            throw new NotImplementedException();
        }

        public void CloseCurrentAndLaunchUpdated(List<IDeployedFile> notUpdatedFiles)
        {
            throw new NotImplementedException();
        }

        public void Deploy(string releaseDiirPath, string version = null, bool needToRollback = false, bool removeOldFile = false, IProgress<string> progressReport = null)
        {
            throw new NotImplementedException();
        }

        public Task DeployAsync(string releaseDiirPath, string version = null, bool needToRollback = false, bool removeOldFile = false, IProgress<string> progressReport = null)
        {
            throw new NotImplementedException();
        }

        public void Launch()
        {
            throw new NotImplementedException();
        }

        public Task<List<IDeployedFile>> UpdateAsync(List<IDeployedFile> deployedFiles, bool canFlush = false, IProgress<string> statusProgress = null, IProgress<int> percentageProgress = null)
        {
            throw new NotImplementedException();
        }
    }
}
