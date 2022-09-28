'use strict';

var CustomerModel = {};
var tblCustomer;
var value_check = new Array();

$(function () {
    tblCustomer = $("#tblCustomer").DataTable({
        "searching": false,
        "processing": true,
        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": "/Customer/GetCustomersData",
            "type": "POST",
            "datatype": "json",
            "data": function (d) {
                d.customerName = $("#customerName").val().trim(),
                    d.customerMobileNo = $("#customerMobileNo").val().trim(),
                    d.customerFromDate = $("#customerFromDate").val().trim(),
                    d.customerToDate = $("#customerToDate").val().trim()
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
                    return '<div class="c-action-btn-group justify-content-start"><a  href="/Customer/Update/' + row.customerId + '" class="btn btn-icon btn-outline-primary"><i class="bx bxs-pencil"></i></a></div>';
                }
            }
        ]
    });
});

$('#customerName').keyup(function () {
    tblCustomer.ajax.reload(null, false);
});

$('#customerMobileNo').keyup(function () {
    tblCustomer.ajax.reload(null, false);
});

$("#customerFromDate").on("input", function () {
    tblCustomer.ajax.reload(null, false);
});

$("#customerToDate").on("input", function () {
    tblCustomer.ajax.reload(null, false);
});

$("#lnkCustomerFilter").click(function () {
    $(this).toggleClass("filter-icon");
    $("#FilterCard").slideToggle("slow");
});

CustomerModel.onSuccess = function (xhr) {
    tblCustomer.ajax.reload(null, false);
    $("#divCustomerModal").modal('hide');
};

CustomerModel.onFailed = function (xhr) {
    tblCustomer.ajax.reload(null, false);
    $("#divCustomerModal").modal('hide');
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
$('#tblCustomer').on('click', 'input[type="checkbox"]', function () {
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
$("#multiSelectCustomer").click(function () {
    $.ajax({
        url: "/Customer/MultipleSendCustomer",
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