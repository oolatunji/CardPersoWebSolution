$(document).ready(function () {
    try {
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
        else {
            $('#dynamicfunctions').html('<p style="font-family:Calibri;color:blue;"><i class="fa fa-cog fa-spin"></i> Loading functions for approval...</p>');
            $.ajax({
                url: settingsManager.websiteURL + 'api/ApprovalAPI/RetrieveConfigurations',
                type: 'GET',
                async: true,
                cache: false,
                success: function (response) {
                    var confs = response.data;
                    var html = '';
                    $.each(confs, function (key, value) {
                        if (value.Approve) {
                            html += '<input type="checkbox" name="functions" checked="checked" value="' + value.Id + '"/>' + value.Type + '<br/>';
                        } else {
                            html += '<input type="checkbox" name="functions" value="' + value.Id + '"/>' + value.Type + '<br/>';
                        }
                    });
                    $('#dynamicfunctions').html('');
                    $('#dynamicfunctions').append(html);
                },
                error: function (xhr) {
                    displayMessage("error", 'Error experienced: ' + xhr.responseText, "Approval Management");
                }
            });
        }
    } catch (err) {
        displayMessage("error", "Error encountered: " + err, "Approval Management");
    }
});

String.prototype.trimRight = function (charlist) {
    if (charlist === undefined)
        charlist = "\s";

    return this.replace(new RegExp("[" + charlist + "]+$"), "");
};

function configure() {
    try {
        var confs = [];
        $("input:checkbox[name=functions]").each(function () {
            var approvalModel = {
                Id: $(this)[0].value,
                Approve: $(this)[0].checked ? 1 : 0
            }
            confs.push(approvalModel);
        });

        $('#addBtn').html('<i class="fa fa-spinner fa-spin"></i> Configuring...');
        $("#addBtn").attr("disabled", "disabled");

        var data = {
            Configurations: confs
        };

        $.ajax({
            url: settingsManager.websiteURL + 'api/ApprovalAPI/UpdateConfiguration',
            type: 'PUT',
            data: data,
            processData: true,
            async: true,
            cache: false,
            success: function (response) {
                if (!_.isEmpty(response.SuccessMsg)) {
                    displayMessage("success", response.SuccessMsg, "Approval Management");
                } else if (!_.isEmpty(response.ErrorMsg)) {
                    displayMessage("error", 'Error experienced: ' + response.ErrorMsg, "Approval Management");
                }

                $("#addBtn").removeAttr("disabled");
                $('#addBtn').html('<i class="fa fa-cog"></i> Configure');
            }
        });
    } catch (err) {
        displayMessage("error", "Error encountered: " + err, "Approval Management");
        $("#addBtn").removeAttr("disabled");
        $('#addBtn').html('<i class="fa fa-cog"></i> Configure');
    }
}