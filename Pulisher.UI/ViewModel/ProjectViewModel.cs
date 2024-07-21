using Pulisher.UI.Command;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Pulisher.UI.ViewModel
{
    internal class ProjectViewModel : ViewModelBase
    {
        public ProjectViewModel(string projectName)
        {
            ProjectName = projectName;
            Deployments = new ObservableCollection<DeploymentViewModel>();
            //Deployments.CollectionChanged += Deployments_CollectionChanged;
        }

        //private void Deployments_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        //{
        //    if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove
        //        && _deploymentVms is not null)
        //    {
        //        if (e.NewItems is null)
        //        {
        //            _deploymentVms.Clear();
        //            return;
        //        }
        //        foreach (var id in _deploymentVms.Keys)
        //        {
        //            if (!e.NewItems.Contains(id))
        //            {
        //                _deploymentVms.Remove(id);
        //            }
        //        }
        //    }
        //}
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

        private int _selectedDeploymentIndex;
        public int SelectedDeploymentIndex
        {
            get { return _selectedDeploymentIndex; }
            set
            {
                _selectedDeploymentIndex = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(SelectedDeploymentView));
            }
        }

        //private Dictionary<string, DeploymentViewModel> _deploymentVms;
        public ViewModelBase SelectedDeploymentView
        {
            get
            {
                if (SelectedDeploymentIndex < 0 || SelectedDeploymentIndex >= Deployments.Count)
                    return new EmptyViewModel("Select or Add Deployment to view/edit configuration and Publish/Rollback");
                return Deployments[SelectedDeploymentIndex];
                //if (_deploymentVms is null)
                //{
                //    _deploymentVms = new Dictionary<string, DeploymentViewModel>();
                //}
                //if (!_deploymentVms.ContainsKey(Deployments[SelectedDeploymentIndex].ID))
                //{
                //    _deploymentVms[Deployments[SelectedDeploymentIndex].ID] = new DeploymentViewModel(Deployments[SelectedDeploymentIndex]);
                //}
                //return _deploymentVms[Deployments[SelectedDeploymentIndex].ID];
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
            var deploymentTobeAdded = new DeploymentViewModel(new() { ProjectName = ProjectName });
            Deployments.Add(deploymentTobeAdded);
            SelectedDeploymentIndex = Deployments.Count - 1;
        }

        private bool CanExecuteAddDeployment(object? arg)
        {
            return true;
        }
    }
}
