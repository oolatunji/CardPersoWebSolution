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
    public class CardAPIController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage RetrieveCards([FromBody]SearchFilterModel model)
        {
            try
            {
                Response cards = CardPL.RetrieveAll(model.Username);
                return Request.CreateResponse(HttpStatusCode.OK, cards.DynamicList);
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                return Request.CreateResponse(HttpStatusCode.OK, new Response { SuccessMsg = string.Empty, ErrorMsg = ex.Message });
            }
        }

        [HttpPost]
        public HttpResponseMessage RetrieveFilteredCards([FromBody]SearchFilterModel model)
        {
            try
            {
                var filters = Mapper.Map<SearhFilter>(model);
                Response cards = CardPL.RetrieveFilteredCards(filters);
                return Request.CreateResponse(HttpStatusCode.OK, cards.DynamicList);
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                return Request.CreateResponse(HttpStatusCode.OK, new Response { SuccessMsg = string.Empty, ErrorMsg = ex.Message });
            }
        }

        [HttpPut]
        public HttpResponseMessage UpdateCard([FromBody]CardModel model)
        {
            try
            {
                Card card = Mapper.Map<Card>(model);
                card.Pan = Crypter.Mask(card.Pan);
                card.ClientIP = HttpContext.Current.Request.UserHostAddress;

                Response result = CardPL.Update(card, model.LoggedInUser, false);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                return Request.CreateResponse(HttpStatusCode.OK, new Response { SuccessMsg = string.Empty, ErrorMsg = ex.Message });
            }
        }
    }
}
