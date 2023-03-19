using System.Windows;

namespace TaskWPFExperiment.Presentation.Dialogs.Views
{
    /// <summary>
    /// Логика взаимодействия для ConfirmationWindow.xaml
    /// </summary>
    public partial class ConfirmationWindow : Window
    {
        public ConfirmationWindow()
        {
            InitializeComponent();
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
