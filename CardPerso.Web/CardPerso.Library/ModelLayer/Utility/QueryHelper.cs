﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardPerso.Library.ModelLayer.Utility
{
    public class QueryHelper
    {
        static string dateFormat = "YYYY/MM/DD HH24:MI:SS";

        public static string CustomApprovalSelectQuery(Dictionary<string, string> filters, string tablename)
        {
            string query = string.Format("SELECT * FROM {0}", tablename);

            if (filters.Any())
            {
                bool whereadded = false;
                filters.Keys.ToList().ForEach(column =>
                {
                    if (!whereadded)
                    {
                        if (column.Equals("REQUESTEDFROM"))
                        {
                            query += string.Format(" WHERE REQUESTEDON >= TO_DATE('{0}','{1}')", filters[column], dateFormat);
                        }
                        else if (column.Equals("REQUESTEDTO"))
                        {
                            query += string.Format(" WHERE REQUESTEDON <= TO_DATE('{0}','{1}')", filters[column], dateFormat);
                        }
                        else
                        {
                            query += string.Format(" WHERE {0} = '{1}'", column, filters[column]);
                        }
                        whereadded = true;
                    }
                    else
                    {
                        if (column.Equals("REQUESTEDFROM"))
                        {
                            query += string.Format(" AND REQUESTEDON >= TO_DATE('{0}','{1}')", filters[column], dateFormat);
                        }
                        else if (column.Equals("REQUESTEDTO"))
                        {
                            query += string.Format(" AND REQUESTEDON <= TO_DATE('{0}','{1}')", filters[column], dateFormat);
                        }
                        else
                        {
                            query += string.Format(" AND {0} = '{1}'", column, filters[column]);
                        }
                    }
                });
            }               

            return query;
        }

        public static string CustomCardSelectQuery(Dictionary<string, string> filters)
        {
            string query = "SELECT * FROM CARDACCOUNTREQUESTS";

            if (filters.Any())
            {
                bool whereadded = false;
                filters.Keys.ToList().ForEach(column =>
                {
                    if (!whereadded)
                    {
                        if (column.Equals("REQUESTEDFROM"))
                        {
                            query += string.Format(" WHERE DATEOFRECORD >= TO_DATE('{0}','{1}')", filters[column], dateFormat);
                        }
                        else if (column.Equals("REQUESTEDTO"))
                        {
                            query += string.Format(" WHERE DATEOFRECORD <= TO_DATE('{0}','{1}')", filters[column], dateFormat);
                        }
                        else
                        {
                            query += string.Format(" WHERE {0} = '{1}'", column, filters[column]);
                        }
                        whereadded = true;
                    }
                    else
                    {
                        if (column.Equals("REQUESTEDFROM"))
                        {
                            query += string.Format(" AND DATEOFRECORD >= TO_DATE('{0}','{1}')", filters[column], dateFormat);
                        }
                        else if (column.Equals("REQUESTEDTO"))
                        {
                            query += string.Format(" AND DATEOFRECORD <= TO_DATE('{0}','{1}')", filters[column], dateFormat);
                        }
                        else
                        {
                            query += string.Format(" AND {0} = '{1}'", column, filters[column]);
                        }
                    }
                });
            }

            return query;
        }
    }
}
