using CardPerso.Library.ModelLayer.Model;
using CardPerso.Library.ModelLayer.Utility;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardPerso.Library.DataLayer
{
    public class AuditTrailDL
    {
        public static bool Save(AuditTrail audit)
        {
            bool result = false;
            var conn = OracleDL.connect();
            OracleTransaction txn = conn.BeginTransaction(IsolationLevel.ReadCommitted);
            try
            {
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "INSERT INTO SYSTEMAUDITTRAIL(TYPE, DETAILS, REQUESTEDBY, REQUESTEDON, APPROVEDBY, APPROVEDON) VALUES(:type, :details, :requestedby, :requestedon, :approvedby, :approvedon)";
                cmd.Parameters.Add(":type", OracleDbType.Varchar2, audit.Type, ParameterDirection.Input);
                cmd.Parameters.Add(":details", OracleDbType.Varchar2, audit.Details, ParameterDirection.Input);
                cmd.Parameters.Add(":requestedby", OracleDbType.Varchar2, audit.RequestedBy, ParameterDirection.Input);
                cmd.Parameters.Add(":requestedon", OracleDbType.Date, audit.RequestedOn, ParameterDirection.Input);
                cmd.Parameters.Add(":approvedby", OracleDbType.Varchar2, audit.ApprovedBy, ParameterDirection.Input);
                cmd.Parameters.Add(":approvedon", OracleDbType.Date, audit.ApprovedOn, ParameterDirection.Input);

                int rowsInserted = cmd.ExecuteNonQuery();
                if (rowsInserted > 0)
                {
                    result = true;
                    txn.Commit();
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

        public static List<AuditTrail> RetrieveAll()
        {
            try
            {
                var audits = new List<AuditTrail>();

                var conn = OracleDL.connect();

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM SYSTEMAUDITTRAIL";

                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    audits.Add(AuditTrail.Transform(dr));
                }

                OracleDL.close(conn);

                return audits;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static List<AuditTrail> RetrieveFilteredAudits(Dictionary<string, string> filters)
        {
            try
            {
                var audits = new List<AuditTrail>();

                var conn = OracleDL.connect();

                string query = QueryHelper.CustomApprovalSelectQuery(filters, StatusUtil.TableNames.SYSTEMAUDITTRAIL.ToString());

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = query;

                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    audits.Add(AuditTrail.Transform(dr));
                }

                OracleDL.close(conn);

                return audits;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
