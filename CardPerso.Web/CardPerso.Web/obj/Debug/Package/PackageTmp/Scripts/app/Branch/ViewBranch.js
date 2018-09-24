var p = this;
p.existingBranch = {};

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
        else
            getBranch();
    } catch (err) {
        displayMessage("error", "Error encountered: " + err, "Branch Management");
    }
});

String.prototype.trimRight = function (charlist) {
    if (charlist === undefined)
        charlist = "\s";

    return this.replace(new RegExp("[" + charlist + "]+$"), "");
};

function getBranch() {

    $('#example tfoot th').each(function () {
        var title = $('#example thead th').eq($(this).index()).text();
        if (title != "")
            $(this).html('<input type="text" placeholder="Search ' + title + '" />');
    });

    var table = $('#example').DataTable({

        "processing": true,

        "ajax": settingsManager.websiteURL + 'api/BranchAPI/RetrieveBranches',

        "columns": [
            {
                "className": 'edit-control',
                "orderable": false,
                "data": null,
                "defaultContent": ''
            },             
            { "data": "Name" },
            { "data": "Code" },
             { "data": "Address" },
            {
                "data": "Id",
                "visible": false
            },
        ],

        "order": [[0, "asc"]],

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

    p.existingBranch = d;

    // `d` is the original data object for the row
    return '<table width="100%" class="cell-border" cellpadding="5" cellspacing="0" border="2" style="padding-left:50px;">' +
        '<tr>' +
            '<td style="color:navy;width:20%;font-family:Arial;">Name:</td>' +
            '<td><input class="form-control" placeholder="Enter Branch Name" id="name" value="' + d.Name + '"/></td>' +
        '</tr>' +
        '<tr>' +
            '<td style="color:navy;width:20%;font-family:Arial;">Code:</td>' +
            '<td><input class="form-control" placeholder="Enter Branch Code" id="code" value="' + d.Code + '"/></td>' +
        '</tr>' +
        '<tr>' +
            '<td style="color:navy;width:20%;font-family:Arial;">Address:</td>' +
            '<td><input class="form-control" placeholder="Enter Branch Address" id="address" value="' + d.Address + '"/></td>' +
        '</tr>' +
        '<tr>' +
            '<td style="display:none">ID:</td>' +
            '<td style="display:none"><input class="form-control" id="id" value="' + d.Id + '"/></td>' +
        '</tr>' +
        '<tr>' +
            '<td style="color:navy;width:20%;font-family:Calibri;"></td>' +
            '<td><button type="button" id="updateBtn" class="btn btn-blue" style="float:right;" onclick="updateBranch();"><i class="fa fa-cog"></i> Update</button></td>' +
        '</tr>' +
    '</table>';
}

function updateBranch() {

    try {
        $('#updateBtn').html('<i class="fa fa-spinner fa-spin"></i> Updating...');
        $("#updateBtn").attr("disabled", "disabled");

        var username = JSON.parse(window.sessionStorage.getItem("loggedInUser")).Username;

        var oldData = {
            Name: p.existingBranch.Name,
            Code: p.existingBranch.Code,
            Address: p.existingBranch.Address,
            Id: p.existingBranch.Id,
            LoggedInUser: username
        };

        var name = $('#name').val();
        var code = $('#code').val();
        var address = $('#address').val();
        var id = $('#id').val();        

        var data = {
            Name: name,
            Code: code,
            Address: address,
            Id: id,
            LoggedInUser: username,
            OldData: JSON.stringify(oldData)
        };

        $.ajax({
            url: settingsManager.websiteURL + 'api/BranchAPI/UpdateBranch',
            type: 'PUT',
            data: data,
            processData: true,
            async: true,
            cache: false,
            success: function (response) {
                if (!_.isEmpty(response.SuccessMsg)) {
                    displayMessage("success", response.SuccessMsg, "Branch Management");
                    refreshResult();
                } else if (!_.isEmpty(response.ErrorMsg)) {
                    displayMessage("error", 'Error experienced: ' + response.ErrorMsg, "Branch Management");
                }
                $("#updateBtn").removeAttr("disabled");
                $('#updateBtn').html('<i class="fa fa-cog"></i> Update');
            },
            error: function (xhr) {
                var errMessage = JSON.parse(xhr.responseText).Message;
                displayMessage("error", errMessage, "Branch Management");
                $("#updateBtn").removeAttr("disabled");
                $('#updateBtn').html('<i class="fa fa-cog"></i> Update');
            }
        });
    } catch (err) {
        displayMessage("error", "Error encountered: " + err, "Branch Management");
        $("#updateBtn").removeAttr("disabled");
        $('#updateBtn').html('<i class="fa fa-cog"></i> Update');
    }
}