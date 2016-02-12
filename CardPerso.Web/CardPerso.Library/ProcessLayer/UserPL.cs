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
    public class UserPL
    {
        public static Response Save(User user, string username, bool overrideApproval)
        {
            try
            {
                if (UserDL.UserExists(user.Username))
                {
                    return new Response
                    {
                        SuccessMsg = string.Empty,
                        ErrorMsg = string.Format("User with username {0} already exists.", user.Username)
                    };
                }
                else
                {
                    if (!overrideApproval)
                    {
                        bool logForApproval = ApprovalConfigurationDL.RetrieveByType(StatusUtil.GetDescription(StatusUtil.ApprovalType.CreateUser)).Approve;

                        if (logForApproval)
                        {
                            Approval approvalObj = new Approval();
                            approvalObj.Type = StatusUtil.GetDescription(StatusUtil.ApprovalType.CreateUser);
                            approvalObj.Details = JsonConvert.SerializeObject(user);
                            approvalObj.Obj = JsonConvert.SerializeObject(user);
                            approvalObj.RequestedBy = username;
                            approvalObj.RequestedOn = System.DateTime.Now;
                            approvalObj.Status = StatusUtil.ApprovalStatus.Pending.ToString();

                            if (ApprovalDL.Save(approvalObj))
                            {
                                return new Response
                                {
                                    SuccessMsg = "User successfully logged for approval",
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
                            if (UserDL.Save(user))
                            {
                                AuditTrail obj = new AuditTrail();
                                obj.Type = StatusUtil.GetDescription(StatusUtil.ApprovalType.CreateUser);
                                obj.Details = JsonConvert.SerializeObject(user);
                                obj.RequestedBy = username;
                                obj.RequestedOn = System.DateTime.Now;
                                obj.ApprovedBy = username;
                                obj.ApprovedOn = System.DateTime.Now;
                                AuditTrailDL.Save(obj);

                                return new Response
                                {
                                    SuccessMsg = "User added successfully",
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
                        if (UserDL.Save(user))
                        {
                            return new Response
                            {
                                SuccessMsg = "User added successfully",
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

        public static Response Update(User user, string username, bool overrideApproval)
        {
            try
            {
                if (!overrideApproval)
                {
                    bool logForApproval = ApprovalConfigurationDL.RetrieveByType(StatusUtil.GetDescription(StatusUtil.ApprovalType.UpdateUser)).Approve;

                    if (logForApproval)
                    {
                        Approval approvalObj = new Approval();
                        approvalObj.Type = StatusUtil.GetDescription(StatusUtil.ApprovalType.UpdateUser);
                        approvalObj.Details = JsonConvert.SerializeObject(user);
                        approvalObj.Obj = JsonConvert.SerializeObject(user);
                        approvalObj.RequestedBy = username;
                        approvalObj.RequestedOn = System.DateTime.Now;
                        approvalObj.Status = StatusUtil.ApprovalStatus.Pending.ToString();

                        if (ApprovalDL.Save(approvalObj))
                        {
                            return new Response
                            {
                                SuccessMsg = "User successfully logged for approval",
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
                        if (UserDL.Update(user))
                        {
                            AuditTrail obj = new AuditTrail();
                            obj.Type = StatusUtil.GetDescription(StatusUtil.ApprovalType.UpdateUser);
                            obj.Details = JsonConvert.SerializeObject(user);
                            obj.RequestedBy = username;
                            obj.RequestedOn = System.DateTime.Now;
                            obj.ApprovedBy = username;
                            obj.ApprovedOn = System.DateTime.Now;
                            AuditTrailDL.Save(obj);

                            return new Response
                            {
                                SuccessMsg = "User updated successfully",
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
                    if (UserDL.Update(user))
                    {
                        return new Response
                        {
                            SuccessMsg = "User updated successfully",
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

        public static Response UpdatePassword(User user)
        {
            try
            {
                if (UserDL.UpdatePassword(user))
                {
                    return new Response
                    {
                        SuccessMsg = "User password updated successfully",
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
                var users = UserDL.RetrieveAll();
                return new Response
                {
                    DynamicList = new { data = users }
                };
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                return new Response
                {
                    SuccessMsg = string.Empty,
                    ErrorMsg = ex.Message,
                    DynamicList = new { data = new List<User>() }
                };
            }
        }

        public static Response AuthenticateUser(User user)
        {
            try
            {
                var authenticatedUser = UserDL.AuthenticateUser(user);
                if (authenticatedUser != null)
                {
                    authenticatedUser.Function = FunctionDL.RetrieveByRoleId(authenticatedUser.UserRole.Id);
                    return new Response
                    {
                        ErrorMsg = string.Empty,
                        DynamicList = new { data = authenticatedUser }
                    };
                }
                else
                {
                    return new Response
                    {
                        ErrorMsg = "Invalid Username/Password",
                        DynamicList = new { data = new User() }
                    };
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                return new Response
                {
                    SuccessMsg = string.Empty,
                    ErrorMsg = ex.Message,
                    DynamicList = new { data = new User() }
                };
            }
        }

        public static Response RetrieveUserByUsername(User user, bool sendMail)
        {
            try
            {
                var authenticatedUser = UserDL.RetrieveUserByUsername(user);
                if (authenticatedUser != null)
                {
                    if (sendMail)
                    {
                        Mail.SendForgotPasswordMail(authenticatedUser);
                    }
                    return new Response
                    {
                        ErrorMsg = string.Empty,
                        DynamicList = new { data = authenticatedUser }
                    };
                }
                else
                {
                    return new Response
                    {
                        ErrorMsg = "Invalid Username",
                        DynamicList = new { data = new User() }
                    };
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                return new Response
                {
                    SuccessMsg = string.Empty,
                    ErrorMsg = ex.Message,
                    DynamicList = new { data = new User() }
                };
            }
        }
    }
}
