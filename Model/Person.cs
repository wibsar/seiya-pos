using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seiya.WpfBindingUtilities;

namespace Seiya
{
    public class Person
    {
        private string _name;
        private int _id;
        private string _address;

        #region Properties

        public string Name  
        {
            get { return _name; }
            set
            {
                _name = value;
            }
        }

        public int Id   
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }
        

        #endregion

        public Person(string name, int id, string address)
        {
            _name = name;
            _id = id;
            _address = address;
        }

        public Person()
        {
            _name = "a";
        }

    }
}
