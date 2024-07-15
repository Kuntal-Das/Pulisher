using Newtonsoft.Json;

namespace Publisher.Logic
{
    public class Configuration : IEquatable<Configuration?>
    {
        public const string tempDownloadPath = "Update";

        private Dictionary<string, string> _publishPathBySite;
        private IDeploymentManager _deploymentMamanger;

        [JsonConstructor]
        public Configuration(string projectName, string mainExeName, Dictionary<string, string> publishPathBySite, string localToolDirFullPath = ".")
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
        public string LocalToolDirFullPath { get; set; }

        internal static Configuration? GetInstance(string json)
        {
            return JsonConvert.DeserializeObject<Configuration>(json);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Configuration);
        }

        public bool Equals(Configuration? other)
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

        public static bool operator ==(Configuration? left, Configuration? right)
        {
            return EqualityComparer<Configuration>.Default.Equals(left, right);
        }

        public static bool operator !=(Configuration? left, Configuration? right)
        {
            return !(left == right);
        }
    }
}