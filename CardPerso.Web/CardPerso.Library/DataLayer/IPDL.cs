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
    public class IPDL
    {
        public static bool Save(IP ip)
        {
            bool result = false;

            try
            {
                var conn = OracleDL.connect();

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"INSERT INTO IPS(NAME, IPADDRESS, DESCRIPTION) VALUES(:name, :ipaddress, :description)";
                cmd.Parameters.Add(":name", OracleDbType.Varchar2, ip.Name, ParameterDirection.Input);
                cmd.Parameters.Add(":ipaddress", OracleDbType.Varchar2, ip.IPAddress, ParameterDirection.Input);
                cmd.Parameters.Add(":description", OracleDbType.Varchar2, ip.Description, ParameterDirection.Input);

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

        public static bool Update(IP ip)
        {
            bool result = false;

            try
            {
                var conn = OracleDL.connect();

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE IPS SET NAME = :name, IPADDRESS = :ipaddress, DESCRIPTION = :description WHERE ID = :id";
                cmd.Parameters.Add(":name", OracleDbType.Varchar2, ip.Name, ParameterDirection.Input);
                cmd.Parameters.Add(":ipaddress", OracleDbType.Varchar2, ip.IPAddress, ParameterDirection.Input);
                cmd.Parameters.Add(":description", OracleDbType.Varchar2, ip.Description, ParameterDirection.Input);
                cmd.Parameters.Add(":id", OracleDbType.Int32, ip.Id, ParameterDirection.Input);

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

        public static bool Delete(IP ip)
        {
            bool result = false;

            try
            {
                var conn = OracleDL.connect();

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "DELETE FROM IPS WHERE ID = :id";                
                cmd.Parameters.Add(":id", OracleDbType.Int32, ip.Id, ParameterDirection.Input);

                int rowsDeleted = cmd.ExecuteNonQuery();
                if (rowsDeleted > 0)
                    result = true;

                OracleDL.close(conn);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<IP> RetrieveAll()
        {
            try
            {
                var ips = new List<IP>();

                var conn = OracleDL.connect();

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM IPS";

                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ips.Add(IP.Transform(dr));
                }

                OracleDL.close(conn);

                return ips;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }            

        public static bool IPAddressExists(string ipAddress)
        {
            try
            {
                var ips = new List<IP>();

                var conn = OracleDL.connect();

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM IPS WHERE IPADDRESS = :ipaddress";
                cmd.Parameters.Add(":ipaddress", OracleDbType.Varchar2, ipAddress, ParameterDirection.Input);

                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ips.Add(IP.Transform(dr));
                }

                OracleDL.close(conn);

                return ips.Any() ? true : false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
