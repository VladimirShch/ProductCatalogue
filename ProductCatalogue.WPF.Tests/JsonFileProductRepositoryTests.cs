using Moq;
using ProductCatalogue.WPF.Core.Common;
using ProductCatalogue.WPF.Core.Products;
using ProductCatalogue.WPF.DataAccess.Common;
using ProductCatalogue.WPF.DataAccess.Products;

namespace ProductCatalogue.WPF.Tests
{
    public class JsonFileProductRepositoryTests
    {
        #region Constructors
        [Fact]
        public void Constructor_WhenStorageIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new JsonFileProductRepository(null));
        }
        #endregion

        #region GetAll
        [Fact]
        public async Task GetAll_WhenStorageException_ThrowsRepositoryException()
        {
            var mockStorage = new Mock<IStorage>();
            mockStorage.Setup(s => s.GetData<IEnumerable<Product>>(It.IsAny<string>())).Throws<Exception>();
            var repository = new JsonFileProductRepository(mockStorage.Object);

            await Assert.ThrowsAsync<RepositoryException>(repository.GetAll);
        }

        [Fact]
        public async Task GetAll_WhenStorageReturnsNull_ReturnsEmptyEnumerable()
        {
            var mockStorage = new Mock<IStorage>();
            mockStorage.Setup(s => s.GetData<IEnumerable<Product>>(It.IsAny<string>())).ReturnsAsync((IEnumerable<Product>?)null);
            var repository = new JsonFileProductRepository(mockStorage.Object);
            var result = await repository.GetAll();
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAll_WhenStorageReturnsProducts_ReturnsSameProducts()
        {
            List<Product> initialProducts = CreateProducts();

            int productsCount = initialProducts.Count;

            var mockStorage = new Mock<IStorage>();
            mockStorage.Setup(s => s.GetData<IEnumerable<Product>>(It.IsAny<string>())).ReturnsAsync(initialProducts);
            var repository = new JsonFileProductRepository(mockStorage.Object);

            var resultProducts = await repository.GetAll();

            Assert.Equal(initialProducts, resultProducts);
            Assert.Equal(productsCount, resultProducts.Count());

            List<Product> productsCopy = CreateProducts();
            foreach (Product product in resultProducts)
            {
                Assert.Contains(productsCopy, pc => pc.Id == product.Id && pc.Name == product.Name && pc.Price == product.Price && pc.Type == product.Type);
            }
        }
        #endregion

        #region Get
        [Fact]
        public async Task Get_WhenStorageThrowsException_ThrowsRepositoryException()
        {
            var storageMock = new Mock<IStorage>();
            storageMock.Setup(s => s.GetData<IEnumerable<Product>>(It.IsAny<string>())).Throws<Exception>();

            var repository = new JsonFileProductRepository(storageMock.Object);

            await Assert.ThrowsAsync<RepositoryException>(() => repository.Get(1));
        }

        [Fact]
        public async Task Get_WhenStorageReturnsNull_ReturnsNull()
        {
            var storageMock = new Mock<IStorage>();
            storageMock.Setup(s => s.GetData<IEnumerable<Product>>(It.IsAny<string>())).ReturnsAsync((IEnumerable<Product>?)null);

            var repository = new JsonFileProductRepository(storageMock.Object);

            Product? result = await repository.Get(1);

            Assert.Null(result);
        }

        [Fact]
        public async Task Get_WhenStorageEmpty_ReturnsNull()
        {
            var storageMock = new Mock<IStorage>();
            storageMock.Setup(s => s.GetData<IEnumerable<Product>>(It.IsAny<string>())).ReturnsAsync(Enumerable.Empty<Product>);

            var repository = new JsonFileProductRepository(storageMock.Object);

            Product? result = await repository.Get(1);

            Assert.Null(result);
        }

        [Fact]
        public async Task Get_WhenThereIsNoProductWithSuchId_ReturnsNull()
        {
            var storageMock = new Mock<IStorage>();
            storageMock.Setup(s => s.GetData<IEnumerable<Product>>(It.IsAny<string>()))
                .ReturnsAsync(new List<Product> { new Product { Id = 2, Name = "Second", Type = ProductType.Peripheral, Price = 1000 } });

            var repository = new JsonFileProductRepository(storageMock.Object);

            Product? result = await repository.Get(1);

            Assert.Null(result);
        }

        [Fact]
        public async Task Get_WhenThereIsProductWithSuchId_ReturnsCorrectProduct()
        {
            int productId = 1;
            string productName = "First";
            ProductType productType = ProductType.Integrated;
            int productPrice = 1348;

            var storageMock = new Mock<IStorage>();
            storageMock.Setup(s => s.GetData<IEnumerable<Product>>(It.IsAny<string>()))
                .ReturnsAsync(new List<Product> { new Product { Id = productId, Name = productName, Type = productType, Price = productPrice } });

            var repository = new JsonFileProductRepository(storageMock.Object);

            Product? result = await repository.Get(productId);

            Assert.NotNull(result);
            Assert.Equal(result.Id, productId);
            Assert.Equal(result.Name, productName);
            Assert.Equal(result.Type, productType);
            Assert.Equal(result.Price, productPrice);
        }
        #endregion

        #region Save
        // TODO: we don't need to know if storage throws on get or set
        [Fact]
        public async Task Save_WhenStorageThrowsExceptionOnGet_ThrowsRepositoryException()
        {
            var storageMock = new Mock<IStorage>();
            storageMock.Setup(s => s.GetData<IEnumerable<Product>>(It.IsAny<string>())).Throws<Exception>();

            var repository = new JsonFileProductRepository(storageMock.Object);

            Product product = new()
            {
                Id = 1,
                Name = "First",
                Type = ProductType.Integrated,
                Price = 1500
            };
            await Assert.ThrowsAsync<RepositoryException>(() => repository.Save(product));
        }

        [Fact]
        public async Task Save_WhenStorageThrowsExceptionOnSet_ThrowsRepositoryException()
        {
            var storageMock = new Mock<IStorage>();
            storageMock.Setup(s => s.SetData(It.IsAny<string>(), It.IsAny<IEnumerable<Product>>())).Throws<Exception>();

            var repository = new JsonFileProductRepository(storageMock.Object);

            Product product = new()
            {
                Id = 1,
                Name = "First",
                Type = ProductType.Integrated,
                Price = 1500
            };
            await Assert.ThrowsAsync<RepositoryException>(() => repository.Save(product));
        }

        // Not the best test
        [Fact]
        public async Task Save_WhenProductNotExists_AddsNew()
        {
            var storageMock = new Mock<IStorage>();
            storageMock.Setup(s => s.GetData<ICollection<Product>>(It.IsAny<string>())).ReturnsAsync(new List<Product>());

            var resultDatabase = new List<Product>();
            storageMock.Setup(s => s.SetData(It.IsAny<string>(), It.IsAny<IEnumerable<Product>>()))
                .Callback<string, IEnumerable<Product>>((path, products) =>
                {
                    resultDatabase = products.ToList();
                });

            var repository = new JsonFileProductRepository(storageMock.Object);

            Product product = new()
            {
                Name = "First",
                Type = ProductType.Integrated,
                Price = 1500
            };

            await repository.Save(product);

            Assert.Single(resultDatabase);
            Assert.Equal(product, resultDatabase.First());
        }

        [Fact]
        public async Task Save_WhenProductExists_UpdatesProduct()
        {
            int productId = 1;
            string initialName = "First";
            ProductType initialType = ProductType.Integrated;
            int initialPrice = 1500;

            var storageMock = new Mock<IStorage>();
            storageMock.Setup(s => s.GetData<ICollection<Product>>(It.IsAny<string>()))
                .ReturnsAsync(new List<Product>() { new()
                    {
                        Id = productId,
                        Name = initialName,
                        Type = initialType,
                        Price = initialPrice
                    }
                });

            var resultDatabase = new List<Product>();
            storageMock.Setup(s => s.SetData(It.IsAny<string>(), It.IsAny<IEnumerable<Product>>()))
                .Callback<string, IEnumerable<Product>>((path, products) =>
                {
                    resultDatabase = products.ToList();
                });

            var repository = new JsonFileProductRepository(storageMock.Object);

            string newName = "Second";
            Product changedProduct = new()
            {
                Id = productId,
                Name = newName,
                Type = initialType,
                Price = initialPrice
            };

            await repository.Save(changedProduct);
            Assert.Single(resultDatabase);
            Assert.Equal(productId, resultDatabase[0].Id);
            Assert.Equal(newName, resultDatabase[0].Name);
            Assert.Equal(initialType, resultDatabase[0].Type);
            Assert.Equal(initialPrice, resultDatabase[0].Price);
        }

        [Fact]
        public async Task Save_WhenProductWithSameNameExits_ThrowsRepositoryException()
        {
            string productName = "First";

            var storageMock = new Mock<IStorage>();
            storageMock.Setup(s => s.GetData<ICollection<Product>>(It.IsAny<string>()))
                .ReturnsAsync(new List<Product>() { new()
                    {
                        Id = 1,
                        Name = productName,
                        Type = ProductType.Integrated,
                        Price = 1500
                    }
                });

            var resultDatabase = new List<Product>();
            storageMock.Setup(s => s.SetData(It.IsAny<string>(), It.IsAny<IEnumerable<Product>>()))
                .Callback<string, IEnumerable<Product>>((path, products) =>
                {
                    resultDatabase = products.ToList();
                });

            var repository = new JsonFileProductRepository(storageMock.Object);

            Product newProduct = new()
            {
                Name = productName,
                Type = ProductType.Peripheral,
                Price = 230
            };

            await Assert.ThrowsAsync<RepositoryException>(() => repository.Save(newProduct));
        }
        #endregion

        private List<Product> CreateProducts()
        {
            return new List<Product>
            {
                new Product{Id = 1, Name = "First", Type = ProductType.Peripheral, Price = 200},
                new Product{Id = 2, Name = "Second", Type = ProductType.Integrated, Price = 1380},
                new Product{Id = 3, Name = "Third", Type = ProductType.Integrated, Price = 2405},
                new Product{Id = 4, Name = "Fourth", Type = ProductType.Peripheral, Price = 8000},
                new Product{Id = 5, Name = "Fifth", Type = ProductType.Integrated, Price = 1880},
            };
        }
    }
}