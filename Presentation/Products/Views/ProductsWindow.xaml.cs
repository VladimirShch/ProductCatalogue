using Autofac;
using System.Windows;
using ProductCatalogue.WPF.Presentation.Common;
using ProductCatalogue.WPF.Presentation.Dialogs.ViewModels;
using ProductCatalogue.WPF.Presentation.Dialogs.Views;
using ProductCatalogue.WPF.Presentation.Products.ViewModels;

namespace ProductCatalogue.WPF.Presentation.Products.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ProductsWindow : Window
    {
        public ProductsWindow()
        {
            InitializeComponent();
            var viewModel = IoC.Container?.Resolve<ProductsViewModel>();
            DataContext = viewModel;
            viewModel.InvokeProductDialog = OnInvokeProductDialog;
            viewModel.InvokeConfirmationDialog = OnInvokeConfirmationDialog;
        }

        private bool OnInvokeProductDialog(ProductViewModel productViewModel)
        {
            var productWindow = new ProductWindow
            {
                DataContext = productViewModel
            };
            bool? confirmed = productWindow.ShowDialog();
            return confirmed ?? false;
        }

        private bool OnInvokeConfirmationDialog(ConfirmationViewModel confirmationViewModel)
        {
            var confirmationWindow = new ConfirmationWindow
            {
                DataContext = confirmationViewModel
            };
            bool? confirmed = confirmationWindow.ShowDialog();
            return confirmed ?? false;
        }
    }
}
