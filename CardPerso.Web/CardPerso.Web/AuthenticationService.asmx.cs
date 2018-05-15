using CardPerso.Library.ModelLayer.Model;
using CardPerso.Library.ProcessLayer;
using CardPerso.Web.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Services;

namespace CardPerso.Web
{
    /// <summary>
    /// Summary description for AuthenticationService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class AuthenticationService : System.Web.Services.WebService
    {

        [WebMethod]
        public AuthenticationResponse ValidateUser(string username, string password)
        {
            var response = new AuthenticationResponse();

            var user = new User
            {
                Username = username,
                Password = password
            };

            var validatedUser = UserPL.AuthenticateUser(user);

            response.IsSuccessful = string.IsNullOrEmpty(validatedUser.ErrorMsg) ? true : false;
            response.FailureReason = string.IsNullOrEmpty(validatedUser.ErrorMsg) ? string.Empty : validatedUser.ErrorMsg;

            return response;
        }

        [WebMethod]
        public AuthenticationResponse ValidateIP(string ipAddress)
        {
            var response = new AuthenticationResponse();

            var validatedIP = IPPL.IPAddressExists(ipAddress);

            response.IsSuccessful = string.IsNullOrEmpty(validatedIP.ErrorMsg) ? true : false;
            response.FailureReason = string.IsNullOrEmpty(validatedIP.ErrorMsg) ? string.Empty : validatedIP.ErrorMsg;

            return response;
        }

        [WebMethod]
        public AuthenticationResponse ValidateUserandIP(string username, string password, string ipAddress)
        {
            var response = new AuthenticationResponse();

            var user = new User
            {
                Username = username,
                Password = password
            };

            var validatedUser = UserPL.AuthenticateUser(user);

            var validatedIP = IPPL.IPAddressExists(ipAddress);

            if (string.IsNullOrEmpty(validatedUser.ErrorMsg) && string.IsNullOrEmpty(validatedIP.ErrorMsg))
            {
                response.IsSuccessful = true;
                response.FailureReason = string.Empty;
            }
            else if (!string.IsNullOrEmpty(validatedUser.ErrorMsg) && string.IsNullOrEmpty(validatedIP.ErrorMsg))
            {
                response.IsSuccessful = false;
                response.FailureReason = validatedUser.ErrorMsg;
            }
            else if (string.IsNullOrEmpty(validatedUser.ErrorMsg) && !string.IsNullOrEmpty(validatedIP.ErrorMsg))
            {
                response.IsSuccessful = false;
                response.FailureReason = validatedIP.ErrorMsg;
            }
            else
            {
                response.IsSuccessful = false;
                response.FailureReason = $"Invalid Username/Password. IP Address {ipAddress} not allowed.";
            }            

            return response;
        }
    }
}
