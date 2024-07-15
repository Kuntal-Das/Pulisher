using System.Windows.Input;

namespace Pulisher.UI.Command
{
    class RelayCommandAsync : ICommand
    {
        private Func<object?, Task> executeAsync;
        private Func<object?, bool> canExecute;

        public RelayCommandAsync(Func<object?, Task> executeAsync, Func<object?, bool> canExecute)
        {
            this.executeAsync = executeAsync;
            this.canExecute = canExecute;
        }

        private bool _isExecuting;

        public bool IsExecuting
        {
            get => _isExecuting;
            set
            {
                _isExecuting = value;
                CommandManager.InvalidateRequerySuggested();
            }
        }


        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object? parameter)
        {
            return !IsExecuting && canExecute.Invoke(parameter);
        }

        public async void Execute(object? parameter)
        {
            IsExecuting = true;
            try
            {
                await executeAsync.Invoke(parameter);
            }
            finally
            {
                IsExecuting = false;
            }
        }
    }
}
