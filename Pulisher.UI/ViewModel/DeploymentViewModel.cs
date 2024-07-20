using Pulisher.UI.Model;
using System.Collections.ObjectModel;

namespace Pulisher.UI.ViewModel
{
    internal class DeploymentViewModel : ViewModelBase
    {
        private DeploymentModel _deploymentModel;
        public DeploymentViewModel(DeploymentModel deploymentModel)
        {
            _deploymentModel = deploymentModel;
            Channels = new ObservableCollection<string> { "alpha", "beta", "stable" };
            ProjectName = _deploymentModel?.ProjectName ?? string.Empty;
            ReleasePath = _deploymentModel?.ReleasePath ?? string.Empty;
            EntryPoint = _deploymentModel?.EntryPoint ?? string.Empty;
            Version = _deploymentModel?.Version ?? string.Empty;
            Channel = _deploymentModel?.Channel ?? string.Empty;

            if (_deploymentModel?.PublishPathsWithGroups is null)
                PublishPathWithGroups = new ObservableCollection<PublishPath>();
            else
                PublishPathWithGroups = new ObservableCollection<PublishPath>(_deploymentModel.PublishPathsWithGroups);
            if (_deploymentModel?.PublishGroups is null)
                PublishGroups = new ObservableCollection<string>();
            else
                PublishGroups = new ObservableCollection<string>(_deploymentModel.PublishGroups);
            IsRollbackSet = _deploymentModel?.IsRollbackSet ?? false;
            IsFlushOldSet = _deploymentModel?.IsFlushOldSet ?? false;
            CreationTimeStamp = _deploymentModel?.CreationTimeStamp ?? DateTime.UtcNow;
            LastEditedTimeStamp = _deploymentModel?.LastEditedTimeStamp ?? DateTime.UtcNow;
            LastPublishTimeStamp = _deploymentModel?.LastPublishTimeStamp?.ToString("R") ?? "N/A";
        }

        private string _releasePath;
        public string ReleasePath
        {
            get => _releasePath;
            set
            {
                _releasePath = value;
                _deploymentModel.ReleasePath = _releasePath;
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
                _deploymentModel.ProjectName = _projectName;
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
                _deploymentModel.EntryPoint = _entryPoint;
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
                _deploymentModel.Version = _version;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<string> Channels { get; set; }

        private string _channel;
        public string Channel
        {
            get { return _channel; }
            set
            {
                _channel = value;
                _deploymentModel.Channel = _channel;
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
                _deploymentModel.IsRollbackSet = _isRollBackSet;
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
                _deploymentModel.IsFlushOldSet = _isFlushOldSet;
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
                _deploymentModel.CreationTimeStamp = _creationTimeStamp;
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
                _deploymentModel.LastEditedTimeStamp = _lastEditedTimeStamp;
                RaisePropertyChanged();
            }
        }

        private string _lastPublishTimeStamp;
        public string LastPublishTimeStamp
        {
            get => _lastPublishTimeStamp;
            set
            {
                _lastPublishTimeStamp = value;
                if (DateTime.TryParse(value, out var lpt))
                {
                    _deploymentModel.LastPublishTimeStamp = lpt;
                }
                RaisePropertyChanged();
            }
        }


        private string _groupNameInput;
        public string GroupNameInput
        {
            get => _groupNameInput;
            set
            {
                _groupNameInput = value;
                RaisePropertyChanged();
            }
        }

        private string _publishFullPathInput;

        public string PublishFullPathInput
        {
            get { return _publishFullPathInput; }
            set
            {
                _publishFullPathInput = value;
                RaisePropertyChanged();
            }
        }

    }
}
