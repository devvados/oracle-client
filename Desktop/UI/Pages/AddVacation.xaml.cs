using Desktop.Data;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for AddVacation.xaml
    /// </summary>
    public partial class AddVacation : Page
    {
        public string ConnStr;
        public Employee Emp = null;

        public AddVacation()
        {
            InitializeComponent();
        }

        private void ButOK_Click(object sender, RoutedEventArgs e)
        {
            DateTime? begin = DpBegin.SelectedDate, end = DpEnd.SelectedDate;

            if(begin != null && end != null)
            {
                Emp.InsertVacation(ConnStr, new Vacation
                {
                    BeginDate = begin.Value,
                    EndDate = end.Value,
                    EmployeeId = Emp.Id
                });

                var wnd = this.Parent as Window;
                wnd.Close();
            }
        }
    }
}
