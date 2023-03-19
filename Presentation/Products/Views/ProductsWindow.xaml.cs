using Autofac;
using System.Windows;
using TaskWPFExperiment.Presentation.Common;
using TaskWPFExperiment.Presentation.Products.ViewModels;

namespace TaskWPFExperiment.Presentation.Products.Views
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
        }

        private bool OnInvokeProductDialog(ProductViewModel productViewModel)
        {
            var productWindow = new ProductWindow();
            productWindow.DataContext = productViewModel;
            bool? confirmed = productWindow.ShowDialog();
            return confirmed ?? false;
        }
    }
}
