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
//using System.Windows.Interactivity;
using System.Windows.Media.Imaging;
using System.IO;

namespace Seiya
{
    public class MainWindowViewModel : ObservableObject
    {
        #region Fields
        //Main instances
        private static Inventory _inventoryInstance = null;
        private static MainWindowViewModel _appInstance = null;
        private static Pos _posInstance = null;
        private static User _userInstance = null;
        private static Expense _expenseInstance = null;


        //Products page list related fields
        private static ObservableCollection<string> _products;
        private static string _pageOneTitle;
        private static string _pageTwoTitle;
        private static string _pageThreeTitle;
        private static string _pageFourTitle;
        private static string _pageFiveTitle;
        private static string _currentPageListTitle;
        private static ObservableCollection<Product> _currentPageListProducts = new ObservableCollection<Product>();
        private static string _currentSelectedItemsListPageFileName;
        private static Product _selectedPageListProduct;
        private static int _lastSelectedProductsPage = 1;
        public ObservableCollection<Product> _productObjects;

        //Navegation related fields
        private string _currentPage;

        private string _code;
        private string _checkoutTotal;// = "2";

        //Carts related fields
        private ObservableCollection<Product> _cartProducts;
        private static Product _selectedCartProduct;
        private int _currentCartNumber;
        private static ObservableCollection<Product> _cartOneProducts = new ObservableCollection<Product>();
        private static ObservableCollection<Product> _cartTwoProducts = new ObservableCollection<Product>();
        private static ObservableCollection<Product> _cartThreeProducts = new ObservableCollection<Product>();

        //Payment related fields
        private decimal _paymentTotalUSD;
        private decimal _paymentTotalMXN;
        private decimal _paymentReceivedUSD;
        private decimal _paymentReceivedMXN;
        private int _paymentCustomerNumber;
        private decimal _paymentChangeMXN;
        private decimal _paymentChangeUSD;
        private decimal _exchangeRate = 18; //TODO: Implement exchange rate based on the er set by the user
        private string _exchangeRateString;
        //Inventory Related Fields
        private Product _inventoryTemporalItem;



        //Holds active cart product list
        private static ObservableCollection<Product> _currentCartProducts = new ObservableCollection<Product>()
        {

        };

        #endregion

        #region Constructors

        private MainWindowViewModel()
        {
            //Initialize current cart and list number
            CurrentCartNumber = 1;
            CurrentCartProducts = _cartOneProducts;
            //Initialize inventory and Pos data files
            _inventoryInstance = Inventory.GetInstance(Constants.DataFolderPath + Constants.InventoryFileName);
            _posInstance = Pos.GetInstance(Constants.DataFolderPath + Constants.PosDataFileName);
            //Page Titles
            GetInitialPagesTitles();
            //Set default page
            CurrentPage = "\\View\\LoginPage.xaml";
        }

        public static MainWindowViewModel GetInstance()
        {
            if (_appInstance == null)
                _appInstance = new MainWindowViewModel();
            return _appInstance;
        }

        #endregion

        #region Properties
        public Product InventoryTemporalItem
        {
            get
            {
                return _inventoryTemporalItem;
            }
            set
            {
                _inventoryTemporalItem = value;
                OnPropertyChanged("InventoryTemporalItem");
            }
        }

        public static Inventory InventoryInstance
        {
            get { return _inventoryInstance; }
        }

        //Holds last selected products page
        public static int LastSelectedProductsPage
        {
            get
            {
                return _lastSelectedProductsPage;
            }
            set
            {
                _lastSelectedProductsPage = value;
            }
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
                PaymentTotalMXN = calculateCurrentCartTotal();
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
        /// Holds currently selected item for the product page list
        /// </summary>
        public Product SelectedPageListProduct
        {
            get { return _selectedPageListProduct; }
            set
            {
                _selectedPageListProduct = value;
                OnPropertyChanged("SelectedPageListProduct");
            }
        }

        /// <summary>
        /// Hold active cart items list
        /// </summary>
        public ObservableCollection<Product> CurrentPageListProducts
        {
            get { return _currentPageListProducts; }
            set
            {
                _currentPageListProducts = value;
                OnPropertyChanged("CurrentPageListProducts");
            }
        }
        
        /// <summary>
        /// Holds active products page title
        /// </summary>
        public string CurrentPageListTitle
        {
            get { return _currentPageListTitle; }
            set
            {
                _currentPageListTitle = value;
                OnPropertyChanged("CurrentPageListTitle");
            }
        }

        /// <summary>
        /// Holds active products page title
        /// </summary>
        public string CurrentSelectedItemsListPageFileName
        {
            get { return _currentSelectedItemsListPageFileName; }
            set
            {
                _currentSelectedItemsListPageFileName = value;
                OnPropertyChanged("CurrentSelectedItemsListPageFileName");
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
                OnPropertyChanged("CurrentPage");
            }
        }

        public string Code
        {
            get { return _code; }
            set
            {
                var product = _inventoryInstance.GetProduct(value + "x");
                if (product.Code != null)
                {
                    product.LastQuantitySold = 1;
                    AddProductToCart(product);
                    _code = string.Empty;
                }
                else
                    _code = value;
                OnPropertyChanged("Code");
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

        public string CheckoutTotalUSD
        {
            //TODO: Add Exchange rate from POS info
            get { return _checkoutTotal; }
            set
            {
                _checkoutTotal = value;
                OnPropertyChanged("CheckoutTotal");
            }
        }

        public ObservableCollection<string> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                OnPropertyChanged("Products");
            }
        }

        public ObservableCollection<Product> ProductObjects
        {
            get { return _productObjects; }
            set
            {
                _productObjects = value;
                OnPropertyChanged("ProductObjects");
            }
        }

        public decimal PaymentTotalUSD
        {
            get { return _paymentTotalUSD; }
            set
            {
                _paymentTotalUSD = value;
                OnPropertyChanged("PaymentTotalUSD");
            }
        }

        public decimal PaymentTotalMXN
        {
            get
            {
                return _paymentTotalMXN;
            }
            set
            {
                _paymentTotalMXN = value;
                PaymentTotalUSD = _paymentTotalMXN / _exchangeRate;
                OnPropertyChanged("PaymentTotalMXN");
            }
        }

        public decimal PaymentReceivedUSD
        {
            get { return _paymentReceivedUSD; }
            set
            {
                _paymentReceivedUSD = value;
                OnPropertyChanged("PaymentReceivedUSD");
            }
        }

        public decimal PaymentReceivedMXN
        {
            get { return _paymentReceivedMXN; }
            set
            {
                _paymentReceivedMXN = value;
                OnPropertyChanged("PaymentReceivedMXN");
            }
        }

        public decimal PaymentChangeMXN
        {
            get { return _paymentChangeMXN; }
            set
            {
                _paymentChangeMXN = value;
                OnPropertyChanged("PaymentChangeMXN");
            }
        }

        public decimal PaymentChangeUSD
        {
            get { return _paymentChangeUSD; }
            set
            {
                _paymentChangeUSD = value;
                OnPropertyChanged("PaymentChangeUSD");
            }
        }

        public int PaymentCustomerNumber
        {
            get { return _paymentCustomerNumber; }
            set
            {
                _paymentCustomerNumber = value;
                OnPropertyChanged("PaymentCustomerNumber");
            }
        }

        #region Products Page Titles

        public string PageOneTitle
        {
            get
            {
                return _pageOneTitle;
            }
            set
            {
                _pageOneTitle = value;
                OnPropertyChanged("PageOneTitle");
            }
        }

        public string PageTwoTitle
        {
            get
            {
                return _pageTwoTitle;
            }
            set
            {
                _pageTwoTitle = value;
                OnPropertyChanged("PageTwoTitle");
            }
        }

        public string PageThreeTitle
        {
            get
            {
                return _pageThreeTitle;
            }
            set
            {
                _pageThreeTitle = value;
                OnPropertyChanged("PageThreeTitle");
            }
        }

        public string PageFourTitle
        {
            get
            {
                return _pageFourTitle;
            }
            set
            {
                _pageFourTitle = value;
                OnPropertyChanged("PageFourTitle");
            }
        }

        public string PageFiveTitle
        {
            get
            {
                return _pageFiveTitle;
            }
            set
            {
                _pageFiveTitle = value;
                OnPropertyChanged("PageFiveTitle");
            }
        }

        #endregion

        #region Inventory Related Properties

        private BitmapImage _productImage;
        public BitmapImage ProductImage
        {
            get
            {
                //return SelectedInventoryProduct == null ? null : SelectedInventoryProduct.Image;
                return InventoryTemporalItem == null ? null : InventoryTemporalItem.Image;
            }
            set
            {
                _productImage = value;
                OnPropertyChanged("ProductImage");
            }
        }

        private string _inventorySearchText;
        public string InventorySearchText
        {
            get
            {
                return _inventorySearchText;
            }
            set
            {
                _inventorySearchText = value.ToLower();
                OnPropertyChanged("InventorySearchText");
            }
        }

        /// <summary>
        /// Hold active cart items list
        /// </summary>
        /// 
        private ObservableCollection<Product> _inventorySearchedProducts;

        public ObservableCollection<Product> InventorySearchedProducts
        {
            get { return _inventorySearchedProducts; }
            set
            {
                _inventorySearchedProducts = value;
                OnPropertyChanged("InventorySearchedProducts");
            }
        }

        private Product _selectedInventoryProduct;
        public Product SelectedInventoryProduct
        {
            get { return _selectedInventoryProduct; }
            set
            {
                _selectedInventoryProduct = value;
                OnPropertyChanged("SelectedInventoryProduct");
            }
        }
        #endregion

        #region Vendor Related Properties
        private Vendor _vendorTemporalItem;
        public Vendor VendorTemporalItem
        {
            get
            {
                return _vendorTemporalItem;
            }
            set
            {
                _vendorTemporalItem = value;
                OnPropertyChanged("VendorTemporalItem");
            }
        }

        private string _vendorsSearchText;
        public string VendorsSearchText
        {
            get
            {
                return _vendorsSearchText;
            }
            set
            {
                _vendorsSearchText = value.ToLower();
                OnPropertyChanged("VendorsSearchText");
            }
        }

        /// <summary>
        /// Hold active cart items list
        /// </summary>
        /// 
        private ObservableCollection<Vendor> _vendorsSearchedEntries;

        public ObservableCollection<Vendor> VendorsSearchedEntries
        {
            get { return _vendorsSearchedEntries; }
            set
            {
                _vendorsSearchedEntries = value;
                OnPropertyChanged("VendorsSearchedEntries");
            }
        }

        private Vendor _selectedVendor;
        public Vendor SelectedVendor
        {
            get { return _selectedVendor; }
            set
            {
                _selectedVendor = value;
                OnPropertyChanged("SelectedVendor");
            }
        }

        #endregion

        #region Users Related Properties
        private User _userTemporalItem;
        public User UserTemporalItem
        {
            get
            {
                return _userTemporalItem;
            }
            set
            {
                _userTemporalItem = value;
                OnPropertyChanged("UserTemporalItem");
            }
        }

        private string _usersSearchText;
        public string UsersSearchText
        {
            get
            {
                return _usersSearchText;
            }
            set
            {
                _usersSearchText = value.ToLower();
                OnPropertyChanged("UsersSearchText");
            }
        }

        /// <summary>
        /// Hold active cart items list
        /// </summary>
        /// 
        private ObservableCollection<User> _usersSearchedEntries;

        public ObservableCollection<User> UsersSearchedEntries
        {
            get { return _usersSearchedEntries; }
            set
            {
                _usersSearchedEntries = value;
                OnPropertyChanged("UsersSearchedEntries");
            }
        }

        private User _selectedUser;
        public User SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                OnPropertyChanged("SelectedUser");
            }
        }

        #endregion

        #region Customers Related Properties
        private Customer _customerTemporalItem;
        public Customer CustomerTemporalItem
        {
            get
            {
                return _customerTemporalItem;
            }
            set
            {
                _customerTemporalItem = value;
                OnPropertyChanged("CustomerTemporalItem");
            }
        }

        private string _customersSearchText;
        public string CustomersSearchText
        {
            get
            {
                return _customersSearchText;
            }
            set
            {
                _customersSearchText = value.ToLower();
                OnPropertyChanged("CustomersSearchText");
            }
        }

        /// <summary>
        /// Hold active cart items list
        /// </summary>
        /// 
        private ObservableCollection<Customer> _customersSearchedEntries;

        public ObservableCollection<Customer> CustomersSearchedEntries
        {
            get { return _customersSearchedEntries; }
            set
            {
                _customersSearchedEntries = value;
                OnPropertyChanged("CustomersSearchedEntries");
            }
        }

        private Customer _selectedCustomer;
        public Customer SelectedCustomer
        {
            get { return _selectedCustomer; }
            set
            {
                _selectedCustomer = value;
                OnPropertyChanged("SelectedCustomer");
            }
        }

        #endregion

        #region Expenses Related Properties
        private Expense _expenseTemporalItem;
        public Expense ExpenseTemporalItem
        {
            get
            {
                return _expenseTemporalItem;
            }
            set
            {
                _expenseTemporalItem = value;
                OnPropertyChanged("ExpenseTemporalItem");
            }
        }

        private string _expensesSearchText;
        public string ExpensesSearchText
        {
            get
            {
                return _expensesSearchText;
            }
            set
            {
                _expensesSearchText = value.ToLower();
                OnPropertyChanged("ExpensesSearchText");
            }
        }

        /// <summary>
        /// Hold active cart items list
        /// </summary>
        /// 
        private ObservableCollection<Expense> _expensesSearchedEntries;

        public ObservableCollection<Expense> ExpensesSearchedEntries
        {
            get { return _expensesSearchedEntries; }
            set
            {
                _expensesSearchedEntries = value;
                OnPropertyChanged("ExpensesSearchedEntries");
            }
        }

        private Expense _selectedExpense;
        public Expense SelectedExpense
        {
            get { return _selectedExpense; }
            set
            {
                _selectedExpense = value;
                OnPropertyChanged("SelectedExpense");
            }
        }

        #endregion

        #region Exchange Rate Related Properties

        public decimal ExchangeRate
        {
            get
            {
                return _exchangeRate;
            }
            set
            {
                _exchangeRate = value;
                OnPropertyChanged("ExchangeRate");
            }
        }

        public string ExchangeRateString
        {
            get
            {
                return "$ " + _posInstance.ExchangeRate.ToString();
            }
            set
            {
                _exchangeRateString = value;
                OnPropertyChanged("ExchangeRateString");
            }
        }
        #endregion


        #endregion

        #region Commands

        #region ChangePageCommand

        public ICommand ChangePageCommand { get { return _changePageCommand ?? (_changePageCommand = new DelegateCommand(Execute_ChangePageCommand, CanExecute_ChangePageCommand)); } }
        private ICommand _changePageCommand;

        internal void Execute_ChangePageCommand(object parameter)
        {
            string pageTitleHolder;
            //Change main frame page based on the parameter
            switch ((string)parameter)
            {
                case "general":
                    CurrentPage = "\\View\\PosGeneralPage.xaml";
                    break;
                case "payment":
                    CurrentPage = "\\View\\PaymentPage.xaml";
                    break;
                case "product_list_1":
                    LastSelectedProductsPage = 1;
               //     Products = GetPageItemsList(LastSelectedProductsPage, out pageTitleHolder);
                    ProductObjects = GetPageProductsList(LastSelectedProductsPage, out pageTitleHolder);
                    PageOneTitle = pageTitleHolder;
                    CurrentPage = "\\View\\ProductsPage.xaml";
                    break;
                case "product_list_2":
                    LastSelectedProductsPage = 2;
                 //   Products = GetPageItemsList(LastSelectedProductsPage, out pageTitleHolder);
                    ProductObjects = GetPageProductsList(LastSelectedProductsPage, out pageTitleHolder);
                    PageTwoTitle = pageTitleHolder;
                    CurrentPage = "\\View\\ProductsPage.xaml";
                    break;
                case "product_list_3":
                    LastSelectedProductsPage = 3;
                   // Products = GetPageItemsList(LastSelectedProductsPage, out pageTitleHolder);
                    ProductObjects = GetPageProductsList(LastSelectedProductsPage, out pageTitleHolder);
                    PageThreeTitle = pageTitleHolder;
                    CurrentPage = "\\View\\ProductsPage.xaml";
                    break;
                case "product_list_4":
                    LastSelectedProductsPage = 4;
                   // Products = GetPageItemsList(LastSelectedProductsPage, out pageTitleHolder);
                    ProductObjects = GetPageProductsList(LastSelectedProductsPage, out pageTitleHolder);
                    PageFourTitle = pageTitleHolder;
                    CurrentPage = "\\View\\ProductsPage.xaml";
                    break;
                case "product_list_5":
                    LastSelectedProductsPage = 5;
                    //Products = GetPageItemsList(LastSelectedProductsPage, out pageTitleHolder);
                    ProductObjects = GetPageProductsList(LastSelectedProductsPage, out pageTitleHolder);
                    PageFiveTitle = pageTitleHolder;
                    CurrentPage = "\\View\\ProductsPage.xaml";
                    break;
                case "menu":
                    CurrentPage = "\\View\\PosMenuPage.xaml";
                    break;
                case "inventory":
                case "inventory_cancel":
                    CurrentPage = "\\View\\InventoryMainPage.xaml";
                    SelectedInventoryProduct = null;
                    break;
                case "sales_report":
                    CurrentPage = "\\View\\EndSalesPage.xaml";
                    break;
                case "product_list":
                    CurrentPage = "\\View\\ProductsListEditPage.xaml";
                    break;
                case "inventory_add":
                    CurrentPage = "\\View\\InventoryItemPage.xaml";
                    SelectedInventoryProduct = null;
                    break;
                case "inventory_details":
                    CurrentPage = "\\View\\InventoryItemPage.xaml";
                    break;
                case "system":
                    CurrentPage = "\\View\\SystemPage.xaml";
                    break;
                case "customers":
                    CurrentPage = "\\View\\CustomerMainPage.xaml";
                    break;
                case "vendors":
                    CurrentPage = "\\View\\VendorMainPage.xaml";
                    break;
                case "users":
                    CurrentPage = "\\View\\UserMainPage.xaml";
                    break;
                case "orders":
                    CurrentPage = "\\View\\OrderPage.xaml";
                    break;
                case "exchange_rate":
                    CurrentPage = "\\View\\ExchangeRatePage.xaml";
                    break;
                case "calculator":
                    CurrentPage = "\\View\\PosGeneralPage.xaml";
                    System.Diagnostics.Process.Start("Calc.exe");
                    break;
                case "users_guide":
                    CurrentPage = "\\View\\PosGeneralPage.xaml";
                    System.Diagnostics.Process.Start(@"C:\Projects\seiya-pos\Resources\UsersGuide\GuiaUsuario.pdf");
                    break;
                case "support":
                    CurrentPage = "";
                    break;
                case "more_options":
                    CurrentPage = "";
                    break;
                case "expenses":
                    CurrentPage = "\\View\\ExpenseMainPage.xaml";
                    break;
                case "returns":
                    CurrentPage = "\\View\\ReturnsPage.xaml";
                    break;
                case "login":
                    CurrentPage = "\\View\\LoginPage.xaml";
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
                if(ProcessPayment())                
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
            var product = _inventoryInstance.GetProduct((string)parameter + "x");
            if (product.Code != null)
            {
                product.LastQuantitySold = 1;
                AddProductToCart(product);
            }
                
            //TODO: Turn red if it is not found

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
            PaymentTotalMXN = calculateCurrentCartTotal();
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
            PaymentTotalMXN = calculateCurrentCartTotal();
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
            PaymentTotalMXN = calculateCurrentCartTotal();
        }
        internal bool CanExecute_ApplyDiscountToProductListCommand(object parameter)
        {
            return parameter != null;
        }

        #endregion

        #region ClearCurrentProductListCommand

        public ICommand ClearCurrentProductListCommand { get { return _clearCurrentProductListCommand ?? (_clearCurrentProductListCommand = new DelegateCommand(Execute_ClearCurrentProductListCommand, CanExecute_ClearCurrentProductListCommand)); } }
        private ICommand _clearCurrentProductListCommand;

        internal void Execute_ClearCurrentProductListCommand(object parameter)
        {
            CurrentCartProducts.Clear();
            PaymentTotalMXN = 0M;
        }
        internal bool CanExecute_ClearCurrentProductListCommand(object parameter)
        {
            return CurrentCartProducts.Count > 0;
        }

        #endregion

        #region SelectProductsListCommand
        private ICommand _selectProductsListCommand;
        public ICommand SelectProductsListCommand { get { return _selectProductsListCommand ?? (_selectProductsListCommand = new DelegateCommand(Execute_SelectProductsListCommand, CanExecute_SelectProductsListCommand)); } }

        internal void Execute_SelectProductsListCommand(object parameter)
        {
            var currentProductListTitle = string.Empty;
            List<Product> items;

            switch ((string)parameter)
            {
                case "1":
                    items = GetPageProductsList(1, out currentProductListTitle).ToList();
                    LastSelectedProductsPage = 1;
                    break;
                case "2":
                    items = GetPageProductsList(2, out currentProductListTitle).ToList();
                    LastSelectedProductsPage = 2;
                    break;
                case "3":
                    items = GetPageProductsList(3, out currentProductListTitle).ToList();
                    LastSelectedProductsPage = 3;
                    break;
                case "4":
                    items = GetPageProductsList(4, out currentProductListTitle).ToList();
                    LastSelectedProductsPage = 4;
                    break;
                case "5":
                    items = GetPageProductsList(5, out currentProductListTitle).ToList();
                    LastSelectedProductsPage = 5;
                    break;
            }
        }

        internal bool CanExecute_SelectProductsListCommand(object parameter)
        {
            return true;
        }
        #endregion

        #region SelectItemCommand

        public ICommand SelectItemCommand { get { return _selectItemCommand ?? (_selectItemCommand = new DelegateCommand(Execute_SelectItemCommand, CanExecute_SelectItemCommand)); } }
        private ICommand _selectItemCommand;

        internal void Execute_SelectItemCommand(object parameter)
        {
            //TODO: Check to make sure the item is found, otherwise show error message
            //Create a new object for every product 
            var product = new Product();
            product = (Product)parameter;
            product.LastQuantitySold = 1;
            AddManualProductToCart(product);
        }

        internal bool CanExecute_SelectItemCommand(object parameter)
        {
            return parameter != null;
        }

        #endregion

        #region AddItemToGeneralCommand

        public ICommand AddItemToGeneralCommand { get { return _addItemToGeneralCommand ?? (_addItemToGeneralCommand = new DelegateCommand(Execute_AddItemToGeneralCommand, CanExecute_Execute_AddItemToGeneralCommand)); } }
        private ICommand _addItemToGeneralCommand;

        internal void Execute_AddItemToGeneralCommand(object parameter)
        {
            //TODO: Check to make sure the item is found, otherwise show error message
            //Create a new object for every product 
            var product = new Product();
            product = (Product)parameter;
            product.LastQuantitySold = 1;
            AddManualProductToCart(product);
        }

        internal bool CanExecute_Execute_AddItemToGeneralCommand(object parameter)
        {
            return parameter != null;
        }

        #endregion

        #region PaymentBillClickCommand
        public ICommand PaymentBillClickCommand { get { return _paymentBillClickCommand ?? (_paymentBillClickCommand = new DelegateCommand(Execute_PaymentBillClickCommand, CanExecute_PaymentBillClickCommand)); } }
        private ICommand _paymentBillClickCommand;

        internal void Execute_PaymentBillClickCommand(object parameter)
        {
            switch ((string)parameter)
            {
                case "exact_mxn":
                    PaymentReceivedMXN = PaymentTotalMXN;
                    break;
                case "20_mxn":
                    PaymentReceivedMXN += 20M;
                    break;
                case "50_mxn":
                    PaymentReceivedMXN += 50M;
                    break;
                case "100_mxn":
                    PaymentReceivedMXN += 100M;
                    break;
                case "200_mxn":
                    PaymentReceivedMXN += 200M;
                    break;
                case "500_mxn":
                    PaymentReceivedMXN += 500M;
                    break;
                case "exact_usd":
                    PaymentReceivedUSD = PaymentTotalUSD;
                    break;
                case "5_usd":
                    PaymentReceivedUSD += 5M;
                    break;
                case "10_usd":
                    PaymentReceivedUSD += 10M;
                    break;
                case "20_usd":
                    PaymentReceivedUSD += 20M;
                    break;
                case "50_usd":
                    PaymentReceivedUSD += 50M;
                    break;
                case "100_usd":
                    PaymentReceivedUSD += 100M;
                    break;
            }
        }
        internal bool CanExecute_PaymentBillClickCommand(object parameter)
        {
            return true;
        }
        #endregion

        #region PaymentProcessCommand
        public ICommand PaymentProcessCommand { get { return _paymentProcessCommand ?? (_paymentProcessCommand = new DelegateCommand(Execute_PaymentProcessCommand, CanExecute_PaymentProcessCommand)); } }
        private ICommand _paymentProcessCommand;

        internal void Execute_PaymentProcessCommand(object parameter)
        {
            ProcessPayment();
        }
        internal bool CanExecute_PaymentProcessCommand(object parameter)
        {
            return PaymentTotalMXN != 0M && PaymentTotalMXN <= PaymentReceivedMXN + PaymentReceivedUSD * _exchangeRate; 
        }
        #endregion

        #region Products List Edit Page Commands

        #region MoveUpListItemCommand
        public ICommand MoveUpListItemCommand { get { return _moveUpListItemCommand ?? (_moveUpListItemCommand = new DelegateCommand(Execute_MoveUpListItemCommand, CanExecute_MoveUpListItemCommand)); } }
        private ICommand _moveUpListItemCommand;

        internal void Execute_MoveUpListItemCommand(object parameter)
        {
            var updatedProductList = Utilities.MoveListItemUp(CurrentPageListProducts, (int)parameter);
            CurrentPageListProducts = new ObservableCollection<Product>(updatedProductList);
        }
        internal bool CanExecute_MoveUpListItemCommand(object parameter)
        {
            return (int)parameter >= 0;
        }
        #endregion

        #region MoveDownListItemCommand
        public ICommand MoveDownListItemCommand { get { return _moveDownListItemCommand ?? (_moveDownListItemCommand = new DelegateCommand(Execute_MoveDownListItemCommand, CanExecute_MoveDownListItemCommand)); } }
        private ICommand _moveDownListItemCommand;

        internal void Execute_MoveDownListItemCommand(object parameter)
        {
            var updatedProductList = Utilities.MoveListItemDown(CurrentPageListProducts, (int)parameter);
            CurrentPageListProducts = new ObservableCollection<Product>(updatedProductList);
        }
        internal bool CanExecute_MoveDownListItemCommand(object parameter)
        {
            return (int)parameter >= 0;
        }
        #endregion

        #region DeleteListItemCommand
        public ICommand DeleteListItemCommand { get { return _deleteListItemCommand ?? (_deleteListItemCommand = new DelegateCommand(Execute_DeleteListItemCommand, CanExecute_DeleteListItemCommand)); } }
        private ICommand _deleteListItemCommand;

        internal void Execute_DeleteListItemCommand(object parameter)
        {
            CurrentPageListProducts.RemoveAt((int)parameter);
        }
        internal bool CanExecute_DeleteListItemCommand(object parameter)
        {
            return (int)parameter >= 0;
        }
        #endregion

        #endregion

        #region Inventory Commands

        #region InventorModifyItemCommand

        public ICommand InventorModifyItemCommand { get { return _inventorModifyItemCommand ?? (_inventorModifyItemCommand = new DelegateCommand(Execute_InventorModifyItemCommand, CanExecute_InventorModifyItemCommand)); } }
        private ICommand _inventorModifyItemCommand;

        internal void Execute_InventorModifyItemCommand(object parameter)
        {
            //Change main frame page based on the parameter
            switch ((string)parameter)
            {
                case "inventory_details":
                    //Create new productt
                    var temporalProduct = new Product()
                    {
                        Id = SelectedInventoryProduct.Id,
                        Code = SelectedInventoryProduct.Code,
                        AlternativeCode = SelectedInventoryProduct.AlternativeCode,
                        AmountSold = SelectedInventoryProduct.AmountSold,
                        Category = SelectedInventoryProduct.Category,
                        Cost = SelectedInventoryProduct.Cost,
                        CostCurrency = SelectedInventoryProduct.CostCurrency,
                        Description = SelectedInventoryProduct.Description,
                        Image = SelectedInventoryProduct.Image,
                        ImageName = SelectedInventoryProduct.ImageName,
                        InternalQuantity = SelectedInventoryProduct.InternalQuantity,
                        LastPurchaseDate = SelectedInventoryProduct.LastPurchaseDate,
                        LastQuantitySold = SelectedInventoryProduct.LastQuantitySold,
                        LastSaleDate = SelectedInventoryProduct.LastSaleDate,
                        LocalQuantityAvailable = SelectedInventoryProduct.LocalQuantityAvailable,
                        MinimumStockQuantity = SelectedInventoryProduct.MinimumStockQuantity,
                        Name = SelectedInventoryProduct.Name,
                        Price = SelectedInventoryProduct.Price,
                        PriceCurrency = SelectedInventoryProduct.PriceCurrency,
                        Provider = SelectedInventoryProduct.Provider,
                        ProviderProductId = SelectedInventoryProduct.ProviderProductId,
                        QuantitySold = SelectedInventoryProduct.QuantitySold,
                        TotalQuantityAvailable = SelectedInventoryProduct.QuantitySold,
                        Brand = SelectedInventoryProduct.Brand
                    };

                    CurrentPage = "\\View\\InventoryItemPage.xaml";
                    InventoryTemporalItem = temporalProduct;
                    break;
            }
        }

        internal bool CanExecute_InventorModifyItemCommand(object parameter)
        {
            return SelectedInventoryProduct != null;
        }

        #endregion

        #region InventoryAddNewItemCommand

        public ICommand InventoryAddNewItemCommand { get { return _inventoryAddNewItemCommand ?? (_inventoryAddNewItemCommand = new DelegateCommand(Execute_InventoryAddNewItemCommand, CanExecute_InventoryAddNewItemCommand)); } }
        private ICommand _inventoryAddNewItemCommand;

        internal void Execute_InventoryAddNewItemCommand(object parameter)
        {
            //Change main frame page based on the parameter
            switch ((string)parameter)
            {
                case "inventory_add":
                    SelectedInventoryProduct = null;    //Clear selected item in the inventory
                    //Create new productt
                    var temporalProduct = new Product()
                    {
                        Id = _inventoryInstance.GetLastItemNumber() + 1,
                        Code = "",
                        AlternativeCode = "",
                        AmountSold = 0M,
                        Category = "",
                        Cost = 0M,
                        CostCurrency = "MXN",
                        Description = "",
                        ImageName = "NA.jpg",
                        InternalQuantity = 0,
                        LastPurchaseDate = DateTime.Now,
                        LastQuantitySold = 0,
                        LastSaleDate = DateTime.Now,
                        LocalQuantityAvailable = 0,
                        MinimumStockQuantity = 1,
                        Name = "",
                        Price = 0M,
                        PriceCurrency = "MXN",
                        Provider = "",
                        ProviderProductId = "",
                        QuantitySold = 0,
                        TotalQuantityAvailable = 0,
                        Brand = ""
                    };

                    CurrentPage = "\\View\\InventoryItemPage.xaml";
                    InventoryTemporalItem = temporalProduct;
                    break;
            }
        }

        internal bool CanExecute_InventoryAddNewItemCommand(object parameter)
        {
            return true;
        }

        #endregion

        #region InventoryAddItemToCartCommand

        public ICommand InventoryAddItemToCartCommand { get { return _inventoryAddItemToCartCommand ?? (_inventoryAddItemToCartCommand = new DelegateCommand(Execute_InventoryAddItemToCartCommand, CanExecute_InventoryAddItemToCartCommand)); } }
        private ICommand _inventoryAddItemToCartCommand;

        internal void Execute_InventoryAddItemToCartCommand(object parameter)
        {
            //TODO: Check to make sure the item is found, otherwise show error message
            //Create a new object for every product 
            var product = new Product();
            product = (Product)parameter;
            product.LastQuantitySold = 1;
            AddProductToCart(product);
        }

        internal bool CanExecute_InventoryAddItemToCartCommand(object parameter)
        {
            return parameter != null;
        }

        #endregion

        #region InventoryStartSearchCommand

        public ICommand InventoryStartSearchCommand { get { return _inventoryStartSearchCommand ?? (_inventoryStartSearchCommand = new DelegateCommand(Execute_InventoryStartSearchCommand, CanExecute_InventoryStartSearchCommand)); } }
        private ICommand _inventoryStartSearchCommand;

        internal void Execute_InventoryStartSearchCommand(object parameter)
        {
            //Inventory search method that returns a list of products for the datagrid
            InventorySearchedProducts = new ObservableCollection<Product>(_inventoryInstance.Search(InventorySearchText));
            InventorySearchText = "";
        }
        internal bool CanExecute_InventoryStartSearchCommand(object parameter)
        {
            return true;//InventorySearchText != "" && InventorySearchText != null;
        }

        #endregion

        #region InventoryItemImageCommand

        private ICommand _inventoryItemImageCommand;
        public ICommand InventoryItemImageCommand { get { return _inventoryItemImageCommand ?? (_inventoryItemImageCommand = new DelegateCommand(Execute_InventoryItemCommand, CanExecute_InventoryItemCommand)); } }

        internal void Execute_InventoryItemCommand(object parameter)
        {
            switch (parameter)
            {
                case "add":
                    InventoryTemporalItem.ImageName = SelectImage();
                    ProductImage = InventoryTemporalItem.Image;
                    break;
                case "remove":
                    InventoryTemporalItem.ImageName = "NA.jpg";
                    ProductImage = InventoryTemporalItem.Image;
                    break;
            }
        }
        internal bool CanExecute_InventoryItemCommand(object parameter)
        {
            return true; // SelectedInventoryProduct.Image != null;
        }
        #endregion

        #region InventorySaveChangesCommand
        public ICommand InventorySaveChangesCommand { get { return _inventorySaveChangesCommand ?? (_inventorySaveChangesCommand = new DelegateCommand(Execute_InventorySaveChangesCommand, CanExecute_InventorySaveChangesCommand)); } }
        private ICommand _inventorySaveChangesCommand;

        internal void Execute_InventorySaveChangesCommand(object parameter)
        {
            //Save temporal product changes to actual product by looking for Code

            //Check if code was updated
            if(SelectedInventoryProduct != null)
            {
                _inventoryInstance.UpdateProductToTable(InventoryTemporalItem);
                _inventoryInstance.SaveDataTableToCsv();
            }
            else
            {
                _inventoryInstance.AddNewProductToTable(InventoryTemporalItem);
                _inventoryInstance.SaveDataTableToCsv();
            }
        }
        internal bool CanExecute_InventorySaveChangesCommand(object parameter)
        {
            return true;
        }
        #endregion

        #endregion Inventory Commands

        #region Users Commands

        #region UserUpdateCommand

        public ICommand UserUpdateCommand { get { return _userUpdateCommand ?? (_userUpdateCommand = new DelegateCommand(Execute_UserUpdateCommand, CanExecute_UserUpdateCommand)); } }
        private ICommand _userUpdateCommand;

        internal void Execute_UserUpdateCommand(object parameter)
        {
            //Change main frame page based on the parameter
            switch ((string)parameter)
            {
                case "user_details":
                    var temporalProduct = new User(Constants.DataFolderPath + Constants.UsersFileName)
                    {
                        Id = SelectedUser.Id,
                        Name = SelectedUser.Name,
                        Email = SelectedUser.Email,
                        Phone = SelectedUser.Phone,
                        RegistrationDate = SelectedUser.RegistrationDate,
                        Rights = SelectedUser.Rights,
                        UserName = SelectedUser.UserName,
                        Password = SelectedUser.Password,
                        LastLogin = SelectedUser.LastLogin
                    };

                    CurrentPage = "\\View\\UserDetailPage.xaml";
                    UserTemporalItem = temporalProduct;
                    break;
            }
        }

        internal bool CanExecute_UserUpdateCommand(object parameter)
        {
            return SelectedUser != null;
        }

        #endregion

        #region UserAddNewItemCommand

        public ICommand UserAddNewItemCommand { get { return _userAddNewItemCommand ?? (_userAddNewItemCommand = new DelegateCommand(Execute_UserAddNewItemCommand, CanExecute_UserAddNewItemCommand)); } }
        private ICommand _userAddNewItemCommand;

        internal void Execute_UserAddNewItemCommand(object parameter)
        {
            //Change main frame page based on the parameter
            switch ((string)parameter)
            {
                case "user_add":
                    SelectedUser = null;    //Clear selected item in the user list
                    //Create new productt
                    var temporalProduct = new User(Constants.DataFolderPath + Constants.UsersFileName)
                    {
                        Name = "",
                        Email = "",
                        Phone = "",
                        RegistrationDate = DateTime.Now,
                        Rights = UserAccessLevelEnum.Basic,
                        UserName = "",
                        Password = "",
                        LastLogin = DateTime.Now
                    };
                    temporalProduct.Id = temporalProduct.GetLastItemNumber() + 1;

                    CurrentPage = "\\View\\UserDetailPage.xaml";
                    UserTemporalItem = temporalProduct;
                    break;
            }
        }

        internal bool CanExecute_UserAddNewItemCommand(object parameter)
        {
            return true;
        }

        #endregion

        #region UserStartSearchCommand

        public ICommand UserStartSearchCommand { get { return _userStartSearchCommand ?? (_userStartSearchCommand = new DelegateCommand(Execute_UserStartSearchCommand, CanExecute_UserStartSearchCommand)); } }
        private ICommand _userStartSearchCommand;

        internal void Execute_UserStartSearchCommand(object parameter)
        {
            //Inventory search method that returns a list of products for the datagrid
            UsersSearchedEntries = new ObservableCollection<User>(new User(Constants.DataFolderPath + Constants.UsersFileName).Search(UsersSearchText));
            UsersSearchText = "";
        }
        internal bool CanExecute_UserStartSearchCommand(object parameter)
        {
            return true;//InventorySearchText != "" && InventorySearchText != null;
        }

        #endregion

        #region  UserSaveChangesCommand
        public ICommand UserSaveChangesCommand { get { return _userSaveChangesCommand ?? (_userSaveChangesCommand = new DelegateCommand(Execute_UserSaveChangesCommand, CanExecute_UserSaveChangesCommand)); } }
        private ICommand _userSaveChangesCommand;

        internal void Execute_UserSaveChangesCommand(object parameter)
        {
            //Check if code was updated
            if (SelectedUser != null)
            {
                SelectedUser.UpdateUserToTable(UserTemporalItem);
                SelectedUser.SaveDataTableToCsv();
            }
            else
            {
                UserTemporalItem.Register();
            }
            UsersSearchedEntries = null;
            CurrentPage = "\\View\\UserMainPage.xaml";
        }

        internal bool CanExecute_UserSaveChangesCommand(object parameter)
        {
            return UserTemporalItem.Password == UserTemporalItem.PasswordVerification && UserTemporalItem.Password != "";
        }
        #endregion

        #endregion User Commands

        #region Customer Commands

        #region CustomerUpdateCommand

        public ICommand CustomerUpdateCommand { get { return _customerUpdateCommand ?? (_customerUpdateCommand = new DelegateCommand(Execute_CustomerUpdateCommand, CanExecute_CustomerUpdateCommand)); } }
        private ICommand _customerUpdateCommand;

        internal void Execute_CustomerUpdateCommand(object parameter)
        {
            //Change main frame page based on the parameter
            switch ((string)parameter)
            {
                case "customer_details":
                    var temporalCustomer = new Customer(Constants.DataFolderPath + Constants.CustomersFileName)
                    {
                        Id = SelectedCustomer.Id,
                        Name = SelectedCustomer.Name,
                        Email = SelectedCustomer.Email,
                        Phone = SelectedCustomer.Phone,
                        Rfc = SelectedCustomer.Rfc,
                        PointsAvailable = SelectedCustomer.PointsAvailable,
                        PointsUsed = SelectedCustomer.PointsUsed,
                        TotalSpent = SelectedCustomer.TotalSpent,
                        TotalVisits = SelectedCustomer.TotalVisits,
                    };

                    CurrentPage = "\\View\\CustomerDetailPage.xaml";
                    CustomerTemporalItem = temporalCustomer;
                    break;
            }
        }

        internal bool CanExecute_CustomerUpdateCommand(object parameter)
        {
            return SelectedCustomer != null;
        }

        #endregion

        #region CustomerAddNewItemCommand

        public ICommand CustomerAddNewItemCommand { get { return _customerAddNewItemCommand ?? (_customerAddNewItemCommand = new DelegateCommand(Execute_CustomerAddNewItemCommand, CanExecute_CustomerAddNewItemCommand)); } }
        private ICommand _customerAddNewItemCommand;

        internal void Execute_CustomerAddNewItemCommand(object parameter)
        {
            //Change main frame page based on the parameter
            switch ((string)parameter)
            {
                case "customer_add":
                    SelectedCustomer = null;    //Clear selected item in the user list
                    //Create new productt
                    var temporalCustomer = new Customer(Constants.DataFolderPath + Constants.CustomersFileName)
                    {
                        Name = "",
                        Email = "",
                        Phone = "",
                        RegistrationDate = DateTime.Now,
                        Rfc = "",
                        PointsAvailable = 0,
                        PointsUsed = 0,
                        TotalSpent = 0,
                        TotalVisits = 0,
                        LastVisitDate = DateTime.Now
                    };
                    temporalCustomer.Id = temporalCustomer.GetLastItemNumber() + 1;

                    CurrentPage = "\\View\\CustomerDetailPage.xaml";
                    CustomerTemporalItem = temporalCustomer;
                    break;
            }
        }

        internal bool CanExecute_CustomerAddNewItemCommand(object parameter)
        {
            return true;
        }

        #endregion

        #region CustomerStartSearchCommand

        public ICommand CustomerStartSearchCommand { get { return _customerStartSearchCommand ?? (_customerStartSearchCommand = new DelegateCommand(Execute_CustomerStartSearchCommand, CanExecute_CustomerStartSearchCommand)); } }
        private ICommand _customerStartSearchCommand;

        internal void Execute_CustomerStartSearchCommand(object parameter)
        {
            //Inventory search method that returns a list of products for the datagrid
            CustomersSearchedEntries = new ObservableCollection<Customer>(new Customer(Constants.DataFolderPath + Constants.CustomersFileName).Search(CustomersSearchText));
            CustomersSearchText = "";
        }
        internal bool CanExecute_CustomerStartSearchCommand(object parameter)
        {
            return true;//InventorySearchText != "" && InventorySearchText != null;
        }

        #endregion

        #region  CustomerSaveChangesCommand
        public ICommand CustomerSaveChangesCommand { get { return _customerSaveChangesCommand ?? (_customerSaveChangesCommand = new DelegateCommand(Execute_CustomerSaveChangesCommand, CanExecute_CustomerSaveChangesCommand)); } }
        private ICommand _customerSaveChangesCommand;

        internal void Execute_CustomerSaveChangesCommand(object parameter)
        {
            //Check if code was updated
            if (SelectedCustomer != null)
            {
                SelectedCustomer.UpdateUserToTable(CustomerTemporalItem);
                SelectedCustomer.SaveDataTableToCsv();
            }
            else
            {
                CustomerTemporalItem.Register();
            }
            CustomersSearchedEntries = null;
            CurrentPage = "\\View\\CustomerMainPage.xaml";
        }

        internal bool CanExecute_CustomerSaveChangesCommand(object parameter)
        {
            return true;// UserTemporalItem.Password == UserTemporalItem.PasswordVerification && UserTemporalItem.Password != "";
        }
        #endregion

        #endregion Customer Commands

        #region Expenses Commands

        #region ExpenseUpdateCommand

        public ICommand ExpenseUpdateCommand { get { return _expenseUpdateCommand ?? (_expenseUpdateCommand = new DelegateCommand(Execute_ExpenseUpdateCommand, CanExecute_ExpenseUpdateCommand)); } }
        private ICommand _expenseUpdateCommand;

        internal void Execute_ExpenseUpdateCommand(object parameter)
        {
            //Change main frame page based on the parameter
            switch ((string)parameter)
            {
                case "expense_details":
                    var temporalExpense = new Expense(Constants.DataFolderPath + Constants.ExpenseFileName)
                    {
                        Id = SelectedExpense.Id,
                        User = SelectedExpense.User,
                        Vendor = SelectedExpense.Vendor,
                        TicketNumber = SelectedExpense.TicketNumber,
                        Description = SelectedExpense.Description,
                        Amount = SelectedExpense.Amount,
                        ExpenseCategory = SelectedExpense.ExpenseCategory,
                        Date = SelectedExpense.Date,
                        PaymentType = SelectedExpense.PaymentType,
                        CurrencyType = SelectedExpense.CurrencyType
                    };
                    CurrentPage = "\\View\\ExpenseDetailPage.xaml";
                    ExpenseTemporalItem = temporalExpense;
                    break;
            }
        }

        internal bool CanExecute_ExpenseUpdateCommand(object parameter)
        {
            return SelectedExpense != null;
        }

        #endregion

        #region ExpenseAddNewItemCommand

        public ICommand ExpenseAddNewItemCommand { get { return _expenseAddNewItemCommand ?? (_expenseAddNewItemCommand = new DelegateCommand(Execute_ExpenseAddNewItemCommand, CanExecute_ExpenseAddNewItemCommand)); } }
        private ICommand _expenseAddNewItemCommand;

        internal void Execute_ExpenseAddNewItemCommand(object parameter)
        {
            //Change main frame page based on the parameter
            switch ((string)parameter)
            {
                case "expense_add":
                    SelectedExpense = null;    //Clear selected item in the user list
                    //Create new productt
                    var temporalExpense = new Expense(Constants.DataFolderPath + Constants.ExpenseFileName)
                    {
                        Vendor = "",
                        TicketNumber = "",
                        Description = "",
                        Amount = 0,
                        ExpenseCategory = "",
                        Date = DateTime.Now,
                        PaymentType = PaymentTypeEnum.Cash,
                        CurrencyType = CurrencyTypeEnum.MXN
                    };
                    temporalExpense.Id = temporalExpense.GetLastItemNumber() + 1;

                    CurrentPage = "\\View\\ExpenseDetailPage.xaml";
                    ExpenseTemporalItem = temporalExpense;
                    break;
            }
        }

        internal bool CanExecute_ExpenseAddNewItemCommand(object parameter)
        {
            return true;
        }

        #endregion

        #region ExpenseStartSearchCommand

        public ICommand ExpenseStartSearchCommand { get { return _expenseStartSearchCommand ?? (_expenseStartSearchCommand = new DelegateCommand(Execute_ExpenseStartSearchCommand, CanExecute_ExpenseStartSearchCommand)); } }
        private ICommand _expenseStartSearchCommand;

        internal void Execute_ExpenseStartSearchCommand(object parameter)
        {
            //Inventory search method that returns a list of products for the datagrid
            ExpensesSearchedEntries = new ObservableCollection<Expense>(new Expense(Constants.DataFolderPath + Constants.ExpenseFileName).Search(ExpensesSearchText));
            ExpensesSearchText = "";
        }
        internal bool CanExecute_ExpenseStartSearchCommand(object parameter)
        {
            return true;//InventorySearchText != "" && InventorySearchText != null;
        }

        #endregion

        #region  ExpenseSaveChangesCommand
        public ICommand ExpenseSaveChangesCommand { get { return _expenseSaveChangesCommand ?? (_expenseSaveChangesCommand = new DelegateCommand(Execute_ExpenseSaveChangesCommand, CanExecute_ExpenseSaveChangesCommand)); } }
        private ICommand _expenseSaveChangesCommand;

        internal void Execute_ExpenseSaveChangesCommand(object parameter)
        {
            //Check if code was updated
            if (SelectedExpense != null)
            {
                SelectedExpense.UpdateExpenseToTable(ExpenseTemporalItem);
                SelectedExpense.SaveDataTableToCsv();
            }
            else
            {
                ExpenseTemporalItem.Register();
            }
            ExpensesSearchedEntries = null;
            CurrentPage = "\\View\\ExpenseMainPage.xaml";
        }

        internal bool CanExecute_ExpenseSaveChangesCommand(object parameter)
        {
            return true; // UserTemporalItem.Password == UserTemporalItem.PasswordVerification && UserTemporalItem.Password != "";
        }
        #endregion

        #endregion Expenses Commands

        #region Vendor Commands

        #region VendorUpdateCommand

        public ICommand VendorUpdateCommand { get { return _vendorUpdateCommand ?? (_vendorUpdateCommand = new DelegateCommand(Execute_VendorUpdateCommand, CanExecute_VendorUpdateCommand)); } }
        private ICommand _vendorUpdateCommand;

        internal void Execute_VendorUpdateCommand(object parameter)
        {
            //Change main frame page based on the parameter
            switch ((string)parameter)
            {
                case "vendor_details":
                    var temporalVendor = new Vendor(Constants.DataFolderPath + Constants.VendorsFileName)
                    {
                        Id = SelectedVendor.Id,
                        Name = SelectedVendor.Name,
                        Email = SelectedVendor.Email,
                        Phone = SelectedVendor.Phone,
                        RegistrationDate = SelectedVendor.RegistrationDate,
                        Rfc = SelectedVendor.Rfc,
                        BusinessName = SelectedVendor.BusinessName,
                        Bank = SelectedVendor.Bank,
                        BankAccount = SelectedVendor.BankAccount
                    };

                    CurrentPage = "\\View\\VendorDetailPage.xaml";
                    VendorTemporalItem = temporalVendor;
                    break;
            }
        }

        internal bool CanExecute_VendorUpdateCommand(object parameter)
        {
            return SelectedVendor != null;
        }

        #endregion

        #region VendorAddNewItemCommand

        public ICommand VendorAddNewItemCommand { get { return _vendorAddNewItemCommand ?? (_vendorAddNewItemCommand = new DelegateCommand(Execute_VendorAddNewItemCommand, CanExecute_VendorAddNewItemCommand)); } }
        private ICommand _vendorAddNewItemCommand;

        internal void Execute_VendorAddNewItemCommand(object parameter)
        {
            //Change main frame page based on the parameter
            switch ((string)parameter)
            {
                case "vendor_add":
                    SelectedVendor = null;    //Clear selected item in the user list
                    //Create new productt
                    var temporalVendor = new Vendor(Constants.DataFolderPath + Constants.VendorsFileName)
                    {
                        Name = "",
                        Email = "",
                        Phone = "",
                        RegistrationDate = DateTime.Now,
                        Rfc = "",
                        BusinessName = "",
                        Bank = "",
                        BankAccount = ""
                    };
                    temporalVendor.Id = temporalVendor.GetLastItemNumber() + 1;

                    CurrentPage = "\\View\\VendorDetailPage.xaml";
                    VendorTemporalItem = temporalVendor;
                    break;
            }
        }

        internal bool CanExecute_VendorAddNewItemCommand(object parameter)
        {
            return true;
        }

        #endregion

        #region VendorStartSearchCommand

        public ICommand VendorStartSearchCommand { get { return _vendorStartSearchCommand ?? (_vendorStartSearchCommand = new DelegateCommand(Execute_VendorStartSearchCommand, CanExecute_VendorStartSearchCommand)); } }
        private ICommand _vendorStartSearchCommand;

        internal void Execute_VendorStartSearchCommand(object parameter)
        {
            //Inventory search method that returns a list of products for the datagrid
            VendorsSearchedEntries = new ObservableCollection<Vendor>(new Vendor(Constants.DataFolderPath + Constants.VendorsFileName).Search(VendorsSearchText));
            VendorsSearchText = "";
        }
        internal bool CanExecute_VendorStartSearchCommand(object parameter)
        {
            return true;//InventorySearchText != "" && InventorySearchText != null;
        }

        #endregion

        #region  VendorSaveChangesCommand
        public ICommand VendorSaveChangesCommand { get { return _vendorSaveChangesCommand ?? (_vendorSaveChangesCommand = new DelegateCommand(Execute_VendorSaveChangesCommand, CanExecute_VendorSaveChangesCommand)); } }
        private ICommand _vendorSaveChangesCommand;

        internal void Execute_VendorSaveChangesCommand(object parameter)
        {
            //Check if code was updated
            if (SelectedVendor != null)
            {
                SelectedVendor.UpdateUserToTable(VendorTemporalItem);
                SelectedVendor.SaveDataTableToCsv();
            }
            else
            {
                VendorTemporalItem.Register();
            }
            VendorsSearchedEntries = null;
            CurrentPage = "\\View\\VendorMainPage.xaml";
        }

        internal bool CanExecute_VendorSaveChangesCommand(object parameter)
        {
            return true;
        }
        #endregion

        #endregion Vendor Commands

        #region Orders Commands

        #region OrderUpdateCommand

        public ICommand OrderUpdateCommand { get { return _orderUpdateCommand ?? (_orderUpdateCommand = new DelegateCommand(Execute_OrderUpdateCommand, CanExecute_OrderUpdateCommand)); } }
        private ICommand _orderUpdateCommand;

        internal void Execute_OrderUpdateCommand(object parameter)
        {
            //Change main frame page based on the parameter
            switch ((string)parameter)
            {
                //case "customer_details":
                //    var temporalProduct = new User(Constants.DataFolderPath + Constants.UsersFileName)
                //    {
                //        Id = SelectedUser.Id,
                //        Name = SelectedUser.Name,
                //        Email = SelectedUser.Email,
                //        Phone = SelectedUser.Phone,
                //        RegistrationDate = SelectedUser.RegistrationDate,
                //        Rights = SelectedUser.Rights,
                //        UserName = SelectedUser.UserName,
                //        Password = SelectedUser.Password,
                //        LastLogin = SelectedUser.LastLogin
                //    };

                //    CurrentPage = "\\View\\UserDetailPage.xaml";
                //    UserTemporalItem = temporalProduct;
                //    break;
            }
        }

        internal bool CanExecute_OrderUpdateCommand(object parameter)
        {
            return SelectedUser != null;
        }

        #endregion

        #region OrderAddNewItemCommand

        public ICommand OrderAddNewItemCommand { get { return _orderAddNewItemCommand ?? (_orderAddNewItemCommand = new DelegateCommand(Execute_OrderAddNewItemCommand, CanExecute_OrderAddNewItemCommand)); } }
        private ICommand _orderAddNewItemCommand;

        internal void Execute_OrderAddNewItemCommand(object parameter)
        {
            //Change main frame page based on the parameter
            switch ((string)parameter)
            {
                //case "user_add":
                //    SelectedUser = null;    //Clear selected item in the user list
                //    //Create new productt
                //    var temporalProduct = new User(Constants.DataFolderPath + Constants.UsersFileName)
                //    {
                //        Name = "",
                //        Email = "",
                //        Phone = "",
                //        RegistrationDate = DateTime.Now,
                //        Rights = UserAccessLevelEnum.Basic,
                //        UserName = "",
                //        Password = "",
                //        LastLogin = DateTime.Now
                //    };
                //    temporalProduct.Id = temporalProduct.GetLastItemNumber() + 1;

                //    CurrentPage = "\\View\\UserDetailPage.xaml";
                //    UserTemporalItem = temporalProduct;
                //    break;
            }
        }

        internal bool CanExecute_OrderAddNewItemCommand(object parameter)
        {
            return true;
        }

        #endregion

        #region OrderStartSearchCommand

        public ICommand OrderStartSearchCommand { get { return _orderStartSearchCommand ?? (_orderStartSearchCommand = new DelegateCommand(Execute_OrderStartSearchCommand, CanExecute_OrderStartSearchCommand)); } }
        private ICommand _orderStartSearchCommand;

        internal void Execute_OrderStartSearchCommand(object parameter)
        {
            ////Inventory search method that returns a list of products for the datagrid
            //UsersSearchedEntries = new ObservableCollection<User>(new User(Constants.DataFolderPath + Constants.UsersFileName).Search(UsersSearchText));
            //UsersSearchText = "";
        }
        internal bool CanExecute_OrderStartSearchCommand(object parameter)
        {
            return true;//InventorySearchText != "" && InventorySearchText != null;
        }

        #endregion

        #region  OrderSaveChangesCommand
        public ICommand OrderSaveChangesCommand { get { return _orderSaveChangesCommand ?? (_orderSaveChangesCommand = new DelegateCommand(Execute_OrderSaveChangesCommand, CanExecute_OrderSaveChangesCommand)); } }
        private ICommand _orderSaveChangesCommand;

        internal void Execute_OrderSaveChangesCommand(object parameter)
        {
            ////Check if code was updated
            //if (SelectedUser != null)
            //{
            //    SelectedUser.UpdateUserToTable(UserTemporalItem);
            //    SelectedUser.SaveDataTableToCsv();
            //}
            //else
            //{
            //    UserTemporalItem.Register();
            //}
            //UsersSearchedEntries = null;
            //CurrentPage = "\\View\\UserMainPage.xaml";
        }

        internal bool CanExecute_OrderSaveChangesCommand(object parameter)
        {
            return UserTemporalItem.Password == UserTemporalItem.PasswordVerification && UserTemporalItem.Password != "";
        }
        #endregion

        #endregion Orders Commands

        #region Return Commands

        #region ReturnUpdateCommand

        public ICommand ReturnUpdateCommand { get { return _returnUpdateCommand ?? (_returnUpdateCommand = new DelegateCommand(Execute_ReturnUpdateCommand, CanExecute_ReturnUpdateCommand)); } }
        private ICommand _returnUpdateCommand;

        internal void Execute_ReturnUpdateCommand(object parameter)
        {
            //Change main frame page based on the parameter
            switch ((string)parameter)
            {
                //case "customer_details":
                //    var temporalProduct = new User(Constants.DataFolderPath + Constants.UsersFileName)
                //    {
                //        Id = SelectedUser.Id,
                //        Name = SelectedUser.Name,
                //        Email = SelectedUser.Email,
                //        Phone = SelectedUser.Phone,
                //        RegistrationDate = SelectedUser.RegistrationDate,
                //        Rights = SelectedUser.Rights,
                //        UserName = SelectedUser.UserName,
                //        Password = SelectedUser.Password,
                //        LastLogin = SelectedUser.LastLogin
                //    };

                //    CurrentPage = "\\View\\UserDetailPage.xaml";
                //    UserTemporalItem = temporalProduct;
                //    break;
            }
        }

        internal bool CanExecute_ReturnUpdateCommand(object parameter)
        {
            return SelectedUser != null;
        }

        #endregion

        #region ReturnAddNewItemCommand

        public ICommand ReturnAddNewItemCommand { get { return _returnAddNewItemCommand ?? (_returnAddNewItemCommand = new DelegateCommand(Execute_ReturnAddNewItemCommand, CanExecute_ReturnAddNewItemCommand)); } }
        private ICommand _returnAddNewItemCommand;

        internal void Execute_ReturnAddNewItemCommand(object parameter)
        {
            //Change main frame page based on the parameter
            switch ((string)parameter)
            {
                //case "user_add":
                //    SelectedUser = null;    //Clear selected item in the user list
                //    //Create new productt
                //    var temporalProduct = new User(Constants.DataFolderPath + Constants.UsersFileName)
                //    {
                //        Name = "",
                //        Email = "",
                //        Phone = "",
                //        RegistrationDate = DateTime.Now,
                //        Rights = UserAccessLevelEnum.Basic,
                //        UserName = "",
                //        Password = "",
                //        LastLogin = DateTime.Now
                //    };
                //    temporalProduct.Id = temporalProduct.GetLastItemNumber() + 1;

                //    CurrentPage = "\\View\\UserDetailPage.xaml";
                //    UserTemporalItem = temporalProduct;
                //    break;
            }
        }

        internal bool CanExecute_ReturnAddNewItemCommand(object parameter)
        {
            return true;
        }

        #endregion

        #region ReturnStartSearchCommand

        public ICommand ReturnStartSearchCommand { get { return _returnStartSearchCommand ?? (_returnStartSearchCommand = new DelegateCommand(Execute_ReturnStartSearchCommand, CanExecute_ReturnStartSearchCommand)); } }
        private ICommand _returnStartSearchCommand;

        internal void Execute_ReturnStartSearchCommand(object parameter)
        {
            ////Inventory search method that returns a list of products for the datagrid
            //UsersSearchedEntries = new ObservableCollection<User>(new User(Constants.DataFolderPath + Constants.UsersFileName).Search(UsersSearchText));
            //UsersSearchText = "";
        }
        internal bool CanExecute_ReturnStartSearchCommand(object parameter)
        {
            return true;//InventorySearchText != "" && InventorySearchText != null;
        }

        #endregion

        #region  ReturnSaveChangesCommand
        public ICommand ReturnSaveChangesCommand { get { return _returnSaveChangesCommand ?? (_returnSaveChangesCommand = new DelegateCommand(Execute_ReturnSaveChangesCommand, CanExecute_ReturnSaveChangesCommand)); } }
        private ICommand _returnSaveChangesCommand;

        internal void Execute_ReturnSaveChangesCommand(object parameter)
        {
            ////Check if code was updated
            //if (SelectedUser != null)
            //{
            //    SelectedUser.UpdateUserToTable(UserTemporalItem);
            //    SelectedUser.SaveDataTableToCsv();
            //}
            //else
            //{
            //    UserTemporalItem.Register();
            //}
            //UsersSearchedEntries = null;
            //CurrentPage = "\\View\\UserMainPage.xaml";
        }

        internal bool CanExecute_ReturnSaveChangesCommand(object parameter)
        {
            return true;//UserTemporalItem.Password == UserTemporalItem.PasswordVerification && UserTemporalItem.Password != "";
        }
        #endregion

        #endregion Returns Commands

        #region Transaction Analysis Commands

        #region TransactionUpdateCommand

        public ICommand TransactionUpdateCommand { get { return _transactionUpdateCommand ?? (_transactionUpdateCommand = new DelegateCommand(Execute_TransactionUpdateCommand, CanExecute_TransactionUpdateCommand)); } }
        private ICommand _transactionUpdateCommand;

        internal void Execute_TransactionUpdateCommand(object parameter)
        {
            //Change main frame page based on the parameter
            switch ((string)parameter)
            {
                //case "customer_details":
                //    var temporalProduct = new User(Constants.DataFolderPath + Constants.UsersFileName)
                //    {
                //        Id = SelectedUser.Id,
                //        Name = SelectedUser.Name,
                //        Email = SelectedUser.Email,
                //        Phone = SelectedUser.Phone,
                //        RegistrationDate = SelectedUser.RegistrationDate,
                //        Rights = SelectedUser.Rights,
                //        UserName = SelectedUser.UserName,
                //        Password = SelectedUser.Password,
                //        LastLogin = SelectedUser.LastLogin
                //    };

                //    CurrentPage = "\\View\\UserDetailPage.xaml";
                //    UserTemporalItem = temporalProduct;
                //    break;
            }
        }

        internal bool CanExecute_TransactionUpdateCommand(object parameter)
        {
            return SelectedUser != null;
        }

        #endregion

        #region TransactionAddNewItemCommand

        public ICommand TransactionAddNewItemCommand { get { return _transactionAddNewItemCommand ?? (_transactionAddNewItemCommand = new DelegateCommand(Execute_TransactionAddNewItemCommand, CanExecute_TransactionAddNewItemCommand)); } }
        private ICommand _transactionAddNewItemCommand;

        internal void Execute_TransactionAddNewItemCommand(object parameter)
        {
            //Change main frame page based on the parameter
            switch ((string)parameter)
            {
                //case "user_add":
                //    SelectedUser = null;    //Clear selected item in the user list
                //    //Create new productt
                //    var temporalProduct = new User(Constants.DataFolderPath + Constants.UsersFileName)
                //    {
                //        Name = "",
                //        Email = "",
                //        Phone = "",
                //        RegistrationDate = DateTime.Now,
                //        Rights = UserAccessLevelEnum.Basic,
                //        UserName = "",
                //        Password = "",
                //        LastLogin = DateTime.Now
                //    };
                //    temporalProduct.Id = temporalProduct.GetLastItemNumber() + 1;

                //    CurrentPage = "\\View\\UserDetailPage.xaml";
                //    UserTemporalItem = temporalProduct;
                //    break;
            }
        }

        internal bool CanExecute_TransactionAddNewItemCommand(object parameter)
        {
            return true;
        }

        #endregion

        #region TransactionStartSearchCommand

        public ICommand TransactionStartSearchCommand { get { return _transactionStartSearchCommand ?? (_transactionStartSearchCommand = new DelegateCommand(Execute_TransactionStartSearchCommand, CanExecute_TransactionStartSearchCommand)); } }
        private ICommand _transactionStartSearchCommand;

        internal void Execute_TransactionStartSearchCommand(object parameter)
        {
            ////Inventory search method that returns a list of products for the datagrid
            //UsersSearchedEntries = new ObservableCollection<User>(new User(Constants.DataFolderPath + Constants.UsersFileName).Search(UsersSearchText));
            //UsersSearchText = "";
        }
        internal bool CanExecute_TransactionStartSearchCommand(object parameter)
        {
            return true;//InventorySearchText != "" && InventorySearchText != null;
        }

        #endregion

        #region  TransactionSaveChangesCommand
        public ICommand TransactionSaveChangesCommand { get { return _transactionSaveChangesCommand ?? (_transactionSaveChangesCommand = new DelegateCommand(Execute_TransactionSaveChangesCommand, CanExecute_TransactionSaveChangesCommand)); } }
        private ICommand _transactionSaveChangesCommand;

        internal void Execute_TransactionSaveChangesCommand(object parameter)
        {
            ////Check if code was updated
            //if (SelectedUser != null)
            //{
            //    SelectedUser.UpdateUserToTable(UserTemporalItem);
            //    SelectedUser.SaveDataTableToCsv();
            //}
            //else
            //{
            //    UserTemporalItem.Register();
            //}
            //UsersSearchedEntries = null;
            //CurrentPage = "\\View\\UserMainPage.xaml";
        }

        internal bool CanExecute_TransactionSaveChangesCommand(object parameter)
        {
            return UserTemporalItem.Password == UserTemporalItem.PasswordVerification && UserTemporalItem.Password != "";
        }
        #endregion

        #endregion Returns Commands

        #region AddListItemCommand

        public ICommand AddListItemCommand { get { return _addListItemCommand ?? (_addListItemCommand = new DelegateCommand(Execute_AddListItemCommand, CanExecute_AddListItemCommand)); } }
        private ICommand _addListItemCommand;

        internal void Execute_AddListItemCommand(object parameter)
        {
            var productFound = _inventoryInstance.GetProduct(parameter.ToString());
            if(productFound.Code != null && CurrentPageListProducts.Count <= Constants.MaxNumberListItems)
            {
                CurrentPageListProducts.Add(productFound);
            }
            //TODO:Do not let the user to put another in the list if it is the max already
        }

        internal bool CanExecute_AddListItemCommand(object parameter)
        {
            return parameter == null ? false : true;//parameter.ToString() != "";
        }

        #endregion

        #region SaveChangesProductListCommand
        public ICommand SaveChangesProductListCommand { get { return _saveChangesProductListCommand ?? (_saveChangesProductListCommand = new DelegateCommand(Execute_SaveChangesProductListCommand, CanExecute_SaveChangesProductListCommand)); } }

        private ICommand _saveChangesProductListCommand;

        internal void Execute_SaveChangesProductListCommand(object parameter)
        {
            CategoryCatalog.UpdateProductListFile(parameter.ToString(), CurrentPageListProducts.ToList(), CurrentPageListTitle);
            if (LastSelectedProductsPage == 1)
                PageOneTitle = CurrentPageListTitle;
            else if (LastSelectedProductsPage == 2)
                PageTwoTitle = CurrentPageListTitle;
            else if (LastSelectedProductsPage == 3)
                PageThreeTitle = CurrentPageListTitle;
            else if (LastSelectedProductsPage == 4)
                PageFourTitle = CurrentPageListTitle;
            else if (LastSelectedProductsPage == 5)
                PageFiveTitle = CurrentPageListTitle;
        }
        internal bool CanExecute_SaveChangesProductListCommand(object parameter)
        {
            return true;
        }
        #endregion

        #region Exchange Rate Commands

        #region ExchangeRateSaveCommand
        public ICommand ExchangeRateSaveCommand { get { return _exchangeRateSaveCommand ?? (_exchangeRateSaveCommand = new DelegateCommand(Execute_ExchangeRateSaveCommand, CanExecute_ExchangeRateSaveCommand)); } }
        private ICommand _exchangeRateSaveCommand;

        internal void Execute_ExchangeRateSaveCommand(object parameter)
        {
            _posInstance.UpdateExchangeRate(ExchangeRate);
            _posInstance.UpdateAllData();
            ExchangeRateString = _posInstance.ExchangeRate.ToString();
        }
        internal bool CanExecute_ExchangeRateSaveCommand(object parameter)
        {
            return ExchangeRate.ToString() != "" || ExchangeRate != 0.0M;
        }
        #endregion


        #endregion

        #region Login Commands

        #region UserSignInCommand
        public ICommand UserSignInCommand { get { return _userSignInCommand ?? (_userSignInCommand = new DelegateCommand(Execute_UserSignInCommand, CanExecute_UserSignInCommand)); } }
        private ICommand _userSignInCommand;

        internal void Execute_UserSignInCommand(object parameter)
        {
            ///TODO: Implement Log in verification
        }
        internal bool CanExecute_UserSignInCommand(object parameter)
        {
            ///TODO: Block if username or password is empty
            return true;
        }
        #endregion

        #endregion

        #region Methods

        #region CartMethods

        public void AddProductToCart(Product product)
        {
            CurrentCartProducts.Insert(0, product);
            PaymentTotalMXN = calculateCurrentCartTotal();
        }

        /// <summary>
        /// Method to add a product to a cart
        /// </summary>
        /// <param name="product"></param>
        public void AddManualProductToCart(Product product)
        {
            CurrentCartProducts.Insert(0, product);
            PaymentTotalMXN = calculateCurrentCartTotal();
        }
        
        public void RemoveProductFromCart(Product product)
        {
            if (CurrentCartProducts.Contains(product))
            {
                CurrentCartProducts.Remove(product);
            }
        }

        private decimal calculateCurrentCartTotal()
        {
            var total = 0M;
            foreach(var item in _currentCartProducts)
            {
                total += item.Price * item.LastQuantitySold;
            }
            return total;
        }

        public void AddOneAdditinoalQuantityToProductInCart(Product product, int cartIndex)
        {
            var productIndex = CurrentCartProducts.IndexOf(product);
            product.LastQuantitySold = product.LastQuantitySold + 1;
            CurrentCartProducts.RemoveAt(productIndex);
            CurrentCartProducts.Insert(productIndex,product);
        }
        #endregion

        #region CheckOutProcessMethods

        /// <summary>
        /// Method to initialize payment page and make payment
        /// </summary>
        /// <returns></returns>
        bool ProcessPayment()
        {
            Transaction currentTransaction;

            RecordTransaction(out currentTransaction);
            
            PrintReceipt(currentTransaction, true);
            //Update POS file ticket info
            //Clean Current Cart
            return true;
        }

        /// <summary>
        /// Method to record transaction
        /// </summary>
        /// <returns></returns>
        private bool RecordTransaction(out Transaction transaction)
        {
            //Create new instance
            transaction = new Transaction(Constants.DataFolderPath + Constants.TransactionsFileName,
                Constants.DataFolderPath + Constants.TransactionsMasterFileName, Constants.DataFolderPath + 
                Constants.TransactionsHistoryFileName, true);
            
            //General transaction information
            var transactionNumber = _posInstance.GetNextTransactionNumber();
            var internalNumber = _posInstance.GetNextInternalNumber();
            var user = "Estrella";
            var customer = "Cliente";
            var fiscalReceipt = "No";
            var saleType = TransactionType.Regular;
            var paymentType = PaymentTypeEnum.Cash;
            int orderNumber = 1;
            var transactionDate = DateTime.Now;
            decimal totalDue = 0M;

            //Get next receipt number, if applicable
            var receiptNumber = saleType == TransactionType.Regular ? _posInstance.GetNextReceiptNumber() : _posInstance.LastReceiptNumber;

            //Record each item in the transactions db
            foreach (var product in CurrentCartProducts)
            {
                transaction.TransactionNumber = transactionNumber;
                transaction.InternalNumber = internalNumber;
                transaction.ReceiptNumber = receiptNumber;
                transaction.Product = product;
                transaction.TransactionDate = transactionDate;
                transaction.CustomerName = customer;
                transaction.UserName = user;
                transaction.FiscalReceiptRequired = fiscalReceipt;
                transaction.SaleType = saleType;
                transaction.PaymentType = paymentType;
                transaction.OrderNumber = orderNumber;
                transaction.Record(TransactionType.Regular);

                //update inventory for each product, if applicatable
                UpdateInventory(product, transactionDate, saleType);

                //Total
                totalDue += product.Price * product.LastQuantitySold;
            }

            //Set total due amount and total paid and calculate the change due
            transaction.TotalDue = totalDue;
            transaction.AmountPaid = PaymentReceivedMXN;
            transaction.ChangeDue = transaction.AmountPaid - transaction.TotalDue;

            //Save inventory
            _inventoryInstance.SaveDataTableToCsv();
            string x = nameof(CurrencyTypeEnum.MXN);
            return true;
        }

        /// <summary>
        /// Method to update inventory after a transaction is sucessful
        /// </summary>
        /// <returns></returns>
        bool UpdateInventory(Product product, DateTime transactionDate, TransactionType transactionType)
        {
            //TODO:Verify
            if (product.Id == 0) return false;
                
            if(transactionType == TransactionType.Regular || transactionType == TransactionType.Internal 
                || transactionType == TransactionType.Removal)
            {
                if (product.LocalQuantityAvailable > 0)
                    _inventoryInstance.UpdateItem(product.Code, "CantidadLocal",
                        (product.LocalQuantityAvailable - product.LastQuantitySold) > 0
                        ? (product.LocalQuantityAvailable - product.LastQuantitySold).ToString() : "0");

                if (product.TotalQuantityAvailable > 0)
                    _inventoryInstance.UpdateItem(product.Code, "CantidadDisponibleTotal",
                        (product.TotalQuantityAvailable - product.LastQuantitySold) > 0
                        ? (product.TotalQuantityAvailable - product.LastQuantitySold).ToString() : "0");

                _inventoryInstance.UpdateItem(product.Code, "UltimaTransaccionFecha",
                    transactionDate.ToString("d"));

                if(transactionType == TransactionType.Internal)
                    _inventoryInstance.UpdateItem(product.Code, "CantidadInternoHistorial",
                        (product.InternalQuantity + product.LastQuantitySold).ToString());

                if(transactionType == TransactionType.Regular)
                    _inventoryInstance.UpdateItem(product.Code, "CantidadVendidoHistorial",
                        (product.QuantitySold + product.LastQuantitySold).ToString());

                if(transactionType != TransactionType.Removal)
                    _inventoryInstance.UpdateItem(product.Code, "VendidoHistorial",
                        (product.AmountSold + product.LastQuantitySold * product.LastAmountSold).ToString());
            }
            else
            {
                //if it is a return
                if (product.LocalQuantityAvailable > 0)
                    _inventoryInstance.UpdateItem(product.Code, "CantidadLocal",
                        (product.LocalQuantityAvailable + product.LastQuantitySold).ToString());

                if (product.TotalQuantityAvailable > 0)
                    _inventoryInstance.UpdateItem(product.Code, "CantidadDisponibleTotal",
                        (product.TotalQuantityAvailable + product.LastQuantitySold).ToString());

                _inventoryInstance.UpdateItem(product.Code, "UltimaTransaccionFecha",
                    transactionDate.ToString("d"));

                _inventoryInstance.UpdateItem(product.Code, "CantidadVendidoHistorial",
                    (product.QuantitySold - product.LastQuantitySold).ToString());

                _inventoryInstance.UpdateItem(product.Code, "VendidoHistorial",
                    (product.AmountSold - product.LastQuantitySold * product.LastAmountSold).ToString());
            }

            return true;
        }

        /// <summary>
        /// Method to print receipt of the transaction
        /// </summary>
        /// <returns></returns>
        bool PrintReceipt(Transaction transaction, bool printToFileOnly = false)
        {
            //Receipt for individual sale
            var receipt = new Receipt(_posInstance, transaction, ReceiptType.RegularSale, CurrentCartProducts);
            receipt.PrintSalesReceipt();
            return true;
        }

        #endregion
     
        #region ProductsPageMethods
 
        /// <summary>
        /// Get items list and title for the pages
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public ObservableCollection<string> GetPageItemsList(int pageNumber, out string pageTitle)
        {
            List<string> items;

            switch (pageNumber)
            {
                case 1:
                    {
                        var tempList = new ObservableCollection<Product>();
                        items = CategoryCatalog.GetList(Constants.DataFolderPath + Constants.ProductPageOne);
                        pageTitle = items.First();
                        items.RemoveAt(0);
                        CurrentPageListProducts = tempList;
                        break;
                    }

                case 2:
                    {
                        var tempList = new ObservableCollection<Product>();
                        items = CategoryCatalog.GetList(Constants.DataFolderPath + Constants.ProductPageTwo);
                        pageTitle = items.First();
                        items.RemoveAt(0);
                        CurrentPageListProducts = tempList;
                        break;
                    }

                case 3:
                    {
                        var tempList = new ObservableCollection<Product>();
                        items = CategoryCatalog.GetList(Constants.DataFolderPath + Constants.ProductPageThree);
                        pageTitle = items.First();
                        items.RemoveAt(0);
                        CurrentPageListProducts = tempList;
                        break;
                    }
                case 4:
                    {
                        var tempList = new ObservableCollection<Product>();
                        items = CategoryCatalog.GetList(Constants.DataFolderPath + Constants.ProductPageFour);
                        pageTitle = items.First();
                        items.RemoveAt(0);
                        CurrentPageListProducts = tempList;
                        break;
                    }
                case 5:
                    {
                        var tempList = new ObservableCollection<Product>();
                        items = CategoryCatalog.GetList(Constants.DataFolderPath + Constants.ProductPageFive);
                        pageTitle = items.First();
                        items.RemoveAt(0);
                        CurrentPageListProducts = tempList;
                        break;
                    }
                default:
                    {
                        items = new List<string>() { "Varios" };
                        pageTitle = "Pagina de Productos";
                        break;
                    }
            }
            return new ObservableCollection<string>(items);
        }

        
        /// <summary>
        /// Get products list and title for the pages
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageTitle"></param>
        /// <returns></returns>
        public ObservableCollection<Product> GetPageProductsList(int pageNumber, out string pageTitle)
        {
            List<Product> items;
            ObservableCollection<Product> products;
            switch (pageNumber)
            {
                case 1:
                    {
                        items = CategoryCatalog.GetProductList(Constants.DataFolderPath + Constants.ProductPageOne, out pageTitle);
                        products = new ObservableCollection<Product>(items);
                        CurrentPageListProducts = products;
                        PageOneTitle = pageTitle;
                        //TODO: need to change to only change page titles when the changes are saved
                        CurrentPageListTitle = pageTitle;
                        CurrentSelectedItemsListPageFileName = Constants.DataFolderPath + Constants.ProductPageOne;
                        break;
                    }

                case 2:
                    {
                        items = CategoryCatalog.GetProductList(Constants.DataFolderPath + Constants.ProductPageTwo, out pageTitle);
                        products = new ObservableCollection<Product>(items);
                        CurrentPageListProducts = products;
                        PageTwoTitle = pageTitle;
                        CurrentPageListTitle = pageTitle;
                        CurrentSelectedItemsListPageFileName = Constants.DataFolderPath + Constants.ProductPageTwo;
                        break;
                    }

                case 3:
                    {
                        items = CategoryCatalog.GetProductList(Constants.DataFolderPath + Constants.ProductPageThree, out pageTitle);
                        products = new ObservableCollection<Product>(items);
                        CurrentPageListProducts = products;
                        PageThreeTitle = pageTitle;
                        CurrentPageListTitle = pageTitle;
                        CurrentSelectedItemsListPageFileName = Constants.DataFolderPath + Constants.ProductPageThree;
                        break;
                    }
                case 4:
                    {
                        items = CategoryCatalog.GetProductList(Constants.DataFolderPath + Constants.ProductPageFour, out pageTitle);
                        products = new ObservableCollection<Product>(items);
                        CurrentPageListProducts = products;
                        PageFourTitle = pageTitle;
                        CurrentPageListTitle = pageTitle;
                        CurrentSelectedItemsListPageFileName = Constants.DataFolderPath + Constants.ProductPageFour;
                        break;
                    }
                case 5:
                    {
                        items = CategoryCatalog.GetProductList(Constants.DataFolderPath + Constants.ProductPageFive, out pageTitle);
                        products = new ObservableCollection<Product>(items);
                        CurrentPageListProducts = products;
                        PageFiveTitle = pageTitle;
                        CurrentPageListTitle = pageTitle;
                        CurrentSelectedItemsListPageFileName = Constants.DataFolderPath + Constants.ProductPageFive;
                        break;
                    }
                default:
                    {
                        items = new List<Product>() { Product.Add("Varios", "Varios", 1, 1)};
                        products = new ObservableCollection<Product>(items);
                        pageTitle = "Pagina de Productos";
                        break;
                    }
            }
            return products;
        }

        /// <summary>
        /// Set page titles based on the initial values
        /// </summary>
        private void GetInitialPagesTitles()
        {
            PageOneTitle = CategoryCatalog.GetList(Constants.DataFolderPath + Constants.ProductPageOne).First();
            PageTwoTitle = CategoryCatalog.GetList(Constants.DataFolderPath + Constants.ProductPageTwo).First();
            PageThreeTitle = CategoryCatalog.GetList(Constants.DataFolderPath + Constants.ProductPageThree).First();
            PageFourTitle = CategoryCatalog.GetList(Constants.DataFolderPath + Constants.ProductPageFour).First();
            PageFiveTitle = CategoryCatalog.GetList(Constants.DataFolderPath + Constants.ProductPageFive).First();
        }

        #endregion

        #region Inventory Methods

        public bool RemoveImage()
        {
            //Remove image from product
            return true;
        }

        public string SelectImage()
        {
            //Open dialog and select jpg image
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.DefaultExt = ".jpg";

            //Display dialog
            bool? result = dialog.ShowDialog();

            if(result == true)
            {
                var fileName = Path.GetFileName(dialog.FileName);
                return fileName;
            }
            else
            {
                return null;
            }
        }

        public void SaveProductIntoTemporalProduct()
        {

        }

        #endregion

        #endregion

    }
}