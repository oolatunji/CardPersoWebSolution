using CardPerso.Library.DataLayer;
using CardPerso.Library.ModelLayer.Model;
using CardPerso.Library.ModelLayer.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardPerso.Library.ProcessLayer
{
    public class AuditTrailPL
    {
        public static Response RetrieveAll()
        {
            try
            {
                var audits = AuditTrailDL.RetrieveAll();
                return new Response
                {
                    DynamicList = new { data = audits }
                };
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                return new Response
                {
                    SuccessMsg = string.Empty,
                    ErrorMsg = ex.Message,
                    DynamicList = new { data = new List<AuditTrail>() }
                };
            }
        }

        public static Response RetrieveFilteredAudits(SearhFilter filter)
        {
            try
            {
                var filters = new Dictionary<string, string>();
                if (!string.IsNullOrEmpty(filter.Type))
                {
                    filters.Add("TYPE", filter.Type);
                }
                if (!string.IsNullOrEmpty(filter.RequestedBy))
                {
                    filters.Add("REQUESTEDBY", filter.RequestedBy);
                }
                if (!string.IsNullOrEmpty(filter.ApprovedBy))
                {
                    filters.Add("APPROVEDBY", filter.ApprovedBy);
                }
                if (!string.IsNullOrEmpty(filter.Status))
                {
                    filters.Add("STATUS", filter.Status);
                }
                if (filter.RequestedFrom != null)
                {
                    filters.Add("REQUESTEDFROM", DateUtil.GetDate(filter.RequestedFrom, false));
                }
                if (filter.RequestedTo != null)
                {
                    filters.Add("REQUESTEDTO", DateUtil.GetDate(filter.RequestedTo, true));
                }

                var audits = AuditTrailDL.RetrieveFilteredAudits(filters);
                return new Response
                {
                    DynamicList = new { data = audits }
                };
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                return new Response
                {
                    SuccessMsg = string.Empty,
                    ErrorMsg = ex.Message,
                    DynamicList = new { data = new List<AuditTrail>() }
                };
            }
        }
    }
}
