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
    public class CardPL
    {
        public static Response RetrieveAll(string username)
        {
            try
            {
                var cards = CardDL.RetrieveAll(username);
                return new Response
                {
                    DynamicList = new { data = cards }
                };
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                return new Response
                {
                    SuccessMsg = string.Empty,
                    ErrorMsg = ex.Message,
                    DynamicList = new { data = new List<Card>() }
                };
            }
        }

        public static Response RetrieveFilteredCards(SearhFilter filter)
        {
            try
            {
                var filters = new Dictionary<string, string>();
                if (!string.IsNullOrEmpty(filter.RequestedBy))
                {
                    filters.Add("VUSERNAME", filter.RequestedBy);
                }
                if (!string.IsNullOrEmpty(filter.Status))
                {
                    filters.Add("PRINTSTATUS", filter.Status);
                }
                if (filter.RequestedFrom.HasValue)
                {
                    filters.Add("REQUESTEDFROM", filter.RequestedFrom.Value.ToString());
                }
                if (filter.RequestedTo.HasValue)
                {
                    filters.Add("REQUESTEDTO", filter.RequestedTo.Value.AddHours(23).ToString());
                }

                var cards = CardDL.RetrieveFilteredCards(filters);
                return new Response
                {
                    DynamicList = new { data = cards }
                };
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                return new Response
                {
                    SuccessMsg = string.Empty,
                    ErrorMsg = ex.Message,
                    DynamicList = new { data = new List<Card>() }
                };
            }
        }

        public static Response Update(Card card, string username, bool overrideApproval)
        {
            try
            {
                if (!overrideApproval)
                {
                    bool logForApproval = ApprovalConfigurationDL.RetrieveByType(StatusUtil.GetDescription(StatusUtil.ApprovalType.ResetCardPrintStatus)).Approve;

                    if (logForApproval)
                    {
                        Approval approvalObj = new Approval();
                        approvalObj.Type = StatusUtil.GetDescription(StatusUtil.ApprovalType.ResetCardPrintStatus);
                        approvalObj.Details = JsonConvert.SerializeObject(card);
                        approvalObj.Obj = JsonConvert.SerializeObject(card);
                        approvalObj.RequestedBy = username;
                        approvalObj.RequestedOn = System.DateTime.Now;
                        approvalObj.Status = StatusUtil.ApprovalStatus.Pending.ToString();

                        if (ApprovalDL.Save(approvalObj))
                        {
                            return new Response
                            {
                                SuccessMsg = "Card record successfully logged for approval",
                                ErrorMsg = string.Empty
                            };
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
                    else
                    {
                        if (CardDL.Update(card))
                        {
                            AuditTrail obj = new AuditTrail();
                            obj.Type = StatusUtil.GetDescription(StatusUtil.ApprovalType.ResetCardPrintStatus);
                            obj.Details = JsonConvert.SerializeObject(card);
                            obj.RequestedBy = username;
                            obj.RequestedOn = System.DateTime.Now;
                            obj.ApprovedBy = username;
                            obj.ApprovedOn = System.DateTime.Now;
                            AuditTrailDL.Save(obj);

                            return new Response
                            {
                                SuccessMsg = "Card record updated successfully",
                                ErrorMsg = string.Empty
                            };
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
                }
                else
                {
                    if (CardDL.Update(card))
                    {
                        return new Response
                        {
                            SuccessMsg = "Card record updated successfully",
                            ErrorMsg = string.Empty
                        };
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
    }
}
