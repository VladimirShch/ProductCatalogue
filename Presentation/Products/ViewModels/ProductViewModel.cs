using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ProductCatalogue.WPF.Core.Products;
using ProductCatalogue.WPF.Presentation.Common;

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
                    .ContinueWith(t => SavingFinished?.Invoke(this, EventArgs.Empty), TaskScheduler.FromCurrentSynchronizationContext());
            });
        }

        public event EventHandler? SavingFinished;

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
