using System.Windows;
using Tokenizer.UI.ViewModels;

namespace Tokenizer.UI.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new Tokenizer.UI.ViewModels.MainViewModel();
        }
    }
}