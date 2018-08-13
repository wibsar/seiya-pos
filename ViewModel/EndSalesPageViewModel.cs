using Seiya.WpfBindingUtilities;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        private decimal _otherTotalSales;
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

        public decimal OtherTotalSales
        {
            get { return _otherTotalSales; }
            set
            {
                _otherTotalSales = value;
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
                CalculateDelta();
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
                CalculateDelta();
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
                CalculateDelta();
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
                CalculateDelta();
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
                CalculateDelta();
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
                CalculateDelta();
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
                CalculateDelta();
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
                CalculateDelta();
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
                CalculateDelta();
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
                CalculateDelta();
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
                CalculateDelta();
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
                CalculateDelta();
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
                CalculateDelta();
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
                CalculateDelta();
                OnPropertyChanged("UsdDollarCoinsTotal");
            }
        }

        #endregion

        #endregion

        #region Properties

        public int FirstReceiptNumber { get; set; }
        public int LastReceiptNumber { get; set; }
        public int TotalItemsSold { get; set; }
        public TransactionDataStruct TransactionData { get; set; }
        #endregion

        #region Constructors

        public EndSalesPageViewModel()
        {
            _pos = Pos.GetInstance(Constants.DataFolderPath + Constants.PosDataFileName);
            //Calculate sales from transactions
            CalculateSales();
            CalculateInitialCash();
            CalculateExpenses();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Method to calculate the sales, record transaction, print receipt, and backup files
        /// </summary>
        void GenerateEndOfDaySalesReport()
        {
            SaveRegisterCashAmount();
            CalculateDelta();
            CalculateSales();
            //Record End Of Sales Transaction in db
            Transaction.RecordEndOfDaySalesTransaction(Constants.DataFolderPath + Constants.MasterEndOfDaySalesFileName,
                _pos.GetNextCorteZNumber(), TransactionData.FirstReceiptNumber, TransactionData.LastReceiptNumber, TransactionData.TotalItemsSold,
                TransactionData.PointsTotal, TransactionData.CashTotal, TransactionData.CardTotal, TransactionData.OtherTotal,
                TransactionData.TotalAmountSold, DateTime.Now.ToString(CultureInfo.CurrentCulture));
            //BackUp Files and Clear
            Transaction.BackUpTransactionFile(Constants.DataFolderPath + Constants.TransactionsFileName);
            Transaction.BackUpTransactionMasterFile(Constants.DataFolderPath + Constants.TransactionsMasterFileName);
            Transaction.ClearTransactionFile(Constants.DataFolderPath + Constants.TransactionsFileName);
            Transaction.ClearTransactionMasterFile(Constants.DataFolderPath + Constants.TransactionsMasterFileName);
            Inventory.InventoryBackUp(Constants.DataFolderPath + Constants.InventoryFileName);
            //Print Receipt
            PrintReceipt();
        }
        /// <summary>
        /// Method to view current sales report and print receupt but do not backup transactoins nor record report as a end of day sales report
        /// </summary>
        void GenerateCurrentSalesReport()
        {
            SaveRegisterCashAmount();
            CalculateDelta();
        }

        private void CalculateSales()
        {
            //Ticket data
            //Read transactions and separate them by payment type
 //           var dataInt = Transaction.GetTransactionsData(TransactionType.Internal, _pos, out var firstReceiptNumber, out var lastReceiptNumber, out var totalItemsSold,
   //             out var totalAmountSold, out var cashTotal, out var cardTotal, out var checkTotal, out var bankTotal, out var otherTotal, out var pointsTotal);

            ///TODO:Remove independent sales and transaction info and just use the struct for displaying data as well
            var dataInt = Transaction.GetTransactionsData(TransactionType.Internal, _pos, out var transactionData);
            TransactionData = transactionData;
            TotalItemsSold = transactionData.TotalItemsSold;
            FirstReceiptNumber = transactionData.FirstReceiptNumber;
            LastReceiptNumber = transactionData.LastReceiptNumber;
            CashTotalSales = transactionData.CashTotal;
            CardTotalSales = transactionData.CardTotal;
            BankTransferTotalSales = transactionData.BankTotal;
            CheckTotalSales = transactionData.CheckTotal;
            PointsTotalUsed = transactionData.PointsTotal;
            OtherTotalSales = transactionData.OtherTotal;
            Delta = 0;
            TotalSales = transactionData.TotalAmountSold;
        }

        private void CalculateExpenses()
        {
            ///TODO: Read expeneses file and add the expenses in cash 
            ExpensesTotal = 10;
        }

        private void CalculateDelta()
        {
            //Total cash available in register
            var cashMxn = MxnPeso20*20 + MxnPeso50*50 + MxnPeso100*100 + MxnPeso200*200 + MxnPeso500*500 + MxnPeso1000*1000 + MxnPesoCoinsTotal;
            var cashUsd = UsdDollar1 + UsdDollar5*5 + UsdDollar10*10 + UsdDollar20*20 + UsdDollar50*50 + UsdDollar100*100 + UsdDollarCoinsTotal;
            var totalCash = cashMxn + cashUsd * _pos.ExchangeRate;
            //Calculate delta
            Delta = (totalCash + ExpensesTotal - RegisterPreviousCash) - CashTotalSales;
        }

        private void CalculateInitialCash()
        {
            RegisterPreviousCash = _pos.GetRegisterCashAmount();
        }

        private void SaveRegisterCashAmount()
        {
            _pos.UpdateRegisterCashAmount(RegisterNewCash);
        }

        private void PrintReceipt()
        {
            var receipt = new Receipt(_pos, ReceiptType.DailyInternal, TransactionData);
            receipt.PrintEndOfDaySalesReceipt();
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
