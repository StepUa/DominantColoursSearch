using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Emgu.CV;
using Emgu.CV.Structure;

namespace DominantColoursSearch.DominantColoursAnalysis
{
    public class DominantColoursAnalyzer
    {
        public DominantColoursAnalyzer()
        {
            this.clusters = new ColorCluster[ClusterNumber]
            {
                new ColorCluster(){NewColor = new MCvScalar(0, 0, 255)}, // red
                new ColorCluster(){NewColor = new MCvScalar(0, 100, 255)},
                new ColorCluster(){NewColor = new MCvScalar(0, 255, 255)},
                new ColorCluster(){NewColor = new MCvScalar(0, 255, 0)},
                new ColorCluster(){NewColor = new MCvScalar(255, 170, 60)},
                new ColorCluster(){NewColor = new MCvScalar(255, 0, 0)},
                new ColorCluster(){NewColor = new MCvScalar(255, 0, 194)},
                new ColorCluster(){NewColor = new MCvScalar(203, 192, 255)}, // pink
                new ColorCluster(){NewColor = new MCvScalar(153, 255, 153)}, // light green
                new ColorCluster(){NewColor = new MCvScalar(0, 75, 150)} // brown
            };
        }

        public Image<Bgr, Byte> SourceImage { get; set; }
        public Image<Bgr, Byte> AnalizedImage { get; set; }
        public ColorCluster[] clusters;
        public const int ClusterNumber = 10;

        private static double Rgb_Euclidean(MCvScalar point1, MCvScalar point2)
        {
            return Math.Sqrt((point1.V0 - point2.V0) * (point1.V0 - point2.V0) +
                (point1.V1 - point2.V1) * (point1.V1 - point2.V1) +
                (point1.V2 - point2.V2) * (point1.V2 - point2.V2) +
                (point1.V3 - point2.V3) * (point1.V3 - point2.V3));
        }

        public BitmapSource Function(string fileName)
        {
            //string fileName = "test.jpg";

            // it's recomennded to call Dispose() method because Image class contains IplImg structure
            this.SourceImage = new Image<Bgr, Byte>(fileName);

            // resize image for better performance [optional]
            //Engine.SourceImage = Engine.SourceImage.Resize(Engine.SourceImage.Width / 2, Engine.SourceImage.Height / 2, Inter.Linear);

            int[,] clusterIndexes = new int[this.SourceImage.Height, this.SourceImage.Width];

            int x;
            int y;
            int k;

            double minRgbEuclidean = 0;
            double oldRgbEuclidean = 0;


            while (true)
            {
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

                        minRgbEuclidean = 16581375; // 255 * 255* 255
                        int clusterIndex = -1;

                        for (k = 0; k < ClusterNumber; k++)
                        {
                            double euclid = Rgb_Euclidean(new MCvScalar(B, G, R, 0),
                                new MCvScalar(this.clusters[k].Color.V0, this.clusters[k].Color.V1, this.clusters[k].Color.V2, 0));

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

                    // new color
                    this.clusters[k].NewColor = new MCvScalar(this.clusters[k].NewColor.V0 / this.clusters[k].Count,
                             this.clusters[k].NewColor.V1 / this.clusters[k].Count,
                             this.clusters[k].NewColor.V2 / this.clusters[k].Count);

                    double ecli = Rgb_Euclidean(new MCvScalar(this.clusters[k].NewColor.V0, this.clusters[k].NewColor.V1, this.clusters[k].NewColor.V2, 0),
                        new MCvScalar(this.clusters[k].Color.V0, this.clusters[k].Color.V1, this.clusters[k].Color.V2, 0));
                    if (ecli > minRgbEuclidean)
                    {
                        minRgbEuclidean = ecli;
                    }
                }

                if (Math.Abs(minRgbEuclidean - oldRgbEuclidean) < 1)
                {
                    break;
                }

                oldRgbEuclidean = minRgbEuclidean;
            }

            this.AnalizedImage = this.SourceImage.Clone();

            ClusterVisualization(this.AnalizedImage, clusterIndexes);

            var image = this.AnalizedImage.ToBitmap();

            this.SourceImage.Dispose();
            //Engine.AnalizedImage.Dispose();

            return Utility.ToBitmapSource(image);
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
    }
}
