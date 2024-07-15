﻿using Publisher.Logic;
using System.Collections.ObjectModel;

namespace Pulisher.UI.ViewModel
{
    internal class ProjectViewModel : ViewModelBase
    {
        public ProjectViewModel(DeploymentConfiguration selectedDeploymentConfiguration)
        {
            _deploymentConfiguration = selectedDeploymentConfiguration;
            ProjectName = _deploymentConfiguration.ProjectName;
            Deployments = new ObservableCollection<DeploymentViewModel>()
            {
                new DeploymentViewModel(new DeploymentConfiguration(ProjectName, ProjectName, new Dictionary<string, string>()))
            };
            SelectedDeploymentIndx = 0;
        }

        private DeploymentConfiguration _deploymentConfiguration;
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
        private int _selectedDeploymentIndx;

        public int SelectedDeploymentIndx
        {
            get { return _selectedDeploymentIndx; }
            set
            {
                _selectedDeploymentIndx = value;
                RaisePropertyChanged(nameof(SelectedDeploymentView));
            }
        }

        public DeploymentViewModel SelectedDeploymentView
        {
            get
            {
                if (SelectedDeploymentIndx < 0)
                    return new DeploymentViewModel(new DeploymentConfiguration(ProjectName, ProjectName, new Dictionary<string, string>()));
                return Deployments[SelectedDeploymentIndx];
            }
        }
    }
}
