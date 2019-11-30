using System;
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
using Microsoft.Win32;
using System.Diagnostics;

namespace DominantColoursSearch.Windows.PictureLoading
{
    /// <summary>
    /// Interaction logic for PictureLoadingWindow.xaml
    /// </summary>
    public partial class PictureLoadingWindow : Window
    {
        public PictureLoadingWindow()
        {
            InitializeComponent();
        }

        public string[] FileNames { get; private set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                CheckFileExists = true,
                AddExtension = true,
                Multiselect = true,
                Filter = "Images |*.png;*.jpg;*.bmp"
            };

            var dialogResult = openFileDialog.ShowDialog();
            if (dialogResult != true)
            {
                return;
            }

            foreach (var str in openFileDialog.FileNames)
            {
                Debug.WriteLine("File name: " + str);
            }

            this.FileNames = openFileDialog.FileNames;
        }
    }
}
