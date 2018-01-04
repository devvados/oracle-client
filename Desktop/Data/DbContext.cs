using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop.Data
{
    public class DbContext
    {
        private readonly string _connStr;

        public DbContext(string conn)
        {
            _connStr = conn;
        }

        public void InitializeContext()
        {
            GetAllOffices(_connStr);
            GetAllPositions(_connStr);
            GetAllDepartments(_connStr);
            GetAllEmployees(_connStr);
            GetAllVacations(_connStr);
            GetAllAddresses(_connStr);
            GetAllPhones(_connStr);
        }

        public List<Office> Offices { get; set; }
        public List<Position> Positions { get; set; }
        public List<Department> Departments { get; set; }
        public List<Employee> Employees { get; set; }
        public List<Vacation> Vacations { get; set; }
        public List<Address> Addresses { get; set; }
        public List<Phone> Phones { get; set; }

        /// <summary>
        /// Получить все офисы
        /// </summary>
        /// <param name="connStr"></param>
        public void GetAllOffices(string connStr)
        {
            using (var objConn = new OracleConnection(connStr))
            {
                //cобираем команду для выполнения
                var objCmd = new OracleCommand
                {
                    Connection = objConn,
                    CommandText = "SYSTEM.GETALLOFFICES",
                    CommandType = CommandType.StoredProcedure
                };
                var op1 = new OracleParameter("office", OracleDbType.RefCursor)
                {
                    Direction = ParameterDirection.Output
                };
                objCmd.Parameters.Add(op1);

                try
                {
                    objConn.Open();
                    objCmd.ExecuteNonQuery();
                    var da = new OracleDataAdapter(objCmd);
                    var dataSet = new DataSet();
                    da.Fill(dataSet, "OFF");

                    var dt = dataSet.Tables[0];
                    var offices = (from rw in dt.AsEnumerable()
                        select new Office()
                        {
                            Id = Convert.ToInt32(rw[0]),
                            Description = Convert.ToString(rw[1]),
                            DepartmentsCount = Convert.ToInt32(rw[2])
                        }).ToList();
                    //заполняем сотрудников
                    offices.ForEach(x => x.GetDepartments(connStr));

                    //заполняем отделы
                    Offices = new List<Office>(offices);
                }
                catch (Exception ex)
                {
                    // ignored
                }

                objConn.Close();
            }
        }

        /// <summary>
        /// Получить все профессии
        /// </summary>
        /// <param name="connStr"></param>
        public void GetAllPositions(string connStr)
        {
            using (var objConn = new OracleConnection(connStr))
            {
                //cобираем команду для выполнения
                var objCmd = new OracleCommand
                {
                    Connection = objConn,
                    CommandText = "SYSTEM.GETALLPOSITIONS",
                    CommandType = CommandType.StoredProcedure
                };
                var op1 = new OracleParameter("POS", OracleDbType.RefCursor)
                {
                    Direction = ParameterDirection.Output
                };
                objCmd.Parameters.Add(op1);

                try
                {
                    objConn.Open();
                    objCmd.ExecuteNonQuery();
                    var da = new OracleDataAdapter(objCmd);
                    var dataSet = new DataSet();
                    da.Fill(dataSet, "POS");

                    var dt = dataSet.Tables[0];
                    var positions = (from rw in dt.AsEnumerable()
                        select new Position()
                        {
                            Id = Convert.ToInt32(rw[0]),
                            Description = Convert.ToString(rw[1]),
                            Salary = Convert.ToInt32(rw[2])
                        }).ToList();

                    //заполняем отделы
                    Positions = new List<Position>(positions);
                }
                catch (Exception ex)
                {
                    // ignored
                }

                objConn.Close();
            }
        }

        /// <summary>
        /// Получить все отделы
        /// </summary>
        /// <param name="connStr"></param>
        public void GetAllDepartments(string connStr)
        {
            var deps = new List<Department>();

            using (var objConn = new OracleConnection(connStr))
            {
                //cобираем команду для выполнения
                var objCmd = new OracleCommand
                {
                    Connection = objConn,
                    CommandText = "SYSTEM.GETALLDEPARTMENTS",
                    CommandType = CommandType.StoredProcedure
                };
                var op1 = new OracleParameter("dep", OracleDbType.RefCursor)
                {
                    Direction = ParameterDirection.Output
                };
                objCmd.Parameters.Add(op1);

                try
                {
                    objConn.Open();
                    objCmd.ExecuteNonQuery();
                    var da = new OracleDataAdapter(objCmd);
                    var dataSet = new DataSet();
                    da.Fill(dataSet, "DEP");

                    var dt = dataSet.Tables[0];
                    deps = (from rw in dt.AsEnumerable()
                            select new Department()
                            {
                                Id = Convert.ToInt32(rw[0]),
                                Description = Convert.ToString(rw[1]),
                                EmployeesCount = Convert.ToInt32(rw[2]),
                                Office = Convert.ToString(rw[5])
                            }).ToList();
                    //заполняем сотрудников
                    deps.ForEach(x => x.GetEmployees(connStr));

                    //заполняем отделы
                    Departments = new List<Department>(deps);
                }
                catch (Exception ex)
                {
                    // ignored
                }

                objConn.Close();
            }

            Departments = new List<Department>(deps);
        }

        /// <summary>
        /// Получить всех сотрудников
        /// </summary>
        /// <param name="connStr"></param>
        public void GetAllEmployees(string connStr)
        {
            var emps = new List<Employee>();

            using (var objConn = new OracleConnection(connStr))
            {
                //cобираем команду для выполнения
                var objCmd = new OracleCommand
                {
                    Connection = objConn,
                    CommandText = "SYSTEM.GETALLEMPLOYEES",
                    CommandType = CommandType.StoredProcedure
                };
                var op1 = new OracleParameter("emp", OracleDbType.RefCursor)
                {
                    Direction = ParameterDirection.Output
                };
                objCmd.Parameters.Add(op1);

                try
                {
                    objConn.Open();
                    objCmd.ExecuteNonQuery();
                    var da = new OracleDataAdapter(objCmd);
                    var dataSet = new DataSet();
                    da.Fill(dataSet, "EMP");

                    var dt = dataSet.Tables[0];
                    emps = (from rw in dt.AsEnumerable()
                            select new Employee()
                            {
                                Id = Convert.ToInt32(rw[0]),
                                Name = Convert.ToString(rw[1]),
                                Surname = Convert.ToString(rw[2]),
                                Patronymic = Convert.ToString(rw[3]),
                                VacationsCount = Convert.ToInt32(rw[4]),
                                Department = Convert.ToString(rw[8]),
                                Position = Convert.ToString(rw[12])
                            }).ToList();
                    emps.ForEach(x => x.GetAddress(connStr));
                    emps.ForEach(x => x.GetTelephone(connStr));
                    emps.ForEach(x => x.GetVacation(connStr));

                    Employees = new List<Employee>(emps);
                }
                catch (Exception ex)
                {
                    // ignored
                }

                objConn.Close();
            }

            Employees = new List<Employee>(emps);
        }

        /// <summary>
        /// Получить все отпуски
        /// </summary>
        /// <param name="connStr"></param>
        public void GetAllVacations(string connStr)
        {
            var vacations = new List<Vacation>();

            using (var objConn = new OracleConnection(connStr))
            {
                //cобираем команду для выполнения
                var objCmd = new OracleCommand
                {
                    Connection = objConn,
                    CommandText = "SYSTEM.GETALLVACATIONS",
                    CommandType = CommandType.StoredProcedure
                };
                var op1 = new OracleParameter("vac", OracleDbType.RefCursor)
                {
                    Direction = ParameterDirection.Output
                };
                objCmd.Parameters.Add(op1);

                try
                {
                    objConn.Open();
                    objCmd.ExecuteNonQuery();
                    var da = new OracleDataAdapter(objCmd);
                    var dataSet = new DataSet();
                    da.Fill(dataSet, "VACS");

                    var dt = dataSet.Tables[0];
                    vacations = (from rw in dt.AsEnumerable()
                                 select new Vacation()
                                 {
                                     Id = Convert.ToInt32(rw[0]),
                                     BeginDate = Convert.ToDateTime(rw[1]),
                                     EndDate = Convert.ToDateTime(rw[2]),
                                     EmployeeId = Convert.ToInt32(rw[3]),
                                     EmpName = Convert.ToString(rw[5]),
                                     EmpSurname = Convert.ToString(rw[6]),
                                     EmpPatronymic = Convert.ToString(rw[7])

                                 }).ToList();


                }
                catch (Exception ex)
                {
                    // ignored
                }

                objConn.Close();
            }

            Vacations = new List<Vacation>(vacations);
        }
        
        /// <summary>
        /// Получить все адреса
        /// </summary>
        /// <param name="connStr"></param>
        public void GetAllAddresses(string connStr)
        {
            var addresses = new List<Address>();

            using (var objConn = new OracleConnection(connStr))
            {
                //cобираем команду для выполнения
                var objCmd = new OracleCommand
                {
                    Connection = objConn,
                    CommandText = "SYSTEM.GETALLADDRESSES",
                    CommandType = CommandType.StoredProcedure
                };
                var op1 = new OracleParameter("addr", OracleDbType.RefCursor)
                {
                    Direction = ParameterDirection.Output
                };
                objCmd.Parameters.Add(op1);

                try
                {
                    objConn.Open();
                    objCmd.ExecuteNonQuery();
                    var da = new OracleDataAdapter(objCmd);
                    var dataSet = new DataSet();
                    da.Fill(dataSet, "ADDR");

                    var dt = dataSet.Tables[0];
                    addresses = (from rw in dt.AsEnumerable()
                                 select new Address()
                                 {
                                     Id = Convert.ToInt32(rw[0]),
                                     Line = Convert.ToString(rw[1]),
                                     PostalCode = Convert.ToString(rw[2]),
                                     Type = Convert.ToInt32(rw[3]),
                                     AddressType = Convert.ToString(rw[5])

                                 }).ToList();


                }
                catch (Exception ex)
                {
                    // ignored
                }

                objConn.Close();
            }

            Addresses = new List<Address>(addresses);
        }

        /// <summary>
        /// Получить все телефоны
        /// </summary>
        /// <param name="connStr"></param>
        public void GetAllPhones(string connStr)
        {
            var phones = new List<Phone>();

            using (var objConn = new OracleConnection(connStr))
            {
                //cобираем команду для выполнения
                var objCmd = new OracleCommand
                {
                    Connection = objConn,
                    CommandText = "SYSTEM.GETALLTELEPHONES",
                    CommandType = CommandType.StoredProcedure
                };
                var op1 = new OracleParameter("tel", OracleDbType.RefCursor)
                {
                    Direction = ParameterDirection.Output
                };
                objCmd.Parameters.Add(op1);

                try
                {
                    objConn.Open();
                    objCmd.ExecuteNonQuery();
                    var da = new OracleDataAdapter(objCmd);
                    var dataSet = new DataSet();
                    da.Fill(dataSet, "PH");

                    var dt = dataSet.Tables[0];
                    phones = (from rw in dt.AsEnumerable()
                              select new Phone()
                              {
                                  Id = Convert.ToInt32(rw[0]),
                                  Number = Convert.ToString(rw[1]),
                                  Type = Convert.ToInt32(rw[2]),
                                  PhoneType = Convert.ToString(rw[4])

                              }).ToList();


                }
                catch (Exception ex)
                {
                    // ignored
                }

                objConn.Close();
            }

            Phones = new List<Phone>(phones);
        }
    }
}
