using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardPerso.Library.ModelLayer.Model
{
    public class Function : BaseModel
    {
        public string PageLink { get; set; }

        public static Function Transform(OracleDataReader record)
        {
            return new Function
            {
                Id = Convert.ToInt32(record["ID"]),
                Name = Convert.ToString(record["NAME"]),
                PageLink = Convert.ToString(record["PAGELINK"])
            };
        }
    }
}
