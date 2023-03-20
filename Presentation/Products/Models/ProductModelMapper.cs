using ProductCatalogue.WPF.Core.Products;

namespace ProductCatalogue.WPF.Presentation.Products.Models
{
    public class ProductModelMapper : IProductModelMapper
    {
        public void FromModel(ProductModel model, Product product)
        {
            product.Id = model.Id;
            product.Name = model.Name;
            product.Price = model.Price;
            product.Type = model.Type;
        }

        public void ToModel(Product product, ProductModel model)
        {
            model.Id = product.Id;
            model.Name = product.Name;
            model.Price = product.Price;
            model.Type = product.Type;
        }
    }
}
