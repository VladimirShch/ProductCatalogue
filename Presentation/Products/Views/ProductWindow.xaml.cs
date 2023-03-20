using System.Windows;
using ProductCatalogue.WPF.Presentation.Dialogs.ViewModels;
using ProductCatalogue.WPF.Presentation.Dialogs.Views;
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

                    viewModel.InvokeMessageDialog = OnInvokeMessage;
                }
            };
        }

        private bool OnInvokeMessage(ConfirmationViewModel viewModel)
        {
            var messageWindow = new MessageWindow()
            {
                DataContext = viewModel
            };

            return messageWindow.ShowDialog() ?? false;
        }
    }
}
