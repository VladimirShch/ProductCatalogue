using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TaskWPFExperiment.Core.Products;
using TaskWPFExperiment.Presentation.Common;
using TaskWPFExperiment.Presentation.Dialogs.ViewModels;

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

            InvokeProductDialog = vm => false;
            InvokeConfirmationDialog = vm => false;

            AddItem = new ParameterlessCommand(() =>
            {
                var productViewModel = new ProductViewModel(this.productRepository, new Product());
                bool productSaved = InvokeProductDialog(productViewModel);
                if (productSaved)
                {
                    this.productRepository.GetAll().ContinueWith(t => SetProducts(t.Result));
                }
            });

            Delete = new ParameterizedCommand(p =>
            {
                if (p is null)
                {
                    return;
                }
                var id = Convert.ToInt32(p);
                Product productToDelete = Products.First(t => t.Id == id);
                bool confirmDelete = InvokeConfirmationDialog(new ConfirmationViewModel($"Are you shure you want to delete \"{productToDelete.Name}\"?"));
                if (!confirmDelete)
                {
                    return;
                }
                this.productRepository.Delete(id)
                    .ContinueWith(o =>
                        this.productRepository.GetAll().ContinueWith(t => SetProducts(t.Result)));
            });
          
            Edit = new ParameterizedCommand(p =>
            {
                if (p is null)
                {
                    return;
                }
                var id = Convert.ToInt32(p);

                Product productToEdit = Products.First(t => t.Id == id);
                var productViewModel = new ProductViewModel(this.productRepository, productToEdit);
                bool confirmed = InvokeProductDialog(productViewModel);
                if (confirmed)
                {
                    this.productRepository.GetAll().ContinueWith(t => SetProducts(t.Result));
                }
            });
            
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

        public Func<ProductViewModel, bool> InvokeProductDialog { get; set; }
        public Func<ConfirmationViewModel, bool> InvokeConfirmationDialog { get; set; }

        public ICommand AddItem { get; private set; }

        public ICommand Edit { get; init; }

        public ICommand Delete { get; init; }

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
