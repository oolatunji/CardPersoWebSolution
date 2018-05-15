using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardPerso.Library.ModelLayer.Model
{
    public class AuditTrail: BaseModel
    {
        public string Type { get; set; }
        public string Details { get; set; }
        public string RequestedBy { get; set; }
        public DateTime RequestedOn { get; set; }
        public string RequestedDate { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime ApprovedOn { get; set; }
        public string ApprovedDate { get; set; }

        public static AuditTrail Transform(OracleDataReader record)
        {
            return new AuditTrail
            {
                Id = Convert.ToInt32(record["ID"]),
                Type = Convert.ToString(record["TYPE"]),
                ClientIP = Convert.ToString(record["CLIENTIP"]),
                Details = Convert.ToString(record["DETAILS"]),
                RequestedBy = Convert.ToString(record["REQUESTEDBY"]),
                RequestedDate = String.Format("{0:d/M/yyyy HH:mm:ss}", Convert.ToDateTime(record["REQUESTEDON"])),
                ApprovedBy = record["APPROVEDBY"] != null ? Convert.ToString(record["APPROVEDBY"]) : string.Empty,
                ApprovedDate = !record.IsDBNull(6) ? String.Format("{0:d/M/yyyy HH:mm:ss}", Convert.ToDateTime(record["APPROVEDON"])) : string.Empty
            };
        }
    }
}
