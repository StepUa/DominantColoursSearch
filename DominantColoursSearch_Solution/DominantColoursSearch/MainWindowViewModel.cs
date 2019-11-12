using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using DominantColoursSearch.DominantColoursAnalysis;
using System.Windows.Media.Imaging;

namespace DominantColoursSearch
{
    public class MainWindowViewModel : BindableBase
    {
        public MainWindowViewModel()
        {
            this.Analyzer = new DominantColoursAnalyzer();
        }

        private string[] _fileNames;
        public string[] FileNames
        {
            get => this._fileNames;
            set
            {
                if (Object.ReferenceEquals(this._fileNames, value))
                {
                    return;
                }

                this._fileNames = value;
                RaisePropertyChanged();
            }
        }

        private DominantColoursAnalyzer Analyzer { get; set; }

        private Task<BitmapSource> RunAnalyze()
        {
            return Task.Run(() =>
            {
                BitmapSource bitmapSource = Analyzer.Function(this.FileNames[0]);
                return bitmapSource;
            });
        }
    }
}
