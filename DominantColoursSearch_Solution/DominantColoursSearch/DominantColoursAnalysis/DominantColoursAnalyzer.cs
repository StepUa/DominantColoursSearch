using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Emgu.CV;
using Emgu.CV.Structure;
using DominantColoursSearch.CustomClasses;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows;
using System.Diagnostics;
using NLog;
using Emgu.CV.CvEnum;

namespace DominantColoursSearch.DominantColoursAnalysis
{
    public class DominantColoursAnalyzer : BindableBase
    {
        private Logger Logger { get; } = LogManager.GetCurrentClassLogger();

        public DominantColoursAnalyzer(string filePath, int uniqueIndex)
        {
            this.FilePath = filePath;
            this.UniqueIndex = uniqueIndex;
            this.AnalysisStopwatch = new Stopwatch();

            // TODO: improve this 
            this.clusters = new ColorCluster[ClusterNumber]
            {
                new ColorCluster(){NewColor = new MCvScalar(0, 0, 255)},
                new ColorCluster(){NewColor = new MCvScalar(0, 100, 255)},
                new ColorCluster(){NewColor = new MCvScalar(0, 255, 255)},
                new ColorCluster(){NewColor = new MCvScalar(0, 255, 0)},
                new ColorCluster(){NewColor = new MCvScalar(255, 170, 60)},
                new ColorCluster(){NewColor = new MCvScalar(255, 0, 0)},
                new ColorCluster(){NewColor = new MCvScalar(255, 0, 194)},
                new ColorCluster(){NewColor = new MCvScalar(203, 192, 255)},
                new ColorCluster(){NewColor = new MCvScalar(153, 255, 153)},
                new ColorCluster(){NewColor = new MCvScalar(0, 75, 150)}
            };
        }

        public event EventHandler AnalysisCompleteEvent;
        public int UniqueIndex { get; set; }

        #region Analysis setup properties

        public int ClustersCount { get; set; }
        public double SizeCrop { get; set; }
        public bool IsColourFilteringEnabled { get; set; }
        public string FilePath { get; private set; }

        #endregion

        private Stopwatch AnalysisStopwatch { get; set; }

        public int IterationsCount { get; private set; }

        public TimeSpan AnalysisTime
        {
            get => this.AnalysisStopwatch == null
                ? new TimeSpan(0, 0, 0)
                : this.AnalysisStopwatch.Elapsed;
        }

        private AnalyzedPictureInfo _analyzedPictureInfo;
        public AnalyzedPictureInfo AnalyzedPictureInfo
        {
            get => this._analyzedPictureInfo;
            private set
            {
                if (Object.ReferenceEquals(this._analyzedPictureInfo, value))
                {
                    return;
                }

                this._analyzedPictureInfo = value;

                RaisePropertyChanged();
            }
        }

        private bool _isFinished;
        public bool IsFinished
        {
            get => this._isFinished;
            set
            {
                if (this._isFinished.Equals(value))
                {
                    return;
                }

                bool oldValue = this._isFinished;

                this._isFinished = value;

                if (!oldValue && this._isFinished)
                {
                    OnAnalysisCompleteEvent();
                }
            }
        }

        public Image<Bgr, Byte> SourceImage { get; set; }

        public Image<Bgr, Byte> AnalizedImage { get; set; }

        public ColorCluster[] clusters;

        public const int ClusterNumber = 10;

        private static double Rgb_Euclidean(MCvScalar point1, MCvScalar point2)
        {
            return Math.Sqrt((point1.V0 - point2.V0) * (point1.V0 - point2.V0) +
                (point1.V1 - point2.V1) * (point1.V1 - point2.V1) +
                (point1.V2 - point2.V2) * (point1.V2 - point2.V2));
        }

        public void AnalysisFunction()
        {
            this.IsFinished = false;

            // it's recomennded to call Dispose() method because Image class contains IplImg structure
            this.SourceImage = new Image<Bgr, Byte>(this.FilePath);

            // resize image for better performance [optional]
            double sizeCrop = 1d;
            //this.SourceImage = this.SourceImage.Resize((int)(this.SourceImage.Width / sizeCrop),
            //    (int)(this.SourceImage.Height / sizeCrop),
            //    Inter.Linear);

            int[,] clusterIndexes = new int[this.SourceImage.Height, this.SourceImage.Width];

            int x;
            int y;
            int k;

            double minRgbEuclidean = 0;
            double oldRgbEuclidean = 0;

            this.IterationsCount = 0;
            this.AnalysisStopwatch.Restart();

            while (true)
            {
                this.IterationsCount++;

                for (k = 0; k < ClusterNumber; k++)
                {
                    this.clusters[k].Count = 0;
                    this.clusters[k].Color = this.clusters[k].NewColor;
                    this.clusters[k].NewColor = new MCvScalar(0, 0, 0);
                }

                for (y = 0; y < this.SourceImage.Height; y++)
                {
                    for (x = 0; x < this.SourceImage.Width; x++)
                    {
                        // get pixel' RGB components
                        int B = (int)this.SourceImage[y, x].Blue;
                        int G = (int)this.SourceImage[y, x].Green;
                        int R = (int)this.SourceImage[y, x].Red;

                        minRgbEuclidean = Double.MaxValue;
                        int clusterIndex = -1;

                        for (k = 0; k < ClusterNumber; k++)
                        {
                            double euclid = Rgb_Euclidean(new MCvScalar(B, G, R),
                                new MCvScalar(this.clusters[k].Color.V0, this.clusters[k].Color.V1, this.clusters[k].Color.V2));

                            if (euclid < minRgbEuclidean)
                            {
                                minRgbEuclidean = euclid;
                                clusterIndex = k;
                            }
                        }
                        // set cluster index
                        clusterIndexes[y, x] = clusterIndex;

                        this.clusters[clusterIndex].Count++;
                        this.clusters[clusterIndex].NewColor = new MCvScalar(this.clusters[clusterIndex].NewColor.V0 + B,
                             this.clusters[clusterIndex].NewColor.V1 + G,
                             this.clusters[clusterIndex].NewColor.V2 + R);
                    }
                }

                minRgbEuclidean = 0;
                for (k = 0; k < ClusterNumber; k++)
                {
                    if (this.clusters[k].Count == 0)
                    {
                        continue;
                    }

                    // new cluster
                    this.clusters[k].NewColor = new MCvScalar(this.clusters[k].NewColor.V0 / this.clusters[k].Count,
                             this.clusters[k].NewColor.V1 / this.clusters[k].Count,
                             this.clusters[k].NewColor.V2 / this.clusters[k].Count);

                    // TODO: add Math.Round ???
                    double ecli = Rgb_Euclidean(new MCvScalar(this.clusters[k].NewColor.V0, this.clusters[k].NewColor.V1, this.clusters[k].NewColor.V2),
                            new MCvScalar(this.clusters[k].Color.V0, this.clusters[k].Color.V1, this.clusters[k].Color.V2));

                    if (ecli > minRgbEuclidean)
                    {
                        minRgbEuclidean = ecli;
                    }
                }

                if (Math.Abs(minRgbEuclidean - oldRgbEuclidean) < 1) // basically when they're equal
                {
                    break;
                }

                oldRgbEuclidean = minRgbEuclidean;
            }

            this.AnalysisStopwatch.Stop();

            this.AnalizedImage = this.SourceImage.Clone();

            ClusterVisualization(this.AnalizedImage, clusterIndexes);

            // TODO: change this code so object creation 
            // (for binding to a view) will take place in SetImageOnAnalysisCompleteEvent() (MainWindow.xaml) and not here
            Application.Current.Dispatcher.Invoke((SetAnalysisResult));
            //SetAnalysisResult();

            // Logging
            this.Logger.Info(String.Format("Analysis took {0}:{1}:{2} and {3} iterations. Total seconds: {4} Image size {5}x{6}",
                this.AnalysisTime.Minutes,
                this.AnalysisTime.Seconds,
                this.AnalysisTime.Milliseconds,
                this.IterationsCount,
                this.AnalysisTime.TotalSeconds,
                this.SourceImage.Width,
                this.SourceImage.Height
                ));

            StringBuilder logString = new StringBuilder();
            logString.AppendLine($"Dominant colours (sizeCrop: {sizeCrop}):");
            for (int i = 0; i < this.clusters.Length; i++)
            {
                var dominantColour = this.clusters[i];

                logString.AppendLine($"[{i}] ({(int)dominantColour.NewColor.V2}, {(int)dominantColour.NewColor.V1}, {(int)dominantColour.NewColor.V0})");
            }

            this.Logger.Info(logString);
            // End of logging

            this.SourceImage.Dispose();
            this.AnalizedImage.Dispose();

            this.IsFinished = true;
        }

        private void ClusterVisualization(Image<Bgr, Byte> image, int[,] clusterIndexes)
        {
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    int clusterIndex = clusterIndexes[y, x];

                    image.Data[y, x, 2] = (byte)clusters[clusterIndex].Color.V2; //Write to the Red Spectrum
                    image.Data[y, x, 1] = (byte)clusters[clusterIndex].Color.V1; //Write to the Green Spectrum
                    image.Data[y, x, 0] = (byte)clusters[clusterIndex].Color.V0; //Write to the Blue Spectrum
                }
            }
        }

        private void SetAnalysisResult()
        {
            var dominantColoursCollection = new ObservableCollection<PictureDominantColorInfoItem>();

            this.clusters = this.clusters.OrderByDescending((colourCluster) => colourCluster.Count).ToArray();

            for (int i = 0; i < this.clusters.Length; i++)
            {
                //Debug.WriteLine(this.clusters[i].Count);

                Color color = Color.FromRgb(
                    (byte)this.clusters[i].Color.V2,
                    (byte)this.clusters[i].Color.V1,
                    (byte)this.clusters[i].Color.V0);

                dominantColoursCollection.Add(new PictureDominantColorInfoItem(color));
            }

            this.AnalyzedPictureInfo = new AnalyzedPictureInfo()
            {
                AnalyzedImage = Utility.ToBitmapSource(this.SourceImage.ToBitmap()),
                AnalyzedImageWithClusters = Utility.ToBitmapSource(this.AnalizedImage.ToBitmap()),
                DominantColours = dominantColoursCollection
            };
        }

        protected virtual void OnAnalysisCompleteEvent()
        {
            this.AnalysisCompleteEvent?.Invoke(this, EventArgs.Empty);
        }

    }
}
