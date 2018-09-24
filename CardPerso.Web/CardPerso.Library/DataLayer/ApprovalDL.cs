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
    public class ApprovalDL
    {
        public static bool Save(Approval approval)
        {
            bool result = false;
            var conn = OracleDL.connect();
            OracleTransaction txn = conn.BeginTransaction(IsolationLevel.ReadCommitted);
            try
            {
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "INSERT INTO SYSTEMAPPROVALS(TYPE, DETAILS, OLDDETAILS, OBJ, REQUESTEDBY, REQUESTEDON, STATUS) VALUES(:type, :details, :olddetails, :obj, :requestedby, :requestedon, :status)";
                cmd.Parameters.Add(":type", OracleDbType.Varchar2, approval.Type, ParameterDirection.Input);
                cmd.Parameters.Add(":details", OracleDbType.Varchar2, approval.Details, ParameterDirection.Input);
                cmd.Parameters.Add(":olddetails", OracleDbType.Varchar2, approval.OldDetails, ParameterDirection.Input);
                cmd.Parameters.Add(":obj", OracleDbType.Varchar2, approval.Obj, ParameterDirection.Input);
                cmd.Parameters.Add(":requestedby", OracleDbType.Varchar2, approval.RequestedBy, ParameterDirection.Input);
                cmd.Parameters.Add(":requestedon", OracleDbType.Date, approval.RequestedOn, ParameterDirection.Input);
                cmd.Parameters.Add(":status", OracleDbType.Varchar2, approval.Status, ParameterDirection.Input);

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

        public static bool Update(Approval approval)
        {
            bool result = false;
            var conn = OracleDL.connect();
            OracleTransaction txn = conn.BeginTransaction(IsolationLevel.ReadCommitted);
            try
            {
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"UPDATE SYSTEMAPPROVALS SET
                                    APPROVEDBY = :approvedby, APPROVEDON = :approvedon, STATUS = :status
                                    WHERE ID = :id";
                cmd.Parameters.Add(":approvedby", OracleDbType.Varchar2, approval.ApprovedBy, ParameterDirection.Input);
                cmd.Parameters.Add(":approvedon", OracleDbType.Date, approval.ApprovedOn, ParameterDirection.Input);
                cmd.Parameters.Add(":status", OracleDbType.Varchar2, approval.Status, ParameterDirection.Input);
                cmd.Parameters.Add(":id", OracleDbType.Int32, approval.Id, ParameterDirection.Input);

                int rowsUpdated = cmd.ExecuteNonQuery();
                if (rowsUpdated > 0)
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

        public static List<Approval> RetrieveAll()
        {
            try
            {
                var approvals = new List<Approval>();

                var conn = OracleDL.connect();

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM SYSTEMAPPROVALS ORDER BY ID DESC";

                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    approvals.Add(Approval.Transform(dr));
                }

                OracleDL.close(conn);

                return approvals;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static List<Approval> RetrieveFilteredApprovals(Dictionary<string, string> filters)
        {
            try
            {
                var approvals = new List<Approval>();

                var conn = OracleDL.connect();

                string query = QueryHelper.CustomApprovalSelectQuery(filters, StatusUtil.TableNames.SYSTEMAPPROVALS.ToString());

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = query;

                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    approvals.Add(Approval.Transform(dr));
                }

                OracleDL.close(conn);

                return approvals;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static Approval RetrieveById(int id)
        {
            try
            {
                var approvals = new List<Approval>();

                var conn = OracleDL.connect();

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM SYSTEMAPPROVALS WHERE ID =: id";
                cmd.Parameters.Add(":id", OracleDbType.Int32, id, ParameterDirection.Input);

                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    approvals.Add(Approval.Transform(dr));
                }

                OracleDL.close(conn);

                return approvals.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static List<Approval> RetrieveByUsername(string username)
        {
            try
            {
                var approvals = new List<Approval>();

                var conn = OracleDL.connect();

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = string.Format("SELECT * FROM SYSTEMAPPROVALS WHERE REQUESTEDBY = '{0}'", username);

                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    approvals.Add(Approval.Transform(dr));
                }

                OracleDL.close(conn);

                return approvals;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
