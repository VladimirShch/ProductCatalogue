using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductCatalogue.WPF.Core.Products
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAll();
        Task<Product?> Get(int id);
        Task Save(Product product);
        Task Delete(int id);
    }
}
