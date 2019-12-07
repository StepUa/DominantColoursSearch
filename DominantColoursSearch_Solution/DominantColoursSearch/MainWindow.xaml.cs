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
using DominantColoursSearch.Windows.PictureLoading;
using DominantColoursSearch.DominantColoursAnalysis;
using DominantColoursSearch.Windows;
using System.Diagnostics;

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

        private PictureLoadingWindow PictureLoadingWindow { get; set; }
        private MainWindowViewModel ViewModel { get; set; }

        private void Button_LoadImageClick(object sender, RoutedEventArgs e)
        {
            this.PictureLoadingWindow = new PictureLoadingWindow();

            this.PictureLoadingWindow.ShowDialog();

            this.ViewModel.InitializeViewModel(this.PictureLoadingWindow.ViewModel.FilePaths, this.PictureLoadingWindow.ViewModel.FileNames, SetImageOnAnalysisCompleteEvent);
        }

        private void SetImageOnAnalysisCompleteEvent(object sender, EventArgs e)
        {
            if (!(sender is DominantColoursAnalyzer analyzer))
            {
                return;
            }

            // TODO: move this to window closing event or smth
            analyzer.AnalysisCompleteEvent -= SetImageOnAnalysisCompleteEvent;

            if (this.ViewModel.SelectedAnalyzerUniqeIndex != analyzer.UniqueIndex)
            {
                return;
            }

            this.Dispatcher.Invoke(() =>
            {
                this.ImageResultInfoControlViewModel.ImageResultInfo = analyzer.AnalyzedPictureInfo;
                this.ViewModel.SelectedAnalyzerIterationCountText = analyzer.IterationsCount.ToString();

                var analyzisTime = analyzer.AnalysisTime;
                this.ViewModel.SelectedAnalyzerTimeText = String.Format($"{analyzisTime.Minutes}:{analyzisTime.Seconds}:{analyzisTime.Milliseconds / 10}");
            });
        }

        private async void Button_RunClick(object sender, RoutedEventArgs e)
        {
            try
            {
                await this.ViewModel.StartImageProcessingAsync().ConfigureAwait(false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void listBoxLoadedImages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(sender is ListBox loadedImages))
            {
                return;
            }

            //Debug.WriteLine(this.ViewModel.SelectedAnalyzerUniqeIndex);

            if (this.ViewModel.SelectedAnalyzerUniqeIndex < 0)
            {
                return;
            }

            var analyzer = this.ViewModel.Analyzers[this.ViewModel.SelectedAnalyzerUniqeIndex];

            this.ViewModel.SelectedAnalyzerIterationCountText = analyzer.IterationsCount.ToString();

            var analyzisTime = analyzer.AnalysisTime;
            this.ViewModel.SelectedAnalyzerTimeText = String.Format($"{analyzisTime.Minutes}:{analyzisTime.Seconds}:{analyzisTime.Milliseconds / 10}");

            this.ImageResultInfoControlViewModel.ImageResultInfo = analyzer.AnalyzedPictureInfo;
        }

        private void DatabaseMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Windows.DatabaseInteraction.DatabaseInteractionWindow databaseInteractionWindow = new Windows.DatabaseInteraction.DatabaseInteractionWindow();

            databaseInteractionWindow.ShowDialog();
        }

        private void XmlExportMenuItem_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void OptionsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Windows.Options.OptionsWindow optionsWindow = new Windows.Options.OptionsWindow();

            optionsWindow.ShowDialog();
        }
    }
}
