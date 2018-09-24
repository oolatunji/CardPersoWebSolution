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
    public class IPPL
    {
        public static Response Save(IP ip, string username, bool overrideApproval)
        {
            try
            {
                if (IPDL.IPAddressExists(ip.IPAddress))
                {
                    return new Response
                    {
                        SuccessMsg = string.Empty,
                        ErrorMsg = string.Format("IP with ip address {0} already exists.", ip.IPAddress)
                    };
                }
                else
                {
                    if (!overrideApproval)
                    {
                        bool logForApproval = ApprovalConfigurationDL.RetrieveByType(StatusUtil.GetDescription(StatusUtil.ApprovalType.CreateIP)).Approve;

                        if (logForApproval)
                        {
                            Approval approvalObj = new Approval();
                            approvalObj.Type = StatusUtil.GetDescription(StatusUtil.ApprovalType.CreateIP);
                            approvalObj.Details = JsonConvert.SerializeObject(ip);
                            approvalObj.Obj = JsonConvert.SerializeObject(ip);
                            approvalObj.RequestedBy = username;
                            approvalObj.RequestedOn = System.DateTime.Now;
                            approvalObj.Status = StatusUtil.ApprovalStatus.Pending.ToString();

                            if (ApprovalDL.Save(approvalObj))
                            {
                                return new Response
                                {
                                    SuccessMsg = "Add IP request was successfully logged for approval",
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
                            if (IPDL.Save(ip))
                            {
                                AuditTrail obj = new AuditTrail();
                                obj.Type = StatusUtil.GetDescription(StatusUtil.ApprovalType.CreateIP);
                                obj.Details = JsonConvert.SerializeObject(ip);
                                obj.RequestedBy = username;
                                obj.RequestedOn = System.DateTime.Now;
                                obj.ApprovedBy = username;
                                obj.ApprovedOn = System.DateTime.Now;
                                obj.ClientIP = ip.ClientIP;
                                AuditTrailDL.Save(obj);

                                return new Response
                                {
                                    SuccessMsg = "IP added successfully",
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
                        if (IPDL.Save(ip))
                        {
                            return new Response
                            {
                                SuccessMsg = "IP added successfully",
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

        public static Response Update(IP ip, IP oldIP, string username, bool overrideApproval)
        {
            try
            {
                if (!overrideApproval)
                {
                    bool logForApproval = ApprovalConfigurationDL.RetrieveByType(StatusUtil.GetDescription(StatusUtil.ApprovalType.UpdateIP)).Approve;

                    if (logForApproval)
                    {
                        Approval approvalObj = new Approval();
                        approvalObj.Type = StatusUtil.GetDescription(StatusUtil.ApprovalType.UpdateIP);
                        approvalObj.Details = JsonConvert.SerializeObject(ip);
                        approvalObj.OldDetails = JsonConvert.SerializeObject(oldIP);
                        approvalObj.Obj = JsonConvert.SerializeObject(ip);
                        approvalObj.RequestedBy = username;
                        approvalObj.RequestedOn = System.DateTime.Now;
                        approvalObj.Status = StatusUtil.ApprovalStatus.Pending.ToString();

                        if (ApprovalDL.Save(approvalObj))
                        {
                            return new Response
                            {
                                SuccessMsg = "Update IP request was successfully logged for approval",
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
                        if (IPDL.Update(ip))
                        {
                            AuditTrail obj = new AuditTrail();
                            obj.Type = StatusUtil.GetDescription(StatusUtil.ApprovalType.UpdateIP);
                            obj.Details = JsonConvert.SerializeObject(ip);
                            obj.OldDetails = JsonConvert.SerializeObject(oldIP);
                            obj.RequestedBy = username;
                            obj.RequestedOn = System.DateTime.Now;
                            obj.ApprovedBy = username;
                            obj.ApprovedOn = System.DateTime.Now;
                            obj.ClientIP = ip.ClientIP;
                            AuditTrailDL.Save(obj);

                            return new Response
                            {
                                SuccessMsg = "IP updated successfully",
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
                    if (IPDL.Update(ip))
                    {
                        return new Response
                        {
                            SuccessMsg = "IP updated successfully",
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

        public static Response Delete(IP ip, string username, bool overrideApproval)
        {
            try
            {
                if (!overrideApproval)
                {
                    bool logForApproval = ApprovalConfigurationDL.RetrieveByType(StatusUtil.GetDescription(StatusUtil.ApprovalType.DeleteIP)).Approve;

                    if (logForApproval)
                    {
                        Approval approvalObj = new Approval();
                        approvalObj.Type = StatusUtil.GetDescription(StatusUtil.ApprovalType.DeleteIP);
                        approvalObj.Details = JsonConvert.SerializeObject(ip);
                        approvalObj.Obj = JsonConvert.SerializeObject(ip);
                        approvalObj.RequestedBy = username;
                        approvalObj.RequestedOn = System.DateTime.Now;
                        approvalObj.Status = StatusUtil.ApprovalStatus.Pending.ToString();

                        if (ApprovalDL.Save(approvalObj))
                        {
                            return new Response
                            {
                                SuccessMsg = "Delete IP request was successfully logged for approval",
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
                        if (IPDL.Delete(ip))
                        {
                            AuditTrail obj = new AuditTrail();
                            obj.Type = StatusUtil.GetDescription(StatusUtil.ApprovalType.DeleteIP);
                            obj.Details = JsonConvert.SerializeObject(ip);
                            obj.RequestedBy = username;
                            obj.RequestedOn = System.DateTime.Now;
                            obj.ApprovedBy = username;
                            obj.ApprovedOn = System.DateTime.Now;
                            obj.ClientIP = ip.ClientIP;
                            AuditTrailDL.Save(obj);

                            return new Response
                            {
                                SuccessMsg = "IP deleted successfully",
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
                    if (IPDL.Delete(ip))
                    {
                        return new Response
                        {
                            SuccessMsg = "IP deleted successfully",
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

        public static Response RetrieveAll()
        {
            try
            {
                var ips = IPDL.RetrieveAll();
                return new Response
                {
                    DynamicList = new { data = ips }
                };
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                return new Response
                {
                    SuccessMsg = string.Empty,
                    ErrorMsg = ex.Message,
                    DynamicList = new { data = new List<IP>() }
                };
            }
        }

        public static Response IPAddressExists(string ipAddress)
        {
            try
            {
                var ipAddressExists = IPDL.IPAddressExists(ipAddress);
                return new Response
                {
                    ErrorMsg = ipAddressExists ? string.Empty : $"IP address: {ipAddress} not allowed.",
                    DynamicList = new { data = ipAddressExists }
                };
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                return new Response
                {
                    SuccessMsg = string.Empty,
                    ErrorMsg = ex.Message,
                    DynamicList = new { data = new List<IP>() }
                };
            }
        }
    }
}
