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
            $('#dynamicfunctions').html('<p style="font-family:Calibri;color:blue;"><i class="fa fa-cog fa-spin"></i> Loading functions...</p>');
            $.ajax({
                url: settingsManager.websiteURL + 'api/FunctionAPI/RetrieveFunctions',
                type: 'GET',
                async: true,
                cache: false,
                success: function (response) {
                    var functions = response.data;
                    var html = '';
                    $.each(functions, function (key, value) {
                        html += '<input type="checkbox" name="functions" value="' + value.Id + '" title="' + value.Name + '"/>' + value.Name + '<br/>';
                    });
                    $('#dynamicfunctions').html('');
                    $('#dynamicfunctions').append(html);
                },
                error: function (xhr) {
                    displayMessage("error", 'Error experienced: ' + xhr.responseText, "Roles Management");
                }
            });
        }
    } catch (err) {
        displayMessage("error", "Error encountered: " + err, "Roles Management");
    }
});

String.prototype.trimRight = function (charlist) {
    if (charlist === undefined)
        charlist = "\s";

    return this.replace(new RegExp("[" + charlist + "]+$"), "");
};

function addRole() {
    try {
        var username = JSON.parse(window.sessionStorage.getItem("loggedInUser")).Username;
        var roleName = $('#roleName').val();
        var functions = [];
        $("input:checkbox[name=functions]:checked").each(function () {
            var _function = {
                Name: $(this)[0].title,
                Id: $(this).val()
            }
            functions.push(_function);
        });

        $('#addBtn').html('<i class="fa fa-spinner fa-spin"></i> Adding...');
        $("#addBtn").attr("disabled", "disabled");

        var data = {
            Name: roleName,
            Functions: functions,
            LoggedInUser: username
        };

        $.ajax({
            url: settingsManager.websiteURL + 'api/RoleAPI/SaveRole',
            type: 'POST',
            data: data,
            processData: true,
            async: true,
            cache: false,
            success: function (response) {
                if (!_.isEmpty(response.SuccessMsg)) {
                    displayMessage("success", response.SuccessMsg, "Roles Management");

                    $('#roleName').val('');
                    $('#dynamicfunctions input[type=checkbox]').removeAttr('checked');
                } else if (!_.isEmpty(response.ErrorMsg)) {
                    displayMessage("error", 'Error experienced: ' + response.ErrorMsg, "Roles Management");
                }

                $("#addBtn").removeAttr("disabled");
                $('#addBtn').html('<i class="fa fa-cog"></i> Add');
            },
             error: function (xhr) {                
                var errMessage = JSON.parse(xhr.responseText).Message;
                displayMessage("error", errMessage, "Roles Management");
                $("#addBtn").removeAttr("disabled");
                $('#addBtn').html('<i class="fa fa-cog"></i> Add');
            }
        });

    } catch (err) {
        displayMessage("error", "Error encountered: " + err, "Roles Management");
        $("#addBtn").removeAttr("disabled");
        $('#addBtn').html('<i class="fa fa-cog"></i> Add');
    }
}