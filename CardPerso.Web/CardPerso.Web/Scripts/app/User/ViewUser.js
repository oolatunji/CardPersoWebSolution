﻿$(document).ready(function () {
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
        else
            getRolesAndDisplayUsers();
    } catch (err) {
        displayMessage("error", "Error encountered: " + err, "User Management");
    }
});

String.prototype.trimRight = function (charlist) {
    if (charlist === undefined)
        charlist = "\s";

    return this.replace(new RegExp("[" + charlist + "]+$"), "");
};

function getRolesAndDisplayUsers() {
    try {
        $.ajax({
            url: settingsManager.websiteURL + 'api/RoleAPI/RetrieveRoles',
            type: 'GET',
            async: true,
            cache: false,
            success: function (response) {

                var roles = [];
                roles = response.data;

                getUsers(roles);
            },
            error: function (xhr) {
                displayMessage("error", 'Error experienced: ' + xhr.responseText, "User Management");
            }
        });
    } catch (err) {
        displayMessage("error", "Error encountered: " + err, "User Management");
    }
}

function getUsers(roles) {

    $('#example tfoot th').each(function () {
        var title = $('#example tfoot th').eq($(this).index()).text();
        if (title != "")
            $(this).html('<input type="text" placeholder="Search ' + title + '" />');
    });

    var table = $('#example').DataTable({

        "processing": true,

        "ajax": settingsManager.websiteURL + 'api/UserAPI/RetrieveUsers',

        "columns": [
            {
                "className": 'details-control',
                "orderable": false,
                "data": null,
                "defaultContent": ''
            },
            {
                "className": 'edit-control',
                "orderable": false,
                "data": null,
                "defaultContent": ''
            },
            { "data": "LastName" },
            { "data": "Othernames" },
            { "data": "Username" },
            { "data": "UserRole.Name" },
            {
                "data": "Gender",
                "visible": false
            },
            {
                "data": "Email",
                "visible": false
            },
            {
                "data": "CreatedOn",
                "visible": false
            },
            {
                "data": "UserRole.Id",
                "visible": false
            },
            {
                "data": "Id",
                "visible": false
            }
        ],

        "order": [[2, "asc"]],

        "sDom": 'T<"clear">lrtip',

        "oTableTools": {
            "sSwfPath": settingsManager.websiteURL + "images/copy_csv_xls_pdf.swf",
            "aButtons": [
                {
                    "sExtends": "copy",
                    "sButtonText": "Copy to Clipboard",
                    "oSelectorOpts": { filter: 'applied', order: 'current' },
                    "mColumns": [2, 3, 4, 5, 6, 7, 8]
                },
                {
                    "sExtends": "csv",
                    "sButtonText": "Save to CSV",
                    "oSelectorOpts": { filter: 'applied', order: 'current' },
                    "mColumns": [2, 3, 4, 5, 6, 7, 8]
                },
                {
                    "sExtends": "xls",
                    "sButtonText": "Save for Excel",
                    "oSelectorOpts": { filter: 'applied', order: 'current' },
                    "mColumns": [2, 3, 4, 5, 6, 7, 8]
                }
            ]
        }
    });

    $('#example tbody').on('click', 'td.details-control', function () {
        var tr = $(this).closest('tr');
        var row = table.row(tr);

        function closeAll() {
            var e = $('#example tbody tr.shown');
            var rows = table.row(e);
            if (tr != e) {
                e.removeClass('shown');
                rows.child.hide();
            }
        }

        if (row.child.isShown()) {
            closeAll();
        }
        else {
            closeAll();

            row.child(formatDetails(row.data())).show();
            tr.addClass('shown');
        }
    });

    $('#example tbody').on('click', 'td.edit-control', function () {
        var tr = $(this).closest('tr');
        var row = table.row(tr);

        function closeAll() {
            var e = $('#example tbody tr.shown');
            var rows = table.row(e);
            if (tr != e) {
                e.removeClass('shown');
                rows.child.hide();
            }
        }

        if (row.child.isShown()) {
            closeAll();
        }
        else {
            closeAll();

            row.child(format(row.data(), roles)).show();
            tr.addClass('shown');
        }
    });

    $("#example tfoot input").on('keyup change', function () {
        table
            .column($(this).parent().index() + ':visible')
            .search(this.value)
            .draw();
    });
};

function refreshResult() {
    try {
        var table = $('#example').DataTable();
        table.ajax.reload();
    } catch (err) {
        displayMessage("error", "Error encountered: " + err, "User Management");
    }
}

$(document).ready(function () {
    $('#dataTables-example').DataTable({
        responsive: true
    });
});

function format(d, roles) {
    var table = '<table width="100%" class="cell-border" cellpadding="5" cellspacing="0" border="2" style="padding-left:50px;">';
    table += '<tr>';
    table += '<td style="color:navy;width:20%;font-family:Arial;">Lastname:</td>';
    table += '<td><input class="form-control" placeholder="Enter Lastname" id="lastname" value="' + d.LastName + '"/></td>';
    table += '</tr>';
    table += '<tr>';
    table += '<td style="color:navy;width:20%;font-family:Arial;">Othernames:</td>';
    table += '<td><input class="form-control" placeholder="Enter Othernames" id="othernames" value="' + d.Othernames + '"/></td>';
    table += '</tr>';
    table += '<tr>';
    table += '<td style="color:navy;width:20%;font-family:Arial;">Gender:</td>';
    table += '<td><select class="form-control" name="gender" id="gender">';
    table += '<option value="">Select Gender</option>';
    if (d.Gender == "Male") {
        table += '<option selected="selected" value="Male">Male</option>';
        table += '<option value="Female">Female</option>';
    }
    else if (d.Gender == "Female") {
        table += '<option value="Male">Male</option>';
        table += '<option selected="selected" value="Female">Female</option>';
    }
    table += '</select></td></tr>';
    table += '<tr>';
    table += '<td style="color:navy;width:20%;font-family:Arial;">Email:</td>';
    table += '<td><input class="form-control" placeholder="Enter Email" id="email" value="' + d.Email + '"/></td>';
    table += '</tr>';
    table += '<tr>';
    table += '<td style="color:navy;width:20%;font-family:Arial;">Role:</td>';
    table += '<td><select class="form-control" name="role" id="role">';
    table += '<option value="">Select Role</option>';
    $.each(roles, function (key, value) {
        if (d.UserRole.Id == value.Id)
            table += '<option selected="selected" value="' + value.Id + '">' + value.Name + '</option>';
        else
            table += '<option value="' + value.Id + '">' + value.Name + '</option>';
    });
    table += '</select></td></tr>'; 
    table += '<tr>';
    table += '<td style="display:none">Username:</td>';
    table += '<td style="display:none"><input class="form-control" id="username" value="' + d.Username + '"/></td>';
    table += '</tr>';
    table += '<tr>';
    table += '<td style="display:none">ID:</td>';
    table += '<td style="display:none"><input class="form-control" id="id" value="' + d.Id + '"/></td>';
    table += '</tr>';
    table += '<tr>';
    table += '<td style="color:navy;width:20%;font-family:Calibri;"></td>';
    table += '<td><button type="button"  id="updateBtn" class="btn btn-blue" style="float:right;" onclick="update();"><i class="fa fa-cog"></i> Update</button></td>';
    table += '</tr>';
    table += '</table>';

    return table;
}

function formatDetails(d) {
    var table = '<table width="100%" class="cell-border" cellpadding="5" cellspacing="0" border="2" style="padding-left:50px;">';
    table += '<tr>';
    table += '<td style="color:navy;width:20%;font-family:Arial;">Gender</td>';
    table += '<td>' + d.Gender + '</td>';
    table += '</tr>';
    table += '<tr>';
    table += '<td style="color:navy;width:20%;font-family:Arial;">Email</td>';
    table += '<td>' + d.Email + '</td>';
    table += '</tr>';
    table += '<tr>';
    table += '<td style="color:navy;width:20%;font-family:Arial;">Created On</td>';
    table += '<td>' + d.CreatedOn + '</td>';
    table += '</tr>';
    table += '</table>';
    return table;
}

function update() {
    try {
        $('#updateBtn').html('<i class="fa fa-spinner fa-spin"></i> Updating...');
        $("#updateBtn").attr("disabled", "disabled");

        var lastname = $('#lastname').val();
        var othernames = $('#othernames').val();
        var gender = $('#gender').val();
        var email = $('#email').val();
        var userRole = $('#role').val();
        var userRoleName = $('#role option:selected').html();
        var username = $('#username').val();
        var id = $('#id').val();
        var loggedInUsername = JSON.parse(window.sessionStorage.getItem("loggedInUser")).Username;

        var data = {
            Lastname: lastname,
            Othernames: othernames,
            Gender: gender,
            Email: email,
            Username: username,
            RoleId: userRole,
            LoggedInUser: loggedInUsername,
            RoleName: userRoleName,
            ID: id
        };

        $.ajax({
            url: settingsManager.websiteURL + 'api/UserAPI/UpdateUser',
            type: 'PUT',
            data: data,
            processData: true,
            async: true,
            cache: false,
            success: function (response) {
                if (!_.isEmpty(response.SuccessMsg)) {
                    displayMessage("success", response.SuccessMsg, "User Management");
                    refreshResult();
                } else if (!_.isEmpty(response.ErrorMsg)) {
                    displayMessage("error", 'Error experienced: ' + response.ErrorMsg, "User Management");
                }
                $("#updateBtn").removeAttr("disabled");
                $('#updateBtn').html('<i class="fa fa-cog"></i> Update');
            },
            error: function (xhr) {
                displayMessage("error", 'Error experienced: ' + xhr.responseText, "User Management");
                $("#updateBtn").removeAttr("disabled");
                $('#updateBtn').html('<i class="fa fa-cog"></i> Update');
            }
        });
    } catch (err) {
        displayMessage("error", "Error encountered: " + err, "User Management");
        $("#updateBtn").removeAttr("disabled");
        $('#updateBtn').html('<i class="fa fa-cog"></i> Update');
    }
}