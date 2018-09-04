using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seiya
{
    public class Expense : DataBase
    {
        #region Fields
        private int _id;
        private string _vendor;
        private string _ticketNumber;
        private CurrencyTypeEnum _currency;
        private decimal _amount;
        private string _description;
        private string _expenseCategory;
        private PaymentTypeEnum _paymentType;
        private string _expensesFilePath;
        private string _user;
        private DateTime _date;
        #endregion

        #region Constructors
        public Expense(string expensesFilePath) : base(expensesFilePath)
        {
            //TODO: Check if path exists
            ExpensesFilePath = expensesFilePath;
            LoadCsvToDataTable();
        }

        public Expense(string expensesFilePath, int id, string user, string vendor, string description, decimal amount, 
            CurrencyTypeEnum currencyType, PaymentTypeEnum paymentType, string expenseCategory = "General") : base(expensesFilePath)
        {
            Id = id;
            ExpensesFilePath = expensesFilePath;
            User = user;
            Vendor = vendor;
            Description = description;
            Amount = amount;
            CurrencyType = currencyType;
            PaymentType = paymentType;
            ExpenseCategory = expenseCategory;
        }

        #endregion

        #region Properties
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }
        public string Vendor
        {
            get
            {
                return _vendor;
            }
            set
            {
                _vendor = value;
            }
        }
        public string TicketNumber
        {
            get
            {
                return _ticketNumber;
            }
            set
            {
                _ticketNumber = value;
            }
        }
        public CurrencyTypeEnum CurrencyType
        {
            get
            {
                return _currency;
            }
            set
            {
                _currency = value;
            }
        }

        public string CurrencyTypeString
        {
            get
            {
                return _currency.ToString();
            }
        }
        public decimal Amount
        {
            get
            {
                return _amount;
            }
            set
            {
                _amount = value;
            }
        }
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }
        public PaymentTypeEnum PaymentType
        {
            get
            {
                return _paymentType;
            }
            set
            {
                _paymentType = value;
            }
        }

        public string PaymentTypeString
        {
            get { return _paymentType.ToString(); }
        }
        public string ExpensesFilePath
        {
            get
            {
                return _expensesFilePath;
            }
            set
            {
                _expensesFilePath = value;
            }
        }
        public string User
        {
            get
            {
                return _user;
            }
            set
            {
                _user = value;
            }
        }
        public string ExpenseCategory
        {
            get
            {
                return _expenseCategory;
            }
            set
            {
                _expenseCategory = value;
            }
        }
        public DateTime Date
        {
            get
            {
                return _date;
            }
            set
            {
                _date = value;
            }
        }

        #endregion

        #region Instance Methods

        public void Register()
        {
            var data = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}", Id.ToString(), User, Vendor, TicketNumber, Description, Amount.ToString(), 
                CurrencyType.ToString(), PaymentType.ToString(), ExpenseCategory, DateTime.Now);
            
            try
            {
                File.AppendAllText(ExpensesFilePath, data);
            }
            catch (Exception e)
            {
                //Report Error in log file
            }
        }

        public List<Expense> Search(string searchInput)
        {
            var expenses = new List<Expense>();

            //Return empty list if invalid inputs are entered for the search
            if (string.IsNullOrWhiteSpace(searchInput) || searchInput == "x")
                return expenses;

            if (searchInput == "*")
            {
                var allFields = base.DataTable.AsEnumerable();
                foreach (var row in allFields)
                {
                    var expense = new Expense(ExpensesFilePath)
                    {
                        Id = Int32.Parse(row["Id"].ToString()),
                        User = row["Usuario"].ToString(),
                        Vendor = row["Proveedor"].ToString(),
                        TicketNumber = row["NumeroTicket"].ToString(),
                        Description = row["Descripcion"].ToString(),
                        Amount = Decimal.Parse(row["Monto"].ToString()),
                        ExpenseCategory = row["CategoriaGasto"].ToString(),
                        Date = Convert.ToDateTime(row["Fecha"].ToString())

                    };
                    if (row["Moneda"].ToString().ToUpper() == "USD")
                        expense.CurrencyType = CurrencyTypeEnum.USD;
                    else
                        expense.CurrencyType = CurrencyTypeEnum.MXN;

                    expense.PaymentType = (PaymentTypeEnum)Enum.Parse(typeof(PaymentTypeEnum),row["MetodoPago"].ToString(), true);

                    expenses.Add(expense);
                }
                return expenses;
            }

            var searchField1 = DataTable.AsEnumerable().Where(r => r.Field<string>("CategoriaGasto").ToLower().Contains(searchInput));
            var searchField2 = DataTable.AsEnumerable().Where(r => r.Field<string>("Proveedor").ToLower().Contains(searchInput));

            foreach (var row in searchField1)
            {
                var expense = new Expense(ExpensesFilePath)
                {
                    Id = Int32.Parse(row["Usuario"].ToString()),
                    User = row["Usuario"].ToString(),
                    Vendor = row["Proveedor"].ToString(),
                    TicketNumber = row["NumeroTicket"].ToString(),
                    Description = row["Descripcion"].ToString(),
                    Amount = Decimal.Parse(row["Monto"].ToString()),
                    ExpenseCategory = row["CategoriaGasto"].ToString(),
                    Date = Convert.ToDateTime(row["Fecha"].ToString())

                };
                if (row["Moneda"].ToString().ToUpper() == "USD")
                    expense.CurrencyType = CurrencyTypeEnum.USD;
                else
                    expense.CurrencyType = CurrencyTypeEnum.MXN;

                expense.PaymentType = (PaymentTypeEnum)Enum.Parse(typeof(PaymentTypeEnum), row["MetodoPago"].ToString(), true);

                expenses.Add(expense);
            }

            foreach (var row in searchField2)
            {
                var expense = new Expense(ExpensesFilePath)
                {
                    Id = Int32.Parse(row["Usuario"].ToString()),
                    User = row["Usuario"].ToString(),
                    Vendor = row["Proveedor"].ToString(),
                    TicketNumber = row["NumeroTicket"].ToString(),
                    Description = row["Descripcion"].ToString(),
                    Amount = Decimal.Parse(row["Monto"].ToString()),
                    ExpenseCategory = row["CategoriaGasto"].ToString(),
                    Date = Convert.ToDateTime(row["Fecha"].ToString())

                };
                if (row["Moneda"].ToString().ToUpper() == "USD")
                    expense.CurrencyType = CurrencyTypeEnum.USD;
                else
                    expense.CurrencyType = CurrencyTypeEnum.MXN;

                expense.PaymentType = (PaymentTypeEnum)Enum.Parse(typeof(PaymentTypeEnum), row["MetodoPago"].ToString(), true);

                //Add if it does not exist already
                if (!expenses.Exists(x => x.Id == expense.Id))
                    expenses.Add(expense);
            }

            return expenses;
        }

        public bool AddNewItemToTable(object product)
        {
            throw new NotImplementedException();
        }

        public object GetTotal(string searchInput)
        {
            throw new NotImplementedException();
        }

        public bool GetTotal(out decimal expensesMxn, out decimal expensesUsd, out decimal expensesCashMxn, out decimal expensesCashUsd)
        {
            expensesMxn = 0;
            expensesUsd = 0;
            expensesCashMxn = 0;
            expensesCashUsd = 0;

            var allFields = base.DataTable.AsEnumerable();
            foreach (var row in allFields)
            {
                if (row["Moneda"].ToString() == "USD")
                {
                    expensesUsd += Convert.ToDecimal(row["Monto"].ToString());
                    if (row["MetodoPago"].ToString() == "Efectivo")
                    {
                        expensesCashUsd += Convert.ToDecimal(row["Monto"].ToString());
                    }
                }
                else
                {
                    expensesMxn += Convert.ToDecimal(row["Monto"].ToString());
                    if (row["MetodoPago"].ToString() == "Efectivo")
                    {
                        expensesCashMxn += Convert.ToDecimal(row["Monto"].ToString());
                    }
                }
            }
            return true;
        }

        public int GetLastItemNumber()
        {
            if (DataTable.Rows.Count == 0)
                return 0;
            var row = DataTable.Rows[DataTable.Rows.Count - 1];
            return Int32.Parse(row["Id"].ToString());
        }

        /// <summary>
        /// Update expense in the datatable
        /// </summary>
        /// <param name="expense"></param>
        /// <returns></returns>
        public bool UpdateExpenseToTable(Expense expense)
        {
            for (int index = 0; index < DataTable.Rows.Count; index++)
            {
                var row = DataTable.Rows[index];
                if (row["Id"].ToString() == expense.Id.ToString())
                {
                    row["Usuario"] = expense.User;
                    row["Proveedor"] = expense.Vendor;
                    row["NumeroTicket"] = expense.TicketNumber;
                    row["Descripcion"] = expense.Description;
                    row["Monto"] = expense.Amount.ToString();
                    row["Moneda"] = expense.CurrencyType.ToString();
                    row["MetodoPago"] = expense.PaymentType.ToString();
                    row["CategoriaGasto"] = expense.ExpenseCategory.ToString();
                    row["Fecha"] = expense.Date.ToString();
                }
            }

            return true;
        }

        /// <summary>
        /// Add new expense to data table
        /// </summary>
        /// <param name="expense"></param>
        /// <returns></returns>
        public bool AddExpenseToTable(Expense expense)
        {
            DataTable.Rows.Add();
            var row = DataTable.Rows[DataTable.Rows.Count - 1];
            row["Id"] = expense.GetLastItemNumber() + 1;
            row["Usuario"] = expense.User;
            row["Proveedor"] = expense.Vendor;
            row["NumeroTicket"] = expense.TicketNumber;
            row["Descripcion"] = expense.Description;
            row["Monto"] = expense.Amount.ToString();
            row["Moneda"] = expense.CurrencyType.ToString();
            row["MetodoPago"] = expense.PaymentType.ToString();
            row["CategoriaGasto"] = expense.ExpenseCategory.ToString();
            row["Fecha"] = expense.Date.ToString();
            return true;
        }

        /// <summary>
        /// Delete expense
        /// </summary>
        public void Delete()
        {
            base.RemoveEntryInDataTable(this.Id.ToString(), "Id");
        }
        #endregion
    }
}
