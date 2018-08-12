using System.Collections.Generic;
using System.Data;

namespace Seiya
{
    public interface IDataBase
    {
        bool AddNewItemToTable(object product);
        object GetProduct(string searchInput);
    }
}