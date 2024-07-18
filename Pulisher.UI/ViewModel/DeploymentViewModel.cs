using Pulisher.UI.Model;
using System.Collections.ObjectModel;

namespace Pulisher.UI.ViewModel
{
    internal class DeploymentViewModel : ViewModelBase
    {
        public DeploymentViewModel(DeploymentModel? selectedDeployment = null)
        {
            MainExeName = selectedDeployment?.MainExeName;
            PublishPathWithSites = new ObservableCollection<PublishPath>();
        }

        private string _mainExeName;
        public string MainExeName
        {
            get => _mainExeName;
            set
            {
                if (value is null)
                    value = "main.exe";
                if (!value.EndsWith(".exe"))
                    _mainExeName = value + ".exe";
                else
                    _mainExeName = value;
                RaisePropertyChanged();
            }
        }
        private bool _isRollBack;
        public bool IsRollBack
        {
            get => _isRollBack;
            set
            {
                _isRollBack = value;
                RaisePropertyChanged();
            }
        }
        private bool _isFlushOld;
        public bool IsFlushOld
        {
            get => _isFlushOld;
            set
            {
                _isFlushOld = value;
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
        private DateTime _utcTimeStamp;
        public DateTime UtcTimeStamp
        {
            get => _utcTimeStamp;
            set
            {
                _utcTimeStamp = value;
                RaisePropertyChanged();
            }
        }
        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                RaisePropertyChanged();
            }
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
        public ObservableCollection<PublishPath> PublishPathWithSites { get; set; }

    }
}
