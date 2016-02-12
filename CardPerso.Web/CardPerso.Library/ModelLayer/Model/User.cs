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
        public Int32 RoleId { get; set; }
        public string RoleName { get; set; }
        public Role UserRole { get; set; }
        public DateTime CreatedOn { get; set; }
        public List<Function> Function { get; set; }

        public static User Transform(OracleDataReader record)
        {
            return new User
            {
                Id = Convert.ToInt32(record["ID"]),
                LastName = Convert.ToString(record["LASTNAME"]),
                Othernames = Convert.ToString(record["OTHERNAMES"]),
                Gender = Convert.ToString(record["GENDER"]),
                Email = Convert.ToString(record["EMAILADDRESS"]),
                Username = Convert.ToString(record["USERNAME"]),
                Password = Convert.ToString(record["PASSWORD"]),
                UserRole = Role.Transform(record, false),
                CreatedOn = Convert.ToDateTime(record["CREATEDON"]),
            };
        }
    }
}
