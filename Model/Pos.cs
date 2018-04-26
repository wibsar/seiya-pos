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
    public class Pos
    {
        //TODO: move internal files to different directory

        #region Fields
        private readonly string _systemDataFilePath;
        private DataTable _dictofdata;
        #endregion

        #region FilePaths
        private const string _clientsDataFilePath = @"C:\Users\Public\Documents\mxData\Data\Clientes.csv";
        private const string _categoryCatalog = @"C:\Users\Public\Documents\mxData\Data\CategoryCatalog.txt";
        private const string _corteZDataFilePath = @"C:\Users\Public\Documents\mxData\Data\CorteZ.csv";
        private const string _inventoryDataFilePath = @"C:\Users\Public\Documents\mxData\Data\Inventario.csv";
        private const string _ordersDataFilePath = @"C:\Users\Public\Documents\mxData\Data\Pedidos.csv";
        private const string _posDataFilePath = @"C:\Users\Public\Documents\mxData\Data\PosData.csv";
        private const string _vendorsDatafilePath = @"C:\Users\Public\Documents\mxData\Data\Provedores.csv";
        private const string _transactionsDataFilePath = @"C:\Users\Public\Documents\mxData\Data\Transacciones.csv";
        private const string _transactionMasterDataFilePath = @"C:\Users\Public\Documents\mxData\Data\TransaccionesMaster.csv";
        private const string _transactionHistoryDataFilePath = @"C:\Users\Public\Documents\mxData\Data\TransaccionesHistorial.csv";
        private const string _transactionTypes = @"C:\Users\Public\Documents\mxData\Data\TransactionTypes.txt";
        private const string _users = @"C:\Users\Public\Documents\mxData\Data\Users.csv";
        private const string _inventoryBackUpPath = @"C:\Users\Public\Documents\mxData\Data\InventoryBackUp\";
        private const string _receiptCustomerBackUpPath = @"C:\Users\Public\Documents\mxData\Data\ReceiptCustomerBackUp\";
        private const string _receiptMasterBackUpPath = @"C:\Users\Public\Documents\mxData\Data\ReceiptMasterBackUp\";
        private const string _transactionBackUpPath = @"C:\Users\Public\Documents\mxData\Data\TransactionBackUp\";

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

        public int GetNextReceiptNumber()
        {
            var row = _dictofdata.Rows[0];
            LastReceiptNumber = LastReceiptNumber + 1;
            row["UltimoReciboNumero"] = LastReceiptNumber;
            return LastReceiptNumber;
        }

        public int GetNextCorteZNumber()
        {
            var row = _dictofdata.Rows[0];
            LastCorteZNumber = LastCorteZNumber + 1;
            row["UltimoCorteZNumero"] = LastCorteZNumber;
            return LastCorteZNumber;
        }

        public int GetNextTransactionNumber()
        {
            var row = _dictofdata.Rows[0];
            LastTransactionNumber = LastTransactionNumber + 1;
            row["UltimoTransaccionNumero"] = LastTransactionNumber;
            return LastTransactionNumber;
        }

        public int GetNextInternalNumber()
        {
            var row = _dictofdata.Rows[0];
            LastInternalNumber = LastInternalNumber + 1;
            row["UltimoNumeroInterno"] = LastInternalNumber;
            return LastInternalNumber;
        }

        public int GetNextOrderNumber()
        {
            var row = _dictofdata.Rows[0];
            LastOrderNumber = LastOrderNumber + 1;
            row["UltimoNumeroPedido"] = LastOrderNumber;
            return LastOrderNumber;
        }

        public void UpdateExchangeRate(decimal newExchangeRate)
        {
            var row = _dictofdata.Rows[0];
            ExchangeRate = newExchangeRate;
            row["TipoCambio"] = ExchangeRate;
        }

        public void UpdatePrinterName(string newPrinterName)
        {
            var row = _dictofdata.Rows[0];
            PrinterName = newPrinterName;
            row["NombreImpresora"] = PrinterName;
        }
        #endregion

    }
}
