using System.Collections.ObjectModel;

namespace Pulisher.UI.ViewModel
{
    internal class MainViewModel : ViewModelBase
    {
        private string _selectedProject;

        public string SelectedProject
        {
            get { return _selectedProject; }
            set
            {
                _selectedProject = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<string> Projects { get; set; }


    }
}
