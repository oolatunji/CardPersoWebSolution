using AutoMapper;
using CardPerso.Library.ModelLayer.Model;
using CardPerso.Library.ModelLayer.Utility;
using CardPerso.Library.ProcessLayer;
using CardPerso.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CardPerso.Web.Controllers
{
    public class RoleAPIController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage SaveRole([FromBody]RoleModel role)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Role roleObj = Mapper.Map<Role>(role);
                    Response result = RolePL.Save(roleObj, role.LoggedInUser, false);
                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }
                else
                {
                    string errors = ModelStateValidation.GetErrorListFromModelState(ModelState);
                    return Request.CreateResponse(HttpStatusCode.OK, new Response { SuccessMsg = string.Empty, ErrorMsg = errors });
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                return Request.CreateResponse(HttpStatusCode.OK, new Response { SuccessMsg = string.Empty, ErrorMsg = ex.Message });
            }
        }

        [HttpPut]
        public HttpResponseMessage UpdateRole([FromBody]RoleModel role)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Role roleObj = Mapper.Map<Role>(role);
                    Response result = RolePL.Update(roleObj, role.LoggedInUser, false);
                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }
                else
                {
                    string errors = ModelStateValidation.GetErrorListFromModelState(ModelState);
                    return Request.CreateResponse(HttpStatusCode.OK, new Response { SuccessMsg = string.Empty, ErrorMsg = errors });
                }
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
