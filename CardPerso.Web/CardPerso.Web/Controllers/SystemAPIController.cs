using AutoMapper;
using CardPerso.Library.ModelLayer.Model;
using CardPerso.Library.ModelLayer.Utility;
using CardPerso.Library.ProcessLayer;
using CardPerso.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Configuration;
using System.Web.Http;

namespace CardPerso.Web.Controllers
{
    public class SystemAPIController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage AuditTrail()
        {
            try
            {
                Response audits = AuditTrailPL.RetrieveAll();
                return Request.CreateResponse(HttpStatusCode.OK, audits.DynamicList);
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                return Request.CreateResponse(HttpStatusCode.OK, new Response { SuccessMsg = string.Empty, ErrorMsg = ex.Message });
            }
        }

        [HttpPost]
        public HttpResponseMessage RetrieveFilteredAudits([FromBody]SearchFilterModel model)
        {
            try
            {
                var filters = Mapper.Map<SearhFilter>(model);
                Response audits = AuditTrailPL.RetrieveFilteredAudits(filters);
                return Request.CreateResponse(HttpStatusCode.OK, audits.DynamicList);
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                return Request.CreateResponse(HttpStatusCode.OK, new Response { SuccessMsg = string.Empty, ErrorMsg = ex.Message });
            }
        }

        [HttpPost]
        public HttpResponseMessage ConfigureSystem([FromBody]SystemModel systemModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string errMsg = "";

                    //Configure JS File used by all APIs (configFile.js)
                    string configFilepath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Scripts/app/Utility/configFile.js");
                    string jsSettings = "var settingsManager = {\"websiteURL\": \"" + systemModel.WebsiteUrl + "\"};";

                    var lines = File.ReadAllLines(configFilepath);
                    lines[0] = jsSettings;
                    File.WriteAllLines(configFilepath, lines);


                    var configuration = WebConfigurationManager.OpenWebConfiguration("~");

                    var appSettingsSection = (AppSettingsSection)configuration.GetSection("appSettings");
                    appSettingsSection.Settings["Organization"].Value = systemModel.Organization;
                    appSettingsSection.Settings["ApplicationName"].Value = systemModel.ApplicationName;
                    appSettingsSection.Settings["WebsiteUrl"].Value = systemModel.WebsiteUrl;
                    appSettingsSection.Settings["OracleDBHost"].Value = systemModel.OracleDBHost;
                    appSettingsSection.Settings["OracleDBPort"].Value = systemModel.OracleDBPort;
                    appSettingsSection.Settings["OracleDBServiceName"].Value = systemModel.OracleDBServiceName;
                    appSettingsSection.Settings["OracleDBUserId"].Value = systemModel.OracleDBUserId;
                    appSettingsSection.Settings["OracleDBPassword"].Value = systemModel.OracleDBPassword;
                    appSettingsSection.Settings["OracleConnectionString"].Value = GetConnectionString(systemModel);

                    var mailHelperSection = (MailHelper)configuration.GetSection("mailHelperSection");
                    mailHelperSection.Mail.FromEmailAddress = systemModel.FromEmailAddress;
                    mailHelperSection.Mail.Username = systemModel.SmtpUsername;
                    mailHelperSection.Mail.Password = systemModel.SmtpPassword;

                    mailHelperSection.Smtp.Host = systemModel.SmtpHost;
                    mailHelperSection.Smtp.Port = systemModel.SmtpPort;

                    configuration.Save();


                    bool result = true;

                    if (string.IsNullOrEmpty(errMsg))
                        return result.Equals(true) ? Request.CreateResponse(HttpStatusCode.OK, "Successful") : Request.CreateResponse(HttpStatusCode.BadRequest, "Request failed");
                    else
                    {
                        var response = Request.CreateResponse(HttpStatusCode.BadRequest, errMsg);
                        return response;
                    }
                }
                else
                {
                    string errors = ModelStateValidation.GetErrorListFromModelState(ModelState);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, errors);
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                var response = Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
                return response;
            }
        }

        [HttpGet]
        public HttpResponseMessage GetSystemSettings()
        {
            try
            {
                object systemSettings = SystemSettings();
                return Request.CreateResponse(HttpStatusCode.OK, systemSettings);
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteError(ex);
                var response = Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
                return response;
            }
        }

        Object SystemSettings()
        {
            Object systemSettings = new object();

            var configuration = WebConfigurationManager.OpenWebConfiguration("~");

            var appSettingsSection = (AppSettingsSection)configuration.GetSection("appSettings");
            string organization = appSettingsSection.Settings["Organization"].Value;
            string applicationName = appSettingsSection.Settings["ApplicationName"].Value;
            string websiteUrl = appSettingsSection.Settings["WebsiteUrl"].Value;
            string logFilePath = appSettingsSection.Settings["LogFilePath"].Value;
            string oracleDBHost = appSettingsSection.Settings["OracleDBHost"].Value;
            string oracleDBPort = appSettingsSection.Settings["OracleDBPort"].Value;
            string oracleDBServiceName = appSettingsSection.Settings["OracleDBServiceName"].Value;
            string oracleDBUserId = appSettingsSection.Settings["OracleDBUserId"].Value;
            string oracleDBPassword = appSettingsSection.Settings["OracleDBPassword"].Value;

            Object generalSettings = new
            {
                Organization = organization,
                ApplicationName = applicationName,
                ApplicationUrl = websiteUrl,
                LogFilePath = logFilePath,
                OracleDBHost = oracleDBHost,
                OracleDBPort = oracleDBPort,
                OracleDBServiceName = oracleDBServiceName,
                OracleDBUserId = oracleDBUserId,
                OracleDBPassword = oracleDBPassword,
            };

            var mailHelperSection = (MailHelper)configuration.GetSection("mailHelperSection");
            string fromEmailAddress = mailHelperSection.Mail.FromEmailAddress;
            string smtpUsername = mailHelperSection.Mail.Username;
            string smtpPassword = mailHelperSection.Mail.Password;
            string smtpHost = mailHelperSection.Smtp.Host;
            string smtpPort = mailHelperSection.Smtp.Port;

            Object mailSettings = new
            {
                SmtpHost = smtpHost,
                SmtpPort = smtpPort,
                SmtpUsername = smtpUsername,
                SmtpPassword = smtpPassword,
                FromEmailAddress = fromEmailAddress,
            };

            systemSettings = new
            {
                GeneralSettings = generalSettings,
                MailSettings = mailSettings
            };

            return systemSettings;
        }

        string GetConnectionString(SystemModel model)
        {
            string connectionString = string.Format("Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={0} )(PORT={1})))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME={2})));User Id={3};Password={4}", model.OracleDBHost, model.OracleDBPort, model.OracleDBServiceName, model.OracleDBUserId, model.OracleDBPassword);

            return connectionString;
        }
    }
}
