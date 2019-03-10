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
    public class RolePL
    {
        public static Response Save(Role role, string username, bool overrideApproval)
        {
            try
            {
                if (RoleDL.RoleExists(role.Name))
                {
                    return new Response
                    {
                        SuccessMsg = string.Empty,
                        ErrorMsg = string.Format("Role with name {0} already exists.", role.Name)
                    };
                }
                else
                {
                    if (!overrideApproval)
                    {
                        bool logForApproval = ApprovalConfigurationDL.RetrieveByType(StatusUtil.GetDescription(StatusUtil.ApprovalType.CreateRole)).Approve;

                        if (logForApproval)
                        {
                            Approval approvalObj = new Approval();
                            approvalObj.Type = StatusUtil.GetDescription(StatusUtil.ApprovalType.CreateRole);
                            approvalObj.Details = JsonConvert.SerializeObject(role);
                            approvalObj.Obj = JsonConvert.SerializeObject(role);
                            approvalObj.RequestedBy = username;
                            approvalObj.RequestedOn = System.DateTime.Now;
                            approvalObj.Status = StatusUtil.ApprovalStatus.Pending.ToString();

                            if (ApprovalDL.Save(approvalObj))
                            {
                                return new Response
                                {
                                    SuccessMsg = "Add Role request was successfully logged for approval",
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
                            if (RoleDL.Save(role))
                            {
                                AuditTrail obj = new AuditTrail();
                                obj.Type = StatusUtil.GetDescription(StatusUtil.ApprovalType.CreateRole);
                                obj.Details = JsonConvert.SerializeObject(role);
                                obj.RequestedBy = username;
                                obj.RequestedOn = System.DateTime.Now;
                                obj.ApprovedBy = username;
                                obj.ApprovedOn = System.DateTime.Now;
                                obj.ClientIP = role.ClientIP;
                                AuditTrailDL.Save(obj);

                                return new Response
                                {
                                    SuccessMsg = "Role added successfully",
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
                        if (RoleDL.Save(role))
                        {
                            return new Response
                            {
                                SuccessMsg = "Role added successfully",
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

        public static Response Update(Role role, Role oldRoleData, string username, bool overrideApproval)
        {
            try
            {
                if (!overrideApproval)
                {
                    bool logForApproval = ApprovalConfigurationDL.RetrieveByType(StatusUtil.GetDescription(StatusUtil.ApprovalType.UpdateRole)).Approve;

                    if (logForApproval)
                    {
                        Approval approvalObj = new Approval();
                        approvalObj.Type = StatusUtil.GetDescription(StatusUtil.ApprovalType.UpdateRole);
                        approvalObj.Details = JsonConvert.SerializeObject(role);
                        approvalObj.OldDetails = JsonConvert.SerializeObject(oldRoleData);
                        approvalObj.Obj = JsonConvert.SerializeObject(role);
                        approvalObj.RequestedBy = username;
                        approvalObj.RequestedOn = System.DateTime.Now;
                        approvalObj.Status = StatusUtil.ApprovalStatus.Pending.ToString();

                        if (ApprovalDL.Save(approvalObj))
                        {
                            return new Response
                            {
                                SuccessMsg = "Update Role request was successfully logged for approval",
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
                        if (RoleDL.Update(role))
                        {
                            AuditTrail obj = new AuditTrail();
                            obj.Type = StatusUtil.GetDescription(StatusUtil.ApprovalType.UpdateRole);
                            obj.Details = JsonConvert.SerializeObject(role);
                            obj.OldDetails = JsonConvert.SerializeObject(oldRoleData);
                            obj.RequestedBy = username;
                            obj.RequestedOn = System.DateTime.Now;
                            obj.ApprovedBy = username;
                            obj.ApprovedOn = System.DateTime.Now;
                            obj.ClientIP = role.ClientIP;

                            AuditTrailDL.Save(obj);

                            return new Response
                            {
                                SuccessMsg = "Role updated successfully",
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
                    if (RoleDL.Update(role))
                    {
                        return new Response
                        {
                            SuccessMsg = "Role updated successfully",
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
                var roles = RoleDL.RetrieveAll();

                var returnedRoles = new List<RoleDto>();

                var roleDict = new Dictionary<string, RoleDto>();

                if (roles.Any())
                {
                    roles.ForEach(role =>
                    {
                        if (roleDict.ContainsKey(role.Name))
                        {
                            roleDict[role.Name].Functions.Add(role.Function);
                        }
                        else
                        {
                            var roleFunctions = new List<Function>();
                            roleFunctions.Add(role.Function);
                            roleDict.Add(role.Name, new RoleDto()
                            {
                                Id = role.Id,
                                Name = role.Name,
                                SuperAdminRole = role.SuperAdminRole,
                                Functions = roleFunctions
                            });
                        }
                    });

                    roleDict.Keys.ToList().ForEach(key =>
                        {
                            returnedRoles.Add(roleDict[key]);
                        });
                }

                return new Response
                {
                    DynamicList = new { data = returnedRoles }
                };
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                return new Response
                {
                    SuccessMsg = string.Empty,
                    ErrorMsg = ex.Message,
                    DynamicList = new { data = new List<Role>() }
                };
            }
        }

        public static string RetrieveRoleNameById(Int32 Id)
        {
            try
            {
                var roles = RoleDL.RetrieveAll();

                var returnedRoles = new List<RoleDto>();

                var roleDict = new Dictionary<string, RoleDto>();

                if (roles.Any())
                {
                    roles.ForEach(role =>
                    {
                        if (roleDict.ContainsKey(role.Name))
                        {
                            roleDict[role.Name].Functions.Add(role.Function);
                        }
                        else
                        {
                            var roleFunctions = new List<Function>();
                            roleFunctions.Add(role.Function);
                            roleDict.Add(role.Name, new RoleDto()
                            {
                                Id = role.Id,
                                Name = role.Name,
                                Functions = roleFunctions
                            });
                        }
                    });

                    roleDict.Keys.ToList().ForEach(key =>
                    {
                        returnedRoles.Add(roleDict[key]);
                    });
                }

                return returnedRoles.Where(role => role.Id == Id).FirstOrDefault().Name;
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                throw ex;
            }
        }
    }
}
