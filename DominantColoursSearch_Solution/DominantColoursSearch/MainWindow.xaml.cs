using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using DominantColoursSearch.PictureLoading;

namespace DominantColoursSearch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.ViewModel = new MainWindowViewModel();

            this.DataContext = this.ViewModel;

            InitializeComponent();
        }


        private PictureLoadingWindow PictureLoadingWindow { get; set; }
        private MainWindowViewModel ViewModel { get; set; }

        private void Button_LoadImageClick(object sender, RoutedEventArgs e)
        {
            this.PictureLoadingWindow = new PictureLoadingWindow();

            this.PictureLoadingWindow.ShowDialog();
        }

        DominantColoursAnalysis.DominantColoursAnalyzer Analyzer = new DominantColoursAnalysis.DominantColoursAnalyzer();

        private async void Button_RunClick(object sender, RoutedEventArgs e)
        {
            await RunAnalyze();
            BitmapSource bitmapSource = Utility.ToBitmapSource(Analyzer.AnalizedImage.ToBitmap());
            Analyzer.AnalizedImage.Dispose();

            this.imgContainer.Source = bitmapSource;


            this.rect1.Fill = new SolidColorBrush(Color.FromRgb((byte)Analyzer.clusters[0].Color.V2, (byte)Analyzer.clusters[0].Color.V1, (byte)Analyzer.clusters[0].Color.V0));
            this.rect2.Fill = new SolidColorBrush(Color.FromRgb((byte)Analyzer.clusters[1].Color.V2, (byte)Analyzer.clusters[1].Color.V1, (byte)Analyzer.clusters[1].Color.V0));
            this.rect3.Fill = new SolidColorBrush(Color.FromRgb((byte)Analyzer.clusters[2].Color.V2, (byte)Analyzer.clusters[2].Color.V1, (byte)Analyzer.clusters[2].Color.V0));
            this.rect4.Fill = new SolidColorBrush(Color.FromRgb((byte)Analyzer.clusters[3].Color.V2, (byte)Analyzer.clusters[3].Color.V1, (byte)Analyzer.clusters[3].Color.V0));
            this.rect5.Fill = new SolidColorBrush(Color.FromRgb((byte)Analyzer.clusters[4].Color.V2, (byte)Analyzer.clusters[4].Color.V1, (byte)Analyzer.clusters[4].Color.V0));
            this.rect6.Fill = new SolidColorBrush(Color.FromRgb((byte)Analyzer.clusters[5].Color.V2, (byte)Analyzer.clusters[5].Color.V1, (byte)Analyzer.clusters[5].Color.V0));
            this.rect7.Fill = new SolidColorBrush(Color.FromRgb((byte)Analyzer.clusters[6].Color.V2, (byte)Analyzer.clusters[6].Color.V1, (byte)Analyzer.clusters[6].Color.V0));
            this.rect8.Fill = new SolidColorBrush(Color.FromRgb((byte)Analyzer.clusters[7].Color.V2, (byte)Analyzer.clusters[7].Color.V1, (byte)Analyzer.clusters[7].Color.V0));
            this.rect9.Fill = new SolidColorBrush(Color.FromRgb((byte)Analyzer.clusters[8].Color.V2, (byte)Analyzer.clusters[8].Color.V1, (byte)Analyzer.clusters[8].Color.V0));
            this.rect10.Fill = new SolidColorBrush(Color.FromRgb((byte)Analyzer.clusters[9].Color.V2, (byte)Analyzer.clusters[9].Color.V1, (byte)Analyzer.clusters[9].Color.V0));
        }

        private Task<BitmapSource> RunAnalyze()
        {
            return Task.Run(() =>
            {
                BitmapSource bitmapSource = Analyzer.Function(this.PictureLoadingWindow.FileNames[0]);
                return bitmapSource;
            });
        }
    }
}
