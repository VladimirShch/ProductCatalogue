using System;

namespace ProductCatalogue.WPF.Presentation.Common
{
    public class ParameterlessCommand : BaseCommand
    {
        private readonly Action action;

        public ParameterlessCommand(Action action, Func<bool>? canExecute = null) : base(canExecute)
        {
            this.action = action ?? throw new ArgumentException("Action cannot be null");
        }

        public override void Execute(object? parameter)
        {
            action.Invoke();
        }
    }
}
