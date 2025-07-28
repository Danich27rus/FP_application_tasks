using System.Windows;
using UdpTrafficGeneratorWPF.ViewModels;

namespace UdpTrafficGeneratorWPF.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}