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
    public class BranchPL
    {
        public static Response Save(Branch branch, string username, bool overrideApproval)
        {
            try
            {
                if (BranchDL.BranchExists(branch))
                {
                    return new Response
                    {
                        SuccessMsg = string.Empty,
                        ErrorMsg = $"Branch with Name: {branch.Name} or Code: {branch.Code} exist already."
                    };
                }
                else
                {
                    if (!overrideApproval)
                    {
                        bool logForApproval = ApprovalConfigurationDL.RetrieveByType(StatusUtil.GetDescription(StatusUtil.ApprovalType.CreateBranch)).Approve;

                        if (logForApproval)
                        {
                            Approval approvalObj = new Approval();
                            approvalObj.Type = StatusUtil.GetDescription(StatusUtil.ApprovalType.CreateBranch);
                            approvalObj.Details = JsonConvert.SerializeObject(branch);
                            approvalObj.Obj = JsonConvert.SerializeObject(branch);
                            approvalObj.RequestedBy = username;
                            approvalObj.RequestedOn = System.DateTime.Now;
                            approvalObj.Status = StatusUtil.ApprovalStatus.Pending.ToString();

                            if (ApprovalDL.Save(approvalObj))
                            {
                                return new Response
                                {
                                    SuccessMsg = "Add branch request was successfully logged for approval",
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
                            if (BranchDL.Save(branch))
                            {
                                AuditTrail obj = new AuditTrail();
                                obj.Type = StatusUtil.GetDescription(StatusUtil.ApprovalType.CreateBranch);
                                obj.Details = JsonConvert.SerializeObject(branch);
                                obj.RequestedBy = username;
                                obj.RequestedOn = System.DateTime.Now;
                                obj.ApprovedBy = username;
                                obj.ApprovedOn = System.DateTime.Now;
                                obj.ClientIP = branch.ClientIP;
                                AuditTrailDL.Save(obj);

                                return new Response
                                {
                                    SuccessMsg = "Branch added successfully",
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
                        if (BranchDL.Save(branch))
                        {
                            return new Response
                            {
                                SuccessMsg = "Branch added successfully",
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

        public static Response Update(Branch branch, string username, bool overrideApproval)
        {
            try
            {
                if (!overrideApproval)
                {
                    bool logForApproval = ApprovalConfigurationDL.RetrieveByType(StatusUtil.GetDescription(StatusUtil.ApprovalType.UpdateBranch)).Approve;

                    if (logForApproval)
                    {
                        Approval approvalObj = new Approval();
                        approvalObj.Type = StatusUtil.GetDescription(StatusUtil.ApprovalType.UpdateBranch);
                        approvalObj.Details = JsonConvert.SerializeObject(branch);
                        approvalObj.Obj = JsonConvert.SerializeObject(branch);
                        approvalObj.RequestedBy = username;
                        approvalObj.RequestedOn = System.DateTime.Now;
                        approvalObj.Status = StatusUtil.ApprovalStatus.Pending.ToString();

                        if (ApprovalDL.Save(approvalObj))
                        {
                            return new Response
                            {
                                SuccessMsg = "Update branch request was successfully logged for approval",
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
                        if (BranchDL.Update(branch))
                        {
                            AuditTrail obj = new AuditTrail();
                            obj.Type = StatusUtil.GetDescription(StatusUtil.ApprovalType.UpdateBranch);
                            obj.Details = JsonConvert.SerializeObject(branch);
                            obj.RequestedBy = username;
                            obj.RequestedOn = System.DateTime.Now;
                            obj.ApprovedBy = username;
                            obj.ApprovedOn = System.DateTime.Now;
                            obj.ClientIP = branch.ClientIP;
                            AuditTrailDL.Save(obj);

                            return new Response
                            {
                                SuccessMsg = "Branch updated successfully",
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
                    if (BranchDL.Update(branch))
                    {
                        return new Response
                        {
                            SuccessMsg = "Branch updated successfully",
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
                var branches = BranchDL.RetrieveAll();
                return new Response
                {
                    DynamicList = new { data = branches }
                };
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                return new Response
                {
                    SuccessMsg = string.Empty,
                    ErrorMsg = ex.Message,
                    DynamicList = new { data = new List<Branch>() }
                };
            }
        }       
    }
}
