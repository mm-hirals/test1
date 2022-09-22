﻿'use strict';

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
            "datatype": "json"
        },
        "columns": [
            {
                "mRender": function (o) {
                    return '<div class="c-action-btn-group justify-content-start"><input type = "checkbox"/></div>';
                }
            },
            {
                "render": (data, type, full) => {
                    return full.firstName + " " + full.lastName;
                }
            },
            { "data": "emailId", "name": "emailId", "autoWidth": true },
            { "data": "phoneNumber", "name": "phoneNumber", "autoWidth": true },
            { "data": "altPhoneNumber", "name": "altPhoneNumber", "autoWidth": true },
            { "data": "gstNo", "name": "GSTNo", "autoWidth": true },
            {
                "mData": null, "bSortable": false,
                "mRender": function (o) {
                    return '<div class="c-action-btn-group justify-content-start"><a  href="/Customer/Update/' + o.customerId + '" class="btn btn-icon btn-outline-primary"><i class="bx bxs-pencil"></i></a></div>';         
                }
            }
        ]
    });
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