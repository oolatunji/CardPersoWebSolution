using CardPerso.Library.ModelLayer.Model;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardPerso.Library.DataLayer
{
    public class SystemIdManagerDL
    {
        public static SystemIdManager Retrieve()
        {
            try
            {
                var systemIdManager = new List<SystemIdManager>();

                var conn = OracleDL.connect();

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM SYSTEMIDMANAGER WHERE ID = 1";                

                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    systemIdManager.Add(SystemIdManager.Transform(dr));
                }

                OracleDL.close(conn);

                return systemIdManager.Any() ? systemIdManager.FirstOrDefault() : null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Update(SystemIdManager systemIdManager, OracleConnection conn)
        {
            try
            {
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "update SYSTEMIDMANAGER set USERID = :1, FUNCTIONID = :2, ROLEID = :3, ROLEFUNCTIONID = :4, APPROVALID = :5 Where ID = :6";
                cmd.Parameters.Add(":1", OracleDbType.Int32, systemIdManager.UserId, ParameterDirection.Input);
                cmd.Parameters.Add(":2", OracleDbType.Int32, systemIdManager.FunctionId, ParameterDirection.Input);
                cmd.Parameters.Add(":3", OracleDbType.Int32, systemIdManager.RoleId, ParameterDirection.Input);
                cmd.Parameters.Add(":4", OracleDbType.Int32, systemIdManager.RoleFunctionId, ParameterDirection.Input);
                cmd.Parameters.Add(":5", OracleDbType.Int32, systemIdManager.ApprovalId, ParameterDirection.Input);
                cmd.Parameters.Add(":6", OracleDbType.Int32, systemIdManager.Id, ParameterDirection.Input);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
