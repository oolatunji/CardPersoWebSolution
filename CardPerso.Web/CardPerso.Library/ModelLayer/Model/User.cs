using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardPerso.Library.ModelLayer.Model
{
    public class User : BaseModel
    {
        public string LastName { get; set; }
        public string Othernames { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public int BranchId { get; set; }
        public string RoleName { get; set; }
        public string BranchName { get; set; }
        public Role UserRole { get; set; }
        public Branch UserBranch { get; set; }
        public DateTime CreatedOn { get; set; }
        public List<Function> Function { get; set; }
        public bool Locked { get; set; } //0 for unlocked, 1 for locked

        public static User Transform(OracleDataReader record)
        {
            var user = new User();
            user.Id = Convert.ToInt32(record["ID"]);
            user.LastName = Convert.ToString(record["LASTNAME"]);
            user.Othernames = Convert.ToString(record["OTHERNAMES"]);
            user.Gender = Convert.ToString(record["GENDER"]);
            user.Email = Convert.ToString(record["EMAILADDRESS"]);
            user.Username = Convert.ToString(record["USERNAME"]);
            user.Password = Convert.ToString(record["PASSWORD"]);
            user.Locked = Convert.ToInt32(record["LOCKEDSTATUS"]) == 0 ? false : true;
            user.UserRole = Role.Transform(record, false);
            user.UserBranch = Branch.TransformBranch(record);
            user.CreatedOn = Convert.ToDateTime(record["CREATEDON"]);

            return user;
        }
    }
}
