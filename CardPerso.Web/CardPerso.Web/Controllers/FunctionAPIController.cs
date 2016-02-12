using CardPerso.Library.ModelLayer.Model;
using CardPerso.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using CardPerso.Library.ModelLayer.Utility;
using CardPerso.Library.ProcessLayer;

namespace CardPerso.Web.Controllers
{
    public class FunctionAPIController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage SaveFunction([FromBody]FunctionModel function)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Function functionObj = Mapper.Map<Function>(function);
                    Response result = FunctionPL.Save(functionObj, function.LoggedInUser, false);
                    return Request.CreateResponse(HttpStatusCode.OK, result); 
                }
                else
                {
                    string errors = ModelStateValidation.GetErrorListFromModelState(ModelState);
                    return Request.CreateResponse(HttpStatusCode.OK, new Response { SuccessMsg = string.Empty, ErrorMsg = errors});
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                return Request.CreateResponse(HttpStatusCode.OK, new Response { SuccessMsg = string.Empty, ErrorMsg = ex.Message });
            }
        }

        [HttpPut]
        public HttpResponseMessage UpdateFunction([FromBody]FunctionModel function)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Function functionObj = Mapper.Map<Function>(function);
                    Response result = FunctionPL.Update(functionObj, function.LoggedInUser, false);
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
        public HttpResponseMessage RetrieveFunctions()
        {
            try
            {
                Response functions = FunctionPL.RetrieveAll();
                return Request.CreateResponse(HttpStatusCode.OK, functions.DynamicList);
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                return Request.CreateResponse(HttpStatusCode.OK, new Response { SuccessMsg = string.Empty, ErrorMsg = ex.Message });
            }
        }
    }
}
