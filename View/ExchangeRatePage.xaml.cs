﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Seiya
{
    /// <summary>
    /// Lógica de interacción para ExchangeRatePage.xaml
    /// </summary>
    public partial class ExchangeRatePage : Page
    {
        public ExchangeRatePage()
        {
            InitializeComponent();
            DataContext = MainWindowViewModel.GetInstance();
        }
    }
}