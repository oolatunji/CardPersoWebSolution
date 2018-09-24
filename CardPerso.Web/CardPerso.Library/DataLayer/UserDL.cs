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
    public class UserDL
    {
        public static bool Save(User user)
        {
            bool result = false;
            var conn = OracleDL.connect();
            OracleTransaction txn = conn.BeginTransaction(IsolationLevel.ReadCommitted);
            try
            {                
                var password = PasswordHash.SHA256Hash(user.Password);

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "INSERT INTO SYSTEMUSERS(LASTNAME, OTHERNAMES, GENDER, EMAILADDRESS, USERNAME, PASSWORD, ROLEID, CREATEDON, BranchId) VALUES(:lastname, :othernames, :gender, :email, :username, :password, :roleid, :createdon, :branchid) RETURNING ID INTO :id";
                cmd.Parameters.Add(":lastname", OracleDbType.Varchar2, user.LastName, ParameterDirection.Input);
                cmd.Parameters.Add(":othernames", OracleDbType.Varchar2, user.Othernames, ParameterDirection.Input);
                cmd.Parameters.Add(":gender", OracleDbType.Varchar2, user.Gender, ParameterDirection.Input);
                cmd.Parameters.Add(":email", OracleDbType.Varchar2, user.Email, ParameterDirection.Input);
                cmd.Parameters.Add(":username", OracleDbType.Varchar2, user.Username, ParameterDirection.Input);
                cmd.Parameters.Add(":password", OracleDbType.Varchar2, password, ParameterDirection.Input);
                cmd.Parameters.Add(":roleid", OracleDbType.Int32, user.RoleId, ParameterDirection.Input);                
                cmd.Parameters.Add(":createdon", OracleDbType.Date, user.CreatedOn, ParameterDirection.Input);
                cmd.Parameters.Add(":branchid", OracleDbType.Int32, user.BranchId, ParameterDirection.Input);

                OracleParameter outputParameter = new OracleParameter("id", OracleDbType.Int32);
                outputParameter.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputParameter);

                int rowsInserted = cmd.ExecuteNonQuery();
                if (rowsInserted > 0)
                {                    
                    Mail.SendNewUserMail(user);
                    result = true;
                    txn.Commit();
                    //var userId = Convert.ToInt32(outputParameter.Value.ToString());

                    //OracleCommand command = conn.CreateCommand();
                    //command.CommandText = "INSERT INTO USERDETAILS(ID1, VUSERNAME, VPASSWORD, VOFFICIALEMAIL) VALUES(:id, :username, :password, :email)";
                    //command.Parameters.Add(":id", OracleDbType.Int32, userId, ParameterDirection.Input);
                    //command.Parameters.Add(":username", OracleDbType.Varchar2, user.Username, ParameterDirection.Input);
                    //command.Parameters.Add(":password", OracleDbType.Varchar2, user.Password, ParameterDirection.Input);
                    //command.Parameters.Add(":email", OracleDbType.Varchar2, user.Email, ParameterDirection.Input);

                    //rowsInserted = command.ExecuteNonQuery();
                    //if (rowsInserted > 0)
                    //{

                    //}
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

        public static bool Update(User user)
        {
            bool result = false;
            var conn = OracleDL.connect();
            OracleTransaction txn = conn.BeginTransaction(IsolationLevel.ReadCommitted);
            try
            {
                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"UPDATE SYSTEMUSERS SET
                                    LASTNAME = :lastname, OTHERNAMES = :othernames, GENDER = :gender,
                                    EMAILADDRESS = :email, ROLEID = :roleid, BRANCHID = :branchid
                                    WHERE ID = :id";
                cmd.Parameters.Add(":lastname", OracleDbType.Varchar2, user.LastName, ParameterDirection.Input);
                cmd.Parameters.Add(":othernames", OracleDbType.Varchar2, user.Othernames, ParameterDirection.Input);
                cmd.Parameters.Add(":gender", OracleDbType.Varchar2, user.Gender, ParameterDirection.Input);
                cmd.Parameters.Add(":email", OracleDbType.Varchar2, user.Email, ParameterDirection.Input);
                cmd.Parameters.Add(":roleid", OracleDbType.Int32, user.RoleId, ParameterDirection.Input);
                cmd.Parameters.Add(":branchid", OracleDbType.Int32, user.BranchId, ParameterDirection.Input);
                cmd.Parameters.Add(":id", OracleDbType.Int32, user.Id, ParameterDirection.Input);

                int rowsUpdated = cmd.ExecuteNonQuery();
                if (rowsUpdated > 0)
                {
                    result = true;
                    txn.Commit();
                    //cmd = conn.CreateCommand();
                    //cmd.CommandText = @"UPDATE USERDETAILS SET 
                    //                    VOFFICIALEMAIL = :email
                    //                    WHERE ID1 =:id";
                    //cmd.Parameters.Add(":email", OracleDbType.Varchar2, user.Email, ParameterDirection.Input);
                    //cmd.Parameters.Add(":id", OracleDbType.Int32, user.Id, ParameterDirection.Input);

                    //rowsUpdated = cmd.ExecuteNonQuery();
                    //if (rowsUpdated > 0)
                    //{

                    //}
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

        public static bool UpdatePassword(User user)
        {
            bool result = false;
            var conn = OracleDL.connect();
            OracleTransaction txn = conn.BeginTransaction(IsolationLevel.ReadCommitted);
            try
            {
                var password = PasswordHash.SHA256Hash(user.Password);

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"UPDATE SYSTEMUSERS SET
                                    PASSWORD = :password
                                    WHERE USERNAME = :username";
                cmd.Parameters.Add(":password", OracleDbType.Varchar2, password, ParameterDirection.Input);
                cmd.Parameters.Add(":username", OracleDbType.Varchar2, user.Username, ParameterDirection.Input);

                int rowsUpdated = cmd.ExecuteNonQuery();
                if (rowsUpdated > 0)
                {
                    result = true;
                    txn.Commit();
                    //cmd = conn.CreateCommand();
                    //cmd.CommandText = @"UPDATE USERDETAILS SET 
                    //                    VPASSWORD = :password 
                    //                    WHERE VUSERNAME =:username";
                    //cmd.Parameters.Add(":password", OracleDbType.Varchar2, user.Password, ParameterDirection.Input);
                    //cmd.Parameters.Add(":username", OracleDbType.Varchar2, user.Username, ParameterDirection.Input);

                    //rowsUpdated = cmd.ExecuteNonQuery();
                    //if (rowsUpdated > 0)
                    //{

                    //}
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

        public static bool UserExists(string username)
        {
            try
            {
                var users = new List<User>();

                var conn = OracleDL.connect();

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"SELECT SU.*, SR.NAME AS ROLENAME, SB.NAME AS BRANCHNAME  
                                    FROM SYSTEMUSERS SU
                                    INNER JOIN SYSTEMROLES SR ON SU.ROLEID = SR.ID
                                    INNER JOIN SYSTEMBRANCHES SB ON SU.BRANCHID = SB.ID 
                                    WHERE SU.USERNAME = :username";

                cmd.Parameters.Add(":username", OracleDbType.Varchar2, username, ParameterDirection.Input);

                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    users.Add(User.Transform(dr));
                }

                OracleDL.close(conn);

                return users.Any() ? true : false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<User> RetrieveAll()
        {
            try
            {
                var users = new List<User>();

                var conn = OracleDL.connect();

                string query = @"SELECT SU.*, SR.NAME AS ROLENAME, SB.NAME AS BRANCHNAME  
                                 FROM SYSTEMUSERS SU
                                 INNER JOIN SYSTEMROLES SR ON SU.ROLEID = SR.ID
                                 INNER JOIN SYSTEMBRANCHES SB ON SU.BRANCHID = SB.ID";

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = query;

                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    users.Add(User.Transform(dr));
                }

                OracleDL.close(conn);

                return users;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static User AuthenticateUser(User user)
        {
            try
            {
                var users = new List<User>();

                var conn = OracleDL.connect();

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"SELECT SU.*, SR.NAME AS ROLENAME, SB.NAME AS BRANCHNAME  
                                    FROM SYSTEMUSERS SU
                                    INNER JOIN SYSTEMROLES SR ON SU.ROLEID = SR.ID
                                    INNER JOIN SYSTEMBRANCHES SB ON SU.BRANCHID = SB.ID 
                                    WHERE SU.USERNAME = :username AND SU.PASSWORD = :password";

                cmd.Parameters.Add(":username", OracleDbType.Varchar2, user.Username, ParameterDirection.Input);
                cmd.Parameters.Add(":password", OracleDbType.Varchar2, user.Password, ParameterDirection.Input);

                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    users.Add(User.Transform(dr));
                }

                OracleDL.close(conn);

                return users.Any() ? users.FirstOrDefault() : null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static User RetrieveUserByUsername(User user)
        {
            try
            {
                var users = new List<User>();

                var conn = OracleDL.connect();

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"SELECT SU.*, SR.NAME AS ROLENAME, SB.NAME AS BRANCHNAME  
                                    FROM SYSTEMUSERS SU
                                    INNER JOIN SYSTEMROLES SR ON SU.ROLEID = SR.ID
                                    INNER JOIN SYSTEMBRANCHES SB ON SU.BRANCHID = SB.ID 
                                    WHERE SU.USERNAME = :username";

                cmd.Parameters.Add(":username", OracleDbType.Varchar2, user.Username, ParameterDirection.Input);

                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    users.Add(User.Transform(dr));
                }

                OracleDL.close(conn);

                return users.Any() ? users.FirstOrDefault() : null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
