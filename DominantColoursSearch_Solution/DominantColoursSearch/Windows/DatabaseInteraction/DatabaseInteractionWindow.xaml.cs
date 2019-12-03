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
using System.Windows.Shapes;

namespace DominantColoursSearch.Windows.DatabaseInteraction
{
    /// <summary>
    /// Interaction logic for DatabaseInteractionWindow.xaml
    /// </summary>
    public partial class DatabaseInteractionWindow : Window
    {
        public DatabaseInteractionWindow()
        {
            this.ViewModel = new DatabaseInteractionWindowViewModel();

            this.DataContext = this.ViewModel;

            InitializeComponent();
        }

        public DatabaseInteractionWindowViewModel ViewModel { get; set; }
    }
}
