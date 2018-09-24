using AutoMapper;
using CardPerso.Library.ModelLayer.Model;
using CardPerso.Library.ModelLayer.Utility;
using CardPerso.Library.ProcessLayer;
using CardPerso.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace CardPerso.Web.Controllers
{
    public class RoleAPIController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage SaveRole([FromBody]RoleModel model)
        {
            try
            {
                Role roleObj = Mapper.Map<Role>(model);
                roleObj.ClientIP = HttpContext.Current.Request.UserHostAddress;

                Response result = RolePL.Save(roleObj, model.LoggedInUser, false);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                return Request.CreateResponse(HttpStatusCode.OK, new Response { SuccessMsg = string.Empty, ErrorMsg = ex.Message });
            }
        }

        [HttpPut]
        public HttpResponseMessage UpdateRole([FromBody]RoleModel model)
        {
            try
            {
                Role roleObj = Mapper.Map<Role>(model);
                roleObj.ClientIP = HttpContext.Current.Request.UserHostAddress;

                RoleModel oldRoleModel = JsonConvert.DeserializeObject<RoleModel>(model.OldData);
                Role oldRoleData = Mapper.Map<Role>(oldRoleModel);
                oldRoleData.ClientIP = roleObj.ClientIP;

                Response result = RolePL.Update(roleObj, oldRoleData, model.LoggedInUser, false);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                return Request.CreateResponse(HttpStatusCode.OK, new Response { SuccessMsg = string.Empty, ErrorMsg = ex.Message });
            }
        }

        [HttpGet]
        public HttpResponseMessage RetrieveRoles()
        {
            try
            {
                Response roles = RolePL.RetrieveAll();
                return Request.CreateResponse(HttpStatusCode.OK, roles.DynamicList);
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                return Request.CreateResponse(HttpStatusCode.OK, new Response { SuccessMsg = string.Empty, ErrorMsg = ex.Message });
            }
        }
    }
}
