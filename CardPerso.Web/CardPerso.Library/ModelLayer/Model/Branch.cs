using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardPerso.Library.ModelLayer.Model
{
    public class Branch : BaseModel
    {
        public string Code { get; set; }
        public string Address { get; set; }

        public static Branch Transform(OracleDataReader record)
        {
            return new Branch
            {
                Id = Convert.ToInt32(record["ID"]),
                Name = Convert.ToString(record["NAME"]),
                Code = Convert.ToString(record["CODE"]),
                Address = Convert.ToString(record["ADDRESS"])                           
            };
        }

        public static Branch TransformBranch(OracleDataReader record)
        {
            return new Branch
            {
                Id = Convert.ToInt32(record["BRANCHID"]),
                Name = Convert.ToString(record["BRANCHNAME"]),
                Code = Convert.ToString(record["BRANCHCODE"])
            };
        }
    }
}
