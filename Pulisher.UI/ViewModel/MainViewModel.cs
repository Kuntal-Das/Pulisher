using Pulisher.UI.Command;
using System.Collections.ObjectModel;
using System.Windows.Input;

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
                //RaisePropertyChanged(nameof(SelectedDeploymentConfiguration));
                RaisePropertyChanged(nameof(SelectedProjectVm));
            }
        }
        private Dictionary<string, ProjectViewModel> _projectVmByProjectName;
        public ViewModelBase SelectedProjectVm
        {
            get
            {
                _projectVmByProjectName ??= new Dictionary<string, ProjectViewModel>();
                if (string.IsNullOrEmpty(SelectedProject))
                {
                    return new EmptyViewModel("Select or Add Project to view Deployments");
                }
                if (!_projectVmByProjectName.ContainsKey(SelectedProject))
                {
                    _projectVmByProjectName[SelectedProject] = new ProjectViewModel(SelectedProject);
                }

                return _projectVmByProjectName[SelectedProject];
            }
        }
        public async Task RefreshDataAsync()
        {
            await Task.Delay(300);
        }
        private ICommand _refreshCommand;
        public ICommand RefreshCommand
        {
            get
            {
                if (_refreshCommand is null)
                {
                    _refreshCommand = new RelayCommandAsync(ExecuteRefreshAsync, (obj) => true);
                }
                return _refreshCommand;
            }
        }
        private async Task ExecuteRefreshAsync(object obj)
        {
            await RefreshDataAsync();
        }

        private string _projectToAdd;

        public string ProjectToAdd
        {
            get => _projectToAdd;
            set
            {
                _projectToAdd = value;
                RaisePropertyChanged();
            }
        }


        private ICommand _addCommand;

        public ICommand AddCommand
        {
            get
            {
                if (_addCommand is null)
                {
                    _addCommand = new RelayCommand(ExecuteAdd, CanExecuteAdd);
                }
                return _addCommand;
            }
        }

        private bool CanExecuteAdd(object? obj)
        {
            if (!string.IsNullOrWhiteSpace(ProjectToAdd) && ProjectToAdd.Length > 3 && !Projects.Contains(ProjectToAdd.Trim()))
            {
                return true;
            }
            return false;
        }

        private void ExecuteAdd(object obj)
        {
            Projects.Add(ProjectToAdd.Trim());
            ProjectToAdd = string.Empty;
        }
    }
}
