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
    /// <summary>
    /// Contains folder file paths and pos information such as current receipt number
    /// </summary>
    public class Pos
    {
        #region Fields
        private readonly string _systemDataFilePath;
        private DataTable _dictofdata;
        #endregion

        #region FilePaths

        private const string _clientsDataFilePath = Constants.DataFolderPath + Constants.ClientsFileName;
        private const string _categoryCatalog = Constants.DataFolderPath + Constants.CategoryListFileName;
        private const string _corteZDataFilePath = Constants.DataFolderPath + Constants.EndOfDaySalesFileName;
        private const string _inventoryDataFilePath = Constants.DataFolderPath + Constants.InventoryFileName;
        private const string _ordersDataFilePath = Constants.DataFolderPath + Constants.OrdersFileName;
        private const string _posDataFilePath = Constants.DataFolderPath + Constants.PosDataFileName;
        private const string _vendorsDatafilePath = Constants.DataFolderPath + Constants.VendorsFileName;
        private const string _transactionsDataFilePath = Constants.DataFolderPath + Constants.TransactionsFileName;
        private const string _transactionMasterDataFilePath = Constants.DataFolderPath + Constants.MasterTransactionsFileName;
        private const string _transactionHistoryDataFilePath = Constants.DataFolderPath + Constants.HistoryTransactionsFileName;
        private const string _transactionTypes = Constants.DataFolderPath + Constants.TransactionsTypesFileName;
        private const string _transactionBackUpPath = Constants.DataFolderPath + Constants.TransactionsBackupFolderPath;
        private const string _users = Constants.DataFolderPath + Constants.UsersFileName;
        private const string _inventoryBackUpPath = Constants.DataFolderPath + Constants.InventoryBackupFolderPath;
        private const string _receiptCustomerBackUpPath = Constants.DataFolderPath + Constants.ReceiptBackupFolderPath;
        private const string _receiptMasterBackUpPath = Constants.DataFolderPath + Constants.MasterReceiptBackupFolderPath;

        #endregion

        #region Properties

        public decimal ExchangeRate { get; set; }
        public string PrinterName { get; set; }
        public decimal LastCashierAmountMxn { get; set; }
        public decimal LastCashierAmountUsd { get; set; }
        public int LastReceiptNumber { get; set; }
        public int LastCorteZNumber { get; set; }
        public int LastTransactionNumber { get; set; }
        public int LastInternalNumber { get; set; }
        public int LastOrderNumber { get; set; }
       
        public string TransactionBackUpPath
        {
            get { return _transactionBackUpPath; }
        }

        public string ReceiptMasterBackUpPath
        {
            get { return _receiptMasterBackUpPath; }
        }

        public string ReceiptCustomerBackUpPath
        {
            get { return _receiptCustomerBackUpPath; }
        }

        public string InventoryBackUpPath
        {
            get { return _inventoryBackUpPath; }
        }

        public static string ClientsDataFilePath
        {
            get { return _clientsDataFilePath; }
        }

        public string Catalog
        {
            get { return _categoryCatalog; }
        }

        public string CorteZDataFilePath
        {
            get { return _corteZDataFilePath; }
        }

        public string InventoryDataFilePath
        {
            get { return _inventoryDataFilePath; }
        }

        public string OrdersDataFilePath
        {
            get { return _ordersDataFilePath; }
        }

        public string PosDataFilePath
        {
            get { return _posDataFilePath; }
        }

        public string VendorsDatafilePath
        {
            get { return _vendorsDatafilePath; }
        }

        public string TransactionsDataFilePath
        {
            get { return _transactionsDataFilePath; }
        }

        public string TransactionMasterDataFilePath
        {
            get { return _transactionMasterDataFilePath; }
        }

        public string TransactionHistoryDataFilePath
        {
            get { return _transactionHistoryDataFilePath; }
        }

        public string TransactionTypes
        {
            get { return _transactionTypes; }
        }

        public string Users
        {
            get { return _users; }
        }

        #endregion

        #region Constructor

        public Pos(string systemDataFilePath)
        {
            _systemDataFilePath = systemDataFilePath;
            InitializeData();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets data from csv database and saves it in a data dictionary
        /// </summary>
        private void InitializeData()
        {
            using (var parser = new GenericParserAdapter(_systemDataFilePath))
            {
                parser.ColumnDelimiter = ',';
                parser.FirstRowHasHeader = true;
                parser.SkipStartingDataRows = 0;
                parser.SkipEmptyRows = true;
                parser.MaxBufferSize = 4096;
                parser.MaxRows = 8000;

                _dictofdata = parser.GetDataTable();
            }

            for (int index = 0; index < _dictofdata.Rows.Count; index++)
            {
                var row = _dictofdata.Rows[index];
                ExchangeRate = decimal.Parse(row["TipoCambio"].ToString());
                LastCashierAmountMxn = decimal.Parse(row["CantidadPesosCaja"].ToString());
                LastCashierAmountUsd = decimal.Parse(row["CantidadDolarCaja"].ToString());
                LastReceiptNumber = Int32.Parse(row["UltimoReciboNumero"].ToString());
                LastCorteZNumber = Int32.Parse(row["UltimoCorteZNumero"].ToString());
                LastTransactionNumber = Int32.Parse(row["UltimoTransaccionNumero"].ToString());
                LastInternalNumber = Int32.Parse(row["UltimoNumeroInterno"].ToString());
                LastOrderNumber = Int32.Parse(row["UltimoNumeroPedido"].ToString());
                PrinterName = row["NombreImpresora"].ToString();
            }
        }

        /// <summary>
        /// Method to write pos data from memory to csv file
        /// </summary>
        public void UpdateAllData()
        {
            StringBuilder sb = new StringBuilder();

            IEnumerable<string> columnNames = _dictofdata.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName);
            sb.AppendLine(string.Join(",", columnNames));

            foreach (DataRow row in _dictofdata.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                sb.AppendLine(string.Join(",", fields));
            }
            File.WriteAllText(_systemDataFilePath, sb.ToString());
        }

        /// <summary>
        /// Method to get the next receipt number assigned
        /// </summary>
        /// <returns></returns>
        public int GetNextReceiptNumber()
        {
            var row = _dictofdata.Rows[0];
            LastReceiptNumber = LastReceiptNumber + 1;
            row["UltimoReciboNumero"] = LastReceiptNumber;
            return LastReceiptNumber;
        }

        /// <summary>
        /// Method to get the next end of sales receipt number
        /// </summary>
        /// <returns></returns>
        public int GetNextCorteZNumber()
        {
            //TODO: Change method name to use End of Sales instead of CorteZ
            var row = _dictofdata.Rows[0];
            LastCorteZNumber = LastCorteZNumber + 1;
            row["UltimoCorteZNumero"] = LastCorteZNumber;
            return LastCorteZNumber;
        }

        /// <summary>
        /// Get next transaction number
        /// </summary>
        /// <returns></returns>
        public int GetNextTransactionNumber()
        {
            var row = _dictofdata.Rows[0];
            LastTransactionNumber = LastTransactionNumber + 1;
            row["UltimoTransaccionNumero"] = LastTransactionNumber;
            return LastTransactionNumber;
        }

        /// <summary>
        /// Get next internal transaction number
        /// </summary>
        /// <returns></returns>
        public int GetNextInternalNumber()
        {
            var row = _dictofdata.Rows[0];
            LastInternalNumber = LastInternalNumber + 1;
            row["UltimoNumeroInterno"] = LastInternalNumber;
            return LastInternalNumber;
        }

        /// <summary>
        /// Returns next order number
        /// </summary>
        /// <returns></returns>
        public int GetNextOrderNumber()
        {
            var row = _dictofdata.Rows[0];
            LastOrderNumber = LastOrderNumber + 1;
            row["UltimoNumeroPedido"] = LastOrderNumber;
            return LastOrderNumber;
        }

        /// <summary>
        /// Updates exchange rate in memory
        /// </summary>
        /// <param name="newExchangeRate"></param>
        public void UpdateExchangeRate(decimal newExchangeRate)
        {
            var row = _dictofdata.Rows[0];
            ExchangeRate = newExchangeRate;
            row["TipoCambio"] = ExchangeRate;
        }

        /// <summary>
        /// Updates printer name in memory
        /// </summary>
        /// <param name="newPrinterName"></param>
        public void UpdatePrinterName(string newPrinterName)
        {
            var row = _dictofdata.Rows[0];
            PrinterName = newPrinterName;
            row["NombreImpresora"] = PrinterName;
        }
        #endregion
    }
}
