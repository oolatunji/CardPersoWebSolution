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
    public class IPAPIController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage SaveIP([FromBody]IPModel model)
        {
            try
            {
                var ipObj = Mapper.Map<IP>(model);
                ipObj.ClientIP = HttpContext.Current.Request.UserHostAddress;

                Response result = IPPL.Save(ipObj, model.LoggedInUser, false);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                return Request.CreateResponse(HttpStatusCode.OK, new Response { SuccessMsg = string.Empty, ErrorMsg = ex.Message });
            }
        }

        [HttpPut]
        public HttpResponseMessage UpdateIP([FromBody]IPModel model)
        {
            try
            {
                var ipObj = Mapper.Map<IP>(model);
                ipObj.ClientIP = HttpContext.Current.Request.UserHostAddress;

                IPModel oldIPModel = JsonConvert.DeserializeObject<IPModel>(model.OldData);
                IP oldIPData = Mapper.Map<IP>(oldIPModel);
                oldIPData.ClientIP = ipObj.ClientIP;

                Response result = IPPL.Update(ipObj, oldIPData, model.LoggedInUser, false);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                return Request.CreateResponse(HttpStatusCode.OK, new Response { SuccessMsg = string.Empty, ErrorMsg = ex.Message });
            }
        }

        [HttpPut]
        public HttpResponseMessage DeleteIP([FromBody]IPModel model)
        {
            try
            {
                var ipObj = Mapper.Map<IP>(model);
                ipObj.ClientIP = HttpContext.Current.Request.UserHostAddress;

                Response result = IPPL.Delete(ipObj, model.LoggedInUser, false);
                return Request.CreateResponse(HttpStatusCode.OK, result);
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
