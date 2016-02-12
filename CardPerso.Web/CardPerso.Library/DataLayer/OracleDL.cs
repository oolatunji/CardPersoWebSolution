using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardPerso.Library.DataLayer
{
    public class OracleDL
    {
        public static OracleConnection connect()
        {
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = @System.Configuration.ConfigurationManager.AppSettings.Get("OracleConnectionString");
            conn.Open();
            return conn;
        }

        public static void close(OracleConnection conn)
        {
            conn.Close();
            conn.Dispose();
        }
    }
}
