using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Seiya.WpfBindingUtilities;

namespace Seiya
{
    /// <summary>
    /// Class for the product list page where the items are selected
    /// </summary>
    public class ProductsPageViewModel : ObservableObject
    {
        #region Fields

        private static ObservableCollection<string> _products;

        #endregion

        #region Constructors

        public ProductsPageViewModel()
        {
            var temp = CategoryCatalog.GetList(Constants.DataFolderPath + Constants.ProductPageList);
            _products = new ObservableCollection<string>(temp);
        }  

        #endregion

        #region Observable Properties

        public ObservableCollection<string> Products
        {
            get { return _products; }
            set { _products = value; OnPropertyChanged("Products");}
        }

        #endregion

        #region SelectItemCommand

        public ICommand SelectItemCommand { get { return _selectItemCommand ?? (_selectItemCommand = new DelegateCommand(Execute_SelectItemCommand, CanExecute_SelectItemCommand)); } }
        private ICommand _selectItemCommand;

        internal void Execute_SelectItemCommand(object parameter)
        {
            //TODO: Check to make sure the item is found, otherwise show error message
            var product = MainWindowViewModel.InventoryInstance.GetProduct(parameter.ToString());
            product.LastQuantitySold = 1;
            MainWindowViewModel.AddManualProductToCart(product);
        }

        internal bool CanExecute_SelectItemCommand(object parameter)
        {
            return parameter != null;
        }

        #endregion

        #region LoadProductsListCommand

        public ICommand LoadProductsListCommand { get { return _loadProductsListCommand ?? (_loadProductsListCommand = new DelegateCommand(Execute_LoadProductsListCommand, CanExecute_LoadProductsListCommand)); } }
        private ICommand _loadProductsListCommand;

        internal void Execute_LoadProductsListCommand(object parameter)
        {
            //Re-loads the list of products to be shown in the page
            var temp = CategoryCatalog.GetList(Constants.DataFolderPath + Constants.ProductPageList);
            _products = new ObservableCollection<string>(temp);
        }

        internal bool CanExecute_LoadProductsListCommand(object parameter)
        {
            //Add logic to check if the command can be executed (if any)
            return true;
        }
        #endregion

    }
}
