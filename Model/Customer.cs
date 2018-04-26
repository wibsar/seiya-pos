using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seiya
{
    //TODO: Implement this class
    public class Customer
    {
        #region Properties
        public int Number { get; set; }
        public string Name { get; set; }
        public int Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime LastPurchaseDate { get; set; }
        public int TotalPurchases { get; set; }
        public decimal TotalSpent { get; set; }
        #endregion

        #region Fields
        private string _customerFilePath;
        #endregion

        #region Constructors

        public Customer()
        {

        }
        #endregion

        #region Methods
        public void Add(int customerNumber, string customerName, int phone, string email, string address, DateTime initialDate, DateTime lastPurchaseDate, int totalPurchases, decimal totalSpent)
        {
            string data = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", customerNumber, customerName, phone, email, address, InitialDate, lastPurchaseDate, totalPurchases, totalSpent) + Environment.NewLine;

            File.AppendAllText(_customerFilePath, data);
        }

        public static Customer Add(string name)
        {
            return new Customer() { Name = name };
        }

        public void Search()
        {
             
        }

        #endregion
    }
}
