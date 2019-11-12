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
        private AnalyzedPictureInfo _imageResultInfo;
        public AnalyzedPictureInfo ImageResultInfo
        {
            get => this._imageResultInfo;
            set
            {
                if (Object.ReferenceEquals(this._imageResultInfo, value))
                {
                    return;
                }

                this._imageResultInfo = value;

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
