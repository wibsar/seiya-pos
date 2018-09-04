using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seiya
{
    public struct TransactionDataStruct
    {
        public List<Tuple<string, int, decimal>> SalesInfoPerCategory;
        public TransactionType TransactionType;
        public int FirstReceiptNumber { get; set; }
        public int LastReceiptNumber { get; set; }
        public int TotalItemsSold { get; set; }
        public decimal TotalAmountSold { get; set; }
        public decimal CashTotal { get; set; }
        public decimal CardTotal { get; set; }
        public decimal CheckTotal { get; set; }
        public decimal BankTotal { get; set; }
        public decimal OtherTotal { get; set; }
        public double PointsTotal { get; set; }
        public decimal ReturnsTotal { get; set; }
    }
}