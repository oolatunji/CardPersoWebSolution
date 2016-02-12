using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardPerso.Library.ModelLayer.Model
{
    public class SystemIdManager
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public int FunctionId { get; set; }
        public int RoleFunctionId { get; set; }
        public int ApprovalId { get; set; }

        public static SystemIdManager Transform(OracleDataReader record)
        {
            return new SystemIdManager
            {
                Id = Convert.ToInt32(record["ID"]),
                UserId = Convert.ToInt32(record["USERID"]),
                RoleId = Convert.ToInt32(record["ROLEID"]),
                FunctionId = Convert.ToInt32(record["FUNCTIONID"]),
                RoleFunctionId = Convert.ToInt32(record["ROLEFUNCTIONID"]),
                ApprovalId = Convert.ToInt32(record["APPROVALID"])
            };
        }
    }
}
