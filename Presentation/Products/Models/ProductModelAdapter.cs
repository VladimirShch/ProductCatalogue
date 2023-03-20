using ProductCatalogue.WPF.Core.Products;

namespace ProductCatalogue.WPF.Presentation.Products.Models
{
    public class ProductModelAdapter : IProductModelAdapter
    {
        private readonly ProductModelFactory modelFactory;
        private readonly IProductModelMapper productModelMapper;

        public ProductModelAdapter(ProductModelFactory productModelFactory, IProductModelMapper productModelMapper)
        {
            modelFactory = productModelFactory;
            this.productModelMapper = productModelMapper;
        }

        public Product FromModel(ProductModel model)
        {
            Product product = new();
            productModelMapper.FromModel(model, product);

            return product;
        }

        public ProductModel ToModel(Product product)
        {
            ProductModel model = modelFactory.Create();
            productModelMapper.ToModel(product, model);

            return model;
        }
    }
}
