using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop.Data
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public int VacationsCount { get; set; }
        public int PosId { get; set; }
        public int DepId { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
        
        public List<Address> Addresses { get; set; }
        public List<Phone> Phones { get; set; }
        public List<Vacation> Vacations { get; set; }

        /// <summary>
        /// Добавить адрес
        /// </summary>
        /// <param name="connStr"></param>
        /// <param name="addr"></param>
        public void InsertAddress(string connStr, Address addr)
        {
            using (var objConn = new OracleConnection(connStr))
            {
                //cобираем команду для выполнения
                var objCmd = new OracleCommand
                {
                    Connection = objConn,
                    CommandText = "SYSTEM.INSERTADDRESS",
                    CommandType = CommandType.StoredProcedure
                };
                var op1 = new OracleParameter("p_line", OracleDbType.NVarchar2)
                {
                    Direction = ParameterDirection.Input,
                    Value = addr.Line
                };
                var op2 = new OracleParameter("p_postalCode", OracleDbType.NVarchar2)
                {
                    Direction = ParameterDirection.Input,
                    Value = addr.PostalCode
                };
                var op3 = new OracleParameter("p_type", OracleDbType.Int32)
                {
                    Direction = ParameterDirection.Input,
                    Value = addr.Type
                };
                var op4 = new OracleParameter("p_empID", OracleDbType.Int32)
                {
                    Direction = ParameterDirection.Input,
                    Value = Id
                };
                objCmd.Parameters.Add(op1);
                objCmd.Parameters.Add(op2);
                objCmd.Parameters.Add(op3);
                objCmd.Parameters.Add(op4);

                try
                {
                    objConn.Open();
                    objCmd.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    // ignored
                }

                objConn.Close();
            }
        }

        /// <summary>
        /// Получить все адреса
        /// </summary>
        /// <param name="connStr"></param>
        public void GetAddress(string connStr)
        {
            var addresses = new List<Address>();

            using (var objConn = new OracleConnection(connStr))
            {
                //cобираем команду для выполнения
                var objCmd = new OracleCommand
                {
                    Connection = objConn,
                    CommandText = "SYSTEM.GETEMPLOYEEADDRESS",
                    CommandType = CommandType.StoredProcedure
                };
                var op1 = new OracleParameter("p_empID", OracleDbType.Int32)
                {
                    Direction = ParameterDirection.Input,
                    Value = Id
                };
                var op2 = new OracleParameter("addr", OracleDbType.RefCursor)
                {
                    Direction = ParameterDirection.Output
                };
                objCmd.Parameters.Add(op1);
                objCmd.Parameters.Add(op2);

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
                catch (Exception ex) { }

                objConn.Close();
            }

            Addresses = new List<Address>(addresses);
        }

        /// <summary>
        /// Удалить адрес
        /// </summary>
        /// <param name="connStr"></param>
        /// <param name="addrId"></param>
        public void DeleteAddress(string connStr, int addrId)
        {
            using (var objConn = new OracleConnection(connStr))
            {
                //cобираем команду для выполнения
                var objCmd = new OracleCommand
                {
                    Connection = objConn,
                    CommandText = "SYSTEM.DELETEADDRESS",
                    CommandType = CommandType.StoredProcedure
                };
                var op1 = new OracleParameter("p_addrID", OracleDbType.Int32)
                {
                    Direction = ParameterDirection.Input,
                    Value = addrId
                };
                objCmd.Parameters.Add(op1);

                try
                {
                    objConn.Open();
                    objCmd.ExecuteNonQuery();
                }
                catch (Exception ex) { }

                objConn.Close();
            }
        }

        /// <summary>
        /// Добавить телефон
        /// </summary>
        /// <param name="connStr"></param>
        /// <param name="ph"></param>
        public void InsertTelephone(string connStr, Phone ph)
        {
            using (var objConn = new OracleConnection(connStr))
            {
                //cобираем команду для выполнения
                var objCmd = new OracleCommand
                {
                    Connection = objConn,
                    CommandText = "SYSTEM.INSERTTELEPHONE",
                    CommandType = CommandType.StoredProcedure
                };
                var op1 = new OracleParameter("p_number", OracleDbType.NVarchar2)
                {
                    Direction = ParameterDirection.Input,
                    Value = ph.Number
                };
                var op2 = new OracleParameter("p_type", OracleDbType.NVarchar2)
                {
                    Direction = ParameterDirection.Input,
                    Value = ph.Type
                };
                var op3 = new OracleParameter("p_empID", OracleDbType.Int32)
                {
                    Direction = ParameterDirection.Input,
                    Value = Id
                };
                objCmd.Parameters.Add(op1);
                objCmd.Parameters.Add(op2);
                objCmd.Parameters.Add(op3);

                try
                {
                    objConn.Open();
                    objCmd.ExecuteNonQuery();
                }
                catch (Exception) { }

                objConn.Close();
            }
        }

        /// <summary>
        /// Получить все телефоны
        /// </summary>
        /// <param name="connStr"></param>
        public void GetTelephone(string connStr)
        {
            var phones = new List<Phone>();

            using (var objConn = new OracleConnection(connStr))
            {
                //cобираем команду для выполнения
                var objCmd = new OracleCommand
                {
                    Connection = objConn,
                    CommandText = "SYSTEM.GETEMPLOYEETELEPHONE",
                    CommandType = CommandType.StoredProcedure
                };
                var op1 = new OracleParameter("p_empID", OracleDbType.Int32)
                {
                    Direction = ParameterDirection.Input,
                    Value = Id
                };
                var op2 = new OracleParameter("tel", OracleDbType.RefCursor)
                {
                    Direction = ParameterDirection.Output
                };
                objCmd.Parameters.Add(op1);
                objCmd.Parameters.Add(op2);

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
                catch (Exception ex) { }

                objConn.Close();
            }

            Phones = new List<Phone>(phones);
        }

        /// <summary>
        /// Удалить телефон
        /// </summary>
        /// <param name="connStr"></param>
        /// <param name="telId"></param>
        public void DeleteTelephone(string connStr, int telId)
        {
            using (var objConn = new OracleConnection(connStr))
            {
                //cобираем команду для выполнения
                var objCmd = new OracleCommand
                {
                    Connection = objConn,
                    CommandText = "SYSTEM.DELETETELEPHONE",
                    CommandType = CommandType.StoredProcedure
                };
                var op1 = new OracleParameter("p_telID", OracleDbType.Int32)
                {
                    Direction = ParameterDirection.Input,
                    Value = telId
                };
                objCmd.Parameters.Add(op1);

                try
                {
                    objConn.Open();
                    objCmd.ExecuteNonQuery();
                }
                catch (Exception ex) { }

                objConn.Close();
            }
        }

        /// <summary>
        /// Обновить данные сотрудника
        /// </summary>
        /// <param name="connStr"></param>
        public void UpdateEmployee(string connStr)
        {
            using (var objConn = new OracleConnection(connStr))
            {
                //cобираем команду для выполнения
                var objCmd = new OracleCommand
                {
                    Connection = objConn,
                    CommandText = "SYSTEM.UPDATEEMPLOYEE",
                    CommandType = CommandType.StoredProcedure
                };
                var op1 = new OracleParameter("p_empID", OracleDbType.Int32)
                {
                    Direction = ParameterDirection.Input,
                    Value = Id
                };
                var op2 = new OracleParameter("p_name", OracleDbType.NVarchar2)
                {
                    Direction = ParameterDirection.Input,
                    Value = Name
                };
                var op3 = new OracleParameter("p_surname", OracleDbType.NVarchar2)
                {
                    Direction = ParameterDirection.Input,
                    Value = Surname
                };
                var op4 = new OracleParameter("p_patronymic", OracleDbType.NVarchar2)
                {
                    Direction = ParameterDirection.Input,
                    Value = Patronymic
                };
                var op5 = new OracleParameter("p_posID", OracleDbType.Int32)
                {
                    Direction = ParameterDirection.Input,
                    Value = PosId
                };
                var op6 = new OracleParameter("p_depID", OracleDbType.Int32)
                {
                    Direction = ParameterDirection.Input,
                    Value = DepId
                };
                objCmd.Parameters.Add(op1);
                objCmd.Parameters.Add(op2);
                objCmd.Parameters.Add(op3);
                objCmd.Parameters.Add(op4);
                objCmd.Parameters.Add(op5);
                objCmd.Parameters.Add(op6);

                try
                {
                    objConn.Open();
                    objCmd.ExecuteNonQuery();
                }
                catch (Exception) { }

                objConn.Close();
            }
        }

        /// <summary>
        /// Добавить отпуск
        /// </summary>
        /// <param name="connStr"></param>
        /// <param name="vac"></param>
        public void InsertVacation(string connStr, Vacation vac)
        {
            using (var objConn = new OracleConnection(connStr))
            {
                //cобираем команду для выполнения
                var objCmd = new OracleCommand
                {
                    Connection = objConn,
                    CommandText = "SYSTEM.INSERTVACATION",
                    CommandType = CommandType.StoredProcedure
                };
                var op1 = new OracleParameter("p_Begin", OracleDbType.NVarchar2)
                {
                    Direction = ParameterDirection.Input,
                    Value = vac.BeginDate.ToShortDateString()
                };
                var op2 = new OracleParameter("p_End", OracleDbType.NVarchar2)
                {
                    Direction = ParameterDirection.Input,
                    Value = vac.EndDate.ToShortDateString()
                };
                var op3 = new OracleParameter("p_empID", OracleDbType.Int32)
                {
                    Direction = ParameterDirection.Input,
                    Value = vac.EmployeeId
                };
                objCmd.Parameters.Add(op1);
                objCmd.Parameters.Add(op2);
                objCmd.Parameters.Add(op3);

                try
                {
                    objConn.Open();
                    objCmd.ExecuteNonQuery();
                }
                catch (Exception) { }

                objConn.Close();
            }
        }

        /// <summary>
        /// Получить все отпуски
        /// </summary>
        /// <param name="connStr"></param>
        public void GetVacation(string connStr)
        {
            var vacations = new List<Vacation>();

            using (var objConn = new OracleConnection(connStr))
            {
                //cобираем команду для выполнения
                var objCmd = new OracleCommand
                {
                    Connection = objConn,
                    CommandText = "SYSTEM.GETEMPLOYEEVACATIONS",
                    CommandType = CommandType.StoredProcedure
                };
                var op1 = new OracleParameter("p_empID", OracleDbType.Int32)
                {
                    Direction = ParameterDirection.Input,
                    Value = Id
                };
                var op2 = new OracleParameter("vac", OracleDbType.RefCursor)
                {
                    Direction = ParameterDirection.Output
                };
                objCmd.Parameters.Add(op1);
                objCmd.Parameters.Add(op2);

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
                catch (Exception ex) { }

                objConn.Close();
            }

            Vacations = new List<Vacation>(vacations);
        }

        /// <summary>
        /// Удалить отпуск
        /// </summary>
        /// <param name="connStr"></param>
        /// <param name="vacId"></param>
        public void DeleteVacation(string connStr, int vacId)
        {
            using (var objConn = new OracleConnection(connStr))
            {
                //cобираем команду для выполнения
                var objCmd = new OracleCommand
                {
                    Connection = objConn,
                    CommandText = "SYSTEM.DELETEVACATION",
                    CommandType = CommandType.StoredProcedure
                };
                var op1 = new OracleParameter("p_vacID", OracleDbType.Int32)
                {
                    Direction = ParameterDirection.Input,
                    Value = vacId
                };
                objCmd.Parameters.Add(op1);

                try
                {
                    objConn.Open();
                    objCmd.ExecuteNonQuery();
                }
                catch (Exception ex) { }

                objConn.Close();
            }
        }
    }
}
