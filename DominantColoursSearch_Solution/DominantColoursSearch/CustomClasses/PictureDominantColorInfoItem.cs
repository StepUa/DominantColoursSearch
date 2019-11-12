using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DominantColoursSearch.CustomClasses
{
    // TODO: add Image property, create DominantColours array and change this class' name 
    public class PictureDominantColorInfoItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Color _dominantColor;
        public Color DominantColor
        {
            get => this._dominantColor;
            set
            {
                if (this._dominantColor.Equals(value))
                {
                    return;
                }

                _dominantColor = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(PictureDominantColorInfoItem.ColorBrush));
                OnPropertyChanged(nameof(PictureDominantColorInfoItem.HexValue));
            }
        }

        public Brush ColorBrush
        {
            get => new SolidColorBrush(this.DominantColor);
        }

        public string HexValue
        {
            get => this.DominantColor.ToString();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
