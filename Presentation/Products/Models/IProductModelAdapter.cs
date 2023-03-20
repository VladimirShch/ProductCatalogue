using ProductCatalogue.WPF.Core.Products;

namespace ProductCatalogue.WPF.Presentation.Products.Models
{
    public interface IProductModelAdapter
    {
        ProductModel ToModel(Product product);
        Product FromModel(ProductModel model);
    }
}
