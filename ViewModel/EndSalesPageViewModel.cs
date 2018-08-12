using Seiya.WpfBindingUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Seiya
{
    public class EndSalesPageViewModel : BaseViewModel
    {
        #region Fields

        private int _mxnPeso20;
        private int _mxnPeso50;
        private int _mxnPeso100;
        private int _mxnPeso200;
        private int _mxnPeso500;
        private int _mxnPeso1000;
        private decimal _mxnPesoCoinsTotal;
        private int _usdDollar1;
        private int _usdDollar5;
        private int _usdDollar10;
        private int _usdDollar20;
        private int _usdDollar50;
        private int _usdDollar100;
        private decimal _usdDollarCoinsTotal;
        private decimal _totalSales;
        private int _totalItemsSold;
        private decimal _cardTotalSales;
        private decimal _cashTotalSales;
        private decimal _checkTotalSales;
        private decimal _bankTransferTotalSales;
        private int _pointsTotalUsed;
        private decimal _expensesTotal;
        private decimal _registerPreviousCash;
        private decimal _registerNewCash;
        private decimal _delta;

        private Pos _pos;
        #endregion

        #region Observable Properties

        public decimal TotalSales
        {
            get
            {
                return _totalSales;
            }
            set
            {
                _totalSales = value;
                OnPropertyChanged("TotalSales");
            }
        }

        public int TotalItemsSold
        {
            get
            {
                return _totalItemsSold;
            }
            set
            {
                _totalItemsSold = value;
                OnPropertyChanged();
            }
        }

        public decimal CardTotalSales
        {
            get
            {
                return _cardTotalSales;
            }
            set
            {
                _cardTotalSales = value;
                OnPropertyChanged();
            }
        }

        public decimal CashTotalSales
        {
            get
            {
                return _cashTotalSales;
            }
            set
            {
                _cashTotalSales = value;
                OnPropertyChanged();
            }
        }

        public decimal CheckTotalSales
        {
            get { return _checkTotalSales; }
            set
            {
                _checkTotalSales = value;
                OnPropertyChanged();
            }
        }

        public decimal BankTransferTotalSales
        {
            get { return _bankTransferTotalSales; }
            set
            {
                _bankTransferTotalSales = value;
                OnPropertyChanged();
            }
        }

        public decimal ExpensesTotal
        {
            get { return _expensesTotal; }
            set
            {
                _expensesTotal = value;
                OnPropertyChanged(); 
            }
        }

        public int PointsTotalUsed
        {
            get
            {
                return _pointsTotalUsed;
            }
            set
            {
                _pointsTotalUsed = value;
                OnPropertyChanged();
            }
        }

        public decimal RegisterPreviousCash
        {
            get { return _registerPreviousCash; }
            set
            {
                _registerPreviousCash = value;
                OnPropertyChanged();
            }
        }

        public decimal RegisterNewCash
        {
            get { return _registerNewCash; }
            set
            {
                _registerNewCash = value;
                OnPropertyChanged();
            }
        }

        public decimal Delta
        {
            get { return _delta; }
            set
            {
                _delta = value;
                OnPropertyChanged();
            }
        }


        #region Bills and Coins Properties

        public int MxnPeso20
        {
            get
            {
                return _mxnPeso20;
            }
            set
            {
                _mxnPeso20 = value;
                OnPropertyChanged("MxnPeso20");
            }
        }

        public int MxnPeso50
        {
            get
            {
                return _mxnPeso50;
            }
            set
            {
                _mxnPeso50 = value;
                OnPropertyChanged("MxnPeso50");
            }
        }

        public int MxnPeso100
        {
            get
            {
                return _mxnPeso100;
            }
            set
            {
                _mxnPeso100 = value;
                OnPropertyChanged("MxnPeso100");
            }
        }

        public int MxnPeso200
        {
            get
            {
                return _mxnPeso200;
            }
            set
            {
                _mxnPeso200 = value;
                OnPropertyChanged("MxnPeso200");
            }
        }

        public int MxnPeso500
        {
            get
            {
                return _mxnPeso500;
            }
            set
            {
                _mxnPeso500 = value;
                OnPropertyChanged("MxnPeso500");
            }
        }

        public int MxnPeso1000
        {
            get
            {
                return _mxnPeso1000;
            }
            set
            {
                _mxnPeso1000 = value;
                OnPropertyChanged("MxnPeso1000");
            }
        }

        public decimal MxnPesoCoinsTotal
        {
            get
            {
                return _mxnPesoCoinsTotal;
            }
            set
            {
                _mxnPesoCoinsTotal = value;
                OnPropertyChanged("MxnPesoCoinsTotal");
            }
        }

        public int UsdDollar1
        {
            get
            {
                return _usdDollar1;
            }
            set
            {
                _usdDollar1 = value;
                OnPropertyChanged("UsdDollar1");
            }
        }

        public int UsdDollar5
        {
            get
            {
                return _usdDollar5;
            }
            set
            {
                _usdDollar5 = value;
                OnPropertyChanged("UsdDollar5");
            }
        }

        public int UsdDollar10
        {
            get
            {
                return _usdDollar10;
            }
            set
            {
                _usdDollar10 = value;
                OnPropertyChanged("UsdDollar10");
            }
        }

        public int UsdDollar20
        {
            get
            {
                return _usdDollar20;
            }
            set
            {
                _usdDollar20 = value;
                OnPropertyChanged("UsdDollar20");
            }
        }

        public int UsdDollar50
        {
            get
            {
                return _usdDollar50;
            }
            set
            {
                _usdDollar50 = value;
                OnPropertyChanged("UsdDollar50");
            }
        }

        public int UsdDollar100
        {
            get
            {
                return _usdDollar100;
            }
            set
            {
                _usdDollar100 = value;
                OnPropertyChanged("UsdDollar100");
            }
        }

        public decimal UsdDollarCoinsTotal
        {
            get
            {
                return _usdDollarCoinsTotal;
            }
            set
            {
                _usdDollarCoinsTotal = value;
                OnPropertyChanged("UsdDollarCoinsTotal");
            }
        }

        #endregion

        #endregion

        #region Constructors

        public EndSalesPageViewModel()
        {
            _pos = Pos.GetInstance(Constants.DataFolderPath + Constants.PosDataFileName);
            //Calculate sales from transactions
            CalculateSales();
            CalculateExpenses();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Method to calculate the sales, record transaction, print receipt, and backup files
        /// </summary>
        void GenerateEndOfDaySalesReport()
        {

        }
        /// <summary>
        /// Method to view current sales report and print receupt but do not backup transactoins nor record report as a end of day sales report
        /// </summary>
        void GenerateCurrentSalesReport()
        {

        }

        private void CalculateSales()
        {
            CashTotalSales = 10;
            CardTotalSales = 20;
            BankTransferTotalSales = 30;
            CheckTotalSales = 40;
            PointsTotalUsed = 100;
            Delta = -1;
            TotalSales = CashTotalSales + CardTotalSales + BankTransferTotalSales + CheckTotalSales;
        }

        private void CalculateExpenses()
        {
            ExpensesTotal = 10;
        }

        #endregion

        #region Commands

        #region GenerateEndOfDaySalesReportCommand

        public ICommand GenerateEndOfDaySalesReportCommand { get { return _generateEndOfDaySalesReportCommand ?? (_generateEndOfDaySalesReportCommand = new DelegateCommand(Execute_GenerateEndOfDaySalesReportCommand, CanExecute_GenerateEndOfDaySalesReportCommand)); } }

        private ICommand _generateEndOfDaySalesReportCommand;

        internal void Execute_GenerateEndOfDaySalesReportCommand(object parameter)
        {
            switch ((string)parameter)
            {
                case "x":
                    GenerateCurrentSalesReport();
                    break;
                case "z":
                    GenerateEndOfDaySalesReport();
                    break;
                default:
                    break;
            }
        }

        internal bool CanExecute_GenerateEndOfDaySalesReportCommand(object parameter)
        {
            return true;
        }
        #endregion

        #endregion
    }

    public enum EndOfSaleReportTypeEnum
    {
        IntraDayReport,
        EndOfDayReport
    }


}
