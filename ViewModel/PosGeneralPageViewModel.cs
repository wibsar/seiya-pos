using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using Seiya.WpfBindingUtilities;

namespace Seiya
{
    /// <summary>
    /// ViewModel class to manipulate manual POS transactions
    /// </summary>
    public class PosGeneralPageViewModel : ObservableObject
    {
        #region Fields

        private string _price;
        private int _quantity = 1;
        private string _description;
        private string _category;
        private bool _clear;
        private readonly DelegateCommand _clickCommand;
        private static Product _manualProduct;
        private static ObservableCollection<string> _categoriesList;

        #endregion

        #region Constructors
        /// <summary>
        /// Constructor; loads the product if there was one as input but did not clear before closing page
        /// </summary>
        public PosGeneralPageViewModel()
        {
            //Load category list
            //TODO: Can be checked against null if categorylist cannot change
            if(_categoriesList == null)
              _categoriesList = new ObservableCollection<string>(CategoryCatalog.GetList(Constants.DataFolderPath + Constants.CategoryListFileName));

            if (ManualProduct == null) return;

            Price = ManualProduct.Price.ToString();
            Quantity = ManualProduct.QuantitySold;
            Description = ManualProduct.Description;
            Category = ManualProduct.Category;
            ManualProduct = _manualProduct;
        }

        #endregion

        #region Observable Properties
        /// <summary>
        /// Shows price entered
        /// </summary>
        public string Price
        {
            get { return _price; }
            set
            {
                //Check if price has something other than digits
                decimal number;
                decimal.TryParse(value, out number);
                if (number > 0)
                    _price = number.ToString();
                else
                    _price = "0";
                
                OnPropertyChanged("Price");
            }
        }

        /// <summary>
        /// Shows quantity entered
        /// </summary>
        public int Quantity
        {
            get { return _quantity; }
            set { _quantity = value; OnPropertyChanged("Quantity");}
        }

        /// <summary>
        /// Description of item entered
        /// </summary>
        public string Description
        {
            get { return _description; }
            set
            {
                _description = string.Empty;
                if(value != null)
                {
                    var desc = value.ToCharArray();
                    for (var index = 0; index < desc.Length; ++index)
                    {
                        if (desc[index] == ',')
                            desc[index] = '.';
                        _description += desc[index];
                    }
                }
                OnPropertyChanged("Description");
            }
        }

        /// <summary>
        /// Category of item entered
        /// </summary>
        public string Category
        {
            get { return _category; }
            set { _category = value; OnPropertyChanged("Category"); }
        }

        /// <summary>
        /// Shows the status
        /// </summary>
        public bool Clear
        {
            get { return _clear; }
            set { _clear = value; OnPropertyChanged("Clear"); OnClearChanged();}
        }

        /// <summary>
        /// List of available categories
        /// </summary>
        public ObservableCollection<string> CategoriesList
        {
            get { return _categoriesList; }
            set { _categoriesList = value; OnPropertyChanged("CategoriesList"); OnClearChanged(); }
        }

        #endregion

        #region Properties
        /// <summary>
        /// Property for the manually created product
        /// </summary>
        public static Product ManualProduct
        {
            get { return _manualProduct; }
            set { _manualProduct = value; }
        }

        #endregion

        #region EnterKeyPadNumberCommand

        public ICommand EnterKeyPadNumberCommand { get { return _enterKeyPadNumberCommand ?? (_enterKeyPadNumberCommand = new DelegateCommand(Execute_EnterKeyPadNumberCommand, CanExecute_EnterKeyPadNumberCommand)); } }
        private ICommand _enterKeyPadNumberCommand;

        internal void Execute_EnterKeyPadNumberCommand(object parameter)
        {
            //Call methods or execute the command
            if ((string) parameter == ".")
            {
                if (!Price.Contains("."))
                {
                    Price += (string)parameter;
                }
            }
            else
            {
                Price += (string)parameter;
            }
        }

        internal bool CanExecute_EnterKeyPadNumberCommand(object parameter)
        {
            return true;
        }
        #endregion

        #region ClickEnterCommand

        public ICommand ClickEnterCommand { get { return _clickEnterCommand ?? (_clickEnterCommand = new DelegateCommand(Execute_ClickEnterCommand, CanExecute_ClickEnterCommand)); } }
        private ICommand _clickEnterCommand;

        internal void Execute_ClickEnterCommand(object parameter)
        {
            ManualProduct = Product.Add(Description, Category, decimal.Parse(Price), Quantity);
            //MainWindowViewModel.AddManualProductToCart(ManualProduct); Changed from static
            var main = MainWindowViewModel.GetInstance();
            main.AddManualProductToCart(ManualProduct);
            Clear = true;
        }

        internal bool CanExecute_ClickEnterCommand(object parameter)
        {
            //Add logic to check if the command can be executed (if any)
            //TODO: Check that there is something for quantity, price, and category
            return Price != string.Empty && Price != "0" && Category != null;
        }
        #endregion

        #region AddOrSubtractQuantityCommand

        public ICommand AddOrSubtractQuantityCommand { get { return _addOrSubtractQuantityCommand ?? (_addOrSubtractQuantityCommand = new DelegateCommand(Execute_AddOrSubtractQuantityCommand, CanExecute_AddOrSubtractQuantityCommand)); } }
        private ICommand _addOrSubtractQuantityCommand;

        internal void Execute_AddOrSubtractQuantityCommand(object parameter)
        {
            switch ((string) parameter)
            {
                case "Add":
                    Quantity += 1;
                    break;
                case "Subtract" when Quantity >1:
                    Quantity -= 1;
                    break;
            }
        }

        internal bool CanExecute_AddOrSubtractQuantityCommand(object parameter)
        {
            //Add logic to check if the command can be executed (if any)
            return true;
        }
        #endregion

        #region Sample Command Implementation
        /*
        public ICommand EnterKeyPadNumberCommand { get { return _enterKeyPadNumberCommand ?? (_enterKeyPadNumberCommand = new DelegateCommand(Execute_SelectCalibrationDirectoryCommand, CanExecute_SelectCalibrationDirectoryCommand)); } }
        private ICommand _enterKeyPadNumberCommand;

        internal void Execute_EnterKeyPadNumberCommand(object parameter)
        {
            //Call methods or execute the command
        }

        internal bool CanExecute_EnterKeyPadNumberCommand(object parameter)
        {
            //Add logic to check if the command can be executed (if any)
            return true;
        }
        */
        #endregion

        #region Methods

        /// <summary>
        /// Set property values to default settings
        /// </summary>
        public void OnClearChanged()
        {
            if (!Clear) return;

            Price = string.Empty;
            Quantity = 1;
            Description = string.Empty;
            Category = string.Empty;
            Clear = false;
        }

        #endregion
    }
}
