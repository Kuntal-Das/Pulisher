using Newtonsoft.Json;

namespace Publisher.Logic
{
    public class DeploymentConfiguration : IEquatable<DeploymentConfiguration?>
    {
        public const string tempDownloadPath = "Update";

        private Dictionary<string, string> _publishPathBySite;
        private IDeploymentManager _deploymentMamanger;

        [JsonConstructor]
        public DeploymentConfiguration(string projectName, string mainExeName, Dictionary<string, string> publishPathBySite, string localToolDirFullPath = ".")
        {
            ProjectName = projectName;
            MainExeName = mainExeName;
            PublishPathBySite = publishPathBySite;
            LocalToolDirFullPath = localToolDirFullPath;
        }

        [JsonIgnore]
        public IDeploymentManager DeploymentManager
        {
            get
            {
                if (_deploymentMamanger is null || this != _deploymentMamanger.Configuration)
                {
                    _deploymentMamanger = new DeploymentManager()
                    {
                        Configuration = this
                    };
                }
                return _deploymentMamanger;
            }
        }
        public string ProjectName { get; set; }
        public string MainExeName { get; set; }
        public Dictionary<string, string> PublishPathBySite
        {
            get => _publishPathBySite;
            set
            {
                if (!string.IsNullOrEmpty(ProjectName))
                {
                    _publishPathBySite = new Dictionary<string, string>();

                    foreach (var site in value.Keys)
                    {
                        _publishPathBySite[site] = Path.Combine(value[site], ProjectName, "Publish");
                    }
                }
                else
                {
                    _publishPathBySite = value;
                }
            }
        }
        /// <summary>
        /// Required at the time of update<br/>
        /// Not Required at the time of publishing 
        /// </summary>
        public string LocalToolDirFullPath { get; set; }

        internal static DeploymentConfiguration? GetInstance(string json)
        {
            return JsonConvert.DeserializeObject<DeploymentConfiguration>(json);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as DeploymentConfiguration);
        }

        public bool Equals(DeploymentConfiguration? other)
        {
            return other is not null &&
                   EqualityComparer<Dictionary<string, string>>.Default.Equals(_publishPathBySite, other._publishPathBySite) &&
                   EqualityComparer<IDeploymentManager>.Default.Equals(_deploymentMamanger, other._deploymentMamanger) &&
                   EqualityComparer<IDeploymentManager>.Default.Equals(DeploymentManager, other.DeploymentManager) &&
                   ProjectName == other.ProjectName &&
                   MainExeName == other.MainExeName &&
                   EqualityComparer<Dictionary<string, string>>.Default.Equals(PublishPathBySite, other.PublishPathBySite) &&
                   LocalToolDirFullPath == other.LocalToolDirFullPath;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_publishPathBySite, _deploymentMamanger, DeploymentManager, ProjectName, MainExeName, PublishPathBySite, LocalToolDirFullPath);
        }

        internal string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public override string? ToString()
        {
            return ToJson();
        }

        public static bool operator ==(DeploymentConfiguration? left, DeploymentConfiguration? right)
        {
            return EqualityComparer<DeploymentConfiguration>.Default.Equals(left, right);
        }

        public static bool operator !=(DeploymentConfiguration? left, DeploymentConfiguration? right)
        {
            return !(left == right);
        }
    }
}