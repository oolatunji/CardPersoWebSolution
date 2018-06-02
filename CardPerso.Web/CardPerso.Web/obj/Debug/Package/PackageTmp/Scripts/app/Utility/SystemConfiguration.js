function getDefaultSettings() {
    try {
        //Get System Settings
        var applicationUrl = prompt("Enter Application Url.","");
        if (applicationUrl) {
            $('#getBtn').html('<i class="fa fa-spinner fa-spin"></i> Get Default Settings...');
            $("#getBtn").attr("disabled", "disabled");
            $.ajax({
                url: applicationUrl + 'api/SystemAPI/GetSystemSettings',
                type: 'GET',
                async: true,
                cache: false,
                success: function (settings) {

                    $('#applicationURL').val(applicationUrl);
                    $('#bankName').val(settings.GeneralSettings.Organization);
                    $('#applicationName').val(settings.GeneralSettings.ApplicationName);
                    $('#logFilePath').val(settings.GeneralSettings.LogFilePath);

                    $('#fromEmailAddress').val(settings.MailSettings.FromEmailAddress);
                    $('#smtpUsername').val(settings.MailSettings.SmtpUsername);
                    $('#smtpPassword').val(settings.MailSettings.SmtpPassword);
                    $('#smtpServer').val(settings.MailSettings.SmtpHost);
                    $('#smtpPort').val(settings.MailSettings.SmtpPort);

                    $('#oracleDBHost').val(settings.GeneralSettings.OracleDBHost);
                    $('#oracleDBPort').val(settings.GeneralSettings.OracleDBPort);
                    $('#oracleDBServiceName').val(settings.GeneralSettings.OracleDBServiceName);
                    $('#oracleDBUserId').val(settings.GeneralSettings.OracleDBUserId);
                    $('#oracleDBPassword').val(settings.GeneralSettings.OracleDBPassword);
                    
                    $('#usesActiveDirectory').prop('checked', settings.ActiveDirectorySettings.UsesActiveDirectory);
                    $('#adServer').val(settings.ActiveDirectorySettings.ADServer);
                    $('#adContainer').val(settings.ActiveDirectorySettings.ADContainer);
                    $('#adUsername').val(settings.ActiveDirectorySettings.ADUsername);
                    $('#adPassword').val(settings.ActiveDirectorySettings.ADPassword);
                    $('#adServer2').val(settings.ActiveDirectorySettings.ADServer2);
                    $('#adContainer2').val(settings.ActiveDirectorySettings.ADContainer2);
                    $('#adUsername2').val(settings.ActiveDirectorySettings.ADUsername2);
                    $('#adPassword2').val(settings.ActiveDirectorySettings.ADPassword2);

                    $("#getBtn").removeAttr("disabled");
                    $('#getBtn').html('<i class="fa fa-cog"></i> Get Default Settings');
                },
                error: function (xhr) {
                    if (xhr.status = 404)
                        displayMessage("error", 'Error experienced: Incorrect Application Url.', "System Management");
                    else
                        displayMessage("error", 'Error experienced: ' + xhr.responseText, "System Management");
                    console.log(xhr);
                    $("#getBtn").removeAttr("disabled");
                    $('#getBtn').html('<i class="fa fa-cog"></i> Get Default Settings');
                }
            });
        } else {
            if (applicationUrl == "") {
                displayMessage("error", "Error encountered: Enter System's Application Url to get Default System's Settings", "System Management");
            }
        }

    } catch (err) {
        displayMessage("error", "Error encountered: " + err, "System Management");
        console.log(xhr);
        $("#getBtn").removeAttr("disabled");
        $('#getBtn').html('<i class="fa fa-cog"></i> Get Default Settings');
    }
}
function configure() {
    try {

        var websiteUrl = $('#applicationURL').val();
        var organization = $('#bankName').val();
        var applicationName = $('#applicationName').val();
        var fromEmailAddress = $('#fromEmailAddress').val();
        var smtpUsername = $('#smtpUsername').val();
        var smtpPassword = $('#smtpPassword').val();
        var smtpHost = $('#smtpServer').val();
        var smtpPort = $('#smtpPort').val();
        var oracleDBHost = $('#oracleDBHost').val();
        var oracleDBPort = $('#oracleDBPort').val();
        var oracleDBServiceName = $('#oracleDBServiceName').val();
        var oracleDBUserId = $('#oracleDBUserId').val();
        var oracleDBPassword = $('#oracleDBPassword').val();
        var usesActiveDirectory = $('#usesActiveDirectory').is(":checked");
        var adServer = $('#adServer').val();
        var adContainer = $('#adContainer').val();
        var adUsername = $('#adUsername').val();
        var adPassword = $('#adPassword').val();
        var adServer2 = $('#adServer2').val();
        var adContainer2 = $('#adContainer2').val();
        var adUsername2 = $('#adUsername2').val();
        var adPassword2 = $('#adPassword2').val();

        if (websiteUrl == "") {
            displayMessage("error", 'Kindly enter Application Url', "System Management");
        } else {
            var acknowledge = confirm("Are you sure you want to configure the System with the captured settings?");
            if (acknowledge) {
                $('#addBtn').html('<i class="fa fa-spinner fa-spin"></i> Configuring System...');
                $("#addBtn").attr("disabled", "disabled");

                var data = {
                    WebsiteUrl: websiteUrl, 
                    Organization: organization, 
                    ApplicationName: applicationName, 
                    FromEmailAddress: fromEmailAddress, 
                    SmtpUsername: smtpUsername, 
                    SmtpPassword: smtpPassword, 
                    SmtpHost: smtpHost, 
                    SmtpPort: smtpPort, 
                    OracleDBHost: oracleDBHost, 
                    OracleDBPort: oracleDBPort, 
                    OracleDBServiceName: oracleDBServiceName, 
                    OracleDBUserId: oracleDBUserId, 
                    OracleDBPassword: oracleDBPassword,
                    UsesActiveDirectory: usesActiveDirectory,
                    ADServer: adServer,
                    ADContainer: adContainer,
                    ADUsername: adUsername,
                    ADPassword: adPassword,
                    ADServer2: adServer2,
                    ADContainer2: adContainer2,
                    ADUsername2: adUsername2,
                    ADPassword2: adPassword2
                };

                $.ajax({
                    url: websiteUrl + 'api/SystemAPI/ConfigureSystem',
                    type: 'POST',
                    data: data,
                    processData: true,
                    async: true,
                    cache: false,
                    success: function (response) {

                        $("#addBtn").removeAttr("disabled");
                        $('#addBtn').html('<i class="fa fa-cog"></i> Configure');

                        var login = confirm("System configured successfully. Click OK to proceed to Login Page");
                        if (login) {
                            window.location = '../';
                        } else {
                            window.location = window.location;
                        }
                        
                    },
                    error: function (xhr) {
                        var errMessage = JSON.parse(xhr.responseText).Message;
                        displayMessage("error", errMessage, "System Management");
                        $("#addBtn").removeAttr("disabled");
                        $('#addBtn').html('<i class="fa fa-cog"></i> Configure');
                    }                   
                });
            } else {
                displayMessage("info", 'System Configuration Cancelled', "System Management");
            }
        }
    } catch (err) {
        displayMessage("error", "Error encountered: " + err, "System Management");
        console.log(err);
        $("#addBtn").removeAttr("disabled");
        $('#addBtn').html('<i class="fa fa-cog"></i> Configure');
    }
}