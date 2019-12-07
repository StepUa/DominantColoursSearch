using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using DominantColoursSearch.DominantColoursAnalysis;
using System.Windows.Media.Imaging;
using NLog;
using System.Diagnostics;
using System.Xml.Serialization;
using System.IO;

namespace DominantColoursSearch
{
    public class MainWindowViewModel : BindableBase
    {
        private Logger Logger { get; } = LogManager.GetCurrentClassLogger();

        private List<Task> TasksWithAnalyzers;

        private string[] _filePaths;
        public string[] FilePaths
        {
            get => this._filePaths;
            set
            {
                if (Object.ReferenceEquals(this._filePaths, value))
                {
                    return;
                }

                this._filePaths = value;
                RaisePropertyChanged();
            }
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

        private string _statusText;
        public string StatusText
        {
            get => this._statusText;
            set => SetProperty(ref this._statusText, value);
        }

        private int _selectedAnalyzerUniqeIndex;
        public int SelectedAnalyzerUniqeIndex
        {
            get => this._selectedAnalyzerUniqeIndex;
            set => SetProperty(ref this._selectedAnalyzerUniqeIndex, value);
        }

        private string _selectedAnalyzerIterationCountText = "0";
        public string SelectedAnalyzerIterationCountText
        {
            get { return _selectedAnalyzerIterationCountText; }
            set { SetProperty(ref _selectedAnalyzerIterationCountText, value); }
        }

        private string _selectedAnalyzerTimeText = "00:00:00";
        public string SelectedAnalyzerTimeText
        {
            get { return _selectedAnalyzerTimeText; }
            set { SetProperty(ref _selectedAnalyzerTimeText, value); }
        }

        public bool IsViewModelInitialized { get; private set; }

        private ObservableCollection<DominantColoursAnalyzer> _analyzers;
        public ObservableCollection<DominantColoursAnalyzer> Analyzers
        {
            get => this._analyzers;
            set
            {
                if (Object.ReferenceEquals(this._analyzers, value))
                {
                    return;
                }

                this._analyzers = value;

                RaisePropertyChanged();
            }
        }

        public void InitializeViewModel(string[] filePaths, string[] fileNames, EventHandler handler)
        {
            if (filePaths == null)
            {
                return;
            }

            this.FilePaths = filePaths;
            this.FileNames = fileNames;

            this.Analyzers = new ObservableCollection<DominantColoursAnalyzer>();
            this.TasksWithAnalyzers = new List<Task>(this.FilePaths.Length);
            for (int i = 0; i < this.FilePaths.Length; i++)
            {
                this.Analyzers.Add(new DominantColoursAnalyzer(this.FilePaths[i], this.FileNames[i], i));

                this.Analyzers[i].AnalysisCompleteEvent -= handler;
                this.Analyzers[i].AnalysisCompleteEvent += handler;
            }

            this.IsViewModelInitialized = true;
        }

        public async Task StartImageProcessingAsync()
        {
            try
            {
                for (int i = 0; i < this.Analyzers.Count; i++)
                {
                    int index = i;
                    this.TasksWithAnalyzers.Add(Task.Run(() => this.Analyzers[index].AnalysisFunction()));
                }

                // Raise exception if any
                await Task.WhenAny(this.TasksWithAnalyzers);
            }
            catch (Exception ex)
            {
                this.Logger.Error(ex, "Error occurred in tasks with analyzers", null);

                throw;
            }
        }

        public void XmlSerialization()
        {
            XmlSerializer formatter = new XmlSerializer(typeof(ObservableCollection<DominantColoursAnalyzer>));

            string outputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Output");
            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }
            
            string fileName = String.Format($"XmlExport({DateTime.Now}).xml").Replace(':', '-'); // TODO: tmp, improve this

            using (FileStream fs = new FileStream(Path.Combine(outputPath, fileName), FileMode.OpenOrCreate))
            {
                Debug.WriteLine(Path.Combine(outputPath, fileName));

                formatter.Serialize(fs, this.Analyzers);
            }
        }

    }
}
