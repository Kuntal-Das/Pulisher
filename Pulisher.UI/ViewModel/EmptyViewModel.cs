namespace Pulisher.UI.ViewModel
{
    class EmptyViewModel : ViewModelBase
    {
        private string _message;

        public EmptyViewModel(string message)
        {
            Message = message;
        }

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                RaisePropertyChanged();
            }
        }

    }
}
