'use strict';

var CustomerModel = {};
var tblCustomer;

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
        "columns": [
            {
                "render": (data, type, full) => {
                    return full.firstName + " " + full.lastName;
                }
            },
            { "data": "emailId", "name": "emailId", "autoWidth": true },
            { "data": "phoneNumber", "name": "phoneNumber", "autoWidth": true },
            {
                "mData": null, "bSortable": false,
                "mRender": function (o) {
                    return '<div class="c-action-btn-group justify-content-start"><a  href="/Customer/Update/' + o.customerId + '" class="btn btn-icon btn-outline-primary"><i class="bx bxs-pencil"></i></a></div>';         
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