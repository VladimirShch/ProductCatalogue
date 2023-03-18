using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TaskWPFExperiment.Core.Products;
using TaskWPFExperiment.Presentation.Common;

namespace TaskWPFExperiment.Presentation.Products.ViewModels
{
    public class ProductsViewModel : ViewModelBase
    {
        private readonly IProductRepository productRepository;
        private bool dataReady;
        private IEnumerable<Product> products;

        public ProductsViewModel(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
            products = Enumerable.Empty<Product>();
            AddItem = new ParameterlessCommand(() => productRepository.Save(new Product
            {
                Name = "Aazz",
                Type = ProductType.Peripheral,
                Price = 5000
            }).ContinueWith(o => productRepository.GetAll().ContinueWith(t => SetProducts(t.Result))));
            
            OnInitialize();
        }

        public bool DataReady
        {
            get => dataReady;
            private set
            {
                if(dataReady != value)
                {
                    dataReady = value;
                    RaisePropertyChanged(nameof(DataReady));
                }
            }
        }

        public IEnumerable<Product> Products
        {
            get => products;
            private set
            {
                products = value;
                RaisePropertyChanged(nameof(Products));
            }
        }

        public ICommand AddItem { get; private set; }

        private void OnInitialize()
        {
            // TODO: repository exception
            productRepository.GetAll().ContinueWith(p => { Products = p.Result; DataReady = true; }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        // TODO: repository exception
        private async Task GetProducts()
        {
            DataReady = false;
            Products = await productRepository.GetAll();
            DataReady = true;
        }

        private void SetProducts(IEnumerable<Product> products)
        {
            Products = products;
            DataReady = true;
        }
    }
}
