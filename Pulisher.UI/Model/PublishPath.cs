using Pulisher.UI.ViewModel;

namespace Pulisher.UI.Model
{
    internal class PublishPath : ViewModelBase, IEquatable<PublishPath?>
    {
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
        private string _publishFullPath;
        public string PublishFullPath
        {
            get => _publishFullPath;
            set
            {
                _publishFullPath = value;
                RaisePropertyChanged();
            }
        }
        private bool _isChecked;
        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                _isChecked = value;
                RaisePropertyChanged();
            }
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as PublishPath);
        }

        public bool Equals(PublishPath? other)
        {
            return other is not null &&
                   GroupName == other.GroupName &&
                   PublishFullPath == other.PublishFullPath &&
                   IsChecked == other.IsChecked;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(GroupName, PublishFullPath, IsChecked);
        }

        public static bool operator ==(PublishPath? left, PublishPath? right)
        {
            return EqualityComparer<PublishPath>.Default.Equals(left, right);
        }

        public static bool operator !=(PublishPath? left, PublishPath? right)
        {
            return !(left == right);
        }
    }
}
