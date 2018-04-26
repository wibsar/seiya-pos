using Seiya.WpfBindingUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Seiya
{
    public class PosMenuFramePageViewModel : BaseViewModel
    {
        private static int x = 0;


        #region Fields

        private static string _menuCurrentPage = "\\Views\\PosMenuPage.xaml";
        private const string _defaultMenuPage = "\\Views\\PosMenuPage.xaml";

        #endregion

        #region Constructor
        public PosMenuFramePageViewModel()
        {
              x++;
              LoadMainPage();
//            CurrentPage = initialPage;
        }

        #endregion

        //Singleton Pattern Implementation
        private static PosMenuFramePageViewModel _instance;
        public static PosMenuFramePageViewModel Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new PosMenuFramePageViewModel();
                }
                return _instance;
            }
        }

        #region Observable Properties

        public string MenuCurrentPage
        {
            get
            {
                return _menuCurrentPage;
            }
            set
            {
                _menuCurrentPage = value;
                OnPropertyChanged("MenuCurrentPage");
            }
        }

        #endregion

        #region ChangePageCommand

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

        #region Methods

        void LoadMainPage()
        {
           Execute_ChangePageCommand("Menu");
        }
        #endregion

    }
}
