using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Athena;

namespace Seiya
{
    /// <summary>
    /// Class for products to be used in the inventory and point of sale system
    /// </summary>
    public class CarPart : DataBase
    {
        #region Fields
        private BitmapImage _image;
        private DateTime _registrationDate;
        private DateTime _lastSaleDate;
        #endregion

        #region Properties

        public int Id { get; set; }
        public string Code { get; set; }
        public string Vin { get; set; }
        public string Model { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public CurrencyTypeEnum PriceCurrency { get; set; }
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
                    bitmap.UriSource = new Uri(@"..\Data\Images\" + ImageName);
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

        public DateTime RegistrationDate
        {
            get { return _registrationDate; }
            set { _registrationDate = value; }
        }

        public string RegistrationDateString
        {
            get { return _registrationDate.ToString("d"); }
        }

        public bool Enabled { get; set; }

        #endregion

        #region Constructors

        public CarPart(string dbPath) : base(dbPath)
        {
            
        }

        public CarPart(string dbPath, CarPart car) : base(dbPath)
        {
            //Basic
            //this.Id = product.Id;
            //this.Code = product.Code;
        }

        #endregion

        #region Methods

        #endregion

    }

    public class CarInteriorParts
    {
        public List<string> items;
    }
}
