using Seiya.WpfBindingUtilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Seiya
{
    public class InventoryMainPageViewModel : BaseViewModel
    {
        #region Fields
        private ObservableCollection<Product> _currentProductItemsList = new ObservableCollection<Product>();
        private string _currentPage;
        #endregion

        //_inventory
        #region Constructor
        public InventoryMainPageViewModel()
        {
            var a = Product.Add("Producto1", "Merceria", 10, 1);
            var b = Product.Add("Producto2", "Regalos", 10, 1);
            var c = Product.Add("Producto3", "Madera", 10, 1);
            var d = Product.Add("Producto4", "Vidrio", 10, 1);
            _currentProductItemsList.Add(a);
            _currentProductItemsList.Add(b);
            _currentProductItemsList.Add(c);
            _currentProductItemsList.Add(d);
        }
        #endregion

        #region Observable Properties

        public ObservableCollection<Product> CurrentCartProducts
        {
            get { return _currentProductItemsList; }
            set
            {
                _currentProductItemsList = value;
                OnPropertyChanged("CurrentProductItemsList");
            }
        }

        /// <summary>
        /// Holds current page
        /// </summary>
        public string CurrentPage
        {
            get { return _currentPage; }
            set
            {
                _currentPage = value;
                OnPropertyChanged("CurrentPage");
            }
        }

        #endregion

        #region Methods

        void AddNewItemToInventory()
        {

        }

        void ModifyItemFromIventory()
        {

        }

        void DeleteItemFromIventory()
        {

        }

        void ModifyItemFromInventory(object parameter)
        {

        }

        #endregion

        #region Commands

        #region ChangePageCommand

        public ICommand ChangePageCommand { get { return _changePageCommand ?? (_changePageCommand = new DelegateCommand(Execute_ChangePageCommand, CanExecute_ChangePageCommand)); } }
        private ICommand _changePageCommand;

        internal void Execute_ChangePageCommand(object parameter)
        {
            //Change main frame page based on the parameter
            switch ((string)parameter)
            {
                case "inventory_details":
                    CurrentPage = "\\View\\InventoryItemPage.xaml";
                    break;
                case "inventory":
                    CurrentPage = "\\View\\InventoryMainPage.xaml";
                    break;
            }
        }

        internal bool CanExecute_ChangePageCommand(object parameter)
        {
            return true;
        }
        #endregion

        #endregion  

    }
}
