using System;

namespace ProductCatalogue.WPF.Presentation.Common
{
    public class ParameterizedCommand : BaseCommand
    {
        private readonly Action<object?> action;

        public ParameterizedCommand(Action<object?> action)
        {
            this.action = action;
        }

        public override void Execute(object? parameter)
        {
            action.Invoke(parameter);
        }
    }
}
