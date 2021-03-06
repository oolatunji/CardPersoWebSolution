﻿function addFunction() {
    try {

        $('#addBtn').html('<i class="fa fa-spinner fa-spin"></i> Adding...');
        $("#addBtn").attr("disabled", "disabled");

        var functionName = $('#functionName').val();
        var functionPageLink = $('#functionPageLink').val();
        var username = JSON.parse(window.sessionStorage.getItem("loggedInUser")).Username;

        var data = {
            Name: functionName,
            PageLink: functionPageLink,
            LoggedInUser: username
        };

        $.ajax({
            url: settingsManager.websiteURL + 'api/FunctionAPI/SaveFunction',
            type: 'POST',
            data: data,
            processData: true,
            async: true,
            cache: false,
            success: function (response) {
                if (!_.isEmpty(response.SuccessMsg)) {
                    displayMessage("success", response.SuccessMsg, "Functions Management");

                    $('#functionName').val('');
                    $('#functionPageLink').val('');
                }else if(!_.isEmpty(response.ErrorMsg)){
                    displayMessage("error", 'Error experienced: ' + response.ErrorMsg, "Functions Management");
                }
                
                $("#addBtn").removeAttr("disabled");
                $('#addBtn').html('<i class="fa fa-cog"></i> Add');
            },
            error: function (xhr) {
                var errMessage = JSON.parse(xhr.responseText).Message;
                displayMessage("error", errMessage, "Functions Management");
                $("#addBtn").removeAttr("disabled");
                $('#addBtn').html('<i class="fa fa-cog"></i> Add');
            }
        });
    } catch (err) {
        displayMessage("error", "Error encountered: " + err, "Functions Management");
        $("#addBtn").removeAttr("disabled");
        $('#addBtn').html('<i class="fa fa-cog"></i> Add');
    }
}