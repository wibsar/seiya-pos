using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Seiya.WpfBindingUtilities;
using System.Windows.Interactivity;

namespace Seiya
{
    public class MainWindowViewModel : ObservableObject
    {
        #region Fields

        private ObservableCollection<Product> _cartProducts;
        private static Product _selectedCartProduct;
        private Product _currentProduct;

        private static Inventory _inventory = null;
        private static MainWindowViewModel _appInstance = null;

        private string _currentPage;
        private int _currentCartNumber;
        private string _code;
        private string _checkoutTotal = "2";



        //Hold cart product lists
        private static ObservableCollection<Product> _cartOneProducts = new ObservableCollection<Product>();
        private static ObservableCollection<Product> _cartTwoProducts = new ObservableCollection<Product>();
        private static ObservableCollection<Product> _cartThreeProducts = new ObservableCollection<Product>();
        //Holds active cart product list
        private static ObservableCollection<Product> _currentCartProducts = new ObservableCollection<Product>()
        {
//            Product.Add("Arturo", "Cat1", 50M, 4),
//            Product.Add("Yadis", "Cat1", 100M, 1),
//            Product.Add("Prueba", "prueba2", 1M, 60)
        };
        #endregion

        #region Constructors

        private MainWindowViewModel()
        {
            //Initialize current cart
            CurrentCartNumber = 1;
            //Initialize inventory and Pos data files
            _inventory = new Inventory(Constants.DataFolderPath + Constants.InventoryFileName);
        }

        public static MainWindowViewModel GetInstance()
        {
            if (_appInstance == null)
                _appInstance = new MainWindowViewModel();
            return _appInstance;
        }

        #endregion

        #region Observable Properties
        
        /// <summary>
        /// Hold active cart items list
        /// </summary>
        public ObservableCollection<Product> CurrentCartProducts
        {
            get { return _currentCartProducts; }
            set
            {
                _currentCartProducts = value;
                OnPropertyChanged("CurrentCartProducts");
            }
        }

        /// <summary>
        /// Holds currently selected item
        /// </summary>
        public Product SelectedCartProduct
        {
            get { return _selectedCartProduct; }
            set
            {
                _selectedCartProduct = value;
                OnPropertyChanged("SelectedCartProduct");
            }
        }

        /// <summary>
        /// Holds the active cart number
        /// </summary>
        public int CurrentCartNumber
        {
            get { return _currentCartNumber; }
            set
            {
                _currentCartNumber = value;
                OnPropertyChanged("CurrentCartNumber");
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
                OnPropertyChanged("CurrentPage"); OnPropertyChanged("Description");
            }
        }

        public string Code
        {
            get { return _code; }
            set
            {
                _code = value;
                OnPropertyChanged("Code");
                var product = _inventory.GetProduct(_code);
                if (product.Code != "")
                {
                    AddProductToCart(product);
                }
            }
        }

        public string CheckoutTotal
        {
            get { return _checkoutTotal; }
            set
            {
                _checkoutTotal = value;
                OnPropertyChanged("CheckoutTotal");
            }
        }

        #endregion

        public static Inventory InventoryInstance
        {
            get { return _inventory; }
        }

        #region Controls

        #region ChangePageCommand

        public ICommand ChangePageCommand { get { return _changePageCommand ?? (_changePageCommand = new DelegateCommand(Execute_ChangePageCommand, CanExecute_ChangePageCommand)); } }
        private ICommand _changePageCommand;

        internal void Execute_ChangePageCommand(object parameter)
        {
            //Change main frame page based on the parameter
            switch ((string)parameter)
            {
                case "general":
                    CurrentPage = "\\View\\PosGeneralPage.xaml";
                    break;
                case "product_list":
                    CurrentPage = "\\View\\ProductsPage.xaml";
                    break;
                case "menu":
                    CurrentPage = "\\View\\PosMenuPage.xaml";
                    break;
                case "inventory":
                    CurrentPage = "\\View\\InventoryMainPage.xaml";
                    break;
                case "sales_report":
                    CurrentPage = "\\View\\EndSalesPage.xaml";
                    break;
                case "analysis":
                    CurrentPage = "";
                    break;
                case "system":
                    CurrentPage = "";
                    break;
                case "returns":
                    CurrentPage = "";
                    break;
                case "clients":
                    CurrentPage = "";
                    break;
                case "vendors":
                    CurrentPage = "";
                    break;
                case "users":
                    CurrentPage = "";
                    break;
                case "orders":
                    CurrentPage = "";
                    break;
                case "calculator":
                    CurrentPage = "";
                    break;
                case "users_guide":
                    CurrentPage = "";
                    break;
                case "support":
                    CurrentPage = "";
                    break;
                case "information":
                    CurrentPage = "";
                    break;
            }
        }

        internal bool CanExecute_ChangePageCommand(object parameter)
        {
            return true;
        }
        #endregion

        #region CartNumberClickCommand
        private ICommand _cartNumberClickCommand;
        public ICommand CartNumberClickCommand { get { return _cartNumberClickCommand ?? (_cartNumberClickCommand = new DelegateCommand(Execute_CartNumberClickCommand, CanExecute_CartNumberClickCommand)); } }


        internal void Execute_CartNumberClickCommand(object parameter)
        {
            switch ((string)parameter)
            {
                case "1":
                    CurrentCartNumber = 1;
                    //Update button colors  
                    CurrentCartProducts = _cartOneProducts;
                    break;
                case "2":
                    CurrentCartNumber = 2;
                    CurrentCartProducts = _cartTwoProducts;
                    break;
                case "3":
                    CurrentCartNumber = 3;
                    CurrentCartProducts = _cartThreeProducts;
                    break;
            }
        }

        internal bool CanExecute_CartNumberClickCommand(object parameter)
        {
            var cartNumber = 0;
            Int32.TryParse((string)parameter, out cartNumber);
            if (cartNumber == CurrentCartNumber)
            {
                return false;
            }
            return true;
        }
        #endregion

        #region SampleClickCommand
        public ICommand ClickCommand { get { return _clickCommand ?? (_clickCommand = new DelegateCommand(Execute_ClickCommand, CanExecute_ClickCommand));} }
        private ICommand _clickCommand;

        internal void Execute_ClickCommand(object parameter)
        {
            CurrentPage = "\\View\\PosGeneralPage.xaml";
        }
        internal bool CanExecute_ClickCommand(object parameter)

        {
            return true;
        }
        #endregion

        #region StartCheckoutCommand
        public ICommand StartCheckoutCommand { get { return _startCheckoutCommand ?? (_startCheckoutCommand = new DelegateCommand(Execute_StartCheckoutCommand, CanExecute_StartCheckoutCommand)); } }
        private ICommand _startCheckoutCommand;

        internal void Execute_StartCheckoutCommand(object parameter)
        {
            var status = ProcessPayment();

            if (status)
            {
                RecordTransaction();
                UpdateInventory();
                PrintReceipt();
                CurrentCartProducts.Clear();
                
            }
        }
        internal bool CanExecute_StartCheckoutCommand(object parameter)
        {
            return _currentCartProducts.Count > 0;
        }
        #endregion

        #region SearchCodeCommand
        public ICommand SearchCodeCommand { get { return _searchCodeCommand ?? (_searchCodeCommand = new DelegateCommand(Execute_SearchCodeCommand, CanExecute_SearchCodeCommand)); } }
        private ICommand _searchCodeCommand;

        internal void Execute_SearchCodeCommand(object parameter)
        {
            var product = _inventory.GetProduct("test");
        }
        internal bool CanExecute_SearchCodeCommand(object parameter)
        {
            return true;
        }
        #endregion

        #region SubtractFromProductListCommand
        public ICommand SubtractFromProductListCommand { get { return _subtractFromProductListCommand ?? (_subtractFromProductListCommand = new DelegateCommand(Execute_SubtractFromProductListCommand, CanExecute_SubtractFromProductListCommand)); } }
        private ICommand _subtractFromProductListCommand;

        internal void Execute_SubtractFromProductListCommand(object parameter)
        {
            var index = CurrentCartProducts.IndexOf((Product)parameter);
            var activeProduct = (Product)parameter;
            CurrentCartProducts.RemoveAt(index);
            activeProduct.LastQuantitySold -= 1;
            if (activeProduct.LastQuantitySold > 0)
            {
                CurrentCartProducts.Insert(index, activeProduct);
                SelectedCartProduct = activeProduct;
            }
        }
        internal bool CanExecute_SubtractFromProductListCommand(object parameter)
        {
            return parameter != null;
        }
        #endregion

        #region AddToProductListCommand
        public ICommand AddToProductListCommand { get { return _addToProductListCommand ?? (_addToProductListCommand = new DelegateCommand(Execute_AddToProductListCommand, CanExecute_AddToProductListCommand)); } }
        private ICommand _addToProductListCommand;

        internal void Execute_AddToProductListCommand(object parameter)
        {
            var index = CurrentCartProducts.IndexOf((Product)parameter);
            var activeProduct = (Product)parameter;
            CurrentCartProducts.RemoveAt(index);
            activeProduct.LastQuantitySold += 1;
            CurrentCartProducts.Insert(index, activeProduct);
            SelectedCartProduct = activeProduct;
        }
        internal bool CanExecute_AddToProductListCommand(object parameter)
        {
            return parameter != null;
        }

        #endregion

        #region ApplyDiscountToProductListCommand

        public ICommand ApplyDiscountToProductListCommand { get { return _applyDiscountToProductListCommand ?? (_applyDiscountToProductListCommand = new DelegateCommand(Execute_ApplyDiscountToProductListCommand, CanExecute_ApplyDiscountToProductListCommand)); } }
        private ICommand _applyDiscountToProductListCommand;

        internal void Execute_ApplyDiscountToProductListCommand(object parameter)
        {
            var discountPercentage = 10M;
            var index = CurrentCartProducts.IndexOf((Product)parameter);
            var activeProduct = (Product)parameter;
            CurrentCartProducts.RemoveAt(index);
            activeProduct.Price = activeProduct.Price*(1 - discountPercentage/100);
            CurrentCartProducts.Insert(index, activeProduct);
            SelectedCartProduct = activeProduct;
        }
        internal bool CanExecute_ApplyDiscountToProductListCommand(object parameter)
        {
            return parameter != null;
        }

        #endregion

        #endregion

        #region Methods

        #region CartMethods

        public void AddProductToCart(Product product)
        {
            CurrentCartProducts.Add(product);
        }

        /// <summary>
        /// Method to add a product to a cart
        /// </summary>
        /// <param name="product"></param>
        public static void AddManualProductToCart(Product product)
        {
            _currentCartProducts.Add(product);
        }
        
        public void RemoveProductFromCart(Product product)
        {
            if (CurrentCartProducts.Contains(product))
            {
                CurrentCartProducts.Remove(product);
            }
        }

        public void AddOneAdditinoalQuantityToProductInCart(Product product, int cartIndex)
        {
            var productIndex = CurrentCartProducts.IndexOf(product);
            product.LastQuantitySold = product.LastQuantitySold + 1;
            CurrentCartProducts.RemoveAt(productIndex);
            CurrentCartProducts.Insert(productIndex,product);
        }

        public static void GetCurrentSelectedCart()
        {
            
        }

        #endregion

        #region CheckOutProcessMethods

        /// <summary>
        /// Method to initialize payment page and make payment
        /// </summary>
        /// <returns></returns>
        bool ProcessPayment()
        {
            return true;
        }

        /// <summary>
        /// Method to record transaction
        /// </summary>
        /// <returns></returns>
        bool RecordTransaction()
        {
            return true;
        }

        /// <summary>
        /// Method to update inventory after a transaction is sucessful
        /// </summary>
        /// <returns></returns>
        bool UpdateInventory()
        {
            return true;
        }

        /// <summary>
        /// Method to print receipt of the transaction
        /// </summary>
        /// <returns></returns>
        bool PrintReceipt()
        {
            return true;
        }

        #endregion

        

        #endregion
    }
}
