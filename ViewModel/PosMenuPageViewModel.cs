using Seiya.WpfBindingUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Seiya
{
    /// <summary>
    /// Viewmodel for the main menu page of the POS
    /// </summary>
    public class PosMenuPageViewModel : BaseViewModel 
    {
        #region Fields

        private string _menuCurrentPage;

        #endregion

        #region Constructors
        public PosMenuPageViewModel()
        {
  //          _currentPage = "\\View\\PosMenuPage.xaml";
        }

        #endregion

        #region Observable Properties

        /// <summary>
        /// Holds current page
        /// </summary>
        public string MenuCurrentPage
        {
            get { return _menuCurrentPage; }
            set
            {
                _menuCurrentPage = value;
                OnPropertyChanged("MenuCurrentPage");
            }
        }

        #endregion



        #region Commands

        public ICommand ChangePageCommand { get { return _changePageCommand ?? (_changePageCommand = new DelegateCommand(Execute_ChangePageCommand, CanExecute_ChangePageCommand)); } }
        private ICommand _changePageCommand;

        internal void Execute_ChangePageCommand(object parameter)
        {
            //Add pages here to be linked to a botton in the main menu page
            switch ((string)parameter)
            {
                case "Menu":
                    MenuCurrentPage = "\\View\\PosMenuPage.xaml";
                    break;
                case "Inventario":
                    MenuCurrentPage = "\\View\\InventoryMainPage.xaml";
                    break;
                case "Corte":
                    MenuCurrentPage = "\\View\\EndSalesPage.xaml";
                    break;
            }
        }

        internal bool CanExecute_ChangePageCommand(object parameter)
        {
            return true;
        }

        #endregion  


    }
}
