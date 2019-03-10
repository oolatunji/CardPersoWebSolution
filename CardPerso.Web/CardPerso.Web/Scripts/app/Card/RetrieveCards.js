String.prototype.trimRight = function (charlist) {
    if (charlist === undefined)
        charlist = "\s";

    return this.replace(new RegExp("[" + charlist + "]+$"), "");
};

var p = this;
p.existingCard = {};

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
                      
            getBranches();
        }
    } catch (err) {
        displayMessage("error", "Error encountered: " + err, "Card Management");
    }
});

function getBranches() {
    $('#userBranch').html('<option>Loading Branches...</option>');
    $('#userBranch').prop('disabled', 'disabled');
    //Get Branches
    $.ajax({
        url: settingsManager.websiteURL + 'api/BranchAPI/RetrieveBranches',
        type: 'GET',
        async: true,
        cache: false,
        success: function (response) {
            $('#userBranch').html('');
            $('#userBranch').prop('disabled', false);
            $('#userBranch').append('<option value="">Select Branch</option>');
            var roles = response.data;
            var html = '';
            $.each(roles, function (key, value) {
                $('#userBranch').append('<option value="' + value.Id + '">' + value.Name + '</option>');
            });

            var user = JSON.parse(window.sessionStorage.getItem("loggedInUser"));
            if (user.UserRole.SuperAdminRole == 'No') {
                $('#userBranch').prop('disabled', 'disabled');
                $('#userBranch').val(user.UserBranch.Id);
            }

            searchCards();
        },
        error: function (xhr) {
            displayMessage("error", 'Error experienced: ' + xhr.responseText, "User Management");
        }
    });
}

function searchCards() {

    $('#searchBtn').html('<i class="fa fa-spinner fa-spin"></i> Searching...');
    $("#searchBtn").attr("disabled", "disabled");

    var searchData = {
        UserBranch: $('#userBranch').val(),
        Status: $('#status').val(),
        RequestedFrom: $('#requestedFrom').val().toDate(),
        RequestedTo: $('#requestedTo').val().toDate()
    };

    $.ajax({
        url: settingsManager.websiteURL + 'api/CardAPI/RetrieveFilteredCards',
        type: 'POST',
        data: searchData,
        processData: true,
        async: true,
        cache: false,
        success: function (response) {

            $("#searchBtn").removeAttr("disabled");
            $('#searchBtn').html('<i class="fa fa-cog"></i> Search');

            displayMessage("success", `${response.data.length} records found.`, "Card Management");
            getLatestCards(response.data);

        },
        error: function (xhr) {
            var errMessage = JSON.parse(xhr.responseText).Message;
            displayMessage("error", errMessage, "Card Management");

            $("#searchBtn").removeAttr("disabled");
            $('#searchBtn').html('<i class="fa fa-cog"></i> Search');
        }
    });

}

function getLatestCards(cards) {

    if ($.fn.DataTable.isDataTable('#example')) {

        var table = $('#example').DataTable();
        table.clear().draw();
        table.rows.add(cards);
        table.columns.adjust().draw();
        table.search("").draw();

    } else {

        var table = $('#example').DataTable({
            "processing": false,
            "data": cards,
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
                { "data": "Name" },
                { "data": "Pan" },
                { "data": "CardStatus" },
                { "data": "Username" },
                { "data": "PrintedName" },
                { "data": "DisplayDate" },
                {
                    "data": "Track1Data",
                    "visible": false
                },
                {
                    "data": "Track2Data",
                    "visible": false
                },
                {
                    "data": "PrintStatus",
                    "visible": false
                },
                {
                    "data": "ID1",
                    "visible": false
                }
            ],
            "order": [[11, "asc"]],
            "sDom": 'T<"clear">lrtip',
            "oTableTools": {
                "sSwfPath": settingsManager.websiteURL + "images/copy_csv_xls_pdf.swf",
                "aButtons": [
                    {
                        "sExtends": "copy",
                        "sButtonText": "Copy to Clipboard",
                        "oSelectorOpts": { filter: 'applied', order: 'current' },
                        "mColumns": [2, 3, 4, 5, 6, 7, 8, 9]
                    },
                    {
                        "sExtends": "csv",
                        "sButtonText": "Save to CSV",
                        "oSelectorOpts": { filter: 'applied', order: 'current' },
                        "mColumns": [2, 3, 4, 5, 6, 7, 8, 9]
                    },
                    {
                        "sExtends": "xls",
                        "sButtonText": "Save for Excel",
                        "oSelectorOpts": { filter: 'applied', order: 'current' },
                        "mColumns": [2, 3, 4, 5, 6, 7, 8, 9]
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
};

$(document).ready(function () {
    $('#dataTables-example').DataTable({
        responsive: true
    });
});

function format(d, roles) {
    
    p.existingCard = d;

    var table = '<table width="100%" class="cell-border" cellpadding="5" cellspacing="0" border="2" style="padding-left:50px;">';
    table += '<tr>';
    table += '<td style="color:navy;width:20%;font-family:Arial;">Track 1 Data</td>';
    table += '<td>' + d.Track1Data + '</td>';
    table += '</tr>';
    table += '<tr>';
    table += '<td style="color:navy;width:20%;font-family:Arial;">Track 2 Data</td>';
    table += '<td>' + d.Track2Data + '</td>';
    table += '</tr>';
    table += '<tr>';
    table += '<td style="color:navy;width:20%;font-family:Arial;">Print Status:</td>';
    table += '<td><select class="form-control" name="printstatus" id="printstatus">';
    table += '<option value="">Select Print Status</option>';
    if (d.PrintStatus === 1) {
        table += '<option selected="selected" value="1">Not Printed</option>';
        table += '<option value="3">Printed</option>';
    }
    else if (d.PrintStatus === 3) {
        table += '<option value="1">Not Printed</option>';
        table += '<option selected="selected" value="3">Printed</option>';
    }
    table += '</select></td></tr>';
    table += '<tr>';
    table += '<td style="display:none">ID:</td>';
    table += '<td style="display:none"><input class="form-control" id="pan" value="' + d.Pan + '"/></td>';
    table += '</tr>';
    table += '<tr>';
    table += '<td style="display:none">ID:</td>';
    table += '<td style="display:none"><input class="form-control" id="printedname" value="' + d.PrintedName + '"/></td>';
    table += '</tr>';
    table += '<tr>';
    table += '<td style="display:none">ID:</td>';
    table += '<td style="display:none"><input class="form-control" id="id" value="' + d.ID1 + '"/></td>';
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
    table += '<td style="color:navy;width:20%;font-family:Arial;">Track 1 Data</td>';
    table += '<td>' + d.Track1Data + '</td>';
    table += '</tr>';
    table += '<tr>';
    table += '<td style="color:navy;width:20%;font-family:Arial;">Track 2 Data</td>';
    table += '<td>' + d.Track2Data + '</td>';
    table += '</tr>';
    table += '</table>';
    return table;
}

function update() {
    try {
        $('#updateBtn').html('<i class="fa fa-spinner fa-spin"></i> Updating...');
        $("#updateBtn").attr("disabled", "disabled");

        var loggedInUsername = JSON.parse(window.sessionStorage.getItem("loggedInUser")).Username;

        var oldData = {
            PrintStatus: p.existingCard.PrintStatus,            
            LoggedInUser: loggedInUsername,
            Status: p.existingCard.CardStatus,
            Pan: p.existingCard.Pan,
            PrintedName: p.existingCard.PrintedName,
            ID1: p.existingCard.ID1
        };

        var printstatus = $('#printstatus').val();
        var pan = $('#pan').val();
        var printedname = $('#printedname').val();
        var printstatusname = $('#printstatus option:selected').html();
        var username = $('#username').val();
        var id = $('#id').val();        

        var data = {
            PrintStatus: printstatus,
            Username: username,
            LoggedInUser: loggedInUsername,
            Status: printstatusname,
            Pan: pan,
            PrintedName: printedname,
            ID1: id,
            OldData: JSON.stringify(oldData)
        };       

        $.ajax({
            url: settingsManager.websiteURL + 'api/CardAPI/UpdateCard',
            type: 'PUT',
            data: data,
            processData: true,
            async: true,
            cache: false,
            success: function (response) {
                if (!_.isEmpty(response.SuccessMsg)) {
                    displayMessage("success", response.SuccessMsg, "Card Management");
                    searchCards();
                } else if (!_.isEmpty(response.ErrorMsg)) {
                    displayMessage("error", 'Error experienced: ' + response.ErrorMsg, "Card Management");
                }
                $("#updateBtn").removeAttr("disabled");
                $('#updateBtn').html('<i class="fa fa-cog"></i> Update');
            }
        });
    } catch (err) {
        displayMessage("error", "Error encountered: " + err, "Card Management");
        $("#updateBtn").removeAttr("disabled");
        $('#updateBtn').html('<i class="fa fa-cog"></i> Update');
    }
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