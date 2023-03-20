using ProductCatalogue.WPF.Presentation.Common;

namespace ProductCatalogue.WPF.Presentation.Dialogs.ViewModels
{
    public class ConfirmationViewModel : ViewModelBase
    {
        public ConfirmationViewModel(string message)
        {
            Message = message;
        }

        public string Message { get; init; }
    }
}
