using System.Windows;
using KMM_HighPerformance.ViewModels;

namespace KMM_HighPerformance.View
{
    public partial class WindowView : Window
    {
        public WindowView()
        {
            this.DataContext = new WindowViewModel();
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
