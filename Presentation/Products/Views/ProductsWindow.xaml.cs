﻿using Autofac;
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
            viewModel.ModifyProduct = OnModifyProduct;
            viewModel.GetConfirmation = OnGetConfirmation;
            viewModel.DisplayMessage = OnDisplayMessage;

        }

        private bool OnModifyProduct(ProductViewModel productViewModel)
        {
            var productWindow = new ProductWindow
            {
                DataContext = productViewModel
            };
            bool? confirmed = productWindow.ShowDialog();
            return confirmed ?? false;
        }

        private bool OnGetConfirmation(ConfirmationViewModel confirmationViewModel)
        {
            var confirmationWindow = new ConfirmationWindow
            {
                DataContext = confirmationViewModel
            };
            bool? confirmed = confirmationWindow.ShowDialog();
            return confirmed ?? false;
        }

        private bool OnDisplayMessage(ConfirmationViewModel confirmationViewModel)
        {
            var confirmationWindow = new MessageWindow
            {
                DataContext = confirmationViewModel
            };
            bool? confirmed = confirmationWindow.ShowDialog();
            return confirmed ?? false;
        }
    }
}
