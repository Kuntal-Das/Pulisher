using Publisher.Logic;
using System.Collections.ObjectModel;

namespace Pulisher.UI.ViewModel
{
    internal class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            Projects = new ObservableCollection<string>();
        }
        public ObservableCollection<string> Projects { get; set; }

        private string _selectedProject;
        public string SelectedProject
        {
            get { return _selectedProject; }
            set
            {
                _selectedProject = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(SelectedDeploymentConfiguration));
                RaisePropertyChanged(nameof(SelectedProjectVm));
            }
        }
        private Dictionary<string, DeploymentConfiguration> _deploymentConfigurationByProjectName;
        public DeploymentConfiguration SelectedDeploymentConfiguration
        {
            get
            {
                _deploymentConfigurationByProjectName ??= new Dictionary<string, DeploymentConfiguration>();

                if (!_deploymentConfigurationByProjectName.ContainsKey(SelectedProject))
                {
                    _deploymentConfigurationByProjectName[SelectedProject] = new DeploymentConfiguration(SelectedProject, $"{SelectedProject}.exe", new Dictionary<string, string>());
                }
                return _deploymentConfigurationByProjectName[SelectedProject];
            }
        }
        private Dictionary<string, ProjectViewModel> _projectVmByProjectName;
        public ProjectViewModel SelectedProjectVm
        {
            get
            {
                _projectVmByProjectName ??= new Dictionary<string, ProjectViewModel>();
                if (string.IsNullOrEmpty(SelectedProject))
                {
                    return new ProjectViewModel(new DeploymentConfiguration("Loading", "Loading", new Dictionary<string, string>()));
                }
                if (!_projectVmByProjectName.ContainsKey(SelectedProject))
                {
                    _projectVmByProjectName[SelectedProject] = new ProjectViewModel(SelectedDeploymentConfiguration);
                }

                return _projectVmByProjectName[SelectedProject];
            }
        }
        public async Task LoadDataAsync()
        {
            await Task.Delay(300);
            Projects.Add("ISAAC");
            Projects.Add("ISAAC_bot");
            Projects.Add("QC Auditor");

            SelectedProject = Projects[0];
        }
    }
}
