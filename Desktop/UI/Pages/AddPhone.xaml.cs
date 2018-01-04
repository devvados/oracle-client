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
    /// Interaction logic for AddPhone.xaml
    /// </summary>
    public partial class AddPhone : Page
    {
        public string ConnStr = null;
        public Employee Emp = null;
        public DbContext Context;

        public AddPhone()
        {
            InitializeComponent();
        }

        private void BSubmit_Click(object sender, RoutedEventArgs e)
        {
            var tel = new Phone
            {
                PhoneType = (CbPhoneTypes.SelectedItem as ComboBoxItem)?.Content.ToString(),
                Number = TbPhoneNumber.Text,
                Type = (CbPhoneTypes.SelectedIndex),
                EmployeeId = Emp.Id
            };

            Emp.InsertTelephone(ConnStr, tel);
        }
    }
}
