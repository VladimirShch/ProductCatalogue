using Microsoft.VisualBasic;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskWPFExperiment.Core.Products;
using TaskWPFExperiment.DataAccess.Common;

namespace TaskWPFExperiment.DataAccess.Products
{
    // TODO: add logger;
    // TODO: add file exceptions handling, throwing core-exception to higher level
    // TODO: add adapter - from Dto to Core-entity
    // TODO: create file accessor in common to read and write information from file
    public class JsonFileProductRepository : IProductRepository
    {
        private readonly string filePath = "Products.json";
        private readonly IStorage storage;

        public JsonFileProductRepository(IStorage storage)
        {
            this.storage = storage;
        }

        public async Task Delete(int id)
        {
            var products = await storage.GetData<ICollection<Product>>(filePath) ?? new List<Product>();
            Product? productToRemove = products.FirstOrDefault(t => t.Id == id);

            if (productToRemove is not null)
            {
                products.Remove(productToRemove);
                await storage.SetData(filePath, products);
            }
        }

        public async Task<Product?> Get(int id)
        {
            var products = await storage.GetData<IEnumerable<Product>>(filePath) ?? Enumerable.Empty<Product>();
            Product? product = products.FirstOrDefault(t => t.Id == id);

            return product;
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            var products = await storage.GetData<IEnumerable<Product>>(filePath) ?? Enumerable.Empty<Product>();

            return products;
        }

        public async Task Save(Product product)
        {
            var products = await storage.GetData<ICollection<Product>>(filePath) ?? new List<Product>();
            if(product.Id == 0)
            {
                product.Id = products.Any() ? products.Max(t => t.Id) + 1 : 1;
                products.Add(product);
            }
            else
            {
                Product? productExisting = products.FirstOrDefault(t => t.Id == product.Id);
                if (productExisting is not null)
                {
                    productExisting.Name = product.Name;
                    productExisting.Price = product.Price;
                    productExisting.Type = product.Type;
                }
            }

            await storage.SetData(filePath, products);
        }
    }
}
