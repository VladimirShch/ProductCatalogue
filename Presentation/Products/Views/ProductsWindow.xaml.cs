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
            DataContext = IoC.Container?.Resolve<ProductsViewModel>();
        }
    }
}
