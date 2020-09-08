using FestiSpec.ViewModel.Festival;
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

namespace FestiSpec.Views.Inspection
{
    /// <summary>
    /// Interaction logic for AddInspectionView.xaml
    /// </summary>
    public partial class AddInspectionView : Window
    {
        public AddInspectionView()
        {
            InitializeComponent();
            ((FestivalListViewModel)DataContext).SelectedFestival = null;
        }

        public AddInspectionView(FestivalViewModel fvm)
        {
            InitializeComponent();
            ((FestivalListViewModel)DataContext).SelectedFestival = fvm;
        }
    }
}
