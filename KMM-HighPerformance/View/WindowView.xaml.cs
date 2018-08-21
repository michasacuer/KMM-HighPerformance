using System;
using System.Collections.Generic;
using System.Drawing;
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
using KMM_HighPerformance.ViewModels;
using KMM_HighPerformance.Models;
using Microsoft.Win32;
using System.Threading;

namespace KMM_HighPerformance.View
{
    
    public partial class WindowView : Window
    {
        public WindowView()
        {
            this.DataContext = new ViewModel();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            InitializeComponent();
            label.Content = GetHardwareInfo.GetCPU();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
