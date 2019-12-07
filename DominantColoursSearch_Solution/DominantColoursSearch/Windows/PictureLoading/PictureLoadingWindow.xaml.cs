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
            this.ViewModel = new PictureLoadingWindowViewModel();

            this.DataContext = this.ViewModel;

            InitializeComponent();
        }

        public PictureLoadingWindowViewModel ViewModel { get; private set; }

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

            this.ViewModel.InitializeViewModel(openFileDialog.FileNames, openFileDialog.SafeFileNames);
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void PreviousImageButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.ViewModel.SelectedImageIndex - 1 >= 0)
            {
                this.ViewModel.SelectedImageIndex--;
            }
        }

        private void NextImageButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.ViewModel.SelectedImageIndex + 1 < this.ViewModel.FilePaths.Length)
            {
                this.ViewModel.SelectedImageIndex++;
            }
        }

        private void RemoveImageButton_Click(object sender, RoutedEventArgs e)
        {
            
        }
        
    }
}
