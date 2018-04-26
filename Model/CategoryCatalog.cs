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
    }
}
