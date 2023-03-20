using System.Windows;
using ProductCatalogue.WPF.Presentation.Products.ViewModels;

namespace ProductCatalogue.WPF.Presentation.Products.Views
{
    /// <summary>
    /// Логика взаимодействия для ProductWindow.xaml
    /// </summary>
    public partial class ProductWindow : Window
    {
        public ProductWindow()
        {
            InitializeComponent();
            this.Activated += (sender, e) =>
            {
                if(this.DataContext is ProductViewModel viewModel)
                {
                    viewModel.SavingFinished += (s, se) =>
                    {
                        this.DialogResult = true;
                        this.Close();
                    };
                }
            };
        }
    }
}
