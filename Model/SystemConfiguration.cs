using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zeus;

namespace Seiya
{
    class SystemConfiguration : ISystemConfiguration
    {
        private string _server = "wibsarlicencias.csqn2onotlww.us-east-1.rds.amazonaws.com";
        private string _dataBaseName = "EstrellaTest";
        private string _userId = "armoag";
        private string _password = "Yadira00";
        private string _customerTableName = "Clientes";
        private string _inventoryTableName = "Inventario";
        private string _queueTableName = "Queue";
        
        public string Server
        {
            get { return _server; }
            set { _server = value; }
        }

        public string DataBaseName
        {
            get { return _dataBaseName; }
            set { _dataBaseName = value; }
        }

        public string UserID
        {
            get { return _userId; }
            set { _userId = value; }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public string CustomerTableName
        {
            get { return _customerTableName; }
            set { _customerTableName = value; }
        }

        public string InventoryTableName
        {
            get { return _inventoryTableName; }
            set { _inventoryTableName = value; }
        }
        public string QueueTableName
        {
            get { return _queueTableName; }
            set { _queueTableName = value; }
        }

        public bool EmailTransactionsFileAfterEndSalesReport { get; set; } = true;
        public bool IntFlag { get; set; } = true;
        public bool LocalCustomers { get; set; } = false;
        public bool CloudCustomers { get; set; } = true;
        public bool LocalInventory { get; set; } = false;
        public bool CloudInventory { get; set; } = true;
        public bool IndirectPrice { get; set; } = false;
        public SystemTypeEnum SystemType { get; set; }
    }
}
