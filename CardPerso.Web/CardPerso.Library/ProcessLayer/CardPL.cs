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
                if (!string.IsNullOrEmpty(filter.UserBranch))
                {
                    filters.Add("BRANCHID", filter.UserBranch);
                }
                if (!string.IsNullOrEmpty(filter.Status))
                {
                    filters.Add("PRINTSTATUS", filter.Status);
                }
                if (filter.RequestedFrom != null)
                {
                    filters.Add("REQUESTEDFROM", DateUtil.GetDate(filter.RequestedFrom, false));
                }
                if (filter.RequestedTo != null)
                {
                    filters.Add("REQUESTEDTO", DateUtil.GetDate(filter.RequestedTo, true));
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

        public static Response Update(Card card, Card oldCardData, string username, bool overrideApproval)
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
                        approvalObj.OldDetails = JsonConvert.SerializeObject(oldCardData);
                        approvalObj.Obj = JsonConvert.SerializeObject(card);
                        approvalObj.RequestedBy = username;
                        approvalObj.RequestedOn = System.DateTime.Now;
                        approvalObj.Status = StatusUtil.ApprovalStatus.Pending.ToString();

                        if (ApprovalDL.Save(approvalObj))
                        {
                            return new Response
                            {
                                SuccessMsg = "Update card print status request was successfully logged for approval",
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
                            obj.ClientIP = card.ClientIP;
                            AuditTrailDL.Save(obj);

                            return new Response
                            {
                                SuccessMsg = "Card print status was updated successfully",
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
                            SuccessMsg = "Card print status was updated successfully",
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
