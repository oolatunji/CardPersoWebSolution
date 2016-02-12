using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardPerso.Library.ModelLayer.Model
{
    public class Card : BaseModel
    {
        public string ID1 { get; set; }
        public string Pan { get; set; }
        public Int32 PrintStatus { get; set; }
        public string Status { get; set; }
        public string CardStatus { get; set; }
        public string Username { get; set; }
        public DateTime Date { get; set; }
        public string PrintedName { get; set; }
        public string Track1Data { get; set; }
        public string Track2Data { get; set; }
        public string DisplayDate { get; set; }

        public static Card Transform(OracleDataReader record)
        {
            return new Card
            {
                ID1 = Convert.ToString(record["ID1"]),
                Name = Convert.ToString(record["NAMEONCARD"]),
                Pan = Convert.ToString(record["PAN"]),
                PrintStatus = Convert.ToInt32(record["PRINTSTATUS"]),
                CardStatus = Convert.ToInt32(record["PRINTSTATUS"]) == 1? "Not Printed" : "Printed",
                Username = Convert.ToString(record["VUSERNAME"]),
                Date = Convert.ToDateTime(record["DATEOFRECORD"]),
                PrintedName = Convert.ToString(record["PRINTEDNAME"]),
                Track1Data = Convert.ToString(record["TRACK1DATA"]),
                Track2Data = Convert.ToString(record["TRACK2DATA"]),
                DisplayDate = String.Format("{0:d/M/yyyy HH:mm:ss}", Convert.ToDateTime(record["DATEOFRECORD"]))
            };
        }
    }
}
