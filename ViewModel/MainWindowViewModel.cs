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
using System.Windows.Media.Imaging;

namespace Seiya
{
    public class MainWindowViewModel : ObservableObject
    {
        #region Fields

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

        public ObservableCollection<Product> _productObjects;
        private ObservableCollection<Product> _cartProducts;
        private static Product _selectedCartProduct;
        private static Product _selectedPageListProduct;

        private Product _currentProduct;

        private static Inventory _inventoryInstance = null;
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

        };
        private static int _lastSelectedProductsPage = 1;

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

        #region Constructors

        private MainWindowViewModel()
        {
            //Initialize current cart and list number
            CurrentCartNumber = 1;
            //Initialize inventory and Pos data files
            _inventoryInstance = Inventory.GetInstance(Constants.DataFolderPath + Constants.InventoryFileName);
            //Page Titles
            GetInitialPagesTitles();
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
                OnPropertyChanged("CurrentPage"); //OnPropertyChanged("Description");
            }
        }

        public string Code
        {
            get { return _code; }
            set
            {
                _code = value;
                OnPropertyChanged("Code");
                var product = _inventoryInstance.GetProduct(_code);
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
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(@"C:\Projects\seiya-pos\Data\Images\mdf_3.jpg");
                bitmap.EndInit();
                _productImage = bitmap;
                return bitmap;
            }
            set
            {
                _productImage = value;
                OnPropertyChanged("ProductImage");
            }
        }

        #endregion

        #endregion

        public static Inventory InventoryInstance
        {
            get { return _inventoryInstance; }
        }

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
                    break;
                case "sales_report":
                    CurrentPage = "\\View\\EndSalesPage.xaml";
                    break;
                case "product_list":
                    CurrentPage = "\\View\\ProductsListEditPage.xaml";
                    break;
                case "inventory_details":
                    CurrentPage = "\\View\\InventoryItemPage.xaml";
                    break;
                case "s":
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
            var product = _inventoryInstance.GetProduct("test");
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
            var product = (Product)parameter;
            product.LastQuantitySold = 1;
            AddManualProductToCart(product);
        }

        internal bool CanExecute_SelectItemCommand(object parameter)
        {
            return parameter != null;
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
            return parameter.ToString() != "";
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

        #endregion
    }
}