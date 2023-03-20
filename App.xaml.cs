using Autofac;
using System.Windows;
using ProductCatalogue.WPF.Core.Products;
using ProductCatalogue.WPF.DataAccess.Common;
using ProductCatalogue.WPF.DataAccess.Products;
using ProductCatalogue.WPF.Presentation.Common;
using ProductCatalogue.WPF.Presentation.Products.ViewModels;

namespace ProductCatalogue.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var builder = new ContainerBuilder();
            builder.RegisterType<FileStorage>().As<IStorage>();
            builder.RegisterType<JsonFileProductRepository>().As<IProductRepository>();
            builder.RegisterType<ProductsViewModel>().AsSelf();

            IoC.Container = builder.Build();
        }
    }
}
