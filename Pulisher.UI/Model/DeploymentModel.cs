namespace Pulisher.UI.Model
{
    class DeploymentModel
    {
        public string ID { get; set; }
        public string ReleasePath { get; set; }
        public string ProjectName { get; set; }
        public string EntryPoint { get; set; }
        public string Version { get; set; }
        public string Channel { get; set; }
        public List<PublishPath> PublishPathsWithGroups { get; set; }
        public List<string> PublishGroups { get; set; }
        public string PublishGroupsStr { get; set; }
        public bool IsRollbackSet { get; set; }
        public bool IsFlushOldSet { get; set; }
        public DateTime CreationTimeStamp { get; set; }
        public DateTime LastEditedTimeStamp { get; set; }
        public DateTime LastPublishTimeStamp { get; set; }
    }
}
