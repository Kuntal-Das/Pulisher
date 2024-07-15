using Publisher.Logic;
using Pulisher.UI.Command;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Pulisher.UI.ViewModel
{
    internal class ProjectViewModel : ViewModelBase
    {
        public ProjectViewModel(DeploymentConfiguration selectedDeploymentConfiguration)
        {
            _deploymentConfiguration = selectedDeploymentConfiguration;
            ProjectName = _deploymentConfiguration.ProjectName;
            Deployments = new ObservableCollection<DeploymentViewModel>();
            SelectedDeploymentIndx = 0;
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
        private int _selectedDeploymentIndx;

        public int SelectedDeploymentIndx
        {
            get { return _selectedDeploymentIndx; }
            set
            {
                _selectedDeploymentIndx = value;
                RaisePropertyChanged(nameof(SelectedDeploymentView));
            }
        }

        public DeploymentViewModel SelectedDeploymentView
        {
            get
            {
                if (SelectedDeploymentIndx <= Deployments.Count)
                    return new DeploymentViewModel(new DeploymentConfiguration(ProjectName, ProjectName, new Dictionary<string, string>()));
                return Deployments[SelectedDeploymentIndx];
            }
        }

        private ICommand _addDeployment;

        public ICommand AddDeployment
        {
            get
            {
                if (_addDeployment is null)
                {
                    _addDeployment = new RelayCommand(ExecuteAddDeployment, CanExecuteAddDeployment);
                }
                return _addDeployment;
            }
        }

        private void ExecuteAddDeployment(object? obj)
        {

        }

        private bool CanExecuteAddDeployment(object? arg)
        {
            throw new NotImplementedException();
        }
    }
}
