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
        public HttpResponseMessage ConfigureSystem([FromBody]SystemModel model)
        {
            try
            {
                string errMsg = "";

                //Configure JS File used by all APIs (configFile.js)
                string configFilepath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Scripts/app/Utility/configFile.js");
                string jsSettings = "var settingsManager = {\"websiteURL\": \"" + model.WebsiteUrl + "\"};";

                var lines = File.ReadAllLines(configFilepath);
                lines[0] = jsSettings;
                File.WriteAllLines(configFilepath, lines);


                var configuration = WebConfigurationManager.OpenWebConfiguration("~");

                var appSettingsSection = (AppSettingsSection)configuration.GetSection("appSettings");
                appSettingsSection.Settings["Organization"].Value = model.Organization;
                appSettingsSection.Settings["ApplicationName"].Value = model.ApplicationName;
                appSettingsSection.Settings["WebsiteUrl"].Value = model.WebsiteUrl;
                appSettingsSection.Settings["OracleDBHost"].Value = model.OracleDBHost;
                appSettingsSection.Settings["OracleDBPort"].Value = model.OracleDBPort;
                appSettingsSection.Settings["OracleDBServiceName"].Value = model.OracleDBServiceName;
                appSettingsSection.Settings["OracleDBUserId"].Value = model.OracleDBUserId;
                appSettingsSection.Settings["OracleDBPassword"].Value = model.OracleDBPassword;
                appSettingsSection.Settings["OracleConnectionString"].Value = GetConnectionString(model);

                var mailHelperSection = (MailHelper)configuration.GetSection("mailHelperSection");
                mailHelperSection.Mail.FromEmailAddress = model.FromEmailAddress;
                mailHelperSection.Mail.Username = model.SmtpUsername;
                mailHelperSection.Mail.Password = model.SmtpPassword;
                mailHelperSection.Smtp.Host = model.SmtpHost;
                mailHelperSection.Smtp.Port = model.SmtpPort;

                var activeDirectoryHelperSection = (ActiveDirectoryHelper)configuration.GetSection("activeDirectorySection");
                activeDirectoryHelperSection.ActiveDirectory.UsesActiveDirectory = model.UsesActiveDirectory;
                activeDirectoryHelperSection.ActiveDirectory.ADServer = model.ADServer;
                activeDirectoryHelperSection.ActiveDirectory.ADContainer = model.ADContainer;
                activeDirectoryHelperSection.ActiveDirectory.ADUsername = model.ADUsername;
                activeDirectoryHelperSection.ActiveDirectory.ADPassword = model.ADPassword;
                activeDirectoryHelperSection.ActiveDirectory.ADServer2 = model.ADServer2;
                activeDirectoryHelperSection.ActiveDirectory.ADContainer2 = model.ADContainer2;
                activeDirectoryHelperSection.ActiveDirectory.ADUsername2 = model.ADUsername2;
                activeDirectoryHelperSection.ActiveDirectory.ADPassword2 = model.ADPassword2;

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

            var activeDirectoryHelperSection = (ActiveDirectoryHelper)configuration.GetSection("activeDirectorySection");
            Object activeDirectorySettings = new
            {
                UsesActiveDirectory = Convert.ToBoolean(activeDirectoryHelperSection.ActiveDirectory.UsesActiveDirectory),
                ADServer = activeDirectoryHelperSection.ActiveDirectory.ADServer,
                ADContainer = activeDirectoryHelperSection.ActiveDirectory.ADContainer,
                ADUsername = activeDirectoryHelperSection.ActiveDirectory.ADUsername,
                ADPassword = activeDirectoryHelperSection.ActiveDirectory.ADPassword,
                ADServer2 = activeDirectoryHelperSection.ActiveDirectory.ADServer2,
                ADContainer2 = activeDirectoryHelperSection.ActiveDirectory.ADContainer2,
                ADUsername2 = activeDirectoryHelperSection.ActiveDirectory.ADUsername2,
                ADPassword2 = activeDirectoryHelperSection.ActiveDirectory.ADPassword2
            };

            systemSettings = new
            {
                GeneralSettings = generalSettings,
                MailSettings = mailSettings,
                ActiveDirectorySettings = activeDirectorySettings
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
