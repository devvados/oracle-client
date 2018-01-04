using Desktop.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /// Interaction logic for ViewEmployee.xaml
    /// </summary>
    public partial class ViewEmployee : Page
    {
        public string ConnStr = null;
        public Employee Emp = null;
        public Data.DbContext Context;

        public ViewEmployee()
        {
            InitializeComponent();
        }

        private void BSubmit_Click(object sender, RoutedEventArgs e)
        {
            var wnd = this.Parent as Window;
            wnd.Close();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            TbName.Text = Emp.Name;
            TbSurname.Text = Emp.Surname;
            TbPatronymic.Text = Emp.Patronymic;

            TbPosition.Text = Emp.Position;
            TbDepartment.Text = Emp.Department;

            LvAddress.ItemsSource = Emp.Addresses;
            LvPhone.ItemsSource = Emp.Phones;
        }
    }
}
