using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericParsing;
using System.Drawing.Printing;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Windows.Documents;


namespace Seiya
{

    public class Receipt
    {
        #region Fields
        PrintDocument _printDocument;
        PrintDialog _printDialog;
        private Transaction _transaction;

        private readonly ReceiptType _receiptType;
        private int _receiptNumber;
        private DateTime _currentDate;
        private string _customerName;
        private Pos _posData;
        private DataTable _data;
        private int _firstReceiptNumber;
        private int _lastReceiptNumber;
        private Pos _pos;
        private ObservableCollection<Product> _products;
        #endregion

        #region Properties
        public ObservableCollection<Product> Products { get; set; }
        public Pos Pos
        {
            get
            {
                return _pos;
            }
            set
            {
                _pos = value;
            }
        }
        public ReceiptType Type { get; set; }    

        //old properties
        public int TotalUnitsSold { get; set; }
        public decimal TotalAmountSold { get; set; }

        public Transaction Transaction
        {
            get
            {
                return _transaction;
            }
            set
            {
                _transaction = value;
            }
        }

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

        public Receipt(Pos posData, Transaction transaction, ReceiptType receiptType, ObservableCollection<Product> products)
        {
            _receiptType = receiptType;
            Pos = posData;
            Transaction = transaction;
            _products = products;
        }

        #endregion

        #region Methods

        public void PrintSalesReceipt()
        {
            bool printToFileOnly = false;

            PrintDocument printDocument = new PrintDocument();

            printDocument.PrintPage += new PrintPageEventHandler(printReceipt);

            var fullReceiptName = Constants.DataFolderPath + Constants.ReceiptBackupFolderPath + "Recibo" +
                                  Pos.LastReceiptNumber.ToString() + "In" + Pos.LastInternalNumber + "Tr" +
                                  Pos.LastTransactionNumber + ".xps";

            if (!printToFileOnly)
            {
                printDocument.PrinterSettings.PrinterName = Pos.PrinterName;

                printDocument.Print();
            }

            try
            {
                printDocument.PrinterSettings.PrinterName = "Microsoft XPS Document Writer";//"CutePDF Writer";//"Microsoft Print to PDF";
                printDocument.PrinterSettings.PrintToFile = true;
                printDocument.PrinterSettings.PrintFileName = fullReceiptName;
                printDocument.Print();
            }
            catch (Exception)
            {

            }

            _products.Clear();
            printDocument.PrintPage -= printReceipt;
        }

        public void printReceipt(object sender, PrintPageEventArgs e)
        {
            Graphics graphic = e.Graphics;

            Font font = new Font("Courier New", 9, System.Drawing.FontStyle.Bold);
            Font storeNameFont = new Font("Courier New", 14, System.Drawing.FontStyle.Bold);
            Font storeInfoFont = new Font("Courier New", 8, System.Drawing.FontStyle.Bold);

            var buf = string.Empty;
            float fontHeight = font.GetHeight();
            float storeNameFontHeight = storeNameFont.GetHeight();
            float storeInfoFontHeight = storeInfoFont.GetHeight();

            int startX = 5;
            int startY = 5;
            int offset = 15;

            int itemsNumber = 0;

            buf = "   " + Pos.BusinessName;
            graphic.DrawString(buf, storeNameFont, new SolidBrush(Color.Black), startX,
                startY + offset);
            offset = offset + (int)storeNameFontHeight;
            buf = "    " + Pos.FiscalName;
            graphic.DrawString(buf, storeInfoFont, new SolidBrush(Color.Black), startX,
                startY + offset);
            offset = offset + (int)storeInfoFontHeight;
            buf = "  " + Pos.FiscalStreetAddress;
            graphic.DrawString(buf, storeInfoFont, new SolidBrush(Color.Black),
                startX, startY + offset);
            offset = offset + (int)storeInfoFontHeight;
            buf = "        " + Pos.FiscalCityAndZipCode;
            graphic.DrawString(buf, storeInfoFont, new SolidBrush(Color.Black), startX,
                startY + offset);
            offset = offset + (int)storeInfoFontHeight;
            buf = "   " + Pos.FiscalPhoneNumber + " " + Pos.FiscalType + " " + Pos.FiscalNumber;
            graphic.DrawString(buf, storeInfoFont, new SolidBrush(Color.Black), startX,
                startY + offset);
            offset = offset + (int)storeInfoFontHeight;
            buf = "       " + Pos.FiscalEmail;
            graphic.DrawString(buf, storeInfoFont, new SolidBrush(Color.Black), startX,
                startY + offset);
            offset = offset + (int)storeInfoFontHeight + 10;

            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-MX");
            var date = DateTime.Now.ToString("g");
            var ticketNumber = "No. " + Pos.LastReceiptNumber.ToString();
            graphic.DrawString(ticketNumber.PadRight(18) + date, storeInfoFont, new SolidBrush(Color.Black), startX,
                startY + offset);
            offset = offset + (int)storeInfoFontHeight + 10;

            foreach (var product in _products)
            {
                string productDescription = product.Category.PadRight(15);
                string productTotal = string.Format("{0:c}", product.Price);
                string productLine = productDescription + productTotal;
                //product line
                graphic.DrawString(product.ToString(), font, new SolidBrush(Color.Black), startX, startY + offset);

                offset = offset + (int)fontHeight;// + 5;

                itemsNumber = itemsNumber + product.LastQuantitySold;
            }

            offset = offset + 20;
            graphic.DrawString("Total: ".PadLeft(20) + string.Format("{0:c}", Transaction.TotalDue), font,
                new SolidBrush(Color.Black), startX, startY + offset);

            offset = offset + (int)fontHeight;// + 5;

            graphic.DrawString("Efectivo: ".PadLeft(20) + string.Format("{0:c}", Transaction.AmountPaid), font,
                new SolidBrush(Color.Black), startX, startY + offset);

            offset = offset + (int)fontHeight;// + 5;

            graphic.DrawString("Cambio: ".PadLeft(20) + string.Format("{0:c}", Transaction.ChangeDue), font,
                new SolidBrush(Color.Black), startX, startY + offset);

            offset = offset + (int)fontHeight;// + 10;

            graphic.DrawString(("Articulos: " + itemsNumber.ToString()).PadLeft(21), font,
                new SolidBrush(Color.Black), startX, startY + offset);


            offset = offset + (int)fontHeight + 5;// + 15;

            graphic.DrawString("       Gracias por su visita!", font, new SolidBrush(Color.Black), startX,
                startY + offset);
            offset = offset + (int)storeInfoFontHeight;

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
        RegularSale,
        DailyRegular,
        DailyInternal,
        DailyInformative,
        Weekly,
        Monthly,
        Unknown
    }

}