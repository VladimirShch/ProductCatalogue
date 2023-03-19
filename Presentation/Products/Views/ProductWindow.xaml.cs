using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TaskWPFExperiment.Presentation.Products.ViewModels;

namespace TaskWPFExperiment.Presentation.Products.Views
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
