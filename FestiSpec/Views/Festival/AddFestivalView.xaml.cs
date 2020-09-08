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
    /// Interaction logic for AddFestivalView.xaml
    /// </summary>
    public partial class AddFestivalView : Window
    {
        public AddFestivalView(ClientViewModel client)
        {
            InitializeComponent();
            ((FestivalViewModel)DataContext).Model.CustomerCompany = client.Model;
        }
    }
}
