using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Pulisher.UI.ViewModel
{
    internal abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string propName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        protected void RaisePropertyChanged(params string[] propNames)
        {
            foreach (var prop in propNames)
            {
                RaisePropertyChanged(prop);
            }
        }
    }
}
