using ProductCatalogue.WPF.Core.Products;

namespace ProductCatalogue.WPF.Presentation.Products.Models
{
    public interface IProductModelMapper
    {
        void ToModel(Product product, ProductModel model);
        void FromModel(ProductModel model, Product product);
    }
}
