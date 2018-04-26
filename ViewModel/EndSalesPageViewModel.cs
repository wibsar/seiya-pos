using Seiya.WpfBindingUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Seiya.ViewModel
{
    public class EndSalesPageViewModel : BaseViewModel
    {
        #region Fields
        private int _mxnPeso20;
        private int _mxnPeso50;
        private int _mxnPeso100;
        private int _mxnPeso200;
        private int _mxnPeso500;
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
                OnPropertyChanged("TotalITemsSold");
            }
        }

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

        #region Constructors

        public EndSalesPageViewModel()
        {

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

        #endregion

        #region Commands

        #region GenerateEndOfDaySalesReportCommand

        public ICommand GenerateEndOfDaySalesReportCommand { get { return _generateEndOfDaySalesReportCommand ?? (_generateEndOfDaySalesReportCommand = new DelegateCommand(Execute_GenerateEndOfDaySalesReportCommand, CanExecute_GenerateEndOfDaySalesReportCommand)); } }
        private ICommand _generateEndOfDaySalesReportCommand;

        internal void Execute_GenerateEndOfDaySalesReportCommand(object parameter)
        {
            if ((string)parameter == EndOfSaleReportTypeEnum.IntraDayReport.ToString())
            {
                GenerateCurrentSalesReport();
            }
            else if ((string)parameter == EndOfSaleReportTypeEnum.EndOfDayReport.ToString())
            {
                GenerateEndOfDaySalesReport();
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
