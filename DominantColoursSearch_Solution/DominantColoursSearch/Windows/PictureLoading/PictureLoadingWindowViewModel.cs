using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using System.IO;
using System.Windows.Media.Imaging;
using Emgu.CV;
using Emgu.CV.Structure;
using DominantColoursSearch.CustomClasses;

namespace DominantColoursSearch.Windows.PictureLoading
{
    public class PictureLoadingWindowViewModel : BindableBase
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

        private int _selectedImageIndex = -1;
        public int SelectedImageIndex
        {
            get { return _selectedImageIndex; }
            set
            {
                var isChanged = SetProperty(ref this._selectedImageIndex, value);

                if (isChanged)
                {
                    LoadImage(this.FilePaths[this.SelectedImageIndex]);
                    RaisePropertyChanged(nameof(this.SelectedImageInfo));
                    RaisePropertyChanged(nameof(this.SelectedImageIndexDisplayText));
                }
            }
        }

        public int SelectedImageIndexDisplayText
        {
            get => this.SelectedImageIndex + 1;
        }

        public int NumberOfElements
        {
            get => this.FilePaths?.Length == null
                ? 0
                : this.FilePaths.Length;
        }

        private string[] _filePaths;
        public string[] FilePaths
        {
            get => this._filePaths;
            set
            {
                var isNewValue = SetProperty(ref this._filePaths, value);
                
                if (isNewValue)
                {
                    RaisePropertyChanged(nameof(this.NumberOfElements));
                }
            }
        }
        public string[] FileNames { get; private set; }

        public ImageInfoContainer SelectedImageInfo
        {
            get
            {
                return this.SelectedImageIndex < 0
                    ? null
                    : this.ImageInfoContainers?[this.SelectedImageIndex];
            }
        }
        public List<ImageInfoContainer> ImageInfoContainers { get; private set; }

        public void InitializeViewModel(string[] filePaths, string[] fileNames)
        {
            this.FilePaths = filePaths;
            this.FileNames = fileNames;
            this.ImageInfoContainers = new List<ImageInfoContainer>(this.FilePaths.Length);
            this.SelectedImageIndex = 0;
        }

        public void LoadImage(string fullPath)
        {
            if (!File.Exists(fullPath))
            {
                return;
            }

            using (var sourceImage = new Image<Bgr, Byte>(fullPath))
            {
                // TODO: improve  this so there wouldn't be OutOfRangeException on Next/Prev button click after re-initializing FullPaths array
                if (this.ImageInfoContainers.Where(imageInfo => imageInfo.PathToFile == fullPath).FirstOrDefault() == null)
                {
                    this.ImageInfoContainers.Add(new ImageInfoContainer(fullPath, sourceImage.Size));
                }

                this.SelectedImage = Utility.ToBitmapSource(sourceImage.ToBitmap());
            }
        }
    }
}
