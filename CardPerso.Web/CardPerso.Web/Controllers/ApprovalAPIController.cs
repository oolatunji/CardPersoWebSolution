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
    public class ApprovalAPIController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage RetrieveConfigurations()
        {
            try
            {
                Response confs = ApprovalConfigurationPL.RetrieveAll();
                return Request.CreateResponse(HttpStatusCode.OK, confs.DynamicList);
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                return Request.CreateResponse(HttpStatusCode.OK, new Response { SuccessMsg = string.Empty, ErrorMsg = ex.Message });
            }
        }

        [HttpPut]
        public HttpResponseMessage UpdateConfiguration([FromBody]ApprovalConfigurationModel model)
        {
            try
            {
                var confs = (from configuration in model.Configurations
                            select new ApprovalConfiguration()
                            {
                                Id = configuration.Id,
                                ApproveId = configuration.Approve
                            }).ToList();

                Response result = ApprovalConfigurationPL.Update(confs);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                return Request.CreateResponse(HttpStatusCode.OK, new Response { SuccessMsg = string.Empty, ErrorMsg = ex.Message });
            }
        }

        [HttpGet]
        public HttpResponseMessage RetrieveApprovals()
        {
            try
            {
                Response approvals = ApprovalPL.RetrieveAll();
                return Request.CreateResponse(HttpStatusCode.OK, approvals.DynamicList);
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                return Request.CreateResponse(HttpStatusCode.OK, new Response { SuccessMsg = string.Empty, ErrorMsg = ex.Message });
            }
        }

        [HttpPost]
        public HttpResponseMessage RetrieveApprovalsByUsername([FromBody]SearchFilterModel model)
        {
            try
            {
                Response approvals = ApprovalPL.RetrieveByUsername(model.Username);
                return Request.CreateResponse(HttpStatusCode.OK, approvals.DynamicList);
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                return Request.CreateResponse(HttpStatusCode.OK, new Response { SuccessMsg = string.Empty, ErrorMsg = ex.Message });
            }
        }

        [HttpPost]
        public HttpResponseMessage RetrieveFilteredApprovals([FromBody]SearchFilterModel model)
        {
            try
            {
                var filters = Mapper.Map<SearhFilter>(model);
                Response approvals = ApprovalPL.RetrieveFilteredApprovals(filters);
                return Request.CreateResponse(HttpStatusCode.OK, approvals.DynamicList);
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                return Request.CreateResponse(HttpStatusCode.OK, new Response { SuccessMsg = string.Empty, ErrorMsg = ex.Message });
            }
        }

        [HttpPut]
        public HttpResponseMessage UpdateApprovals([FromBody]ApprovalModel approval)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Approval approvalObj = Mapper.Map<Approval>(approval);
                    approvalObj.ApprovedOn = System.DateTime.Now;
                    Response result = ApprovalPL.Update(approvalObj);
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
    }
}
