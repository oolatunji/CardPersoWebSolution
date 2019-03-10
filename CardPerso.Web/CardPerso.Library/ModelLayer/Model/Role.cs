using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardPerso.Library.ModelLayer.Model
{
    public class Role : BaseModel
    {
        public Function Function { get; set; }
        public Function[] Functions { get; set; }
        public string SuperAdminRole { get; set; }

        public static Role Transform(OracleDataReader record, bool getFunction)
        {
            return new Role
            {
                Id = Convert.ToInt32(record["ROLEID"]),
                Name = Convert.ToString(record["ROLENAME"]),
                Function = getFunction ? Function.Transform(record) : null,
                SuperAdminRole = record["SUPERADMINROLE"] != DBNull.Value ? Convert.ToString(record["SUPERADMINROLE"]) : "No"
            };
        }
    }
}
