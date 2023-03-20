using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ProductCatalogue.WPF.Presentation.Common;
using ProductCatalogue.WPF.Presentation.Dialogs.ViewModels;
using ProductCatalogue.WPF.Presentation.Products.Models;

namespace ProductCatalogue.WPF.Presentation.Products.ViewModels
{
    public class ProductsViewModel : ViewModelBase
    {
        private readonly IProductModelService productRepository;
        private readonly ProductModelFactory productModelFactory;
        private bool dataReady;
        private IEnumerable<ProductModel> products;

        public ProductsViewModel(IProductModelService productRepository, ProductModelFactory productModelFactory)
        {
            this.productRepository = productRepository;
            this.productModelFactory = productModelFactory;

            products = Enumerable.Empty<ProductModel>();

            InvokeProductDialog = vm => false;
            InvokeConfirmationDialog = vm => false;
            InvokeMessageDialog = vm => false;

            AddItem = new ParameterlessCommand(() =>
            {
                var productViewModel = new ProductViewModel(this.productRepository, this.productModelFactory.Create());
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
                ProductModel productToDelete = Products.First(t => t.Id == id);
                bool confirmDelete = InvokeConfirmationDialog(new ConfirmationViewModel($"Are you sure you want to delete \"{productToDelete.Name}\"?"));
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

                ProductModel productToEdit = Products.First(t => t.Id == id);
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
                if (dataReady != value)
                {
                    dataReady = value;
                    RaisePropertyChanged(nameof(DataReady));
                }
            }
        }

        public IEnumerable<ProductModel> Products
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
        public Func<ConfirmationViewModel, bool> InvokeMessageDialog { get; set; }

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

        private void SetProducts(IEnumerable<ProductModel> products)
        {
            Products = products;
            DataReady = true;
        }
    }
}
