$(document).ready(function () {
    var currentUrl = window.location.href;
    var user = JSON.parse(window.sessionStorage.getItem("loggedInUser"));
    var userFunctions = user.Function;

    var exist = false;
    $.each(userFunctions, function (key, userfunction) {
        var link = settingsManager.websiteURL.trimRight('/') + userfunction.PageLink;
        if (currentUrl == link) {
            exist = true;
        }
    });

    if (!exist)
        window.location.href = '../System/UnAuthorized';
    else
        getSystemSettings();
});

String.prototype.trimRight = function (charlist) {
    if (charlist === undefined)
        charlist = "\s";

    return this.replace(new RegExp("[" + charlist + "]+$"), "");
};

function getSystemSettings() {
    try {

        //Get System Settings
        $.ajax({
            url: settingsManager.websiteURL + 'api/SystemAPI/GetSystemSettings',
            type: 'GET',
            async: true,
            cache: false,
            success: function (settings) {

                $('#applicationURL').val(settings.GeneralSettings.ApplicationUrl);
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
            },
            error: function (xhr) {
                displayMessage("error", 'Error experienced: ' + xhr.responseText, "System Management");
            }
        });

    } catch (err) {
        displayMessage("error", "Error encountered: " + err, "System Management");
    }
}

function configureSystem() {
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

        if (websiteUrl == "") {
            displayMessage("error", 'Kindly enter Application Url', "System Management");
        } else {
            var acknowledge = confirm("Are you sure you want to configure the System with the captured settings?");
            if (acknowledge) {
                $('#addBtn').html('<i class="fa fa-spinner fa-spin"></i> Configuring System...');
                $("#addBtn").attr("disabled", "disabled");

                var data = { WebsiteUrl: websiteUrl, Organization: organization, ApplicationName: applicationName, FromEmailAddress: fromEmailAddress, SmtpUsername: smtpUsername, SmtpPassword: smtpPassword, SmtpHost: smtpHost, SmtpPort: smtpPort, OracleDBHost: oracleDBHost, OracleDBPort: oracleDBPort, OracleDBServiceName: oracleDBServiceName, OracleDBUserId: oracleDBUserId, OracleDBPassword: oracleDBPassword };

                $.ajax({
                    url: websiteUrl + 'api/SystemAPI/ConfigureSystem',
                    type: 'POST',
                    data: data,
                    processData: true,
                    async: true,
                    cache: false,
                    success: function (response) {
                        displayMessage("success", 'System configured successfully.', "System Management");
                        
                        $("#addBtn").removeAttr("disabled");
                        $('#addBtn').html('<i class="fa fa-cog"></i> Configure');
                    },
                    error: function (xhr) {
                        if (xhr.status == 404)
                            displayMessage("error", 'Error experienced: Incorrect Application Url.', "System Management");
                        else
                            displayMessage("error", 'Error experienced: ' + xhr.responseText, "System Management");
                        console.log(xhr);
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