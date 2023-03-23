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
        private IEnumerable<ProductModel> products;

        public ProductsViewModel(IProductModelService productRepository, ProductModelFactory productModelFactory)
        {
            this.productRepository = productRepository;
            this.productModelFactory = productModelFactory;

            products = Enumerable.Empty<ProductModel>();

            ModifyProduct = vm => false;
            GetConfirmation = vm => false;
            DisplayMessage = vm => false;

            AddItem = new ParameterlessCommand(() =>
            {
                if (!DataReady)
                {
                    return;
                }
                var productViewModel = new ProductViewModel(this.productRepository, this.productModelFactory.Create());
                bool productSaved = ModifyProduct(productViewModel);
                if (productSaved)
                {
                    _ = GetProducts();
                }
            });

            Delete = new ParameterizedCommand(p =>
            {
                if (p is null || !DataReady)
                {
                    return;
                }
                var id = Convert.ToInt32(p);
                ProductModel productToDelete = Products.First(t => t.Id == id);
                bool confirmDelete = GetConfirmation?
                    .Invoke(new ConfirmationViewModel($"Are you sure you want to delete \"{productToDelete.Name}\"?"))
                        ?? false;

                if (!confirmDelete)
                {
                    return;
                }

                DataReady = false;
                this.productRepository.Delete(id)
                    .ContinueWith(o =>
                    {
                        DataReady = true;
                        if (o.Exception is not null)
                        {
                            _ = DisplayMessage?.Invoke(new ConfirmationViewModel(o.Exception.Message));
                            return;
                        }

                        _ = GetProducts();
                    }, TaskScheduler.FromCurrentSynchronizationContext());
            });

            Edit = new ParameterizedCommand(p =>
            {
                if (p is null || !DataReady)
                {
                    return;
                }
                var id = Convert.ToInt32(p);

                ProductModel productToEdit = Products.First(t => t.Id == id);
                var productViewModel = new ProductViewModel(this.productRepository, productToEdit);
                bool confirmed = ModifyProduct?.Invoke(productViewModel) ?? false;
                if (confirmed)
                {
                    _ = GetProducts();
                }
            });

            OnInitialize();
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

        public Func<ProductViewModel, bool> ModifyProduct { get; set; }
        public Func<ConfirmationViewModel, bool> GetConfirmation { get; set; }
        public Func<ConfirmationViewModel, bool> DisplayMessage { get; set; }

        public ICommand AddItem { get; private set; }

        public ICommand Edit { get; init; }

        public ICommand Delete { get; init; }

        private void OnInitialize()
        {
            _ = GetProducts();
        }

        private async Task GetProducts()
        {
            DataReady = false;
            try
            {
                Products = await productRepository.GetAll();
            }
            catch (Exception e)
            {
                _ = DisplayMessage(new ConfirmationViewModel(e.Message));
            }
            finally
            {
                DataReady = true;
            }
        }

        private void SetProducts(IEnumerable<ProductModel> products)
        {
            Products = products;
            DataReady = true;
        }
    }
}
