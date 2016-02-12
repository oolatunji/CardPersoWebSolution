using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFWithOracleTest
{
    public class User
    {
        public string LastName { get; set; }
        public string Othernames { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Int32 RoleId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
