using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ProductCatalogue.WPF.Presentation.Common
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        protected bool dataReady;

        public event PropertyChangedEventHandler? PropertyChanged;

        public bool DataReady
        {
            get => dataReady;
            protected set
            {
                if (dataReady != value)
                {
                    dataReady = value;
                    RaisePropertyChanged(nameof(DataReady));
                }
            }
        }

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
