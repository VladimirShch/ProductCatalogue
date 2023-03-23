using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ProductCatalogue.WPF.Presentation.Common;
using ProductCatalogue.WPF.Presentation.Dialogs.ViewModels;
using ProductCatalogue.WPF.Presentation.Products.Models;

namespace ProductCatalogue.WPF.Presentation.Products.ViewModels
{
    public class ProductViewModel : ViewModelBase
    {
        private readonly IProductModelService productRepository;
        private ProductModel product;

        public ProductViewModel(IProductModelService productRepository, ProductModel product)
        {
            this.productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            this.product = product ?? throw new ArgumentNullException(nameof(product));
            DataReady = true;
            Save = new ParameterlessCommand(() =>
            {
                DataReady = false;

                this.productRepository.Save(Product)
                    .ContinueWith(t =>
                    {
                        DataReady = true;

                        if (t.Exception is null)
                        {
                            SavingFinished?.Invoke(this, EventArgs.Empty);
                        }
                        else
                        {
                            _ = DisplayMessage?.Invoke(new ConfirmationViewModel(t.Exception.Message));
                        }
                    }, TaskScheduler.FromCurrentSynchronizationContext());
            }/*, () => Product.IsValid*/);
        }

        public EventHandler? SavingFinished { get; set; }
        public Func<ConfirmationViewModel, bool>? DisplayMessage { get; set; }

        public ProductModel Product
        {
            get => product;
            set
            {
                if (product != value)
                {
                    product = value;
                    RaisePropertyChanged(nameof(Product));
                }
            }
        }

        public ICommand Save { get; init; }
    }
}
