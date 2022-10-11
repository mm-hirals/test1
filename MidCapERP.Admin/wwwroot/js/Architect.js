'use strict';

var ArchitectModel = {};
var tblArchitect;
var value_check = new Array();

$(function () {
    tblArchitect = $("#tblArchitect").DataTable({
        "searching": false,
        "processing": true,
        "serverSide": true,
        "filter": true,
        "iDisplayLength": 50,
        "ajax": {
            "url": "/Architect/GetArchitectsData",
            "type": "POST",
            "datatype": "json",
            "data": function (d) {
                d.customerName = $("#architectName").val().trim(),
                    d.customerMobileNo = $("#architectMobileNo").val().trim(),
                    d.customerFromDate = $("#architectFromDate").val().trim(),
                    d.customerToDate = $("#architectToDate").val().trim()
            }
        },
        "columnDefs": [
            {
                "orderable": false,
                "targets": 0
            }
        ],
        "columns": [
            {
                "bSortable": false,
                "mRender": (data, type, row) => {
                    return '<div class="c-action-btn-group justify-content-start"><input type="checkbox" class="case" value="' + row.customerId + '" id="' + row.customerId + '" /></div>';
                }
            },
            {
                "render": (data, type, row) => {
                    return row.firstName + " " + row.lastName;
                }
            },
            { "data": "emailId", "name": "emailId", "autoWidth": true },
            { "data": "phoneNumber", "name": "phoneNumber", "autoWidth": true },
            {
                "bSortable": false,
                "mRender": (data, type, row) => {
                    return '<div class="c-action-btn-group justify-content-start"><a  href="/Architect/Update/' + row.customerId + '" class="btn btn-icon btn-outline-primary"><i class="bx bxs-pencil"></i></a></div>';
                }
            }
        ]
    });
});

$('#architectName').keyup(function () {
    tblArchitect.ajax.reload(null, false);
});

$('#architectMobileNo').keyup(function () {
    tblArchitect.ajax.reload(null, false);
});

$("#architectFromDate").on("input", function () {
    tblArchitect.ajax.reload(null, false);
});

$("#architectToDate").on("input", function () {
    tblArchitect.ajax.reload(null, false);
});

$("#lnkArchitectFilter").click(function () {
    $(this).toggleClass("filter-icon");
    $("#FilterCard").slideToggle("slow");
});

ArchitectModel.onSuccess = function (xhr) {
    tblArchitect.ajax.reload(null, false);
    $("#divArchitectModal").modal('hide');
};

ArchitectModel.onFailed = function (xhr) {
    tblArchitect.ajax.reload(null, false);
    $("#divArchitectModal").modal('hide');
};

// Check all checkbox values and store it in array
$("#selectall").click(function () {
    if (this.checked) {
        $('.case').prop('checked', true);
        for (var i = 0; i < $(".case:checked").length; i++) {
            value_check.push($(".case:checked")[i].id);
        }
    }
    else {
        $('.case').prop('checked', false);
        value_check = [];
    }
});

// Check single checkbox value and store it in array
$('#tblArchitect').on('click', 'input[type="checkbox"]', function () {
    if ($(this).prop("checked")) {
        value_check.push($(this).val());
    }
    else {
        var a = value_check.findIndex(q => q == $(this).val());
        if (a > -1) {
            value_check.splice(a, 1);
        }
    }
});

// On button click send checkbox values to controller
$("#multiSelectArchitect").click(function () {
    $.ajax({
        url: "/Architect/MultipleSendArchitect",
        type: "POST",
        data: { 'value_check': value_check },
        success: function (response) {
            if (response == "success")
                alert("Success : ", response);
            else
                alert("Error : ", response)
        }
    });
});