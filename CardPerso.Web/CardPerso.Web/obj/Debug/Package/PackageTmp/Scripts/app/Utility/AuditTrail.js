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

            var today = new Date();
            var date = (today.getMonth() + 1) + '/' + today.getDate() + '/' + today.getFullYear();
            $('#requestedFrom').val(date);
            getPendingAudits();
        }
    } catch (err) {
        displayMessage("error", "Error encountered: " + err, "Audit Trail Management");
    }
});

String.prototype.trimRight = function (charlist) {
    if (charlist === undefined)
        charlist = "\s";

    return this.replace(new RegExp("[" + charlist + "]+$"), "");
};

function search() {
    try {
        getAudits();
    } catch (err) {
        displayMessage("error", "Error encountered: " + err, "Audit Trail Management");
    }
}

function getAudits() {
    var table = $('#example').DataTable();
    table.ajax.reload();
}

function getPendingAudits() {

    $('#example tfoot th').each(function () {
        var title = $('#example tfoot th').eq($(this).index()).text();
        if (title != "")
            $(this).html('<input type="text" placeholder="Search ' + title + '" />');
    });

    var table = $('#example').DataTable({

        "processing": true,

        "ajax": {
            "url": settingsManager.websiteURL + 'api/SystemAPI/RetrieveFilteredAudits',
            "type": "POST",
            "data": function (d) {
                d.Type = $('#type').val();
                d.RequestedBy = $('#requestedby').val();
                d.ApprovedBy = $('#approvedby').val();
                d.RequestedFrom = $('#requestedFrom').val().toDate();
                d.RequestedTo = $('#requestedTo').val().toDate();
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
            { "data": "ClientIP" },
            { "data": "RequestedDate" },
            { "data": "ApprovedBy" },
            { "data": "ApprovedDate" },
            {
                "data": "Details",
                "visible": false
            }
        ],

        "order": [[3, "desc"]],

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

String.prototype.toDate = function () {

    var date = this;
    if (date.length != 0) {

        var formatedDate = null;
        var formatLowerCase = 'mm/dd/yyyy';
        var formatItems = formatLowerCase.split('/');
        var dateItems = date.split('/');
        var monthIndex = formatItems.indexOf("mm");
        var monthNameIndex = formatItems.indexOf("mmm");
        var dayIndex = formatItems.indexOf("dd");
        var yearIndex = formatItems.indexOf("yyyy");
        var d = dateItems[dayIndex];

        if (d < 10) {
            d = "0" + d;
        }

        if (monthIndex > -1) {
            var month = parseInt(dateItems[monthIndex]);
            if (month < 10) {
                month = "0" + month;
            }
        } else if (monthNameIndex > -1) {
            var monthName = dateItems[monthNameIndex];
            month = getMonthIndex(monthName);
            if (month < 10) {
                month = "0" + month;
            }
        }

        return {
            Day: parseInt(d),
            Month: parseInt(month),
            Year: parseInt(dateItems[yearIndex])
        };

    } else {

        var d = new Date();
        return {
            Day: d.getDate(),
            Month: d.getMonth() + 1,
            Year: d.getFullYear()
        };

    }
};