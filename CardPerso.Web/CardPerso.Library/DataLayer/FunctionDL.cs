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
    public class FunctionDL
    {
        public static bool Save(Function function)
        {
            bool result = false;

            try
            {
                var conn = OracleDL.connect();

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"INSERT INTO SYSTEMFUNCTIONS(NAME, PAGELINK) VALUES(:name, :pagelink)";
                cmd.Parameters.Add(":name", OracleDbType.Varchar2, function.Name, ParameterDirection.Input);
                cmd.Parameters.Add(":pagelink", OracleDbType.Varchar2, function.PageLink, ParameterDirection.Input);

                  

                int rowsInserted = cmd.ExecuteNonQuery();
                if (rowsInserted > 0)
                {
                    result = true;
                }

                OracleDL.close(conn);

                return result;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public static bool Update(Function function)
        {
            bool result = false;

            try
            {
                var conn = OracleDL.connect();

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE SYSTEMFUNCTIONS SET NAME = :name, PAGELINK = :pagelink WHERE ID = :id";
                cmd.Parameters.Add(":name", OracleDbType.Varchar2, function.Name, ParameterDirection.Input);
                cmd.Parameters.Add(":pagelink", OracleDbType.Varchar2, function.PageLink, ParameterDirection.Input);
                cmd.Parameters.Add(":id", OracleDbType.Int32, function.Id, ParameterDirection.Input);

                int rowsUpdated = cmd.ExecuteNonQuery();
                if (rowsUpdated > 0)
                    result = true;

                OracleDL.close(conn);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Function> RetrieveAll()
        {
            try
            {
                var functions = new List<Function>();

                var conn = OracleDL.connect();

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM SYSTEMFUNCTIONS";

                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    functions.Add(Function.Transform(dr));
                }

                OracleDL.close(conn);

                return functions;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static List<Function> RetrieveByRoleId(Int32 roleId)
        {
            try
            {
                var functions = new List<Function>();

                var conn = OracleDL.connect();

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"SELECT SF.* 
                                    FROM SYSTEMROLEFUNCTIONS SRF
                                    INNER JOIN SYSTEMFUNCTIONS SF ON SRF.FUNCTIONID = SF.ID
                                    WHERE SRF.ROLEID = :id";
                cmd.Parameters.Add(":id", OracleDbType.Int32, roleId, ParameterDirection.Input);

                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    functions.Add(Function.Transform(dr));
                }

                OracleDL.close(conn);

                return functions;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static Function RetrieveById(int id)
        {
            try
            {
                var functions = new List<Function>();

                var conn = OracleDL.connect();

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * SYSTEMFUNCTIONS WHERE ID = :id";
                cmd.Parameters.Add(":id", OracleDbType.Int32, id, ParameterDirection.Input);

                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    functions.Add(Function.Transform(dr));
                }

                OracleDL.close(conn);

                return functions.Any() ? functions.FirstOrDefault() : null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool FunctionExists(string name)
        {
            try
            {
                var functions = new List<Function>();

                var conn = OracleDL.connect();

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM SYSTEMFUNCTIONS WHERE NAME = :name";
                cmd.Parameters.Add(":name", OracleDbType.Varchar2, name, ParameterDirection.Input);

                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    functions.Add(Function.Transform(dr));
                }

                OracleDL.close(conn);

                return functions.Any() ? true : false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
