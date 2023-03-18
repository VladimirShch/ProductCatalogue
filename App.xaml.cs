using Autofac;
using System.Windows;
using TaskWPFExperiment.Core.Products;
using TaskWPFExperiment.DataAccess.Common;
using TaskWPFExperiment.DataAccess.Products;
using TaskWPFExperiment.Presentation.Common;
using TaskWPFExperiment.Presentation.Products.ViewModels;

namespace TaskWPFExperiment
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
