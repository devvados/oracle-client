using Desktop.Data;
using MahApps.Metro.Controls.Dialogs;
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
    /// Interaction logic for AddEmployee.xaml
    /// </summary>
    public partial class AddEmployee : Page
    {
        public string ConnStr = null;
        public Department Dep = null;
        public DbContext Context = null;

        public AddEmployee()
        {
            InitializeComponent();
        }

        private void CBPositions_Loaded(object sender, RoutedEventArgs e)
        {
            
            
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            CbDepartments.ItemsSource = new List<Department> { Dep };
            CbOffices.ItemsSource = new List<Office> {
                new Office
                {
                    Description = Dep.Office
                }
            };
        }

        private void BSubmit_Click(object sender, RoutedEventArgs e)
        {
            Dep.InsertEmployee(ConnStr, new Employee
            {
                Name = TbName.Text,
                Surname = TbSurname.Text,
                Patronymic = TbPatronymic.Text,
                PosId = ((Position) CbPositions.SelectedItem).Id,
                DepId = ((Department) CbDepartments.SelectedItem).Id
            });

            var wnd = this.Parent as Window;
            wnd?.Close();
        }
    }
}
