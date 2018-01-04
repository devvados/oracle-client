using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop.Data
{
    public class Department
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int EmployeesCount { get; set; }
        public int OfficeId { get; set; }
        public string Office { get; set; }

        public List<Employee> Employees { get; set; }

        /// <summary>
        /// Получить сотрудников в отделе
        /// </summary>
        /// <param name="connStr"></param>
        public void GetEmployees(string connStr)
        {
            using (var objConn = new OracleConnection(connStr))
            {
                //cобираем команду для выполнения
                var objCmd = new OracleCommand
                {
                    Connection = objConn,
                    CommandText = "SYSTEM.GETDEPARTMENTEMPLOYEES",
                    CommandType = CommandType.StoredProcedure
                };
                var op1 = new OracleParameter("p_depID", OracleDbType.Int32)
                {
                    Direction = ParameterDirection.Input,
                    Value = Id
                };
                objCmd.Parameters.Add(op1);
                var op2 = new OracleParameter("emp", OracleDbType.RefCursor)
                {
                    Direction = ParameterDirection.Output
                };
                objCmd.Parameters.Add(op2);

                try
                {
                    objConn.Open();
                    objCmd.ExecuteNonQuery();
                    var da = new OracleDataAdapter(objCmd);
                    var dataSet = new DataSet();
                    da.Fill(dataSet, "EMP");

                    var dt = dataSet.Tables[0];
                    var emps = (from rw in dt.AsEnumerable()
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
        }

        /// <summary>
        /// Удалить сотрудника
        /// </summary>
        /// <param name="connStr"></param>
        /// <param name="empId"></param>
        public void DeleteEmployee(string connStr, int empId)
        {
            using (var objConn = new OracleConnection(connStr))
            {
                //cобираем команду для выполнения
                var objCmd = new OracleCommand
                {
                    Connection = objConn,
                    CommandText = "SYSTEM.DELETEEMPLOYEE",
                    CommandType = CommandType.StoredProcedure
                };
                var op1 = new OracleParameter("p_empID", OracleDbType.Int32)
                {
                    Direction = ParameterDirection.Input,
                    Value = empId
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
        /// Добавить сотрудника
        /// </summary>
        /// <param name="connStr"></param>
        /// <param name="emp"></param>
        public void InsertEmployee(string connStr, Employee emp)
        {
            using (var objConn = new OracleConnection(connStr))
            {
                //cобираем команду для выполнения
                var objCmd = new OracleCommand
                {
                    Connection = objConn,
                    CommandText = "SYSTEM.INSERTEMPLOYEE",
                    CommandType = CommandType.StoredProcedure
                };
                var op1 = new OracleParameter("p_Name", OracleDbType.NVarchar2)
                {
                    Direction = ParameterDirection.Input,
                    Value = emp.Name
                };
                var op2 = new OracleParameter("p_Surname", OracleDbType.NVarchar2)
                {
                    Direction = ParameterDirection.Input,
                    Value = emp.Surname
                };
                var op3 = new OracleParameter("p_Patronymic", OracleDbType.NVarchar2)
                {
                    Direction = ParameterDirection.Input,
                    Value = emp.Patronymic
                };
                var op4 = new OracleParameter("p_Position", OracleDbType.Int32)
                {
                    Direction = ParameterDirection.Input,
                    Value = emp.PosId
                };
                var op5 = new OracleParameter("p_depID", OracleDbType.Int32)
                {
                    Direction = ParameterDirection.Input,
                    Value = emp.DepId
                };
                objCmd.Parameters.Add(op1);
                objCmd.Parameters.Add(op2);
                objCmd.Parameters.Add(op3);
                objCmd.Parameters.Add(op4);
                objCmd.Parameters.Add(op5);

                try
                {
                    objConn.Open();
                    objCmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    // ignored
                }

                objConn.Close();
            }
        }
    }
}
