using System;
using System.Windows.Input;

namespace TaskWPFExperiment.Presentation.Common
{
    public abstract class BaseCommand : ICommand 
    {
        private readonly Func<bool> canExecute;

        public BaseCommand(Func<bool>? canExecute = null)
        {
            this.canExecute = canExecute ?? (() => true);
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return canExecute();
        }

        public abstract void Execute(object? parameter);

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
