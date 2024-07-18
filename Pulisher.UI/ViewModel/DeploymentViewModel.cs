using Pulisher.UI.Command;
using Pulisher.UI.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Pulisher.UI.ViewModel
{
    internal class DeploymentViewModel : ViewModelBase
    {
        private DeploymentModel? _deploymentModel;
        public DeploymentViewModel(DeploymentModel? deploymentModel = null)
        {
            _deploymentModel = deploymentModel;
            PublishPathWithGroups = new ObservableCollection<PublishPath>();
        }

        private string _releasePath;
        public string ReleasePath
        {
            get => _releasePath;
            set
            {
                _releasePath = value;
                RaisePropertyChanged();
            }
        }

        private string _projectName;
        public string ProjectName
        {
            get => _projectName;
            set
            {
                _projectName = value;
                RaisePropertyChanged();
            }
        }

        private string _entryPoint;
        public string EntryPoint
        {
            get => _entryPoint;
            set
            {
                if (value is null)
                    value = "main.exe";
                if (!value.EndsWith(".exe"))
                    _entryPoint = value + ".exe";
                else
                    _entryPoint = value;
                RaisePropertyChanged();
            }
        }

        private string _version;
        public string Version
        {
            get => _version;
            set
            {
                _version = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<string> Channels { get; set; }

        private string _selectedChannel;
        public string SelectedChannel
        {
            get { return _selectedChannel; }
            set
            {
                _selectedChannel = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<PublishPath> PublishPathWithGroups { get; set; }
        public ObservableCollection<string> PublishGroups { get; set; }

        private bool _isRollBackSet;
        public bool IsRollbackSet
        {
            get => _isRollBackSet;
            set
            {
                _isRollBackSet = value;
                RaisePropertyChanged();
            }
        }

        private bool _isFlushOldSet;
        public bool IsFlushOldSet
        {
            get => _isFlushOldSet;
            set
            {
                _isFlushOldSet = value;
                RaisePropertyChanged();
            }
        }

        private DateTime _creationTimeStamp;
        public DateTime CreationTimeStamp
        {
            get => _creationTimeStamp;
            set
            {
                _creationTimeStamp = value;
                RaisePropertyChanged();
            }
        }

        private DateTime _lastEditedTimeStamp;
        public DateTime LastEditedTimeStamp
        {
            get => _lastEditedTimeStamp;
            set
            {
                _lastEditedTimeStamp = value;
                RaisePropertyChanged();
            }
        }

        private DateTime _lastPublishTimeStamp;
        public DateTime LastPublishTimeStamp
        {
            get => _lastPublishTimeStamp;
            set
            {
                _lastPublishTimeStamp = value;
                RaisePropertyChanged();
            }
        }


        private ICommand _browseCommand;
        public ICommand BrowseCommand
        {
            get
            {
                if (_browseCommand is null)
                {
                    _browseCommand = new RelayCommand(ExecuteBrowseCommand, CanExecuteBrowseCommand);
                }
                return _browseCommand;
            }
        }

        private bool CanExecuteBrowseCommand(object? arg)
        {
            return true;
        }

        private void ExecuteBrowseCommand(object? obj)
        {
        }

        private string _groupName;

        public string GroupName
        {
            get => _groupName;
            set
            {
                _groupName = value;
                RaisePropertyChanged();
            }
        }

    }
}
