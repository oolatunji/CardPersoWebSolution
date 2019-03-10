using CardPerso.Library.DataLayer;
using CardPerso.Library.ModelLayer.Model;
using CardPerso.Library.ModelLayer.Utility;
using CardPerso.Library.ProcessLayer;
using CardPerso.Web.Models;
using CardPerso.Web.Responses;
using Newtonsoft.Json;
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

            var ip = HttpContext.Current.Request.UserHostAddress;                  

            response = ValidateClientUser(username, password, ip);

            return response;
        }

        [WebMethod]
        public AuthenticationResponse ValidateIP(string ipAddress)
        {
            var response = new AuthenticationResponse();

            var callingIP = HttpContext.Current.Request.UserHostAddress;

            var validatedCallingIP = IPPL.IPAddressExists(callingIP);

            if(string.IsNullOrEmpty(validatedCallingIP.ErrorMsg))
            {
                var validatedIP = IPPL.IPAddressExists(ipAddress);

                response.IsSuccessful = string.IsNullOrEmpty(validatedIP.ErrorMsg) ? true : false;
                response.FailureReason = string.IsNullOrEmpty(validatedIP.ErrorMsg) ? string.Empty : validatedIP.ErrorMsg;
            }
            else
            {
                response.IsSuccessful = false;
                response.FailureReason = $"Calling IP address: {callingIP} is not allowed.";
            }
           
            return response;
        }

        [WebMethod]
        public AuditResponse AuditCardAccountRequestAction(string printerSerialNumber, string pan, string printedName, string userPrinting, string clientIP)
        {
            var response = new AuditResponse();

            var callingIP = HttpContext.Current.Request.UserHostAddress;

            var validatedCallingIP = IPPL.IPAddressExists(callingIP);

            if (string.IsNullOrEmpty(validatedCallingIP.ErrorMsg))
            {
                var validatedIP = IPPL.IPAddressExists(clientIP);

                if (string.IsNullOrEmpty(validatedIP.ErrorMsg))
                {
                    var cardModel = new PrintedCardModel
                    {
                        Pan = Crypter.Mask(pan),
                        PrintedName = printedName,
                        UserPrinting = userPrinting,
                        PrinterSerialNumber = printerSerialNumber
                    };

                    AuditTrail obj = new AuditTrail();
                    obj.Type = StatusUtil.GetDescription(StatusUtil.ApprovalType.InsertedPrintRecords);
                    obj.Details = JsonConvert.SerializeObject(cardModel);
                    obj.RequestedBy = userPrinting;
                    obj.RequestedOn = System.DateTime.Now;
                    obj.ApprovedBy = userPrinting;
                    obj.ApprovedOn = System.DateTime.Now;
                    obj.ClientIP = clientIP;
                    AuditTrailDL.Save(obj);

                    response.IsSuccessful = true;
                    response.FailureReason = string.Empty;
                }
                else
                {
                    response.IsSuccessful = true;
                    response.FailureReason = $"Client IP: {clientIP} is not allowed.";
                }                                   
            }
            else
            {
                response.IsSuccessful = false;
                response.FailureReason = $"Calling IP address: {callingIP} is not allowed.";
            }

            return response;
        }

        //[WebMethod]
        private AuthenticationResponse ValidateClientUser(string username, string password, string ipAddress)
        {
            var response = new AuthenticationResponse();

            var user = new User
            {
                Username = username,
                Password = password
            };

            var validatedUser = UserPL.AuthenticateUser(user, false);

            if (string.IsNullOrEmpty(validatedUser.ErrorMsg))
            {
                response.IsSuccessful = true;
                response.BranchCode = validatedUser.BranchCode;
                response.BranchId = validatedUser.BranchId;
                response.FailureReason = string.Empty;

                AuditTrail obj = new AuditTrail();
                obj.Type = StatusUtil.GetDescription(StatusUtil.ApprovalType.PrintUtilityUserLogin);
                obj.Details = JsonConvert.SerializeObject(user);
                obj.RequestedBy = user.Username;
                obj.RequestedOn = System.DateTime.Now;
                obj.ApprovedBy = user.Username;
                obj.ApprovedOn = System.DateTime.Now;
                obj.ClientIP = ipAddress;
                AuditTrailDL.Save(obj);
            }            
            else
            {
                response.IsSuccessful = false;
                response.FailureReason = "Invalid Username/Password.";
            }            

            return response;
        }
    }
}
