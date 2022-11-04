﻿'use strict';

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
                d.customerName = $("#architectName").val().trim();
                d.customerMobileNo = $("#architectMobileNo").val().trim();
                d.customerFromDate = $("#architectFromDate").val().trim();
                d.customerToDate = $("#architectToDate").val().trim();
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
                    return '<div class="c-action-btn-group justify-content-end"><a  href="/Architect/Update/' + row.customerId + '" class="btn btn-icon btn-outline-primary"><i class="bx bxs-pencil"></i></a></div>';
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
        if (value_check.length > 0) {
            value_check = [];
        }
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

// On button click open modal popup
$("#multiSelectArchitect").click(function () {
    if (value_check.length > 0) {
        $("#sendSMSModal").modal('show');
    }
    else {
        //Swal.fire('Please select architect to send message.')
        toastr.error('Please select architect to send message.');
    }
});

// On button click send checkbox values to controller
$(".sendSMSToClient").click(function () {
    if ($("#txtMessage").val() == '') {
        $("#errorMessage").text("Please enter message.");
    }
    else if ($("#txtSubject").val() == '') {
        $("#errorSubject").text("Please enter subject.");
    }
    else {
        $('.sendSMSToClient').buttonLoader('start');
        $("#errorMessage").hide();

        if ($("#selectall").prop('checked')) {
            var architectName = $("#architectName").val().trim();
            var architectMobileNo = $("#architectMobileNo").val().trim();
            var architectFromDate = $("#architectFromDate").val().trim();
            var architectToDate = $("#architectToDate").val().trim();
        }

        var data = {
            CustomerName: architectName,
            CustomerMobileNo: architectMobileNo,
            CustomerFromDate: architectFromDate,
            CustomerToDate: architectToDate,
            IsCheckedAll: $("#selectall").prop('checked'),
            CustomerList: value_check,
            Message: $("#txtMessage").val(),
            Subject: $("#txtSubject").val()
        };
        console.log(data);
        $.ajax({
            url: "/Architect/MultipleSendArchitect",
            type: "POST",
            data: { model: data },
            success: function (response) {
                $('.sendSMSToClient').buttonLoader('stop');
                if (response == "success") {
                    toastr.success('Messages sent successfully.');
                    $('#sendSMSModal').modal('hide');
                }
                else
                    toastr.error(response, 'Error in sending messages.');
            }
        });
    }
});

// Reset modal values after close
$('#sendSMSModal').on('hidden.bs.modal', function () {
    $(this).find('form').trigger('reset');
})

$(document).on('submit', '#frmArchitectEdit', function (e) {
    $('#dataSave').buttonLoader('start');
    toastr.success('Information saved successfully.');
});