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

namespace DominantColoursSearch
{
    public class MainWindowViewModel : BindableBase
    {
        private Logger Logger { get; } = LogManager.GetCurrentClassLogger();

        private List<Task> TasksWithAnalyzers;

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

        public bool IsViewModelInitialized { get; private set; }

        private DominantColoursAnalyzer[] Analyzers { get; set; }

        public void InitializeViewModel(string[] fileNamesInput, EventHandler handler)
        {
            if (fileNamesInput == null)
            {
                return;
            }

            this.FileNames = fileNamesInput;

            this.Analyzers = new DominantColoursAnalyzer[this.FileNames.Length];
            this.TasksWithAnalyzers = new List<Task>(this.Analyzers.Length);
            for (int i = 0; i < this.Analyzers.Length; i++)
            {
                this.Analyzers[i] = new DominantColoursAnalyzer(this.FileNames[i], i);

                this.Analyzers[i].AnalysisCompleteEvent -= handler;
                this.Analyzers[i].AnalysisCompleteEvent += handler;
            }

            this.IsViewModelInitialized = true;
        }

        public async Task StartImageProcessingAsync()
        {
            try
            {
                for (int i = 0; i < this.Analyzers.Length; i++)
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

    }
}
