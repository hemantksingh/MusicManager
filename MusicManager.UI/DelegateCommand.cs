using System;
using System.Windows.Input;

namespace MusicManager.UI
{
    public class DelegateCommand<T> : ICommand where T : class
    {
        private readonly Predicate<T> _canExecute;
        private readonly Action<T> _execute;

        /// <summary>
        /// Initializes an instance of the <see cref="DelegateCommand"/> class>
        /// </summary>
        /// <param name="execute">The action to execute.</param>
        /// <param name="canExecute">Represents the set of criteria to determine whether the command can be executed.</param>
        public DelegateCommand(Action<T> execute, Predicate<T> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        bool ICommand.CanExecute(object parameter)
        {
            bool canExecute = true;

            if (_canExecute != null)
            {
                canExecute = _canExecute((T) parameter);
            }
            return canExecute;
        }

        event EventHandler ICommand.CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        void ICommand.Execute(object parameter)
        {
            _execute((T) parameter);
        }
    }
}