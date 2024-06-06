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
using System.Windows.Shapes;
using List.VM;

namespace List.View
{
    /// <summary>
    /// Логика взаимодействия для ListOfProducts.xaml
    /// </summary>
    public partial class ListOfProducts : Window
    {
        public ListOfProducts()
        {
            InitializeComponent();
            this.DataContext = new ListOfProductsVM();
        }

    }
}
