using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace Desktop.UI.Windows
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : MetroWindow
    {
        public event Action<bool?> LogIn;
        public event Action<string> Conn;


        public Login()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Подключение к базе
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BConnectAdmin_Click(object sender, RoutedEventArgs e)
        {
            string user = TbLogin.Text, password = TbPassword.Text;
            var cs = BuildConnectionString(user, password);

            using (var oc = new OracleConnection(cs))
            {
                oc.Open();
                Conn?.Invoke(cs);
                LogIn?.Invoke(true);
            }

            this.Close();
        }

        private static string BuildConnectionString(string user, string password)
        {
            var builder = new SqlConnectionStringBuilder
            {
                // Supply the additional values.
                DataSource = "orcl",
                UserID = user,
                Password = password
            };

            return builder.ToString();
        }

        private void BConnectGuest_Click(object sender, RoutedEventArgs e)
        {
            string user = "StaffGuest", password = "1234";
            var cs = BuildConnectionString(user, password);

            using (var oc = new OracleConnection(cs))
            {
                oc.Open();
                Conn?.Invoke(cs);
                LogIn?.Invoke(false);
            }

            this.Close();
        }
    }
}
