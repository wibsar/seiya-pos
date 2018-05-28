using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Seiya
{
    /// <summary>
    /// Class for products to be used in the inventory and point of sale system
    /// </summary>
    public class Product
    {
        #region Fields
        private BitmapImage _image;
        #endregion

        #region Properties

        public string Name { get; set; }
        public int Id { get; set; }
        public string Code { get; set; }
        public string AlternativeCode { get; set; }
        public string Provider { get; set; }
        public string ProviderProductId { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public string Category { get; set; }
        public decimal Cost { get; set; }
        public string CostCurrency { get; set; }
        public decimal Price { get; set; }
        public string PriceCurrency { get; set; }
        public int MinimumStockQuantity { get; set; }
        public decimal AmountSold { get; set; }
        public int InternalQuantity { get; set; }
        public int QuantitySold { get; set; } 
        public int LocalQuantityAvailable { get; set; }
        public int TotalQuantityAvailable { get; set; }
        public string ImageName { get; set; }

        public BitmapImage Image
        {
            get
            {
                BitmapImage bitmap = new BitmapImage();
                if (ImageName != null)
                {
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(@"C:\Projects\seiya-pos\Data\Images\" + ImageName);
                    bitmap.EndInit();
                    _image = bitmap;
                }
                return bitmap;
            }
            set
            {
                _image = value;
            }
        }
        public DateTime LastPurchaseDate { get; set; }
        public DateTime LastSaleDate { get; set; }
        public int LastQuantitySold { get; set; }

        public decimal LastAmountSold
        {
            get { return Price * LastQuantitySold; }
        }

        #endregion

        #region Constructors

        #endregion

        #region Methods

        //Create a basic product with minimal information for manual transactions
        public static Product Add(string description, string category, decimal soldPrice, int lastQuantitySold)
        {
            return new Product()
            {
                Description = description,
                Category = category,
                Price = soldPrice,
                LastQuantitySold = lastQuantitySold
            };
        }

        //Format for transaction log and display with category
        public override string ToString()
        {
            return string.Format("{0,-8}", LastQuantitySold) + Category.PadRight(10) + string.Format("{0,-11:c}", Price) + string.Format("{0,-11:c}", Price * LastQuantitySold);
        }

        //Format for transaction log and display with description
        public string ToString(bool detail)
        {
            string trimmedDescription = Description;
            if (Description.Length > 16)
            {
                trimmedDescription = Description.Substring(0, 16);
            }
            return string.Format("{0,-4}", LastQuantitySold) + trimmedDescription.PadRight(18) + string.Format("{0,-10:c}", Price) + string.Format("{0,-10:c}", Price * LastQuantitySold);
        }

        //Format for receipts with basic information
        public string ToString(ReceiptType receiptType)
        {
            return string.Format("{0,-5}", LastQuantitySold) + Category.PadRight(15) + string.Format("{0,-11:c}", Price);
        }

        //Calculate product margin
        public decimal GetMargin()
        {
            if (Price > 0)
            {
                return 100M * this.GetProfit() / Price;
            }
            return -1;
        }

        //Calculate product margin
        public decimal GetProfit()
        {
            if (CostCurrency == "USD")
            {
                return Price - Cost * 20;
            }
            return Price - Cost;
        }

        #endregion

    }
}
