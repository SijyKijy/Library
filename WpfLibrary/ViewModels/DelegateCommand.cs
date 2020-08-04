using System;
using System.Windows.Input;

namespace WpfLibrary.ViewModels
{
    public class DelegateCommand : ICommand
    {
        private readonly Action<object> execute;
        private readonly Func<object, bool> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public DelegateCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object item = null)
        {
            return canExecute == null || canExecute(item);
        }

        public void Execute(object item = null)
        {
            execute(item);
        }
    }
}
