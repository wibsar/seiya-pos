using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Seiya
{
    public class Transaction
    {
        #region Fields

        private string _transactionFilePath;
        private string _transactionMasterFilePath;
        private string _transactionHistoryFilePath;
        private int _internalNumber;
        private int _transactionNumber;
        private int _receiptNumber;
        private Product _product;
        private DateTime _transactionDate;
        private string _customerName;
        private string _userName;
        private string _fiscalReceiptRequireed;
        private TransactionType _saleType;
        private PaymentTypeEnum _paymentType;
        private int _orderNumber;
        private decimal _totalDue;
        private decimal _amountPaid;
        private decimal _changeDue;

        #endregion

        #region Properties

        public string TranscationFilePath { get; set; }
        public List<string> TransactionTypes { get; set; }

        public int TransactionNumber { get => _transactionNumber; set => _transactionNumber = value; }
        public int InternalNumber { get => _internalNumber; set => _internalNumber = value; }
        public int ReceiptNumber { get; set; }
        public Product Product { get => _product; set => _product = value; }
        public DateTime TransactionDate { get => _transactionDate; set => _transactionDate = value; }
        public string CustomerName { get => _customerName; set => _customerName = value; }
        public string UserName { get => _userName; set => _userName = value; }
        public string FiscalReceiptRequired { get => _fiscalReceiptRequireed; set => _fiscalReceiptRequireed = value; }
        public TransactionType SaleType { get => _saleType; set => _saleType = value; }
        public PaymentTypeEnum PaymentType { get => _paymentType; set => _paymentType = value; }
        public int OrderNumber { get => _orderNumber; set => _orderNumber = value; }
        public decimal TotalDue
        {
            get
            {
                return _totalDue;
            }
            set
            {
                _totalDue = value;
            }
        }

        public decimal AmountPaid
        {
            get
            {
                return _amountPaid;
            }
            set
            {
                _amountPaid = value;
            }
        }

        public decimal ChangeDue
        {
            get
            {
                return _changeDue;
            }
            set
            {
                _changeDue = value;
            }
        }

        #endregion

        #region Constructors

        public Transaction(string transactionFilePath)
        {
            TranscationFilePath = transactionFilePath;

            try
            {
                var lines = File.ReadAllLines(transactionFilePath);
                TransactionTypes = lines.ToList();
            }
            catch (Exception)
            {
                TransactionTypes.Add("Regular");
            }
        }

        public Transaction(string transactionFilePath, string transactionMasterFilePath, string transactionHistoryFilePath,
            bool datafile)
        {
            _transactionFilePath = transactionFilePath;
            _transactionMasterFilePath = transactionMasterFilePath;
            _transactionHistoryFilePath = transactionHistoryFilePath;
        }

        public Transaction(string transactionFilePath, bool datafile)
        {
            _transactionFilePath = transactionFilePath;
        }

        #endregion  

        #region Methods

        //TODO: Remove this function
        //Method to add transaction details to transaction files
        public void Add(int transactionNumber, int internalNumber, int receiptNumber, string productCode,
            int productNumber, string productCategory, string productDescription, decimal soldPrice,
            int soldUnits, decimal totalSold, DateTime transactionDate, string customerName, string userName,
            string fiscalReceiptReq, string saleType, PaymentTypeEnum paymentType, int orderNumber)
        {
            string data;

            if (saleType != "Devolucion")
            {
                data = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16}",
                    transactionNumber, internalNumber, receiptNumber, productCode, productNumber, productCategory,
                    productDescription, soldPrice, soldUnits, totalSold, TransactionDate, customerName, userName,
                    fiscalReceiptReq, saleType, paymentType.ToString(), orderNumber) + Environment.NewLine;
            }
            else
            {
                data = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16}",
                    transactionNumber, internalNumber, receiptNumber, productCode, productNumber, productCategory,
                    productDescription, -1 * soldPrice, -1 * soldUnits, -1 * totalSold, TransactionDate, customerName,
                    userName, fiscalReceiptReq, saleType, paymentType.ToString(), orderNumber) + Environment.NewLine;
            }

            //Append to daily master
            File.AppendAllText(_transactionMasterFilePath, data);
            //Append to history transaction file
            File.AppendAllText(_transactionHistoryFilePath, data);
            //Append to daily regular
            if (saleType == "Regular")
            {
                data = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14}", receiptNumber,
                    productCode, productNumber, productCategory, productDescription, soldPrice, soldUnits, totalSold,
                    TransactionDate, customerName, userName, fiscalReceiptReq, saleType, paymentType.ToString(),
                    orderNumber) + Environment.NewLine;

                File.AppendAllText(_transactionFilePath, data);
            }
            else if (saleType == "Devolucion")
            {
                data = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14}", receiptNumber,
                    productCode, productNumber, productCategory, productDescription, -1 * soldPrice, -1 * soldUnits,
                    -1 * totalSold, TransactionDate, customerName, userName, FiscalReceiptRequired, SaleType,
                    PaymentType.ToString(), OrderNumber) + Environment.NewLine;

                File.AppendAllText(_transactionFilePath, data);
            }
        }

        /// <summary>
        /// Record transaction data into database
        /// </summary>
        /// <param name="transactionType"></param>
        /// <returns></returns>
        public bool Record(TransactionType transactionType)
        {
            switch (transactionType)
            {
               
                case TransactionType.Regular:

                    var data = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16}",
                         TransactionNumber, InternalNumber, ReceiptNumber, Product.Code, Product.Id, Product.Category,
                         Product.Description, Product.Price, Product.LastQuantitySold, Product.LastQuantitySold * Product.Price,
                         TransactionDate, CustomerName, UserName, FiscalReceiptRequired, SaleType, PaymentType.ToString(), OrderNumber) + Environment.NewLine;

                    var dataShort = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14}",
                         ReceiptNumber, Product.Code, Product.Id, Product.Category,
                         Product.Description, Product.Price, Product.LastQuantitySold, Product.LastQuantitySold * Product.Price,
                         TransactionDate, CustomerName, UserName, FiscalReceiptRequired, SaleType, PaymentType.ToString(), OrderNumber) + Environment.NewLine;

                    return SaveTransaction(_transactionFilePath, dataShort) && SaveTransaction(_transactionMasterFilePath, data) &&
                        SaveTransaction(_transactionHistoryFilePath, data);
                   
                case TransactionType.Internal:
                    var internalData = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16}",
                          TransactionNumber, InternalNumber, ReceiptNumber, Product.Code, Product.Id, Product.Category,
                          Product.Description, Product.Price, Product.LastQuantitySold, Product.LastQuantitySold * Product.Price,
                          TransactionDate, CustomerName, UserName, FiscalReceiptRequired, SaleType, PaymentType.ToString(), OrderNumber) + Environment.NewLine;

                    SaveTransaction(_transactionMasterFilePath, internalData);
                    SaveTransaction(_transactionHistoryFilePath, internalData);

                    return SaveTransaction(_transactionMasterFilePath, internalData) && SaveTransaction(_transactionHistoryFilePath, internalData);

                case TransactionType.Removal:
                    var removalData = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16}",
                          TransactionNumber, InternalNumber, ReceiptNumber, Product.Code, Product.Id, Product.Category,
                          Product.Description, Product.Price * 0, Product.LastQuantitySold, Product.LastQuantitySold * Product.Price * 0,
                          TransactionDate, CustomerName, UserName, FiscalReceiptRequired, SaleType, PaymentType.ToString(), OrderNumber) + Environment.NewLine;

                    SaveTransaction(_transactionMasterFilePath, removalData);
                    SaveTransaction(_transactionHistoryFilePath, removalData);

                    return SaveTransaction(_transactionMasterFilePath, removalData) && SaveTransaction(_transactionHistoryFilePath, removalData);

                case TransactionType.Return:

                    var returnData = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16}",
                        TransactionNumber, InternalNumber, ReceiptNumber, Product.Code, Product.Id, Product.Category,
                        Product.Description, -1 * Product.Price, -1 * Product.LastQuantitySold, -1 * Product.LastQuantitySold * Product.Price,
                        TransactionDate, CustomerName, UserName, FiscalReceiptRequired, SaleType, PaymentType.ToString(), OrderNumber) + Environment.NewLine;

                    var returnDataShort = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14}",
                         ReceiptNumber, Product.Code, Product.Id, Product.Category,
                         Product.Description, -1 * Product.Price, -1 * Product.LastQuantitySold, -1 * Product.LastQuantitySold * Product.Price,
                         TransactionDate, CustomerName, UserName, FiscalReceiptRequired, SaleType, PaymentType.ToString(), OrderNumber) + Environment.NewLine;

                    return SaveTransaction(_transactionFilePath, returnDataShort) && SaveTransaction(_transactionMasterFilePath, returnData) &&
                        SaveTransaction(_transactionHistoryFilePath, returnData);

                default:
                    return false;
            }
        }

        private bool SaveTransaction(string transactionDataFilePath, string data)
        {
            try
            {
                File.AppendAllText(transactionDataFilePath, data);
            }
            catch (Exception e)
            {
                //Report Error in log file
            }
            return true;
        }

        //Method to create a backup file
        public static void BackUpTransactionFile(string transactionFilePath)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-MX");
            var currentTime = DateTime.Now;
            //Load inventory csv file and create a backup copy
            string TransactionFileBackUpCopyName = @"C:\Users\Public\Documents\mxData\Data\TransactionBackUp\" + "Transacciones" +
                currentTime.Day.ToString("00") + currentTime.Month.ToString("00") + currentTime.Year.ToString("0000") +
                currentTime.Hour.ToString("00") + currentTime.Minute.ToString("00") + currentTime.Second.ToString("00") + ".csv";

            File.Copy(transactionFilePath, TransactionFileBackUpCopyName);
        }
        //Method to create a backup file
        public static void BackUpTransactionMasterFile(string transactionFilePath)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-MX");
            var currentTime = DateTime.Now;
            //Load inventory csv file and create a backup copy
            string TransactionFileBackUpCopyName = Constants.DataFolderPath + Constants.TransactionsBackupFolderPath +
                Constants.TransactionsMasterFileName + currentTime.Day.ToString("00") + currentTime.Month.ToString("00") +
                currentTime.Year.ToString("0000") + currentTime.Hour.ToString("00") + currentTime.Minute.ToString("00") +
                currentTime.Second.ToString("00") + ".csv";

            File.Copy(transactionFilePath, TransactionFileBackUpCopyName);
        }

        //Method to clear file
        public static void ClearTransactionFile(string transactionFilePath)
        {
            File.Copy(Constants.DataFolderPath + Constants.TransactionsBackupFolderPath + Constants.TransactionBlankFileName,
                Constants.DataFolderPath + Constants.TransactionsFileName, true);
        }

        //Method to clear file
        public static void ClearTransactionMasterFile(string transactionFilePath)
        {
            File.Copy(Constants.DataFolderPath + Constants.TransactionsBackupFolderPath + Constants.TransactionMasterBlankFileName,
                Constants.DataFolderPath + Constants.TransactionsMasterFileName, true);
        }
        #endregion
    }

    public enum PaymentTypeEnum
    {
        Cash,
        Card,
        BankTransfer,
        Points,
        Check,
        Tarjeta,
        Efectivo,
        Unknown
    }

    public enum TransactionType
    {
        Regular,
        Internal,
        Return,
        Removal
    }
}
