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

namespace DominantColoursSearch.Windows.Options
{
    /// <summary>
    /// Interaction logic for OptionsWindow.xaml
    /// </summary>
    public partial class OptionsWindow : Window
    {
        public OptionsWindow()
        {
            this.ViewModel = new OptionsWindowViewModel();

            this.DataContext = this.ViewModel;

            InitializeComponent();
        }

        public OptionsWindowViewModel ViewModel { get; set; }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
