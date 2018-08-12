﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericParsing;
using System.Data;
using System.IO;

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

        /// <summary>
        /// Load CSV database into a datatable object
        /// </summary>
        public static DataTable LoadCsvToDataTable(string csvFilePath)
        {
            using (var parser = new GenericParserAdapter(csvFilePath))
            {
                parser.ColumnDelimiter = ',';
                parser.FirstRowHasHeader = true;
                parser.SkipStartingDataRows = 0;
                parser.SkipEmptyRows = true;
                parser.MaxBufferSize = 4096;
                parser.MaxRows = 8000;

                return parser.GetDataTable();
            }
        }

        /// <summary>
        /// Save datatable object in a CSV file
        /// </summary>
        public static void SaveDataTableToCsv(string csvFilePath, DataTable dataTable)
        {
            StringBuilder sb = new StringBuilder();

            IEnumerable<string> columnNames = dataTable.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName);
            sb.AppendLine(string.Join(",", columnNames));

            foreach (DataRow row in dataTable.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                sb.AppendLine(string.Join(",", fields));
            }
            File.WriteAllText(csvFilePath, sb.ToString());
        }
    }
}
