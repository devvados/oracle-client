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
    /// Interaction logic for EditEmployee.xaml
    /// </summary>
    public partial class EditEmployee : Page
    {
        public string ConnStr = null;
        public Employee Emp = null;
        public DbContext Context;

        public EditEmployee()
        {
            InitializeComponent();
        }

        private void BSubmit_Click(object sender, RoutedEventArgs e)
        {
            Emp.Name = TbName.Text;
            Emp.Surname = TbSurname.Text;
            Emp.Patronymic = TbPatronymic.Text;
            Emp.DepId = Context.Departments.Find(x => x.Id == ((Department) CbDepartments.SelectedItem).Id).Id;
            Emp.PosId = ((Position) CbPositions.SelectedItem).Id;

            Emp.UpdateEmployee(ConnStr);
            
            var wnd = this.Parent as Window;
            wnd?.Close();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            TbName.Text = Emp.Name;
            TbSurname.Text = Emp.Surname;
            TbPatronymic.Text = Emp.Patronymic;

            CbOffices.ItemsSource = Context.Offices;
            CbDepartments.ItemsSource = (CbOffices.SelectedItem as Office)?.Departments;
            CbPositions.ItemsSource = Context.Positions;
        }

        private void CBOffices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CbDepartments.ItemsSource = (CbOffices.SelectedItem as Office)?.Departments;
            CbDepartments.SelectedIndex = 0;
        }
    }
}
