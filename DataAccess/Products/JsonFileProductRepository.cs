using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductCatalogue.WPF.Core.Products;
using ProductCatalogue.WPF.DataAccess.Common;
using ProductCatalogue.WPF.Core.Common;
using System;

namespace ProductCatalogue.WPF.DataAccess.Products
{
    // TODO: add logger;
    // TODO: add file exceptions handling, throwing core-exception to higher level
    // TODO: add adapter - from Dto to Core-entity
    // TODO: create file accessor in common to read and write information from file
    public class JsonFileProductRepository : IProductRepository
    {
        private readonly string storageWritingError = "Error writing to the storage";
        private readonly string storageReadingError = "Error reading from the storage";
        private readonly string filePath = "Products.json";
        private readonly IStorage storage;

        public JsonFileProductRepository(IStorage storage)
        {
            this.storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        public async Task Delete(int id)
        {
            ICollection<Product> products;
            try
            {
                products = await storage.GetData<ICollection<Product>>(filePath) ?? new List<Product>();
            }
            catch (Exception e)
            {
                throw new RepositoryException(storageReadingError, e);
            }

            Product? productToRemove = products.FirstOrDefault(t => t.Id == id);

            if (productToRemove is not null)
            {
                products.Remove(productToRemove);
                try
                {
                    await storage.SetData(filePath, products);
                }
                catch (Exception e)
                {
                    throw new RepositoryException(storageWritingError, e);
                }
            }
        }

        public async Task<Product?> Get(int id)
        {
            IEnumerable<Product> products;
            try
            {
                products = await storage.GetData<IEnumerable<Product>>(filePath) ?? Enumerable.Empty<Product>();
            }
            catch (Exception e)
            {
                throw new RepositoryException(storageReadingError, e);
            }

            Product? product = products.FirstOrDefault(t => t.Id == id);

            return product;
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            IEnumerable<Product> products;
            try
            {
                products = await storage.GetData<IEnumerable<Product>>(filePath) ?? Enumerable.Empty<Product>();
            }
            catch (Exception e)
            {
                throw new RepositoryException(storageReadingError, e);
            }

            return products;
        }

        public async Task Save(Product product)
        {
            ICollection<Product> products;
            try
            {
                products = await storage.GetData<ICollection<Product>>(filePath) ?? new List<Product>();
            }
            catch (Exception e)
            {
                throw new RepositoryException(storageReadingError, e);
            }

            //! Checking name uniqueness here 
            Product? productWithDuplicatedName = products.FirstOrDefault(p => p.Name == product.Name && p.Id != product.Id);
            if (productWithDuplicatedName is not null)
            {
                throw new RepositoryException($"There is a duplicated name for \"{product.Name}\"");
            }

            if (product.Id == 0)
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
            try
            {
                await storage.SetData(filePath, products);
            }
            catch (Exception e)
            {
                throw new RepositoryException(storageWritingError, e);
            }
        }
    }
}
