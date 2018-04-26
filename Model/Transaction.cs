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
    class Transaction
    {
        #region Fields

        private string _transactionFilePath;
        private string _transactionMasterFilePath;
        private string _transactionHistoryFilePath;

        #endregion

        #region Properties

        public DateTime TransactionDate { get; set; }
        public string TranscationFilePath { get; set; }
        public List<string> TransactionTypes { get; set; }

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

        public Transaction(string transactionFilePath, string transactionMasterFilePath, string transactionHistoryFilePath, bool datafile)
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

        //Method to add transaction details to transaction files
        public void Add(int transactionNumber, int internalNumber, int receiptNumber, string productCode, int productNumber, string productCategory, string productDescription, decimal soldPrice, int soldUnits, decimal totalSold, DateTime transactionDate, string customerName, string userName, string fiscalReceiptReq, string saleType, PaymentType paymentType, int orderNumber)
        {
            string data;

            if (saleType != "Devolucion")
            {
                data = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16}", transactionNumber, internalNumber, receiptNumber, productCode, productNumber, productCategory, productDescription, soldPrice, soldUnits, totalSold, TransactionDate, customerName, userName, fiscalReceiptReq, saleType, paymentType.ToString(), orderNumber) + Environment.NewLine;
            }
            else
            {
                data = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16}", transactionNumber, internalNumber, receiptNumber, productCode, productNumber, productCategory, productDescription, -1 * soldPrice, -1 * soldUnits, -1 * totalSold, TransactionDate, customerName, userName, fiscalReceiptReq, saleType, paymentType.ToString(), orderNumber) + Environment.NewLine;
            }

            //Append to daily master
            File.AppendAllText(_transactionMasterFilePath, data);
            //Append to history transaction file
            File.AppendAllText(_transactionHistoryFilePath, data);
            //Append to daily regular
            if (saleType == "Regular")
            {
                data = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14}", receiptNumber, productCode, productNumber, productCategory, productDescription, soldPrice, soldUnits, totalSold, TransactionDate, customerName, userName, fiscalReceiptReq, saleType, paymentType.ToString(), orderNumber) + Environment.NewLine;

                File.AppendAllText(_transactionFilePath, data);
            }
            else if (saleType == "Devolucion")
            {
                data = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14}", receiptNumber, productCode, productNumber, productCategory, productDescription, -1 * soldPrice, -1 * soldUnits, -1 * totalSold, TransactionDate, customerName, userName, fiscalReceiptReq, saleType, paymentType.ToString(), orderNumber) + Environment.NewLine;

                File.AppendAllText(_transactionFilePath, data);
            }
        }

        //Method to create a backup file
        public static void BackUpTransactionFile(string transactionFilePath)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-MX");
            var currentTime = DateTime.Now;
            //Load inventory csv file and create a backup copy
            string TransactionFileBackUpCopyName = @"C:\Users\Public\Documents\mxData\Data\TransactionBackUp\" + "Transacciones" + currentTime.Day.ToString("00") + currentTime.Month.ToString("00") + currentTime.Year.ToString("0000") + currentTime.Hour.ToString("00") + currentTime.Minute.ToString("00") + currentTime.Second.ToString("00") + ".csv";

            File.Copy(transactionFilePath, TransactionFileBackUpCopyName);
        }
        //Method to create a backup file
        public static void BackUpTransactionMasterFile(string transactionFilePath)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-MX");
            var currentTime = DateTime.Now;
            //Load inventory csv file and create a backup copy
            string TransactionFileBackUpCopyName = @"C:\Users\Public\Documents\mxData\Data\TransactionBackUp\" + "TransaccionesMaster" + currentTime.Day.ToString("00") + currentTime.Month.ToString("00") + currentTime.Year.ToString("0000") + currentTime.Hour.ToString("00") + currentTime.Minute.ToString("00") + currentTime.Second.ToString("00") + ".csv";

            File.Copy(transactionFilePath, TransactionFileBackUpCopyName);
        }

        //Method to clear file
        public static void ClearTransactionFile(string transactionFilePath)
        {
            File.Copy(@"C:\Users\Public\Documents\mxData\Data\TransactionBackUp\TransaccionesBlank.csv", @"C:\Users\Public\Documents\mxData\Data\Transacciones.csv", true);
        }

        //Method to clear file
        public static void ClearTransactionMasterFile(string transactionFilePath)
        {
            File.Copy(@"C:\Users\Public\Documents\mxData\Data\TransactionBackUp\TransaccionesMasterBlank.csv", @"C:\Users\Public\Documents\mxData\Data\TransaccionesMaster.csv", true);
        }

        #endregion
    }

    public enum PaymentType
    {
        Cash,
        Card,
        Unknown
    }

}
