using Desktop.Data;
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

namespace Desktop.UI.Pages
{
    /// <summary>
    /// Interaction logic for EditVacation.xaml
    /// </summary>
    public partial class EditVacation : Page
    {
        public string ConnStr = null;
        public Vacation Vac = null;
        public DbContext Context;

        public EditVacation()
        {
            InitializeComponent();
        }

        private void ButOK_Click(object sender, RoutedEventArgs e)
        {
            DateTime? begin = DpBegin.SelectedDate, end = DpEnd.SelectedDate;

            if (begin != null && end != null)
            {
                Vac.BeginDate = begin.Value;
                Vac.EndDate = end.Value;
            }
            Vac.UpdateVacation(ConnStr);

            var wnd = this.Parent as Window;
            wnd.Close();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DpBegin.SelectedDate = Vac.BeginDate;
            DpEnd.SelectedDate = Vac.EndDate;
        }
    }
}
