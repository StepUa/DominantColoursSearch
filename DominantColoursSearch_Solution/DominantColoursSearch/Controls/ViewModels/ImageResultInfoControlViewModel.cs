using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DominantColoursSearch.CustomClasses;

namespace DominantColoursSearch.Controls.ViewModels
{
    public class ImageResultInfoControlViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<PictureDominantColorInfoItem> _imageResultInfoCollection;
        public ObservableCollection<PictureDominantColorInfoItem> ImageResultInfoCollection
        {
            get => this._imageResultInfoCollection;
            set
            {
                if (Object.ReferenceEquals(this._imageResultInfoCollection, value))
                {
                    return;
                }

                this._imageResultInfoCollection = value;

                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
