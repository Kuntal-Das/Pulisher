using Publisher.Logic;
using System.Collections.ObjectModel;

namespace Pulisher.UI.ViewModel
{
    internal class ProjectViewModel : ViewModelBase
    {
        public ProjectViewModel(DeploymentConfiguration selectedDeploymentConfiguration)
        {
            _deploymentConfiguration = selectedDeploymentConfiguration;
            ProjectName = _deploymentConfiguration.ProjectName;
            Deployments = new ObservableCollection<DeploymentViewModel>();
        }

        private DeploymentConfiguration _deploymentConfiguration;
        private string _projName;

        public string ProjectName
        {
            get => _projName;
            set
            {
                _projName = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<DeploymentViewModel> Deployments { get; set; }
        private DeploymentViewModel _selectedDeploymentView;

        public DeploymentViewModel SelectedDeploymentView
        {
            get => _selectedDeploymentView;
            set
            {
                _selectedDeploymentView = value;
                RaisePropertyChanged();
            }
        }

    }
}
