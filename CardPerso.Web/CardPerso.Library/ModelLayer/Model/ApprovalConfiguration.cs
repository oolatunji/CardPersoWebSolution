using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardPerso.Library.ModelLayer.Model
{
    public class ApprovalConfiguration : BaseModel
    {
        public string Type { get; set; }
        public bool Approve { get; set; }
        public int ApproveId { get; set; }

        public static ApprovalConfiguration Transform(OracleDataReader record)
        {
            return new ApprovalConfiguration
            {
                Id = Convert.ToInt32(record["ID"]),
                Type = Convert.ToString(record["TYPE"]),
                Approve = Convert.ToInt32(record["APPROVE"]) == 0 ? false : true
            };
        }
    }
}
