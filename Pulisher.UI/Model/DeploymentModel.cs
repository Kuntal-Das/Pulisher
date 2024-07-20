namespace Pulisher.UI.Model
{
    class DeploymentModel
    {
        public DeploymentModel()
        {
            ID = Guid.NewGuid().ToString();
            ReleasePath = string.Empty;
            ProjectName = string.Empty;
            EntryPoint = string.Empty;
            Version = "1.0.0.0";
            Channel = "beta";
            PublishPathsWithGroups = new();
            PublishGroups = new();
            CreationTimeStamp = DateTime.UtcNow;
            LastEditedTimeStamp = DateTime.UtcNow;
            LastPublishTimeStamp = null;
        }
        public string ID { get; set; }
        public string ReleasePath { get; set; }
        public string ProjectName { get; set; }
        public string EntryPoint { get; set; }
        public string Version { get; set; }
        public string Channel { get; set; }
        public List<PublishPath> PublishPathsWithGroups { get; set; }
        public List<string> PublishGroups { get; set; }
        public string PublishGroupsStr => string.Join(",", PublishGroups);
        public bool IsRollbackSet { get; set; }
        public bool IsFlushOldSet { get; set; }
        public DateTime CreationTimeStamp { get; set; }
        public DateTime LastEditedTimeStamp { get; set; }
        public DateTime? LastPublishTimeStamp { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj is DeploymentModel dm && dm.ID == ID) return true;
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public static bool operator ==(DeploymentModel left, DeploymentModel right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(DeploymentModel left, DeploymentModel right)
        {
            return !left.Equals(right);
        }
    }
}
