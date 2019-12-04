using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Emgu.CV;
using Emgu.CV.Structure;
using Prism.Mvvm;

namespace DominantColoursSearch.Windows.DatabaseInteraction
{
    public class DatabaseInteractionWindowViewModel : BindableBase
    {
        private BitmapSource _selectedImage;
        public BitmapSource SelectedImage
        {
            get { return _selectedImage; }
            set
            {
                if (Object.ReferenceEquals(this._selectedImage, value))
                {
                    return;
                }

                this._selectedImage = value;

                RaisePropertyChanged();
            }
        }

        private int _selectedImageIndex;
        public int SelectedImageIndex
        {
            get { return _selectedImageIndex; }
            set { SetProperty(ref this._selectedImageIndex, value); }
        }


        public void LoadImage(string fullPath)
        {
            if (!File.Exists(fullPath))
            {
                return;
            }

            using (var sourceImage = new Image<Bgr, Byte>(fullPath))
            {
                this.SelectedImage = Utility.ToBitmapSource(sourceImage.ToBitmap());
            }
        }
    }
}
