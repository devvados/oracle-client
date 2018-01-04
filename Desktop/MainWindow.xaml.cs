using Desktop.Data;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public string ConnStr;                       //строка подключения
        public static bool? Admin;                   //роль пользователя
        public DbContext Context;

        public MainWindow()
        {            
            var wnd = new UI.Windows.Login();
            wnd.Conn += value => ConnStr = value;
            wnd.LogIn += value => Admin = value;
            wnd.ShowDialog();

            Context = new DbContext(ConnStr);
            Context.InitializeContext();

            if (Admin != null)
            {
                InitializeComponent();

                DgEmployees.ContextMenu = EmployeesCm();
                DgVacations.ContextMenu = VacationsCm();
            }
            else
            {
                //пользователь не выбрал режим входа
                Close();
            }
        }

        /// <summary>
        /// Отображение режима авторизованного пользователя
        /// </summary>
        private void ShowUserType()
        {
            if (Admin == true)
            {
                ShowMessageBox("Вы вошли как Администратор.\nРазрешено просматривать, вносить изменения в базу данных (добавлять/удалять/редактировать записи)", "Информация");
                BAddEmployee.IsEnabled = true;
            }
            else if (Admin == false) ShowMessageBox("Вы вошли как Гость.\nРазрешено только просматривать записи базы данных", "Информация");

            //string type = (admin == true) ? "Администратор" : "Гость";
            //LabUserType.Content = "Вы вошли как: " + type;
        }

        /// <summary>
        /// Окошко с сообщениями
        /// </summary>
        /// <param name="text"></param>
        /// <param name="title"></param>
        public async void ShowMessageBox(string text, string title)
        {
            var ms = new MetroDialogSettings()
            {
                ColorScheme = MetroDialogColorScheme.Theme,
                AnimateShow = true,
                AnimateHide = true
            };
            await this.ShowMessageAsync(title, text, MessageDialogStyle.Affirmative, ms);
        }

        /// <summary>
        /// Отображение режима пользователя при запуске
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ShowUserType();

            CbPositions.ItemsSource = Context.Positions;

            FillDataGrid(DgOffices, 0, null);
        }

        /// <summary>
        /// Контекстное меню для таблицы сотрудников
        /// </summary>
        /// <returns></returns>
        private ContextMenu EmployeesCm()
        {
            var menu = new ContextMenu();
            var viewEmp = new MenuItem { Header = "Информация" };
            var editEmp = new MenuItem { Header = "Редактировать" };
            var deleteEmp = new MenuItem { Header = "Удалить" };
            var address = new MenuItem { Header = "Адрес" };
            var phone = new MenuItem { Header = "Телефон" };

            var addAddress = new MenuItem { Header = "Добавить" };
            var addPhone = new MenuItem { Header = "Добавить" };

            viewEmp.Click += ViewClick;
            editEmp.Click += EditClick;
            deleteEmp.Click += DeleteClick;

            addAddress.Click += AddAddressClick;
            addPhone.Click += AddPhoneClick;

            address.Items.Add(addAddress);
            phone.Items.Add(addPhone);

            menu.Items.Add(viewEmp);
            menu.Items.Add(editEmp);
            menu.Items.Add(deleteEmp);
            menu.Items.Add(address);
            menu.Items.Add(phone);

            if (Admin == false)
                EnableMenuItems(menu, new bool[] { true, false, false, false, false });
            else if (Admin == true)
                EnableMenuItems(menu, new bool[] { true, true, true, true, true });

            return menu;
        }

        /// <summary>
        /// Контекстное меню для таблицы отпусков
        /// </summary>
        /// <returns></returns>
        private ContextMenu VacationsCm()
        {
            var menu = new ContextMenu();
            var editVac = new MenuItem { Header = "Редактировать" };
            var deleteVac = new MenuItem { Header = "Удалить" };

            editVac.Click += EditClick;
            deleteVac.Click += DeleteClick;
            
            menu.Items.Add(editVac);
            menu.Items.Add(deleteVac);

            if (Admin is false)
                EnableMenuItems(menu, new bool[] {false, false, false});
            else if (Admin is true)
            {
                EnableMenuItems(menu, new bool[] {true, true, true});
            }

            return menu;
        }

        #region Работа с адресами и телефонами

        /// <summary>
        /// Добавить номер телефона
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddPhoneClick(object sender, RoutedEventArgs e)
        {
            var emp = ((DgEmployees).SelectedItem as Employee);

            var wnd = new UI.Windows.Employee
            {
                Content = new UI.Pages.AddPhone
                {
                    ConnStr = ConnStr,
                    Emp = emp,
                    Context = Context
                }
            };
            wnd.ShowDialog();
        }

        /// <summary>
        /// Добавить адрес
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddAddressClick(object sender, RoutedEventArgs e)
        {
            var emp = ((DgEmployees).SelectedItem as Employee);

            var wnd = new UI.Windows.Employee
            {
                Content = new UI.Pages.AddAddress
                {
                    ConnStr = ConnStr,
                    Emp = emp,
                    Context = Context
                }
            };
            wnd.ShowDialog();
        }

        #endregion

        #region Операции над элементами в таблицах

        /// <summary>
        /// Просмотр информации о сотруднике
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewClick(object sender, RoutedEventArgs e)
        {
            var emp = ((DgEmployees).SelectedItem as Employee);

            var wnd = new UI.Windows.Employee
            {
                Content = new UI.Pages.ViewEmployee
                {
                    Context = Context,
                    Emp = emp,
                    ConnStr = ConnStr
                }
            };
            wnd.ShowDialog();
        }

        /// <summary>
        /// Удаление позиции в таблице
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteClick(object sender, RoutedEventArgs e)
        {
            dynamic unit = 0, unitParent = 0;
            var dataGrid = sender as DataGrid;

            if (dataGrid.SelectedItem != null)
            {
                if (dataGrid.Name == "DGEmployees")
                {
                    unit = DgEmployees.SelectedItem as Employee;
                    unitParent = DgDepartments.SelectedItem as Department;

                    unitParent.DeleteEmployee(ConnStr, unit.Id);
                    unitParent.Employees.Remove(unit);
                }
                else if (dataGrid.Name == "DGVacations")
                {
                    unit = DgVacations.SelectedItem as Vacation;
                    unitParent = DgEmployees.SelectedItem as Employee;

                    unitParent.DeleteVacation(ConnStr, unit.Id);
                    unitParent.Vacations.Remove(unit);
                }
            }
        }

        /// <summary>
        /// Добавление позиции в таблицу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddClick(object sender, RoutedEventArgs e)
        {
            if (DgDepartments.SelectedItem != null)
            {
                if (TcTables.SelectedIndex == 2)
                {
                    //получим отдел, в который добавить сотрудника
                    var selectedDepartament = DgDepartments.SelectedItem as Department;
                    var wnd = new UI.Windows.Employee();

                    //добавим в указанный отдел или в общий список
                    if (selectedDepartament != null)
                    {
                        wnd.Content = new UI.Pages.AddEmployee
                        {
                            Dep = selectedDepartament,
                            ConnStr = ConnStr
                        };
                    }
                    wnd.ShowDialog();
                }
                else if (TcTables.SelectedIndex == 3)
                {
                    if (DgEmployees.SelectedItem != null)
                    {
                        //получаем сотрудника, которому добавить отпуск
                        var selectedEmployee = DgEmployees.SelectedItem as Employee;
                        var wnd = new UI.Windows.Vacation();

                        if (selectedEmployee != null)
                        {
                            wnd.Content = new UI.Pages.AddVacation
                            {
                                Emp = selectedEmployee,
                                ConnStr = ConnStr
                            };
                            wnd.ShowDialog();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Редактирование позиции в таблице
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditClick(object sender, RoutedEventArgs e)
        {
            var dgTable = new DataGrid();

            if (TcTables.SelectedIndex == 2)
            {
                dgTable = DgEmployees;
                if (dgTable.SelectedItem is Employee emp)
                {
                    var wnd = new UI.Windows.Employee
                    {
                        Content = new UI.Pages.EditEmployee
                        {
                            Emp = emp,
                            ConnStr = ConnStr,
                            Context = Context
                        }
                    };
                    wnd.ShowDialog();
                }
            }
            if (TcTables.SelectedIndex == 3)
            {
                dgTable = DgVacations;
                if (dgTable.SelectedItem is Vacation vac)
                {
                    var wnd = new UI.Windows.Vacation
                    {
                        Content = new UI.Pages.EditVacation
                        {
                            Vac = vac,
                            ConnStr = ConnStr,
                            Context = Context
                        }
                    };
                    wnd.ShowDialog();
                }
            }
            Context.InitializeContext();
        }

        #endregion

        #region Заполнение таблиц

        /// <summary>
        /// Заполняем определенную таблицу данными
        /// </summary>
        /// <param name="dg"></param>
        /// <param name="foreignKey"></param>
        /// <param name="position"></param>
        private void FillDataGrid(DataGrid dg, int foreignKey, string position)
         {
            //заполним таблицу управлений
            if (Equals(dg, DgOffices))
            {
                DgOffices.ItemsSource = Context.Offices;

                DgOffices.Visibility = Visibility.Visible;
                Dispatcher.BeginInvoke((Action)(() => TcTables.SelectedIndex = 0));
            }
            //заполним датагрид отделов
            if (Equals(dg, DgDepartments))
            {
                var office = Context.Offices.Find(x => x.Id == foreignKey);
                DgDepartments.ItemsSource = office.Departments;
                                   
                DgDepartments.Visibility = Visibility.Visible;
                Dispatcher.BeginInvoke((Action)(() => TcTables.SelectedIndex = 1));
            }
            //заполним датагрид сотрудников
            if (Equals(dg, DgEmployees))
            {
                var dep = Context.Departments.Find(x => x.Id == foreignKey);

                DgEmployees.ItemsSource = position != null ? dep.Employees.Where(x => x.Position == position) : dep.Employees;

                DgEmployees.Visibility = Visibility.Visible;
                Dispatcher.BeginInvoke((Action)(() => TcTables.SelectedIndex = 2));
            }
            //заполним датагрид отпусков
            if (Equals(dg, DgVacations))
            {
                var emp = Context.Employees.Find(x => x.Id == foreignKey);
                DgVacations.ItemsSource = emp.Vacations;

                DgVacations.Visibility = Visibility.Visible;
                Dispatcher.BeginInvoke((Action)(() => TcTables.SelectedIndex = 3));
            }
        }

        #endregion

        /// <summary>
        /// Работают определенные пункты контекстного меню
        /// </summary>
        /// <param name="cm"> Контекстное меню </param>
        /// <param name="m1"> работает/нет </param>
        /// <param name="m2"> работает/нет </param>
        /// <param name="m3"> работает/нет </param>
        /// <param name="enabled"></param>
        private static void EnableMenuItems(ItemsControl cm, IReadOnlyList<bool> enabled)
        {
            var items = cm.Items.Cast<MenuItem>().ToList();
            
            for(var i = 0; i < items.Count; i++)
            {
                items[i].IsEnabled = enabled[i];
            }
        }

        /// <summary>
        /// Переход между табоицами
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DG_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var dataGrid = sender as DataGrid;
            if (dataGrid != null && dataGrid.Name == "DGOffices")
            {
                if ((dataGrid.SelectedItem) is Office officeItem)
                {
                    FillDataGrid(DgDepartments, officeItem.Id, null);
                }
            }
            if (dataGrid != null && dataGrid.Name == "DGDepartments")
            {
                if ((dataGrid.SelectedItem) is Department departmentItem)
                {
                    FillDataGrid(DgEmployees, departmentItem.Id, null);
                }
            }
            if (dataGrid != null && dataGrid.Name == "DGEmployees")
            {
                if ((dataGrid.SelectedItem) is Employee employeeItem)
                {
                    FillDataGrid(DgVacations, employeeItem.Id, null);
                }
            }
        }

        private void TCTables_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TcTables.SelectedIndex == 2 && DgEmployees.ItemsSource != null)
            {
                GShowEmps.IsEnabled = true;
            }
            else
            {
                GShowEmps.IsEnabled = false;
            }
        }

        private void CBPositions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FillDataGrid(DgEmployees, ((Department) DgDepartments.SelectedItem).Id, (CbPositions.SelectedItem as Data.Position)?.Description);
        }
    }
}
