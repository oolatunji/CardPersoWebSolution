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
    public class ApprovalConfigurationDL
    {
        public static List<ApprovalConfiguration> RetrieveAll()
        {
            try
            {
                var confs = new List<ApprovalConfiguration>();

                var conn = OracleDL.connect();

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM SYSTEMAPPROVALCONFIGURATION";

                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    confs.Add(ApprovalConfiguration.Transform(dr));
                }

                OracleDL.close(conn);

                return confs;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static ApprovalConfiguration RetrieveByType(string type)
        {
            try
            {
                var confs = new List<ApprovalConfiguration>();

                var conn = OracleDL.connect();

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM SYSTEMAPPROVALCONFIGURATION WHERE TYPE = :type";
                cmd.Parameters.Add(":type", OracleDbType.Varchar2, type, ParameterDirection.Input);

                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    confs.Add(ApprovalConfiguration.Transform(dr));
                }

                OracleDL.close(conn);

                return confs.Any() ? confs.FirstOrDefault() : null;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static bool Update(List<ApprovalConfiguration> confs)
        {
            bool updated = false;
            var conn = OracleDL.connect();
            OracleTransaction txn = conn.BeginTransaction(IsolationLevel.ReadCommitted);
            try
            {
                string query = "UPDATE SYSTEMAPPROVALCONFIGURATION SET APPROVE = :approve WHERE ID =: id";
                using (var command = conn.CreateCommand())
                {
                    command.CommandText = query;
                    command.CommandType = CommandType.Text;
                    command.BindByName = true;
                    // In order to use ArrayBinding, the ArrayBindCount property
                    // of OracleCommand object must be set to the number of records to be inserted
                    command.ArrayBindCount = confs.Count;
                    command.Parameters.Add(":approve", OracleDbType.Int32, confs.Select(rf => rf.ApproveId).ToArray(), ParameterDirection.Input);
                    command.Parameters.Add(":id", OracleDbType.Int32, confs.Select(rf => rf.Id).ToArray(), ParameterDirection.Input);

                    int result = command.ExecuteNonQuery();
                    if (result == confs.Count)
                    {
                        updated = true;
                        txn.Commit();
                    }

                    return updated;
                }
            }
            catch (Exception ex)
            {
                txn.Rollback();
                throw ex;
            }
        }
    }
}
