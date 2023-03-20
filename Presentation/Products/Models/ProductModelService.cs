using ProductCatalogue.WPF.Core.Products;
using ProductCatalogue.WPF.Presentation.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalogue.WPF.Presentation.Products.Models
{
    public class ProductModelService : IProductModelService
    {
        private readonly IProductRepository productRepository;
        private readonly IProductModelAdapter productModelAdapter;

        public ProductModelService(IProductRepository productRepository, IProductModelAdapter productModelAdapter)
        {
            this.productRepository = productRepository;
            this.productModelAdapter = productModelAdapter;
        }

        public async Task Delete(int id)
        {
            await productRepository.Delete(id);
        }

        public async Task<ProductModel?> Get(int id)
        {
            Product? product = await productRepository.Get(id);
            if(product is null)
            {
                return null;
            }
            ProductModel model = productModelAdapter.ToModel(product);
            return model;
        }

        public async Task<IEnumerable<ProductModel>> GetAll()
        {
            IEnumerable<Product> products = await productRepository.GetAll();
            IEnumerable<ProductModel> models = products.Select(productModelAdapter.ToModel);
            
            return models;
        }

        public async Task Save(ProductModel productModel)
        {
            if (productModel is null)
                throw new ArgumentNullException(nameof(productModel));

            if (!productModel.IsValid)
            {
                throw new ModelException($"Product \"{productModel.Name}\" is not valid!");
            }

            Product product = productModelAdapter.FromModel(productModel);
            await productRepository.Save(product);
        }
    }
}
