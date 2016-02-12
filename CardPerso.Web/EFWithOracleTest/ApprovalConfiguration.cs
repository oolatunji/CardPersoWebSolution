using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFWithOracleTest
{
    public class ApprovalConfiguration
    {
        public Int32 Id { get; set; }
        public string Type { get; set; }
        public Int32 Approve { get; set; }

        public static List<ApprovalConfiguration> DefaultApprovalConfigurations()
        {
            var confs = new List<ApprovalConfiguration>();

            confs.Add(new ApprovalConfiguration()
            {
                Id = 1,
                Type = "Create User",
                Approve = 0
            });

            confs.Add(new ApprovalConfiguration()
            {
                Id = 2,
                Type = "Update User",
                Approve = 0
            });

            confs.Add(new ApprovalConfiguration()
            {
                Id = 3,
                Type = "Create Role",
                Approve = 0
            });

            confs.Add(new ApprovalConfiguration()
            {
                Id = 4,
                Type = "Update Role",
                Approve = 0
            });

            confs.Add(new ApprovalConfiguration()
            {
                Id = 5,
                Type = "Create Function",
                Approve = 0
            });

            confs.Add(new ApprovalConfiguration()
            {
                Id = 6,
                Type = "Update Function",
                Approve = 0
            });

            confs.Add(new ApprovalConfiguration()
            {
                Id = 7,
                Type = "Reset Card Print Status",
                Approve = 0
            });

            return confs;
        }
    }
}
