﻿function retrievePassword() {

    try{
        var username = $('#username').val();

        var err = usernameValidation(username);

        if (err != "") {
            displayMessage("error", "Error encountered: " + err, "Password Management");
        } else {
            $('#addBtn').html('<i class="fa fa-spinner fa-spin"></i> Retrieve...');
            $("#addBtn").attr("disabled", "disabled");

            var data = { Username: username, Password: "na" };
            $.ajax({
                url: settingsManager.websiteURL + 'api/UserAPI/ForgotPassword',
                type: 'PUT',
                data: data,
                processData: true,
                async: true,
                cache: false,
                success: function (data) {

                    if (!_.isEmpty(data.ErrorMsg)) {
                        displayMessage("error", 'Error experienced: ' + data.ErrorMsg, "Password Management");
                    } else {
                        displayMessage("success", "An email that contains a link to continue with your password reset has been sent to your email: " + data.DynamicList.data.Email + ". If this email address is not correct, kindly contact your administrator to modify accordingly", "Password Management");
                        $('#username').val("");
                    }
                    $("#addBtn").removeAttr("disabled");
                    $('#addBtn').html('<i class="fa fa-user"></i> Retrieve');
                },
                error: function (xhr) {                
                    var errMessage = JSON.parse(xhr.responseText).Message;
                    displayMessage("error", errMessage, "Password Management");
                    $("#addBtn").removeAttr("disabled");
                    $('#addBtn').html('<i class="fa fa-user"></i> Retrieve');
                }
            });
        }
    } catch (err) {
        displayMessage("error", "Error encountered: " + err, "Password Management");
        console.log(err);
        $("#addBtn").removeAttr("disabled");
        $('#addBtn').html('<i class="fa fa-user"></i> Retrieve');
    }
}

function usernameValidation(username) {
    var err = "";
    var missingFields = "";

    if (username == "") {
        missingFields += "Username";
    }
    if (missingFields != "") {
        err = "The field " + missingFields + " is required.";
    }
    return err;
}