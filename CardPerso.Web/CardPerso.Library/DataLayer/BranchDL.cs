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
    public class BranchDL
    {
        public static bool Save(Branch branch)
        {
            bool result = false;

            try
            {
                var conn = OracleDL.connect();

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"INSERT INTO SYSTEMBRANCHES(NAME, CODE, ADDRESS) VALUES(:name, :code, :address)";
                cmd.Parameters.Add(":name", OracleDbType.Varchar2, branch.Name, ParameterDirection.Input);
                cmd.Parameters.Add(":code", OracleDbType.Varchar2, branch.Code, ParameterDirection.Input);
                cmd.Parameters.Add(":address", OracleDbType.Varchar2, branch.Address, ParameterDirection.Input);

                int rowsInserted = cmd.ExecuteNonQuery();
                if (rowsInserted > 0)
                {
                    result = true;
                }

                OracleDL.close(conn);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Update(Branch branch)
        {
            bool result = false;

            try
            {
                var conn = OracleDL.connect();

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE SYSTEMBRANCHES SET NAME = :name, Code = :code, ADDRESS = :address WHERE ID = :id";
                cmd.Parameters.Add(":name", OracleDbType.Varchar2, branch.Name, ParameterDirection.Input);
                cmd.Parameters.Add(":code", OracleDbType.Varchar2, branch.Code, ParameterDirection.Input);
                cmd.Parameters.Add(":address", OracleDbType.Varchar2, branch.Address, ParameterDirection.Input);
                cmd.Parameters.Add(":id", OracleDbType.Int32, branch.Id, ParameterDirection.Input);

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

        public static List<Branch> RetrieveAll()
        {
            try
            {
                var branches = new List<Branch>();

                var conn = OracleDL.connect();

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM SYSTEMBRANCHES";

                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    branches.Add(Branch.Transform(dr));
                }

                OracleDL.close(conn);

                return branches;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static bool BranchExists(Branch branch)
        {
            try
            {
                var branches = new List<Branch>();

                var conn = OracleDL.connect();

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM SYSTEMBRANCHES WHERE NAME = :name OR CODE = :code";
                cmd.Parameters.Add(":name", OracleDbType.Varchar2, branch.Name, ParameterDirection.Input);
                cmd.Parameters.Add(":code", OracleDbType.Varchar2, branch.Code, ParameterDirection.Input);

                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    branches.Add(Branch.Transform(dr));
                }

                OracleDL.close(conn);

                return branches.Any() ? true : false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
