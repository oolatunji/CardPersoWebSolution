using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardPerso.Library.ModelLayer.Model
{
    public class IP : BaseModel
    {
        public string IPAddress { get; set; }
        public string Description { get; set; }

        public static IP Transform(OracleDataReader record)
        {
            return new IP
            {
                Id = Convert.ToInt32(record["ID"]),
                Name = Convert.ToString(record["NAME"]),
                IPAddress = Convert.ToString(record["IPADDRESS"]),
                Description = Convert.ToString(record["DESCRIPTION"])
            };
        }
    }
}
