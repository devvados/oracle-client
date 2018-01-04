using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop.Data
{
    public class Vacation
    {
        public int Id { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public int EmployeeId { get; set; }
        public string EmpName { get; set; }
        public string EmpSurname { get; set; }
        public string EmpPatronymic { get; set; }

        public void UpdateVacation(string connStr)
        {
            using (var objConn = new OracleConnection(connStr))
            {
                //cобираем команду для выполнения
                var objCmd = new OracleCommand
                {
                    Connection = objConn,
                    CommandText = "SYSTEM.UPDATEVACATION",
                    CommandType = CommandType.StoredProcedure
                };
                var op1 = new OracleParameter("p_vacID", OracleDbType.Int32)
                {
                    Direction = ParameterDirection.Input,
                    Value = Id
                };
                var op2 = new OracleParameter("p_Begin", OracleDbType.NVarchar2)
                {
                    Direction = ParameterDirection.Input,
                    Value = BeginDate
                };
                var op3 = new OracleParameter("p_End", OracleDbType.NVarchar2)
                {
                    Direction = ParameterDirection.Input,
                    Value = EndDate
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
    }
}
