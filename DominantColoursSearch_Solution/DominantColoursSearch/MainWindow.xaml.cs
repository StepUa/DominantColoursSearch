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
using DominantColoursSearch.Controls.ViewModels;
using DominantColoursSearch.PictureLoading;
using DominantColoursSearch.DominantColoursAnalysis;

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

        public ImageResultInfoControlViewModel ImageResultInfoControlViewModel
        {
            get => this.imageResultInfoControlContainer?.DataContext as ImageResultInfoControlViewModel;
        }

        public int SelectedImageIndex { get; set; }

        private PictureLoadingWindow PictureLoadingWindow { get; set; }
        private MainWindowViewModel ViewModel { get; set; }

        private void Button_LoadImageClick(object sender, RoutedEventArgs e)
        {
            this.PictureLoadingWindow = new PictureLoadingWindow();

            this.PictureLoadingWindow.ShowDialog();

            this.ViewModel.InitializeViewModel(this.PictureLoadingWindow.FileNames, SetImageOnAnalysisCompleteEvent);
        }

        private void SetImageOnAnalysisCompleteEvent(object sender, EventArgs e)
        {
            if (!(sender is DominantColoursAnalyzer analyzer))
            {
                return;
            }

            // TODO: move this to window closing event or smth
            analyzer.AnalysisCompleteEvent -= SetImageOnAnalysisCompleteEvent;

            if (this.SelectedImageIndex != analyzer.UniqueIndex)
            {
                return;
            }

            this.Dispatcher.Invoke(() => this.ImageResultInfoControlViewModel.ImageResultInfo = analyzer.AnalyzedPictureInfo);
        }

        private async void Button_RunClick(object sender, RoutedEventArgs e)
        {
            try
            {
                await this.ViewModel.StartImageProcessingAsync().ConfigureAwait(false); ;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
