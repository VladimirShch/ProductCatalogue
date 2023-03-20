using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ProductCatalogue.WPF.Core.Products;
using ProductCatalogue.WPF.Presentation.Common;
using ProductCatalogue.WPF.Presentation.Dialogs.ViewModels;

namespace ProductCatalogue.WPF.Presentation.Products.ViewModels
{
    public class ProductViewModel : ViewModelBase
    {
        private readonly IProductRepository productRepository;
        private Product product;

        public ProductViewModel(IProductRepository productRepository, Product? product)
        {
            this.productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            this.product = product ?? new Product();
            Save = new ParameterlessCommand(() =>
            {
                this.productRepository.Save(Product)
                    .ContinueWith(t =>
                    {
                        if (t.Exception is null)
                        {
                            SavingFinished?.Invoke(this, EventArgs.Empty);
                        }
                        else
                        {
                            _ = InvokeMessageDialog?.Invoke(new ConfirmationViewModel(t.Exception.Message));
                        }
                    }, TaskScheduler.FromCurrentSynchronizationContext());
            }/*, () => Product.IsValid*/);
        }

        public EventHandler? SavingFinished { get; set; }
        public Func<ConfirmationViewModel, bool>? InvokeMessageDialog { get; set; }

        public Product Product
        {
            get => product;
            set
            {
                if(product != value)
                {
                    product = value;
                    RaisePropertyChanged(nameof(Product));
                }                
            }
        }

        public ICommand Save { get; init; }
    }
}
