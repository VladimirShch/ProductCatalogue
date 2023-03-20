using ProductCatalogue.WPF.Core.Common;
using ProductCatalogue.WPF.Core.Products;

namespace ProductCatalogue.WPF.Presentation.Products.Models
{
    public class ProductModelFactory
    {
        private readonly IValidator<Product> productValidator;
        private readonly IProductModelMapper productModelMapper;

        public ProductModelFactory(IValidator<Product> productValidator, IProductModelMapper productModelMapper)
        {
            this.productValidator = productValidator;
            this.productModelMapper = productModelMapper;
        }

        public ProductModel Create()
        {
            return new ProductModel(productValidator, productModelMapper);
        }
    }
}
