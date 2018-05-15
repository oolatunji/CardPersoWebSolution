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
using System.Web;
using System.Web.Http;

namespace CardPerso.Web.Controllers
{
    public class IPAPIController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage SaveIP([FromBody]IPModel ip)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var ipObj = Mapper.Map<IP>(ip);
                    ipObj.ClientIP = HttpContext.Current.Request.UserHostAddress;

                    Response result = IPPL.Save(ipObj, ip.LoggedInUser, false);
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
        public HttpResponseMessage UpdateIP([FromBody]IPModel ip)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var ipObj = Mapper.Map<IP>(ip);
                    ipObj.ClientIP = HttpContext.Current.Request.UserHostAddress;

                    Response result = IPPL.Update(ipObj, ip.LoggedInUser, false);
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
        public HttpResponseMessage DeleteIP([FromBody]IPModel ip)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var ipObj = Mapper.Map<IP>(ip);
                    ipObj.ClientIP = HttpContext.Current.Request.UserHostAddress;

                    Response result = IPPL.Delete(ipObj, ip.LoggedInUser, false);
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
        public HttpResponseMessage RetrieveIPs()
        {
            try
            {
                Response ips = IPPL.RetrieveAll();
                return Request.CreateResponse(HttpStatusCode.OK, ips.DynamicList);
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                return Request.CreateResponse(HttpStatusCode.OK, new Response { SuccessMsg = string.Empty, ErrorMsg = ex.Message });
            }
        }
    }
}
