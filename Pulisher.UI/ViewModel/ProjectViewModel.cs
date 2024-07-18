using Pulisher.UI.Command;
using Pulisher.UI.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Pulisher.UI.ViewModel
{
    internal class ProjectViewModel : ViewModelBase
    {
        public ProjectViewModel(string projectName)
        {
            ProjectName = projectName;
            Deployments = new ObservableCollection<DeploymentModel>();
        }

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

        public ObservableCollection<DeploymentModel> Deployments { get; set; }

        private int _selectedDeploymentIndex;
        public int SelectedDeploymentIndex
        {
            get { return _selectedDeploymentIndex; }
            set
            {
                _selectedDeploymentIndex = value;
                RaisePropertyChanged(nameof(SelectedDeploymentView));
            }
        }
        public ViewModelBase SelectedDeploymentView
        {
            get
            {
                if (SelectedDeploymentIndex < 0 || SelectedDeploymentIndex >= Deployments.Count)
                    return new EmptyViewModel("Select or Add Deployment to view/edit configuration and Publish/Rollback");
                return new DeploymentViewModel(Deployments[SelectedDeploymentIndex]);
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
            Deployments.Add(new DeploymentModel());
        }

        private bool CanExecuteAddDeployment(object? arg)
        {
            return true;
        }
    }
}
