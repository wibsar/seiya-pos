using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seiya
{
    public class Formatter
    {
        /// <summary>
        /// Remove commas from string input
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string SanitizeInput(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            //TODO: What happens if there is no ,
            return input.Replace(",", ".");
        }
    }
}
