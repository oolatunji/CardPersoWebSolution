let counter = 0;
function LoginIn() {

    try {

        var loginAttempt = $("#loginAttempt");
        var msg = $("#msg");

        var username = $('#username').val();
        var password = $('#password').val();

        var err = loginValidation(username, password);
        if (err != "") {
            displayMessage("error", "Error encountered: " + err, "User Login");
        } else {

            $('#addBtn').html('<i class="fa fa-spinner fa-spin"></i> Login...');
            $("#addBtn").attr("disabled", "disabled");

            var data = { Username: username, Password: password };
            $.ajax({
                url: settingsManager.websiteURL + 'api/UserAPI/AuthenticateUser',
                type: 'POST',
                data: data,
                processData: true,
                async: true,
                cache: false,
                success: function (data) {
                    //Remove local storages if they exist before adding new ones
                    $("#addBtn").removeAttr("disabled");
                    $('#addBtn').html('<i class="fa fa-user"></i> Login');

                    if (window.sessionStorage.getItem("loggedInUser") != null)
                        window.sessionStorage.removeItem("loggedInUser");

                    console.log(data);

                    if (!_.isEmpty(data.ErrorMsg)) {
                        displayMessage("error", 'Error experienced: ' + data.ErrorMsg, "User Login");
                        counter = counter + 1;
                        loginAttempt.show();
                        msg.text(`Failed Login Attempt: ${counter}/3. Your account will locked after 3 failed attempts.`);

                        if (counter == 3) {
                            counter = 0;
                            lockUser();
                        } else {
                            return;
                        }
                        
                    } else {
                        //Add new local storages
                        counter = 0;
                        var user = data.DynamicList.data;
                        if (user.Locked) {
                            displayMessage("error", 'Your account has been locked. Contact your administrator to unlock your account.', "User Login");                            
                            loginAttempt.show();
                            msg.text(`Your account has been locked. Contact your administrator to unlock your account.`);
                            return;
                        } else {
                            window.sessionStorage.setItem("loggedInUser", JSON.stringify(user));
                            window.location = ("Home/Dashboard");
                        }                       
                    }
                }
            });
        }
    } catch (err) {
        displayMessage("error", "Error encountered: " + err, "User Login");        
        $("#addBtn").removeAttr("disabled");
        $('#addBtn').html('<i class="fa fa-user"></i> Login');
    }
}


function lockUser() {
    try {

        $('#addBtn').html('<i class="fa fa-spinner fa-spin"></i>...');
        $("#addBtn").attr("disabled", "disabled");

        var username = $('#username').val();
        var url = `${settingsManager.websiteURL}api/UserAPI/LockUser/${username}`;
        var msg = $("#msg");

        $.ajax({
            url: url,
            type: 'PUT',
            data: {},
            processData: true,
            async: true,
            cache: false,
            success: function (response) {
                if (!_.isEmpty(response.SuccessMsg)) {
                    msg.text(`Your account has been locked. Contact your administrator to unlock your account.`);
                    displayMessage("error", response.SuccessMsg, "User Login");
                }
                
                $('#addBtn').html('<i class="fa fa-user"></i> Login');
            },
            error: function (xhr) {
                var errMessage = JSON.parse(xhr.responseText).Message;
                displayMessage("error", errMessage, "User Login");
                $("#addBtn").removeAttr("disabled");
                $('#addBtn').html('<i class="fa fa-user"></i> Login');
            }
        });

    } catch (err) {
        displayMessage("error", "Error encountered: " + err, "User Management");
        $("#addBtn").removeAttr("disabled");
        $('#addBtn').html('<i class="fa fa-user"></i> Login');
    }
}

function loginValidation(username, password) {
    var err = "";
    var missingFields = "";
    var errCount = 0;
    if (username == "") {
        missingFields += "Username";
        errCount++;
    }
    if (password == "") {
        missingFields += missingFields == "" ? "Password" : ", Password";
        errCount++;
    }

    if (missingFields != "" && errCount == 1) {
        err = "The field " + missingFields + " is required.";
    } else if (missingFields != "" && errCount > 1) {
        err = "The following fields " + missingFields + " are required.";
    }
    return err;
}