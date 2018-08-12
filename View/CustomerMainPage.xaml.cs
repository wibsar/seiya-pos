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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Seiya
{
    /// <summary>
    /// Lógica de interacción para CustomerMainPage.xaml
    /// </summary>
    public partial class CustomerMainPage : Page
    {
        public CustomerMainPage()
        {
            DataContext = MainWindowViewModel.GetInstance();

            InitializeComponent();
        }
    }
}
