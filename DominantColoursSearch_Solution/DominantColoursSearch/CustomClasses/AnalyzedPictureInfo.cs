using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Prism.Mvvm;

namespace DominantColoursSearch.CustomClasses
{
    public class AnalyzedPictureInfo : BindableBase
    {
        private BitmapSource _analyzedImage;
        public BitmapSource AnalyzedImage
        {
            get => this._analyzedImage;
            set
            {
                if (Object.ReferenceEquals(this._analyzedImage, value))
                {
                    return;
                }

                this._analyzedImage = value;

                RaisePropertyChanged();
            }
        }

        private BitmapSource _analyzedImageWithClusters;
        public BitmapSource AnalyzedImageWithClusters
        {
            get => this._analyzedImageWithClusters;
            set
            {
                if (Object.ReferenceEquals(this._analyzedImageWithClusters, value))
                {
                    return;
                }

                this._analyzedImageWithClusters = value;

                RaisePropertyChanged();
            }
        }

        private ObservableCollection<PictureDominantColorInfoItem> _dominantColours;
        public ObservableCollection<PictureDominantColorInfoItem> DominantColours
        {
            get => this._dominantColours;
            set
            {
                if (Object.ReferenceEquals(this._dominantColours, value))
                {
                    return;
                }

                this._dominantColours = value;

                RaisePropertyChanged();
            }
        }
    }
}
