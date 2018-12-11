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
using Athena;

namespace Seiya
{
    /// <summary>
    /// Interaction logic for PosMenuPage.xaml
    /// </summary>
    public partial class PosMenuPage : Page
    {
        public PosMenuPage()
        {
            InitializeComponent();
            this.DataContext = MainWindowViewModel.GetInstance();
        }
    }
}
