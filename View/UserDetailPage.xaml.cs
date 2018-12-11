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
    /// Lógica de interacción para ExpensesPage.xaml
    /// </summary>
    public partial class UserDetailPage : Page
    {
        public UserDetailPage()
        {
            InitializeComponent();
            DataContext = MainWindowViewModel.GetInstance();
        }

        private void KeyUpNoSymbolsEvent(object sender, KeyEventArgs e)
        {
            ((TextBox)sender).Text = Formatter.RemoveInvalidCharacters(((TextBox)sender).Text, out bool status);
            if (status)
            {
                MainWindowViewModel.GetInstance().Code = "Símbolo inválido!";
            }
        }

        private void KeyUpNoSymbolsNoSpaceEvent(object sender, KeyEventArgs e)
        {
            ((TextBox)sender).Text = Formatter.RemoveInvalidCharacters(((TextBox)sender).Text, out var status);
            if (status)
            {
                MainWindowViewModel.GetInstance().Code = "Símbolo inválido!";
            }
            ((TextBox)sender).Text = Formatter.RemoveWhiteSpace(((TextBox)sender).Text, out status);
            if (status)
            {
                MainWindowViewModel.GetInstance().Code = "Espacio inválido!";
            }
        }

        private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            var password = PasswordBox.Password;
            Formatter.RemoveInvalidCharacters(password, out var status);
            if (status)
            {
                MainWindowViewModel.GetInstance().Code = "Símbolo inválido!";
                ((PasswordBox) sender).Password = "";
                return;
            }

            Formatter.RemoveWhiteSpace(password, out var spaceStatus);
            if (spaceStatus)
            {
                MainWindowViewModel.GetInstance().Code = "Espacio inválido!";
                ((PasswordBox)sender).Password = "";
                return;
            }

            MainWindowViewModel.GetInstance().UserTemporalItem.Password = password;
        }

        private void PasswordVerificationBox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            var password = PasswordVerificationBox.Password;
            Formatter.RemoveInvalidCharacters(password, out var status);
            if (status)
            {
                MainWindowViewModel.GetInstance().Code = "Símbolo inválido!";
                ((PasswordBox)sender).Password = "";
                return;
            }

            Formatter.RemoveWhiteSpace(password, out var spaceStatus);
            if (spaceStatus)
            {
                MainWindowViewModel.GetInstance().Code = "Espacio inválido!";
                ((PasswordBox)sender).Password = "";
                return;
            }

            MainWindowViewModel.GetInstance().UserTemporalItem.PasswordVerification = password;
        }
    }
}
