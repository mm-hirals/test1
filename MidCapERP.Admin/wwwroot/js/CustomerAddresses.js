'use strict';

var CustomerAddressesModel = {};
var tblCustomerAddresses;

$(function () {
    tblCustomerAddresses = $("#tblCustomerAddresses").DataTable({
        "searching": false,
        "processing": true,
        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": "/Customer/GetCustomerAddressesData",
            "type": "POST",
            "datatype": "json",
            "data": function (d) {
                d.customerId = $('#customerId').val().trim()
            }
        },
        "columns": [
            {
                "data": "addressType", "name": "Address Type", "autoWidth": true,
                "mRender": function (o) {
                    var addressTypeName = "Other";
                    if (o == "Home") {
                        addressTypeName = "Home";
                    } else if (o == "Office") {
                        addressTypeName = "Office";
                    }
                    return addressTypeName;
                }
            },
            { "data": "street1", "name": "Street1", "autoWidth": true },
            { "data": "street2", "name": "Street2", "autoWidth": true },
            { "data": "landmark", "name": "Landmark", "autoWidth": true },
            { "data": "area", "name": "Area", "autoWidth": true },
            { "data": "city", "name": "City", "autoWidth": true },
            { "data": "state", "name": "State", "autoWidth": true },
            { "data": "zipCode", "name": "ZipCode", "autoWidth": true },
            {
                "data": "isDefault", "name": "Default", "autoWidth": true,
                "mRender": function (o) {
                    var isDefaultAddress = "No";
                    if (o) {
                        isDefaultAddress = "Yes";
                    }
                    return isDefaultAddress;
                }
            },
            {
                "mData": null, "bSortable": false,
                "mRender": function (o) {
                    return '<div class="c-action-btn-group justify-content-end"><a data-ajax-complete="CustomerAddressesModel.onComplete" data-ajax="true" class="btn btn-icon btn-outline-primary" data-ajax-mode="replace" data-ajax-update="#divUpdateCustomerAddresses" href="/Customer/UpdateCustomerAddresses/' + o.customerAddressId + '"><i class="bx bxs-pencil"></i></a>' +
                        '<a data-ajax-complete="CustomerAddressesModel.onDelete" data-ajax="true" data-ajax-confirm="Are you sure you want to delete?" class="btn btn-icon btn-outline-danger" data-ajax-mode="replace" href="/Customer/DeleteCustomerAddresses/' + o.customerAddressId + '"><i class="bx bxs-trash"></i></a></div>';
                }
            }
        ]
    });
});

$("#lnkCustemerAddressFilter").click(function () {
    $(this).toggleClass("filter-icon");
    $("#FilterCard").slideToggle("slow");
});

CustomerAddressesModel.onComplete = function () {
    $("#divCustomerAddressModal").modal('show');
}

CustomerAddressesModel.onDelete = function () {
    tblCustomerAddresses.ajax.reload(null, false);
}

CustomerAddressesModel.onSuccess = function (xhr) {
    tblCustomerAddresses.ajax.reload(null, false);
    $("#divCustomerAddressModal").modal('hide');
};

CustomerAddressesModel.onFailed = function (xhr) {
    tblCustomerAddresses.ajax.reload(null, false);
    $("#divCustomerAddressModal").modal('hide');
};