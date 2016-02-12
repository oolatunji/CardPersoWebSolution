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
    public class RoleDL
    {
        public static bool Save(Role role)
        {
            bool result = false;
            var conn = OracleDL.connect();
            OracleTransaction txn = conn.BeginTransaction(IsolationLevel.ReadCommitted);
            try
            {
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "INSERT INTO SYSTEMROLES(NAME) VALUES(:name) RETURNING ID INTO :id";
                cmd.Parameters.Add(":name", OracleDbType.Varchar2, role.Name, ParameterDirection.Input);

                OracleParameter outputParameter = new OracleParameter("id", OracleDbType.Int32);
                outputParameter.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputParameter);

                int rowsInserted = cmd.ExecuteNonQuery();
                if (rowsInserted > 0)
                {
                    var roleId = Convert.ToInt32(outputParameter.Value.ToString());
                    var rolefunctions = (from function in role.Functions
                                         select new RoleFunctions()
                                         {
                                             RoleId = roleId,
                                             FunctionId = function.Id
                                         }).ToList();

                    if(RoleFunctionsDL.Save(rolefunctions, conn))
                    {
                        result = true;
                        txn.Commit();
                    }
                }

                OracleDL.close(conn);

                return result;
            }
            catch (Exception ex)
            {
                txn.Rollback();
                throw ex;
            }
        }

        public static bool Update(Role role)
        {
            bool result = false;
            var conn = OracleDL.connect();
            OracleTransaction txn = conn.BeginTransaction(IsolationLevel.ReadCommitted);
            try
            {
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE SYSTEMROLES SET NAME = :name WHERE ID = :id";
                cmd.Parameters.Add(":name", OracleDbType.Varchar2, role.Name, ParameterDirection.Input);
                cmd.Parameters.Add(":id", OracleDbType.Int32, role.Id, ParameterDirection.Input);

                int rowsInserted = cmd.ExecuteNonQuery();
                if (rowsInserted > 0)
                {
                    cmd = conn.CreateCommand();
                    cmd.CommandText = "DELETE FROM SYSTEMROLEFUNCTIONS WHERE ROLEID = :id";
                    cmd.Parameters.Add(":id", OracleDbType.Int32, role.Id, ParameterDirection.Input);

                    int rowsdeleted = cmd.ExecuteNonQuery();
                    if (rowsdeleted > 0)
                    {
                        var roleId = role.Id;
                        var rolefunctions = (from function in role.Functions
                                             select new RoleFunctions()
                                             {
                                                 RoleId = roleId,
                                                 FunctionId = function.Id
                                             }).ToList();

                        if (RoleFunctionsDL.Save(rolefunctions, conn))
                        {
                            result = true;
                            txn.Commit();
                        }
                    }
                }

                OracleDL.close(conn);

                return result;
            }
            catch (Exception ex)
            {
                txn.Rollback();
                throw ex;
            }
        }

        public static bool RoleExists(string name)
        {
            try
            {
                var roles = new List<Role>();

                var conn = OracleDL.connect();

                string query = @"SELECT SR.ID AS ROLEID, SR.NAME AS ROLENAME, SF.ID, SF.NAME, SF.PAGELINK
                                 FROM SYSTEMROLES SR
                                 INNER JOIN SYSTEMROLEFUNCTIONS SRF ON SR.ID = SRF.ROLEID
                                 INNER JOIN SYSTEMFUNCTIONS SF ON SRF.FUNCTIONID = SF.ID
                                 WHERE SR.Name = :name";

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = query;
                cmd.Parameters.Add(":name", OracleDbType.Varchar2, name, ParameterDirection.Input);

                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    roles.Add(Role.Transform(dr, true));
                }

                OracleDL.close(conn);

                return roles.Any() ? true : false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Role> RetrieveAll()
        {
            try
            {
                var roles = new List<Role>();

                var conn = OracleDL.connect();

                string query = @"SELECT SR.ID AS ROLEID, SR.NAME AS ROLENAME, SF.ID, SF.NAME, SF.PAGELINK
                                 FROM SYSTEMROLES SR
                                 INNER JOIN SYSTEMROLEFUNCTIONS SRF ON SR.ID = SRF.ROLEID
                                 INNER JOIN SYSTEMFUNCTIONS SF ON SRF.FUNCTIONID = SF.ID";

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = query;

                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    roles.Add(Role.Transform(dr, true));
                }

                OracleDL.close(conn);

                return roles;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
