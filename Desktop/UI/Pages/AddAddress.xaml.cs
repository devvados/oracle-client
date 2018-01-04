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
    /// Interaction logic for AddAddress.xaml
    /// </summary>
    public partial class AddAddress : Page
    {
        public string ConnStr = null;
        public Employee Emp = null;
        public DbContext Context;

        public AddAddress()
        {
            InitializeComponent();
        }

        private void BSubmit_Click(object sender, RoutedEventArgs e)
        {
            var addr = new Address
            {
                AddressType = (CbAddressTypes.SelectedItem as ComboBoxItem)?.Content.ToString(),
                Line = TbAddressLine.Text,
                PostalCode = TbPostalCode.Text,
                Type = Convert.ToInt32(CbAddressTypes.SelectedIndex)
            };

            Emp.InsertAddress(ConnStr, addr);
        }
    }
}
