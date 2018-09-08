using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GenericParsing;

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
/*
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
*/
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
                case TransactionType.Interno:
                    var internalData = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16}",
                          TransactionNumber, InternalNumber, ReceiptNumber, Product.Code, Product.Id, Product.Category,
                          Product.Description, Product.Price, Product.LastQuantitySold, Product.LastQuantitySold * Product.Price,
                          TransactionDate, CustomerName, UserName, FiscalReceiptRequired, SaleType, PaymentType.ToString(), OrderNumber) + Environment.NewLine;

                    return SaveTransaction(_transactionMasterFilePath, internalData) && SaveTransaction(_transactionHistoryFilePath, internalData);

                case TransactionType.Removal:
                case TransactionType.Remover:
                    var removalData = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16}",
                          TransactionNumber, InternalNumber, ReceiptNumber, Product.Code, Product.Id, Product.Category,
                          Product.Description, Product.Price * 0, Product.LastQuantitySold, Product.LastQuantitySold * Product.Price * 0,
                          TransactionDate, CustomerName, UserName, FiscalReceiptRequired, SaleType, PaymentType.ToString(), OrderNumber) + Environment.NewLine;

                    return SaveTransaction(_transactionMasterFilePath, removalData) && SaveTransaction(_transactionHistoryFilePath, removalData);

                case TransactionType.Return:
                case TransactionType.DevolucionEfectivo:
                case TransactionType.DevolucionTarjeta:
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
            string TransactionFileBackUpCopyName = Constants.DataFolderPath + Constants.TransactionsBackupFolderPath + "Transacciones" +
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
                "TransaccionesMaster" + currentTime.Day.ToString("00") + currentTime.Month.ToString("00") +
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

        //Get transactions data per payment type
        public static List<Tuple<string, int, decimal>> GetTransactionsData2(TransactionType transactionType,
            Pos posData, out int firstReceiptNumber, out int lastReceiptNumber, out int totalItemsSold, out decimal totalAmountSold,
            out decimal cashTotal, out decimal cardTotal, out decimal checkTotal, out decimal bankTotal, 
            out decimal otherTotal, out double pointsTotal)
        {
            System.Data.DataTable data;
            totalAmountSold = 0;
            totalItemsSold = 0;
            cashTotal = 0;
            cardTotal = 0;
            checkTotal = 0;
            bankTotal = 0;
            otherTotal = 0;
            pointsTotal = 0;

            string selectedFilePath = String.Empty;

            var categoryData = new List<Tuple<string, int, decimal>>();

            //Open current transaction file and get data
            if (transactionType == TransactionType.Internal)
            {
                selectedFilePath = posData.TransactionMasterDataFilePath;
            }
            else if (transactionType == TransactionType.Regular)
            {
                selectedFilePath = posData.TransactionsDataFilePath;
            }

            using (var parser = new GenericParserAdapter(selectedFilePath))
            {
                parser.ColumnDelimiter = ',';
                parser.FirstRowHasHeader = true;
                parser.SkipStartingDataRows = 0;
                parser.SkipEmptyRows = true;
                parser.MaxBufferSize = 4096;
                parser.MaxRows = 8000;

                data = parser.GetDataTable();
            }

            var categories = CategoryCatalog.GetList(posData.Catalog);

            //Get each category
            foreach (var category in categories)
            {
                var amount = 0M;
                var itemsNumber = 0;

                for (var index = 0; index < data.Rows.Count; index++)
                {
                    var row = data.Rows[index];
                    //Separate categories
                    if (row["CategoriaProducto"].ToString() == category)
                    {
                        amount += decimal.Parse(row["TotalVendido"].ToString());
                        itemsNumber += int.Parse(row["UnidadesVendidas"].ToString());

                        //Get payment method
                        switch (row["MetodoPago"].ToString())
                        {
                            case "Cash":
                            case "Efectivo":
                                cashTotal += decimal.Parse(row["TotalVendido"].ToString());
                                break;
                            case "Card":
                            case "Tarjeta":
                                cardTotal += decimal.Parse(row["TotalVendido"].ToString());
                                break;
                            case "Check":
                            case "Cheque":
                                checkTotal += decimal.Parse(row["TotalVendido"].ToString());
                                break;
                            case "BankTransfer":
                            case "Transferencia":
                                bankTotal += decimal.Parse(row["TotalVendido"].ToString());
                                break;
                            case "Other":
                            case "Otro":
                                otherTotal += decimal.Parse(row["TotalVendido"].ToString());
                                break;
                            default:
                                otherTotal += decimal.Parse(row["TotalVendido"].ToString());
                                break;
                        }

                        //Add all points used
                        ///TODO: Add once the Puntos Usados Column is added to the transfer list, if needed
                        //                   pointsTotal = int.Parse(row["PuntosUsados"].ToString());
                    }
                }

                categoryData.Add(new Tuple<string, int, decimal>(category.ToString(), itemsNumber, amount));
                totalAmountSold += amount;
                totalItemsSold += itemsNumber;
            }

            //Get first and last receipt number
            var firstRow = data.Rows[0];
            var lastRow = data.Rows[data.Rows.Count - 1];
            firstReceiptNumber = Int32.Parse(firstRow["NumeroTicket"].ToString());
            lastReceiptNumber = Int32.Parse(lastRow["NumeroTicket"].ToString());

            return categoryData;
        }

        //Get transactions data per payment type
        public static List<Tuple<string, int, decimal>> GetTransactionsData(TransactionType transactionType,
            Pos posData, out TransactionDataStruct transactionData)
        {
            System.Data.DataTable data;
            transactionData = new TransactionDataStruct
            {
                TotalAmountSold = 0,
                TotalItemsSold = 0,
                CashTotal = 0,
                CardTotal = 0,
                CheckTotal = 0,
                BankTotal = 0,
                OtherTotal = 0,
                PointsTotal = 0,
                ReturnsTotal = 0
            };

            string selectedFilePath = String.Empty;

            var categoryData = new List<Tuple<string, int, decimal>>();

            //Open current transaction file and get data
            if (transactionType == TransactionType.Internal)
            {
                selectedFilePath = posData.TransactionMasterDataFilePath;
            }
            else if (transactionType == TransactionType.Regular)
            {
                selectedFilePath = posData.TransactionsDataFilePath;
            }

            using (var parser = new GenericParserAdapter(selectedFilePath))
            {
                parser.ColumnDelimiter = ',';
                parser.FirstRowHasHeader = true;
                parser.SkipStartingDataRows = 0;
                parser.SkipEmptyRows = true;
                parser.MaxBufferSize = 4096;
                parser.MaxRows = 8000;

                data = parser.GetDataTable();
            }

            var categories = CategoryCatalog.GetList(posData.Catalog);

            //Get each category
            foreach (var category in categories)
            {
                var amount = 0M;
                var itemsNumber = 0;

                for (var index = 0; index < data.Rows.Count; index++)
                {
                    var row = data.Rows[index];
                    //Separate categories
                    if (row["CategoriaProducto"].ToString() == category)
                    {
                        amount += decimal.Parse(row["TotalVendido"].ToString());
                        itemsNumber += int.Parse(row["UnidadesVendidas"].ToString());

                        //Get payment method
                        if (row["Descripcion"].ToString() != "Puntos Descuento")
                        {
                            switch (row["MetodoPago"].ToString())
                            {
                                case "Cash":
                                case "Efectivo":
                                    transactionData.CashTotal += decimal.Parse(row["TotalVendido"].ToString());
                                    break;
                                case "Card":
                                case "Tarjeta":
                                    transactionData.CardTotal += decimal.Parse(row["TotalVendido"].ToString());
                                    break;
                                case "Check":
                                case "Cheque":
                                    transactionData.CheckTotal += decimal.Parse(row["TotalVendido"].ToString());
                                    break;
                                case "BankTransfer":
                                case "Transferencia":
                                    transactionData.BankTotal += decimal.Parse(row["TotalVendido"].ToString());
                                    break;
                                case "Other":
                                case "Otro":
                                    transactionData.OtherTotal += decimal.Parse(row["TotalVendido"].ToString());
                                    break;
                                default:
                                    transactionData.OtherTotal += decimal.Parse(row["TotalVendido"].ToString());
                                    break;
                            }
                        }

                        if (row["Descripcion"].ToString() == "Puntos Descuento")
                        {
                            transactionData.PointsTotal += double.Parse(row["TotalVendido"].ToString())*-1;
                        }

                        if (row["TipoVenta"].ToString() == "Devolucion")
                        {
                            transactionData.ReturnsTotal += decimal.Parse(row["TotalVendido"].ToString()) * -1;
                        }
                    }
                }

                categoryData.Add(new Tuple<string, int, decimal>(category.ToString(), itemsNumber, amount));
                transactionData.TotalAmountSold += amount;
                transactionData.TotalItemsSold += itemsNumber;
            }

            //Get first and last receipt number
            if (data.Rows.Count > 1)
            {
                var firstRow = data.Rows[0];
                var lastRow = data.Rows[data.Rows.Count - 1];
                transactionData.FirstReceiptNumber = Int32.Parse(firstRow["NumeroTicket"].ToString());
                transactionData.LastReceiptNumber = Int32.Parse(lastRow["NumeroTicket"].ToString());
            }
            transactionData.SalesInfoPerCategory = categoryData;
            return categoryData;
        }

        public static void RecordEndOfDaySalesTransaction(string filePath, int endOfDayReceiptNumber, int firstReceipt, int lastReceipt,
            int totalUnitsSold, double totalPointsUsed, decimal totalCashSold, decimal totalCardSold, decimal totalOthersSold, 
            decimal totalAmountSold, string date)
        {
            var data = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}", endOfDayReceiptNumber, firstReceipt, lastReceipt, totalUnitsSold,
                              totalPointsUsed, totalCashSold, totalCardSold, totalOthersSold, totalAmountSold, date) + Environment.NewLine;

            //Append to daily receipt
            File.AppendAllText(filePath, data);
        }
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
        Cheque,
        Transferencia,
        Puntos,
        Otro,
        Desconocido,
        Unknown
    }

    public enum TransactionType
    {
        Regular,
        Internal,
        Interno,
        Return,
        DevolucionEfectivo,
        DevolucionTarjeta,
        Removal,
        Remover
    }
}
