using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Seiya.WpfBindingUtilities;

namespace Seiya
{
    public class CarRegistrationViewModel : ObservableObject
    {
        #region Fields
        private ObservableCollection<CarPart> _carPartsSearchedEntries;
        private CarPart _selectedCarPart;
        #endregion

        #region Constructors
        public CarRegistrationViewModel()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-MX");
            _carPartsSearchedEntries = new ObservableCollection<CarPart>();
            _carPartsSearchedEntries.Add(new CarPart()
            {
                Id = 1,
                Category = "interior",
                Code = "123x",
                Description = "Puerta",
                Model = "Honda 1998",
                Vin = "FDG43BDBSG1435",
                Price = 100M,
                PriceCurrency = CurrencyTypeEnum.USD,
                Enabled = true,
                TotalQuantityAvailable = 4
            });
            _carPartsSearchedEntries.Add(new CarPart()
            {
                Id = 1,
                Category = "interior",
                Code = "12343x",
                Description = "Cofre",
                Model = "Honda 1998",
                Vin = "FDG43BDBSG1435",
                Price = 150M,
                PriceCurrency = CurrencyTypeEnum.USD,
                Enabled = true,
                TotalQuantityAvailable = 1
            });
        }
        #endregion

        #region Observable Properties

        public ObservableCollection<CarPart> CarPartsSearchedEntries
        {
            get { return _carPartsSearchedEntries; }
            set
            {
                _carPartsSearchedEntries = value;
                OnPropertyChanged();
            }
        }

        public CarPart SelectedCarPart
        {
            get { return _selectedCarPart; }
            set
            {
                _selectedCarPart = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Methods

        #endregion

        #region Commands

        #region RegisterCarCommand
        public ICommand RegisterCarCommand { get { return _registerCarCommand ?? (_registerCarCommand = new DelegateCommand(Execute_RegisterCarCommand, CanExecute_RegisterCarCommand)); } }
        private ICommand _registerCarCommand;

        internal void Execute_RegisterCarCommand(object parameter)
        {
            var x = 1;
            var y = CarPartsSearchedEntries;
        }

        internal bool CanExecute_RegisterCarCommand(object parameter)
        {
            return true;
        }
        #endregion

        #endregion
    }
}
