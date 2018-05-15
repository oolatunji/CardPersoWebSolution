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
    public class UserAPIController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage SaveUser([FromBody]UserModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User user = Mapper.Map<User>(model);
                    user.CreatedOn = System.DateTime.Now;
                    user.Password = System.Web.Security.Membership.GeneratePassword(6, 0);
                    user.ClientIP = HttpContext.Current.Request.UserHostAddress;

                    Response result = UserPL.Save(user, model.LoggedInUser, false);
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
        public HttpResponseMessage UpdateUser([FromBody]UserModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User user = Mapper.Map<User>(model);
                    user.ClientIP = HttpContext.Current.Request.UserHostAddress;

                    Response result = UserPL.Update(user, model.LoggedInUser, false);
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
        public HttpResponseMessage ChangePassword([FromBody]PasswordModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User user = Mapper.Map<User>(model);
                    Response result = UserPL.UpdatePassword(user);
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
        public HttpResponseMessage ForgotPassword([FromBody]PasswordModel model)
        {
            try
            {
                User user = Mapper.Map<User>(model);
                Response authenticatedUser = UserPL.RetrieveUserByUsername(user, true);
                return Request.CreateResponse(HttpStatusCode.OK, authenticatedUser);
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                return Request.CreateResponse(HttpStatusCode.OK, new Response { SuccessMsg = string.Empty, ErrorMsg = ex.Message });
            }
        }

        [HttpPost]
        public HttpResponseMessage ConfirmUsername([FromBody]PasswordModel model)
        {
            try
            {
                string key = System.Configuration.ConfigurationManager.AppSettings.Get("ekey");
                User user = new User();
                user.Username = Crypter.Decrypt(key, model.Username);

                Response authenticatedUser = UserPL.RetrieveUserByUsername(user, false);
                return Request.CreateResponse(HttpStatusCode.OK, authenticatedUser);
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                return Request.CreateResponse(HttpStatusCode.OK, new Response { SuccessMsg = string.Empty, ErrorMsg = ex.Message });
            }
        }

        [HttpGet]
        public HttpResponseMessage RetrieveUsers()
        {
            try
            {
                Response users = UserPL.RetrieveAll();
                return Request.CreateResponse(HttpStatusCode.OK, users.DynamicList);
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                return Request.CreateResponse(HttpStatusCode.OK, new Response { SuccessMsg = string.Empty, ErrorMsg = ex.Message });
            }
        }

        [HttpPost]
        public HttpResponseMessage AuthenticateUser([FromBody]PasswordModel model)
        {
            try
            {
                User user = Mapper.Map<User>(model);
                user.ClientIP = HttpContext.Current.Request.UserHostAddress;

                Response authenticatedUser = UserPL.AuthenticateUser(user);
                return Request.CreateResponse(HttpStatusCode.OK, authenticatedUser);
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                return Request.CreateResponse(HttpStatusCode.OK, new Response { SuccessMsg = string.Empty, ErrorMsg = ex.Message });
            }
        }
    }
}
