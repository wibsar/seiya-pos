using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seiya
{
    public static class Utilities
    {
        public static IEnumerable<Product> MoveListItemUp(IEnumerable<Product> item, int selectedItemIndex)
        {
            var updatedList = item.ToList();
            var tempItem = updatedList[selectedItemIndex];
            if (selectedItemIndex < updatedList.Count && selectedItemIndex > 0)
            {
                updatedList[selectedItemIndex] = updatedList[selectedItemIndex - 1];
                updatedList[selectedItemIndex - 1] = tempItem;
            }
            return updatedList;
        }

        public static IEnumerable<Product> MoveListItemDown(IEnumerable<Product> item, int selectedItemIndex)
        {
            var updatedList = item.ToList();
            var tempItem = updatedList[selectedItemIndex];
            if (selectedItemIndex < updatedList.Count - 1)
            {
                updatedList[selectedItemIndex] = updatedList[selectedItemIndex + 1];
                updatedList[selectedItemIndex + 1] = tempItem;
            }
            return updatedList;
        }

        public static IEnumerable<Product> DeleteListItem(IEnumerable<Product> item, int selectedItemIndex)
        {
            var updatedList = item.ToList();
            var tempItem = updatedList[selectedItemIndex];
            updatedList.RemoveAt(selectedItemIndex);
            return updatedList;
        }

        public static IEnumerable<Product> AddListItem(IEnumerable<Product> item, Product newItem, int selectedItemIndex)
        {
            var updatedList = item.ToList();
            if(updatedList.Count < 20)
            {
                updatedList.Insert(selectedItemIndex + 1, newItem);
            }
            var tempItem = updatedList[selectedItemIndex];
            updatedList.RemoveAt(selectedItemIndex);
            return updatedList;
        }
    }
}
