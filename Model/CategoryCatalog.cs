using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Seiya
{
    public class CategoryCatalog
    {
        //Load category list from file
        public static List<string> categories {get; set;}

        public CategoryCatalog(string filePath)
        {
            //Read file
            try
            {
                var lines = File.ReadAllLines(filePath);
                categories = lines.ToList();               
            }
            catch (Exception e)
            {
                var lines = new List<string>();
                lines.Add("Varios");
                categories = lines.ToList();
            }
        }

        public static List<string> GetList(string filePath)
        {
            //Read file
            try
            {
                var lines = File.ReadAllLines(filePath);
                categories = lines.ToList();
                return categories;
            }
            catch (Exception e)
            {
                var lines = new List<string>();
                lines.Add("Varios");
                categories = lines.ToList();
                return categories;
            }
        }

        //TODO: Check if it is a good implementation; might need to check if there is an instance of inventory or create a singleton
        //Inventory
        public static List<Product> GetProductList(string filePath, out string listName)
        {
            var productList = new List<Product>();

            if (Inventory._inventory != null)
            {
                //Get codes from product lists
                var list = CategoryCatalog.GetList(filePath);
                //Skip first line, which is title of the list
                for (int i = 1; i < list.Count; ++i)
                {
                    productList.Add(Inventory._inventory.GetProduct(list[i]));
                }
                listName = list.First();
                return productList;
            }
            else
            {
                listName = "Lista";
                return productList;
            }
        }
        /// <summary>
        /// Update products list file with new changes
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool UpdateProductListFile(string filePath, List<Product> products, string listName)
        {
            //Creates or overwrites file
            StreamWriter writer = File.CreateText(filePath);
            //Write list name
            writer.WriteLine(listName);
            //Write code for each item
            foreach (var product in products)
            {
                writer.WriteLine(product.Code);
            }
            writer.Close();
            writer.Dispose();

            return true;
        }

    }
}
