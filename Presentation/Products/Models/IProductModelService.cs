using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductCatalogue.WPF.Presentation.Products.Models
{
    public interface IProductModelService
    {
        Task<IEnumerable<ProductModel>> GetAll();
        Task<ProductModel?> Get(int id);
        Task Save(ProductModel productModel);
        Task Delete(int id);
    }
}
