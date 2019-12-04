using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominantColoursSearch.Windows.Options
{
    public class OptionsWindowViewModel
    {
        public OptionsWindowViewModel()
        {
            this.NumberOfClusters = new int[] { 3, 4, 5, 6, 7, 8, 9, 10, 11, 12};
            this.ImageResizintScale = new double[] { 100, 67, 50, 40, 33.3, 28.6, 25 };
            this.IsSensitivityModeToBrightColorsEnabled = false;
            this.LanguageList = new string[] { "English", "Ukrainian" };
        }

        public int[] NumberOfClusters { get; set; }
        public double[] ImageResizintScale { get; set; }
        public bool IsSensitivityModeToBrightColorsEnabled { get; set; }
        public string[] LanguageList { get; set; }
    }
}
