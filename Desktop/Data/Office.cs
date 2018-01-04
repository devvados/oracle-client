using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop.Data
{
    public class Office
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int DepartmentsCount { get; set; }

        public List<Department> Departments { get; set; }

        /// <summary>
        /// Получить отделы в офисе
        /// </summary>
        /// <param name="connStr"></param>
        public void GetDepartments(string connStr)
        {
            using (var objConn = new OracleConnection(connStr))
            {
                //cобираем команду для выполнения
                var objCmd = new OracleCommand
                {
                    Connection = objConn,
                    CommandText = "SYSTEM.GETOFFICEDEPARTMENTS",
                    CommandType = CommandType.StoredProcedure
                };
                var op1 = new OracleParameter("p_officeID", OracleDbType.Int32)
                {
                    Direction = ParameterDirection.Input,
                    Value = Id
                };
                objCmd.Parameters.Add(op1);
                var op2 = new OracleParameter("dep", OracleDbType.RefCursor)
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
                    da.Fill(dataSet, "DEP");

                    var dt = dataSet.Tables[0];
                    var deps = (from rw in dt.AsEnumerable()
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
        }
    }
}
