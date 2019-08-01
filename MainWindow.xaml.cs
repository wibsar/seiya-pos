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
using Zeus;

namespace Seiya
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            RetailItem product = new RetailItem();
            SystemConfiguration systemConfiguration = new SystemConfiguration();
            DataContext = MainWindowViewModel.GetInstance(product, systemConfiguration);
            InitializeComponent();
        }

        private void KeyUpNoSymbolsEvent(object sender, KeyEventArgs e)
        {
            ((TextBox) sender).Text = Formatter.RemoveInvalidCharacters(((TextBox) sender).Text, out var status);
            ((TextBox) sender).CaretIndex = ((TextBox) sender).Text.Length;
            if (status)
            {
                MainWindowViewModel.GetInstance(null, null).Code = "Símbolo inválido!";
            }
        }

        private void KeyUpNoSymbolsNoSpaceEvent(object sender, KeyEventArgs e)
        {
            ((TextBox) sender).Text = Formatter.RemoveInvalidCharacters(((TextBox) sender).Text, out var status);
            ((TextBox) sender).CaretIndex = ((TextBox) sender).Text.Length;
            if (status)
            {
                MainWindowViewModel.GetInstance(null, null).Code = "Símbolo inválido!";
            }

            ((TextBox) sender).Text = Formatter.RemoveWhiteSpace(((TextBox) sender).Text, out status);
            ((TextBox) sender).CaretIndex = ((TextBox) sender).Text.Length;
            if (status)
            {
                MainWindowViewModel.GetInstance(null, null).Code = "Espacio inválido!";
            }
        }

        private void TxtCode_OnMouseLeftButtonDown(object sender, RoutedEventArgs routedEventArgs)
        {
            //Clear textbox when the focus is on txtbox
            ((TextBox) sender).Text = "";
            ((TextBox) sender).Focus();
            var color = new BrushConverter();
            ((TextBox) sender).Foreground = (Brush) color.ConvertFrom("#FF2C5066");
        }
    }
}
