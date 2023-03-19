using TaskWPFExperiment.Presentation.Common;

namespace TaskWPFExperiment.Presentation.Dialogs.ViewModels
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
