using TaskWPFExperiment.Core.Products;
using TaskWPFExperiment.Presentation.Common;

namespace TaskWPFExperiment.Presentation.Products.ViewModels
{
    public class ProductViewModel : ViewModelBase
    {
        private Product product;

        public ProductViewModel(Product? product)
        {
            this.product = product ?? new Product();
        }

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
    }
}
