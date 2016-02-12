using CardPerso.Library.ModelLayer.Model;
using CardPerso.Library.ModelLayer.Utility;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardPerso.Library.DataLayer
{
    public class CardDL
    {
        public static List<Card> RetrieveAll(string username)
        {
            try
            {
                var cards = new List<Card>();

                var conn = OracleDL.connect();

                string query = @"SELECT *  
                                 FROM CARDACCOUNTREQUESTS WHERE VUSERNAME =: username";

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = query;
                cmd.Parameters.Add(":username", OracleDbType.Varchar2, username, ParameterDirection.Input);

                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    cards.Add(Card.Transform(dr));
                }

                OracleDL.close(conn);

                return cards;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static List<Card> RetrieveFilteredCards(Dictionary<string, string> filters)
        {
            try
            {
                var cards = new List<Card>();

                var conn = OracleDL.connect();

                string query = QueryHelper.CustomCardSelectQuery(filters);

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = query;

                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    cards.Add(Card.Transform(dr));
                }

                OracleDL.close(conn);

                return cards;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static bool Update(Card card)
        {
            bool result = false;

            try
            {
                var conn = OracleDL.connect();

                OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE CARDACCOUNTREQUESTS SET PRINTSTATUS = :printstatus WHERE ID1 = :id";
                cmd.Parameters.Add(":printstatus", OracleDbType.Varchar2, card.PrintStatus, ParameterDirection.Input);
                cmd.Parameters.Add(":id", OracleDbType.Varchar2, card.ID1, ParameterDirection.Input);

                int rowsUpdated = cmd.ExecuteNonQuery();
                if (rowsUpdated > 0)
                    result = true;

                OracleDL.close(conn);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
