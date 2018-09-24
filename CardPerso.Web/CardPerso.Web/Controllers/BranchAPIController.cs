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
    public class BranchAPIController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage SaveBranch([FromBody]BranchModel model)
        {
            try
            {
                var branchObj = Mapper.Map<Branch>(model);
                branchObj.ClientIP = HttpContext.Current.Request.UserHostAddress;

                Response result = BranchPL.Save(branchObj, model.LoggedInUser, false);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                return Request.CreateResponse(HttpStatusCode.OK, new Response { SuccessMsg = string.Empty, ErrorMsg = ex.Message });
            }
        }

        [HttpPut]
        public HttpResponseMessage UpdateBranch([FromBody]BranchModel model)
        {
            try
            {
                var branchObj = Mapper.Map<Branch>(model);
                branchObj.ClientIP = HttpContext.Current.Request.UserHostAddress;

                BranchModel oldBranchModel = JsonConvert.DeserializeObject<BranchModel>(model.OldData);
                Branch oldBranchData = Mapper.Map<Branch>(oldBranchModel);
                oldBranchData.ClientIP = branchObj.ClientIP;

                Response result = BranchPL.Update(branchObj, oldBranchData, model.LoggedInUser, false);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                return Request.CreateResponse(HttpStatusCode.OK, new Response { SuccessMsg = string.Empty, ErrorMsg = ex.Message });
            }
        }        

        [HttpGet]
        public HttpResponseMessage RetrieveBranches()
        {
            try
            {
                Response branches = BranchPL.RetrieveAll();
                return Request.CreateResponse(HttpStatusCode.OK, branches.DynamicList);
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                return Request.CreateResponse(HttpStatusCode.OK, new Response { SuccessMsg = string.Empty, ErrorMsg = ex.Message });
            }
        }
    }
}
