$(document).ready(function () {
    try {
        var currentUrl = window.location.href;
        var user = JSON.parse(window.sessionStorage.getItem("loggedInUser"));
        var userIP = user.Function;

        var exist = false;
        $.each(userIP, function (key, userfunction) {
            var link = settingsManager.websiteURL.trimRight('/') + userfunction.PageLink;
            if (currentUrl == link) {
                exist = true;
            }
        });

        if (!exist)
            window.location.href = '../System/UnAuthorized';
        else
            getIP();
    } catch (err) {
        displayMessage("error", "Error encountered: " + err, "Allowed IP Management");
    }
});

String.prototype.trimRight = function (charlist) {
    if (charlist === undefined)
        charlist = "\s";

    return this.replace(new RegExp("[" + charlist + "]+$"), "");
};

function getIP() {

    $('#example tfoot th').each(function () {
        var title = $('#example thead th').eq($(this).index()).text();
        if (title != "")
            $(this).html('<input type="text" placeholder="Search ' + title + '" />');
    });

    var table = $('#example').DataTable({

        "processing": true,

        "ajax": settingsManager.websiteURL + 'api/IPAPI/RetrieveIPs',

        "columns": [
            {
                "className": 'edit-control',
                "orderable": false,
                "data": null,
                "defaultContent": ''
            },
             {
                 "className": 'delete-control',
                 "orderable": false,
                 "data": null,
                 "defaultContent": '<i class="fa fa-trash-o fa-2x" style="color:red;cursor:pointer;"></i>'
             },
            { "data": "Name" },
            { "data": "IPAddress" },
             { "data": "Description" },
            {
                "data": "Id",
                "visible": false
            },
        ],

        "order": [[3, "asc"]],

        "sDom": 'T<"clear">lrtip',

        "oTableTools": {
            "sSwfPath": settingsManager.websiteURL + "images/copy_csv_xls_pdf.swf",
            "aButtons": [
                {
                    "sExtends": "copy",
                    "sButtonText": "Copy to Clipboard",
                    "oSelectorOpts": { filter: 'applied', order: 'current' },
                    "mColumns": "visible"
                },
                {
                    "sExtends": "csv",
                    "sButtonText": "Save to CSV",
                    "oSelectorOpts": { filter: 'applied', order: 'current' },
                    "mColumns": "visible"
                },
                {
                    "sExtends": "xls",
                    "sButtonText": "Save for Excel",
                    "oSelectorOpts": { filter: 'applied', order: 'current' },
                    "mColumns": "visible"
                }
            ]
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

            row.child(format(row.data())).show();
            tr.addClass('shown');
        }
    });

    $('#example tbody').on('click', 'td.delete-control', function () {
        var tr = $(this).closest('tr');
        var row = table.row(tr);
        var ip = row.data();

        var confirmDelete = confirm("Are you sure you want to delete IP?");
        if (confirmDelete) {
            deleteIP(ip);
        }
    });

    $("#example tfoot input").on('keyup change', function () {
        table
            .column($(this).parent().index() + ':visible')
            .search(this.value)
            .draw();
    });
}

function refreshResult() {
    try {
        var table = $('#example').DataTable();
        table.ajax.reload();
    } catch (err) {
        displayMessage("error", "Error encountered: " + err, "Allowed IP Management");
    }
}

$(document).ready(function () {
    $('#dataTables-example').DataTable({
        responsive: true
    });
});

function format(d) {
    // `d` is the original data object for the row
    return '<table width="100%" class="cell-border" cellpadding="5" cellspacing="0" border="2" style="padding-left:50px;">' +
        '<tr>' +
            '<td style="color:navy;width:20%;font-family:Arial;">Name:</td>' +
            '<td><input class="form-control" placeholder="Enter Name" id="name" value="' + d.Name + '"/></td>' +
        '</tr>' +
        '<tr>' +
            '<td style="color:navy;width:20%;font-family:Arial;">IP Address:</td>' +
            '<td><input class="form-control" placeholder="Enter Page Link" id="ipAddress" value="' + d.IPAddress + '"/></td>' +
        '</tr>' +
        '<tr>' +
            '<td style="color:navy;width:20%;font-family:Arial;">IP Address:</td>' +
            '<td><input class="form-control" placeholder="Enter Page Link" id="description" value="' + d.Description + '"/></td>' +
        '</tr>' +
        '<tr>' +
            '<td style="display:none">ID:</td>' +
            '<td style="display:none"><input class="form-control" id="id" value="' + d.Id + '"/></td>' +
        '</tr>' +
        '<tr>' +
            '<td style="color:navy;width:20%;font-family:Calibri;"></td>' +
            '<td><button type="button" id="updateBtn" class="btn btn-blue" style="float:right;" onclick="updateIP();"><i class="fa fa-cog"></i> Update</button></td>' +
        '</tr>' +
    '</table>';
}

function updateIP() {

    try {
        $('#updateBtn').html('<i class="fa fa-spinner fa-spin"></i> Updating...');
        $("#updateBtn").attr("disabled", "disabled");

        var name = $('#name').val();
        var ipAddress = $('#ipAddress').val();
        var description = $('#description').val();
        var id = $('#id').val();
        var username = JSON.parse(window.sessionStorage.getItem("loggedInUser")).Username;

        var data = {
            Name: name,
            IPAddress: ipAddress,
            Description: description,
            Id: id,
            LoggedInUser: username
        };

        $.ajax({
            url: settingsManager.websiteURL + 'api/IPAPI/UpdateIP',
            type: 'PUT',
            data: data,
            processData: true,
            async: true,
            cache: false,
            success: function (response) {
                if (!_.isEmpty(response.SuccessMsg)) {
                    displayMessage("success", response.SuccessMsg, "Allowed IP Management");
                    refreshResult();
                } else if (!_.isEmpty(response.ErrorMsg)) {
                    displayMessage("error", 'Error experienced: ' + response.ErrorMsg, "Allowed IP Management");
                }
                $("#updateBtn").removeAttr("disabled");
                $('#updateBtn').html('<i class="fa fa-cog"></i> Update');
            }
        });
    } catch (err) {
        displayMessage("error", "Error encountered: " + err, "Allowed IP Management");
        $("#updateBtn").removeAttr("disabled");
        $('#updateBtn').html('<i class="fa fa-cog"></i> Update');
    }
}

function deleteIP(ip) {
    try {                
        var username = JSON.parse(window.sessionStorage.getItem("loggedInUser")).Username;

        var data = {
            Name: ip.Name,
            IPAddress: ip.IPAddress,
            Description: ip.Description,
            Id: ip.Id,
            LoggedInUser: username
        };

        $.ajax({
            url: settingsManager.websiteURL + 'api/IPAPI/DeleteIP',
            type: 'PUT',
            data: data,
            processData: true,
            async: true,
            cache: false,
            success: function (response) {
                if (!_.isEmpty(response.SuccessMsg)) {
                    displayMessage("success", response.SuccessMsg, "Allowed IP Management");
                    refreshResult();
                } else if (!_.isEmpty(response.ErrorMsg)) {
                    displayMessage("error", 'Error experienced: ' + response.ErrorMsg, "Allowed IP Management");
                }               
            }
        });
    } catch (err) {
        displayMessage("error", "Error encountered: " + err, "Allowed IP Management");       
    }
}