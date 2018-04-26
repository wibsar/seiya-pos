using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericParsing;

namespace Seiya
{

    public class Receipt
    {
        #region Fields
        private readonly ReceiptType _receiptType;
        private int _receiptNumber;
        private DateTime _currentDate;
        private string _customerName;
        private Pos _posData;
        private DataTable _data;
        private int _firstReceiptNumber;
        private int _lastReceiptNumber;
        #endregion

        #region Properties

        public int TotalUnitsSold { get; set; }
        public decimal TotalAmountSold { get; set; }

        public int FirstReceiptNumber
        {
            get
            {
                return _firstReceiptNumber;
            }
        }

        public int LastReceiptNumber
        {
            get
            {
                return _lastReceiptNumber;
            }
        }

        #endregion

        #region Constructors

        public Receipt(Pos posData, ReceiptType receiptType)
        {
            _receiptType = receiptType;
            _posData = posData;
        }

        #endregion

        #region Methods

        public void Print()
        {
            if (_receiptType == ReceiptType.DailyInternal)
            {

            }
        }

        private void PrintDailyInternal()
        {

        }

        public List<Tuple<string, int, decimal>> GetDataPerCategory(out decimal cashAmount, out decimal cardAmount)
        {
            TotalAmountSold = 0;
            TotalUnitsSold = 0;
            _firstReceiptNumber = 0;
            _lastReceiptNumber = 0;
            cashAmount = 0;
            cardAmount = 0;

            string selectedFilePath = String.Empty;

            var categoryData = new List<Tuple<string, int, decimal>>();

            //Open current transaction file and get data
            if (_receiptType == ReceiptType.DailyInternal)
            {
                selectedFilePath = _posData.TransactionMasterDataFilePath;
            }
            else if (_receiptType == ReceiptType.DailyRegular)
            {
                selectedFilePath = _posData.TransactionsDataFilePath;
            }

            using (var parser = new GenericParserAdapter(selectedFilePath))
            {
                parser.ColumnDelimiter = ',';
                parser.FirstRowHasHeader = true;
                parser.SkipStartingDataRows = 0;
                parser.SkipEmptyRows = true;
                parser.MaxBufferSize = 4096;
                parser.MaxRows = 8000;

                _data = parser.GetDataTable();
            }

            var categories = CategoryCatalog.GetList(Constants.DataFolderPath + Constants.CategoryListFileName); //new CategoryCatalog(_posData.Catalog).categories;

            //Get each category
            foreach (var category in categories)
            {
                decimal amount = 0M;
                int itemsNumber = 0;

                for (var index = 0; index < _data.Rows.Count; index++)
                {
                    var row = _data.Rows[index];

                    if (row["CategoriaProducto"].ToString() == category)
                    {
                        var tempAmount = decimal.Parse(row["TotalVendido"].ToString());
                        amount += tempAmount;
                        itemsNumber += Int32.Parse(row["UnidadesVendidas"].ToString());
                        if (row["MetodoPago"].ToString() == "Cash")
                        {
                            cashAmount += tempAmount;
                        }
                        else
                        {
                            cardAmount += tempAmount;
                        }
                    }
                }

                categoryData.Add(new Tuple<string, int, decimal>(category.ToString(), itemsNumber, amount));
                TotalAmountSold += amount;
                TotalUnitsSold += itemsNumber;
            }

            //Get first and last receipt number
            var firstRow = _data.Rows[0];
            var lastRow = _data.Rows[_data.Rows.Count - 1];
            _firstReceiptNumber = Int32.Parse(firstRow["NumeroTicket"].ToString());
            _lastReceiptNumber = Int32.Parse(lastRow["NumeroTicket"].ToString());

            return categoryData;
        }


        public int GetNextNumber()
        {
            return 0;
        }

        //Get customer information, if available
        public void GetCustomerName(Customer customer)
        {
            _customerName = customer.Name;
        }

        public static void AddReceiptTransaction(string filePath, int receiptNumber, int totalUnitsSold, decimal totalCashSold, decimal totalCardSold, decimal totalAmountSold, string date)
        {
            string data = string.Format("{0},{1},{2},{3},{4},{5}", receiptNumber, totalUnitsSold, totalCashSold, totalCardSold, totalAmountSold, date) + Environment.NewLine;

            //Append to daily receipt
            File.AppendAllText(filePath, data);
        }

        #endregion

    }

    public enum ReceiptType
    {
        Single,
        DailyRegular,
        DailyInternal,
        Weekly,
        Monthly,
        Unknown
    }

}