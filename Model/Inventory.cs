using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using GenericParsing;

namespace Seiya
{
    public class Inventory
    {
        #region Fields

        public readonly static Inventory _inventory = null; 

        const string cantidadLocalCol = "CantidadLocal";
        DataTable _dictofdata;

        private string _filePath;

        #endregion

        #region Properties

        #endregion

        #region Constructors

        public Inventory(string filePath)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-MX");
            //Read inventory CSV format
            _filePath = filePath;
            LoadCsvToDataTable();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Load CSV database into a datatable object
        /// </summary>
        public void LoadCsvToDataTable()
        {
            using (var parser = new GenericParserAdapter(_filePath))
            {
                parser.ColumnDelimiter = ',';
                parser.FirstRowHasHeader = true;
                parser.SkipStartingDataRows = 0;
                parser.SkipEmptyRows = true;
                parser.MaxBufferSize = 4096;
                parser.MaxRows = 8000;

                _dictofdata = parser.GetDataTable();

            }
        }

        /// <summary>
        /// Save datatable object in a CSV file
        /// </summary>
        public void SaveDataTableToCsv()
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
            File.WriteAllText(_filePath, sb.ToString());
        }

        /// <summary>
        /// Query any item data from the code
        /// </summary>
        /// <param name="code">Code to find item</param>
        /// <param name="columnName">Column header to retrive the data</param>
        /// <returns></returns>
        public string QueryDataFromCode(string code, string columnName)
        {
            for (int index = 0; index < _dictofdata.Rows.Count; index++)
            {
                var row = _dictofdata.Rows[index];
                if (row["Codigo"].ToString() == code)
                {
                    return row[columnName].ToString();
                }
            }
            return string.Format("No se encontro el codigo {0}", code);
        }
        
        /// <summary>
        /// Get the last item number in the inventory
        /// </summary>
        /// <returns></returns>
        public int GetLastItemNumber()
        {
            var row = _dictofdata.Rows[_dictofdata.Rows.Count - 1];
            return Int32.Parse(row["NumeroProducto"].ToString());
        }

        /// <summary>
        /// Add new data to a specific item column name based on the code
        /// </summary>
        /// <param name="code"></param>
        /// <param name="columnName"></param>
        /// <param name="newData"></param>
        public void UpdateItem(string code, string columnName, string newData)
        {
            for (int index = 0; index < _dictofdata.Rows.Count; index++)
            {
                var row = _dictofdata.Rows[index];
                if (row["Codigo"].ToString() == code)
                {
                    row[columnName] = newData;
                    return;
                }
            }
        }

        /// <summary>
        /// Update the number of items sold
        /// </summary>
        /// <param name="code"></param>
        /// <param name="unitsSold"></param>
        public void UpdateSoldItemQuantity(string code, int unitsSold)
        {

            for (int index = 0; index < _dictofdata.Rows.Count; index++)
            {
                var row = _dictofdata.Rows[index];
                if (row["Codigo"].ToString() == code)
                {
                    int quantity = Int32.Parse(row[cantidadLocalCol].ToString());
                    row[cantidadLocalCol] = (quantity - unitsSold).ToString();
                    return;
                }
            }
        }

        /// <summary>
        /// Get product based on a code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Product GetProduct(string code)
        {
            try
            {
                for (int index = 0; index < _dictofdata.Rows.Count; index++)
                {
                    var row = _dictofdata.Rows[index];
                    if (row["Codigo"].ToString() == code)
                    {
                        return new Product()
                        {
                            Id = Int32.Parse(row["NumeroProducto"].ToString()),
                            Code = row["Codigo"].ToString(),
                            AlternativeCode = row["CodigoAlterno"].ToString(),
                            ProviderProductId = row["ProveedorProductoId"].ToString(),
                            Description = row["Descripcion"].ToString(),
                            Provider = row["Proveedor"].ToString(),
                            Category = row["Categoria"].ToString(),
                            LastPurchaseDate = Convert.ToDateTime(row["UltimoPedidoFecha"].ToString()),
                            Cost = Decimal.Parse(row["Costo"].ToString()),
                            CostCurrency = row["CostoMoneda"].ToString(),
                            Price = decimal.Parse(row["Precio"].ToString()),
                            PriceCurrency = row["PrecioMoneda"].ToString(),
                            InternalQuantity = Int32.Parse(row["CantidadInternoHistorial"].ToString()),
                            QuantitySold = Int32.Parse(row["CantidadVendidoHistorial"].ToString()),
                            LocalQuantityAvailable = Int32.Parse(row["CantidadLocal"].ToString()),
                            TotalQuantityAvailable = Int32.Parse(row["CantidadDisponibleTotal"].ToString()),
                            MinimumStockQuantity = Int32.Parse(row["CantidadMinima"].ToString()),                   
                            LastSaleDate = Convert.ToDateTime(row["UltimaTransaccionFecha"].ToString())
                        };
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error en el Codigo", "Error");

            }

            return new Product() { Description = "", Category = "", Cost = 0M };
        }

        /// <summary>
        /// Get product based on the description
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public Product GetProductFromDescription(string description)
        {
            try
            {
                for (int index = 0; index < _dictofdata.Rows.Count; index++)
                {
                    var row = _dictofdata.Rows[index];
                    if (row["Descripcion"].ToString() == description)
                    {
                        return new Product()
                        {
                            Id = Int32.Parse(row["NumeroProducto"].ToString()),
                            Code = row["Codigo"].ToString(),
                            AlternativeCode = row["CodigoAlterno"].ToString(),
                            ProviderProductId = row["ProveedorProductoId"].ToString(),
                            Description = row["Descripcion"].ToString(),
                            Provider = row["Proveedor"].ToString(),
                            Category = row["Categoria"].ToString(),
                            LastPurchaseDate = Convert.ToDateTime(row["UltimoPedidoFecha"].ToString()),
                            Cost = Decimal.Parse(row["Costo"].ToString()),
                            CostCurrency = row["CostoMoneda"].ToString(),
                            Price = decimal.Parse(row["Precio"].ToString()),
                            PriceCurrency = row["PrecioMoneda"].ToString(),
                            InternalQuantity = Int32.Parse(row["CantidadInternoHistorial"].ToString()),
                            QuantitySold = Int32.Parse(row["CantidadVendidoHistorial"].ToString()),
                            LocalQuantityAvailable = Int32.Parse(row["CantidadLocal"].ToString()),
                            TotalQuantityAvailable = Int32.Parse(row["CantidadDisponibleTotal"].ToString()),
                            MinimumStockQuantity = Int32.Parse(row["CantidadMinima"].ToString()),
                            LastSaleDate = Convert.ToDateTime(row["UltimaTransaccionFecha"].ToString())
                        };
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error en el Codigo", "Error");
            }

            return new Product() { Description = "", Category = "", Cost = 0M };
        }

        /// <summary>
        /// Update the sold product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public bool UpdateSoldProductToTable(Product product)
        {
            for (int index = 0; index < _dictofdata.Rows.Count; index++)
            {
                var row = _dictofdata.Rows[index];
                if (row["Codigo"].ToString() == product.Code)
                {
                    row["CantidadDisponibleTotal"] = product.TotalQuantityAvailable.ToString();
                    row["Precio"] = product.Price.ToString();
                    row["CantidadVendidoHistorial"] = product.QuantitySold.ToString();
                    row["VendidoHistorial"] = product.AmountSold.ToString();
                    row["CantidadInternoHistorial"] = product.InternalQuantity.ToString();
                    row["CantidadLocal"] = product.LocalQuantityAvailable.ToString();
                    row["UltimaTransaccionFecha"] = product.LastSaleDate.ToString();
                }
            }

            return true;
        }

        /// <summary>
        /// Update product in the datatable
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public bool UpdateProductToTable(Product product)
        {
            for (int index = 0; index < _dictofdata.Rows.Count; index++)
            {
                var row = _dictofdata.Rows[index];
                if (row["Codigo"].ToString() == product.Code)
                {
                    row["NumeroProducto"] = product.Id.ToString();
                    row["CodigoAlterno"] = product.AlternativeCode;
                    row["ProveedorProductoId"] = product.ProviderProductId;
                    row["Descripcion"] = product.Description;
                    row["Proveedor"] = product.Provider;
                    row["Categoria"] = product.Category;
                    row["Costo"] = product.Cost.ToString(CultureInfo.InvariantCulture);
                    row["CostoMoneda"] = product.CostCurrency;
                    row["Precio"] = product.Price.ToString();
                    row["PrecioMoneda"] = product.PriceCurrency.ToString();
                    row["CantidadInternoHistorial"] = product.InternalQuantity.ToString();
                    row["CantidadVendidoHistorial"] = product.QuantitySold.ToString();
                    row["CantidadLocal"] = product.LocalQuantityAvailable.ToString();
                    row["CantidadDisponibleTotal"] = product.TotalQuantityAvailable.ToString();
                    row["CantidadMinima"] = product.MinimumStockQuantity.ToString();
                    row["UltimoPedidoFecha"] = product.LastPurchaseDate.ToString();
                    row["UltimaTransaccionFecha"] = product.LastSaleDate.ToString();
                }
            }

            return true;
        }

        /// <summary>
        /// Add new product to data table
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public bool AddNewProductToTable(Product product)
        {
            _dictofdata.Rows.Add();
            var row = _dictofdata.Rows[_dictofdata.Rows.Count - 1];
            row["NumeroProducto"] = product.Id.ToString();
            row["CodigoAlterno"] = product.AlternativeCode;
            row["ProveedorProductoId"] = product.ProviderProductId;
            row["Descripcion"] = product.Description;
            row["Proveedor"] = product.Provider;
            row["Categoria"] = product.Category;
            row["Costo"] = product.Cost.ToString(CultureInfo.InvariantCulture);
            row["CostoMoneda"] = product.CostCurrency;
            row["Precio"] = product.Price.ToString();
            row["PrecioMoneda"] = product.PriceCurrency;
            row["CantidadInternoHistorial"] = product.InternalQuantity.ToString();
            row["CantidadVendidoHistorial"] = product.QuantitySold.ToString();
            row["CantidadLocal"] = product.LocalQuantityAvailable.ToString();
            row["CantidadDisponibleTotal"] = product.TotalQuantityAvailable.ToString();
            row["VendidioHistorial"] = product.AmountSold.ToString();
            row["CantidadMinima"] = product.MinimumStockQuantity.ToString();
            row["UltimoPedidoFecha"] = product.LastPurchaseDate.ToString();
            row["UltimaTransaccionFecha"] = product.LastSaleDate.ToString();

            return true;
        }
        
        /// <summary>
        /// Create a copy of the inventory file
        /// </summary>
        /// <param name="filePath"></param>
        public static void InventoryBackUp(string filePath)
        {
            //Set date format
            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-MX");
            var currentTime = DateTime.Now;
            //Load inventory csv file and create a backup copy
            string InventoryFileBackUpCopyName = @"C:\Users\Public\Documents\mxData\Data\InventoryBackUp\" + "Inventario" + currentTime.Day.ToString("00") + currentTime.Month.ToString("00") + currentTime.Year.ToString("0000") + currentTime.Hour.ToString("00") + currentTime.Minute.ToString("00") + currentTime.Second.ToString("00") + ".csv";

            File.Copy(filePath, InventoryFileBackUpCopyName);
        }

        #endregion  
    }
}
