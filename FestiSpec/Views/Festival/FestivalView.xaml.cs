using FestiSpec.ViewModel.Client;
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

namespace FestiSpec.Views.Festival
{
    /// <summary>
    /// Interaction logic for FestivalView.xaml
    /// </summary>
    public partial class FestivalView : Window
    {
        public FestivalView()
        {
            InitializeComponent();
            ((FestivalListViewModel)DataContext).SelectedClient = null;
            ((FestivalListViewModel)DataContext).prevIsMain = true;
        }

        public FestivalView(ClientViewModel cvm)
        {
            InitializeComponent();
            ((FestivalListViewModel)DataContext).SelectedClient = cvm;
            ((FestivalListViewModel)DataContext).prevIsMain = false;
         
        }
    }
}
