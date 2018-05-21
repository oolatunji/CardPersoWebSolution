using CardPerso.Library.DataLayer;
using CardPerso.Library.ModelLayer.Model;
using CardPerso.Library.ModelLayer.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardPerso.Library.ProcessLayer
{
    public class ApprovalPL
    {
        public static Response RetrieveAll()
        {
            try
            {
                var approvals = ApprovalDL.RetrieveAll();
                return new Response
                {
                    DynamicList = new { data = approvals }
                };
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                return new Response
                {
                    SuccessMsg = string.Empty,
                    ErrorMsg = ex.Message,
                    DynamicList = new { data = new List<Approval>() }
                };
            }
        }

        public static Response RetrieveFilteredApprovals(SearhFilter filter)
        {
            try
            {
                var filters = new Dictionary<string, string>();
                if(!string.IsNullOrEmpty(filter.Type))
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

                var approvals = ApprovalDL.RetrieveFilteredApprovals(filters);
                return new Response
                {
                    DynamicList = new { data = approvals }
                };
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                return new Response
                {
                    SuccessMsg = string.Empty,
                    ErrorMsg = ex.Message,
                    DynamicList = new { data = new List<Approval>() }
                };
            }
        }

        public static Response RetrieveByUsername(string username)
        {
            try
            {
                var approvals = ApprovalDL.RetrieveByUsername(username);
                return new Response
                {
                    DynamicList = new { data = approvals }
                };
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                return new Response
                {
                    SuccessMsg = string.Empty,
                    ErrorMsg = ex.Message,
                    DynamicList = new { data = new List<Approval>() }
                };
            }
        }

        public static Response Update(Approval approval)
        {
            try
            {
                if (ApprovalDL.Update(approval))
                {
                    if (approval.Status == StatusUtil.ApprovalStatus.Approved.ToString())
                    {
                        var operation = new Response();

                        operation = UpdateObject(approval);

                        return operation;
                    }
                    else
                    {
                        return new Response
                        {
                            SuccessMsg = "Request declined successfully",
                            ErrorMsg = string.Empty
                        };
                    }
                }
                else
                {
                    return new Response
                    {
                        SuccessMsg = string.Empty,
                        ErrorMsg = "Operation failed"
                    };
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                return new Response
                {
                    SuccessMsg = string.Empty,
                    ErrorMsg = ex.Message
                };
            }
        }

        private static Response UpdateObject(Approval app)
        {
            var approval = ApprovalDL.RetrieveById(app.Id);

            var response = new Response();

            if (approval.Type.Equals(StatusUtil.GetDescription(StatusUtil.ApprovalType.CreateIP)))
            {
                var obj = JsonConvert.DeserializeObject<IP>(approval.Obj);
                response = IPPL.Save(obj, string.Empty, true);
            }
            else if (approval.Type.Equals(StatusUtil.GetDescription(StatusUtil.ApprovalType.UpdateIP)))
            {
                var obj = JsonConvert.DeserializeObject<IP>(approval.Obj);
                response = IPPL.Update(obj, string.Empty, true);
            }
            else if (approval.Type.Equals(StatusUtil.GetDescription(StatusUtil.ApprovalType.DeleteIP)))
            {
                var obj = JsonConvert.DeserializeObject<IP>(approval.Obj);
                response = IPPL.Delete(obj, string.Empty, true);
            }
            else if (approval.Type.Equals(StatusUtil.GetDescription(StatusUtil.ApprovalType.CreateRole)))
            {
                var obj = JsonConvert.DeserializeObject<Role>(approval.Obj);
                response = RolePL.Save(obj, string.Empty, true);
            }
            else if (approval.Type.Equals(StatusUtil.GetDescription(StatusUtil.ApprovalType.UpdateRole)))
            {
                var obj = JsonConvert.DeserializeObject<Role>(approval.Obj);
                response = RolePL.Update(obj, string.Empty, true);
            }
            else if (approval.Type.Equals(StatusUtil.GetDescription(StatusUtil.ApprovalType.CreateUser)))
            {
                var obj = JsonConvert.DeserializeObject<User>(approval.Obj);
                response = UserPL.Save(obj, string.Empty, true);
            }
            else if (approval.Type.Equals(StatusUtil.GetDescription(StatusUtil.ApprovalType.UpdateUser)))
            {
                var obj = JsonConvert.DeserializeObject<User>(approval.Obj);
                response = UserPL.Update(obj, string.Empty, true);
            }
            else if (approval.Type.Equals(StatusUtil.GetDescription(StatusUtil.ApprovalType.ResetCardPrintStatus)))
            {
                var obj = JsonConvert.DeserializeObject<Card>(approval.Obj);
                response = CardPL.Update(obj, string.Empty, true);
            }

            AuditTrail audit = new AuditTrail();
            audit.Type = approval.Type;
            audit.Details = approval.Details;
            audit.RequestedBy = approval.RequestedBy;
            audit.RequestedOn = approval.RequestedOn;
            audit.ApprovedBy = approval.ApprovedBy;
            audit.ApprovedOn = System.DateTime.Now;
            audit.ClientIP = approval.ClientIP;
            AuditTrailDL.Save(audit);

            return response;
        }
    }
}
