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
    public class RoleFunctionsDL
    {
        public static bool Save(List<RoleFunctions> rolefunctions, OracleConnection conn)
        {
            try
            {
                bool saved = false;
                string query = "INSERT INTO SYSTEMROLEFUNCTIONS (ROLEID, FUNCTIONID) values(:roleid, :functionid)";
                using (var command = conn.CreateCommand())
                {
                    command.CommandText = query;
                    command.CommandType = CommandType.Text;
                    command.BindByName = true;
                    // In order to use ArrayBinding, the ArrayBindCount property
                    // of OracleCommand object must be set to the number of records to be inserted
                    command.ArrayBindCount = rolefunctions.Count;
                    command.Parameters.Add(":roleid", OracleDbType.Int32, rolefunctions.Select(rf => rf.RoleId).ToArray(), ParameterDirection.Input);
                    command.Parameters.Add(":functionid", OracleDbType.Int32, rolefunctions.Select(rf => rf.FunctionId).ToArray(), ParameterDirection.Input);

                    int result = command.ExecuteNonQuery();
                    if (result == rolefunctions.Count)
                        saved = true;

                    return saved;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
