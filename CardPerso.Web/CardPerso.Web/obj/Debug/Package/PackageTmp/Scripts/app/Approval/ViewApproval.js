﻿$(document).ready(function () {
    try {
        var username = JSON.parse(window.sessionStorage.getItem("loggedInUser")).Username;

        $.datetimepicker.setLocale('en');

        $('#requestedFrom').datetimepicker({
            lang: 'ch',
            timepicker: false,
            format: 'm/d/Y',
            formatDate: 'm/d/Y'
        });
        $('#openrequestedFrom').click(function () {
            $('#requestedFrom').datetimepicker('show');
        });
        $('#closerequestedFrom').click(function () {
            $('#requestedFrom').datetimepicker('hide');
        });

        $('#requestedTo').datetimepicker({
            lang: 'ch',
            timepicker: false,
            format: 'm/d/Y',
            formatDate: 'm/d/Y'
        });
        $('#openrequestedTo').click(function () {
            $('#requestedTo').datetimepicker('show');
        });
        $('#closerequestedTo').click(function () {
            $('#requestedTo').datetimepicker('hide');
        });

        $('#status').val('Pending');
        $('#requestedby').val(username);
        getPendingApprovals();
    } catch (err) {
        displayMessage("error", "Error encountered: " + err, "Approval Management");
    }
});

String.prototype.trimRight = function (charlist) {
    if (charlist === undefined)
        charlist = "\s";

    return this.replace(new RegExp("[" + charlist + "]+$"), "");
};

function getPendingApprovals() {

    var username = JSON.parse(window.sessionStorage.getItem("loggedInUser")).Username;

    $('#example tfoot th').each(function () {
        var title = $('#example tfoot th').eq($(this).index()).text();
        if (title != "")
            $(this).html('<input type="text" placeholder="Search ' + title + '" />');
    });

    var table = $('#example').DataTable({

        "processing": true,

        "ajax": {
            "url": settingsManager.websiteURL + 'api/ApprovalAPI/RetrieveFilteredApprovals',
            "type": "POST",
            "data": function (d) {
                d.Type = $('#type').val();
                d.Status = $('#status').val();
                d.RequestedBy = $('#requestedby').val();
                d.ApprovedBy = $('#approvedby').val();
                d.RequestedFrom = $('#requestedFrom').val();
                d.RequestedTo = $('#requestedTo').val();
            },
        },

        "columns": [
            {
                "className": 'details-control',
                "orderable": false,
                "data": null,
                "defaultContent": ''
            },
            { "data": "Type" },
            { "data": "RequestedBy" },
            { "data": "RequestedDate" },
            { "data": "ApprovedBy" },
            { "data": "ApprovedDate" },
            { "data": "Status" },
            {
                "data": "Details",
                "visible": false
            },
            {
                "data": "Id",
                "visible": false
            }
        ],

        "order": [[8, "desc"]],

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

    $("#example tfoot input").on('keyup change', function () {
        table
            .column($(this).parent().index() + ':visible')
            .search(this.value)
            .draw();
    });
}

function search() {
    try {
        getApprovals();
    } catch (err) {
        displayMessage("error", "Error encountered: " + err, "Approval Management");
    }
}

function getApprovals() {
    var table = $('#example').DataTable();
    table.ajax.reload();
};

$(document).ready(function () {
    $('#dataTables-example').DataTable({
        responsive: true
    });
});

function formatDetails(d) {
    var details = JSON.parse(d.Details);
    var table = '<table width="100%" class="cell-border" cellpadding="5" cellspacing="0" border="2" style="padding-left:50px;">';
    table += '<tr>';
    table += '<td style="color:navy;width:20%;font-family:Arial;">Request Details</td>';
    table += '<td>' + printObject(details) + '</td>';
    table += '</tr>';
    table += '</table>';
    return table;
}

function printObject(obj) {
    var result = "";
    function traverse(obj, showtitle) {
        for (var l in obj) {
            if (obj.hasOwnProperty(l)) {
                if (obj[l] instanceof Object) {
                    if (isNaN(l)) {
                        result += '<b style="color:green;">' + l + ':</b><br/>';
                    }
                    traverse(obj[l], false);
                } else {
                    if (l != 'Id' && l != 'Password' && l != 'CreatedOn' && l != 'Date' && obj[l] != null) {
                        if (showtitle === undefined) {
                            result += '<b style="color:green;">' + l + '</b>' + ': ' + obj[l] + '<br/>';
                        } else {
                            result += obj[l] + '<br/>';
                        }
                    }
                }
            }
        }
    }
    traverse(obj);
    return result;
}