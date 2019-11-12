using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DominantColoursSearch.Controls.ViewModels;

namespace DominantColoursSearch.Controls
{
    /// <summary>
    /// Interaction logic for ImageResultInfoControl.xaml
    /// </summary>
    public partial class ImageResultInfoControl : UserControl
    {
        public ImageResultInfoControl()
        {
            InitializeComponent();
        }

        public ImageResultInfoControlViewModel ViewModel { get; set; }
    }
}
