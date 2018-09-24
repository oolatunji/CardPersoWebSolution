using CardPerso.Library.DataLayer;
using CardPerso.Library.ModelLayer.Model;
using CardPerso.Library.ModelLayer.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace CardPerso.Library.ProcessLayer
{
    public class UserPL
    {
        public static bool UserExistsAD(string username, string password)
        {
            try
            {
                var auth = false;

                var configuration = WebConfigurationManager.OpenWebConfiguration("~");
                var activeDirectoryHelperSection = (ActiveDirectoryHelper)configuration.GetSection("activeDirectorySection");

                string adServer = activeDirectoryHelperSection.ActiveDirectory.ADServer;
                string adContainer = activeDirectoryHelperSection.ActiveDirectory.ADContainer;
                string adUsername = activeDirectoryHelperSection.ActiveDirectory.ADUsername;
                string adPassword = activeDirectoryHelperSection.ActiveDirectory.ADPassword;
                string adServer2 = activeDirectoryHelperSection.ActiveDirectory.ADServer2;
                string adContainer2 = activeDirectoryHelperSection.ActiveDirectory.ADContainer2;
                string adUsername2 = activeDirectoryHelperSection.ActiveDirectory.ADUsername2;
                string adPassword2 = activeDirectoryHelperSection.ActiveDirectory.ADPassword2;

                try
                {
                    PrincipalContext insPrincipalContext = new PrincipalContext(ContextType.Domain, adServer, adContainer, adUsername, adPassword);

                    UserPrincipal insUserPrincipal = new UserPrincipal(insPrincipalContext);
                    auth = insPrincipalContext.ValidateCredentials(username, password);
                }
                catch
                {
                    try
                    {
                        PrincipalContext insPrincipalContext = new PrincipalContext(ContextType.Domain, adServer2, adContainer2, adUsername2, adPassword2);

                        UserPrincipal insUserPrincipal = new UserPrincipal(insPrincipalContext);
                        auth = insPrincipalContext.ValidateCredentials(username, password);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                return auth;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

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
                                    SuccessMsg = "Add User request was successfully logged for approval",
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
                                obj.ClientIP = user.ClientIP;
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

        public static Response Update(User user, User oldUserData, string username, bool overrideApproval)
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
                        approvalObj.OldDetails = JsonConvert.SerializeObject(oldUserData);
                        approvalObj.Obj = JsonConvert.SerializeObject(user);
                        approvalObj.RequestedBy = username;
                        approvalObj.RequestedOn = System.DateTime.Now;
                        approvalObj.Status = StatusUtil.ApprovalStatus.Pending.ToString();

                        if (ApprovalDL.Save(approvalObj))
                        {
                            return new Response
                            {
                                SuccessMsg = "Update User request was successfully logged for approval",
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
                            obj.OldDetails = JsonConvert.SerializeObject(oldUserData);
                            obj.RequestedBy = username;
                            obj.RequestedOn = System.DateTime.Now;
                            obj.ApprovedBy = username;
                            obj.ApprovedOn = System.DateTime.Now;
                            obj.ClientIP = user.ClientIP;
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

        public static Response AuthenticateUser(User user, bool auditAction)
        {
            try
            {
                var authenticatedUser = new User();
                var response = new Response();

                var configuration = WebConfigurationManager.OpenWebConfiguration("~");
                var activeDirectoryHelperSection = (ActiveDirectoryHelper)configuration.GetSection("activeDirectorySection");
                bool useActiveDirectory = Convert.ToBoolean(activeDirectoryHelperSection.ActiveDirectory.UsesActiveDirectory);

                if (useActiveDirectory)
                {
                    var adAuthenticatorService = new ADAuthenticatorService.ADAuthenticator();
                    var authResponse = adAuthenticatorService.getUserStatus(user.Username, user.Password);
                    var responseCode = authResponse.Split('|')[0];
                    if (responseCode == "00")
                    {
                        authenticatedUser = UserDL.RetrieveUserByUsername(user);
                    }
                    else
                    {
                        authenticatedUser = null;
                    }
                    //var adAuthenticationSuccessful = UserExistsAD(user.Username, user.Password);
                    //if(adAuthenticationSuccessful)
                    //{
                    //    authenticatedUser = UserDL.RetrieveUserByUsername(user);                        
                    //}                    
                }
                else
                {
                    user.Password = PasswordHash.SHA256Hash(user.Password);
                    authenticatedUser = UserDL.AuthenticateUser(user);
                }

                if (authenticatedUser != null)
                {
                    if (auditAction)
                    {
                        AuditTrail obj = new AuditTrail();
                        obj.Type = StatusUtil.GetDescription(StatusUtil.ApprovalType.UserLogin);
                        obj.Details = JsonConvert.SerializeObject(authenticatedUser);
                        obj.RequestedBy = authenticatedUser.Username;
                        obj.RequestedOn = System.DateTime.Now;
                        obj.ApprovedBy = authenticatedUser.Username;
                        obj.ApprovedOn = System.DateTime.Now;
                        obj.ClientIP = user.ClientIP;
                        AuditTrailDL.Save(obj);
                    }

                    authenticatedUser.Function = FunctionDL.RetrieveByRoleId(authenticatedUser.UserRole.Id);

                    response.ErrorMsg = string.Empty;
                    response.DynamicList = new { data = authenticatedUser };
                    response.BranchId = authenticatedUser.UserBranch.Id;
                }
                else
                {
                    response.ErrorMsg = useActiveDirectory ? "Active Directory Authentication Failed. Invalid Username/Password!" : "Invalid Username/Password!";
                    response.DynamicList = new { data = new User() };
                }

                return response;
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
