using FestiSpec.ViewModel.Festival;
using FestiSpec.ViewModel.Schedule;
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
    /// Interaction logic for InspectionView.xaml
    /// </summary>
    public partial class EditInspectionView : Window
    {
        public EditInspectionView()
        {
            InitializeComponent();
            //((InspectionViewModel)DataContext).SelectedFestival = null;

        }

        public EditInspectionView(InspectionViewModel inspectionViewModel)
        {
            InitializeComponent();
            //((InspectionViewModel)DataContext).SetModel(inspectionViewModel.Model);
            //((InspectionViewModel)DataContext).SelectedFestival = inspectionViewModel.SelectedFestival;

        }
    }
}
