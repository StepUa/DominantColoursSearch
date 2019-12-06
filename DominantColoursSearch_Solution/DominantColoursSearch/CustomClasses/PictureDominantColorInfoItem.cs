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
    public class PictureDominantColorInfoItem : INotifyPropertyChanged
    {
        public PictureDominantColorInfoItem(Color color)
        {
            this.DominantColor = color;
        }

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
                OnPropertyChanged(nameof(PictureDominantColorInfoItem.ColorTextRepresentation));
            }
        }

        public SolidColorBrush ColorBrush
        {
            get => new SolidColorBrush(this.DominantColor);
        }

        public string ColorTextRepresentation
        {
            get => this.DominantColor.ToString() + 
                "\n" + $"({this.DominantColor.R}, {this.DominantColor.G}, {this.DominantColor.B})";
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
