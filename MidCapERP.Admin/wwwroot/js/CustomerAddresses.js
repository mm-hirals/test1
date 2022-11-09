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
                        '<a id="' + o.customerAddressId + '" class="btn btn-icon btn-outline-danger btnRemoveAddress" data-ajax-mode="replace"><i class="bx bxs-trash"></i></a></div>';
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
    $('#customersId').buttonLoader('stop');
    $("#divCustomerAddressModal").modal('show');
}

CustomerAddressesModel.onDelete = function () {
    tblCustomerAddresses.ajax.reload(null, false);
    toastr.error('Data deleted successfully.');
}

CustomerAddressesModel.onSuccess = function (xhr) {
    tblCustomerAddresses.ajax.reload(null, false);
    $('.CustomerAddressDetails').buttonLoader('stop');
    $('#customersId').buttonLoader('stop');
    $("#divCustomerAddressModal").modal('hide');
    toastr.success('Information saved successfully.');
};

CustomerAddressesModel.onFailed = function (xhr) {
    tblCustomerAddresses.ajax.reload(null, false);
    $('.CustomerAddressDetails').buttonLoader('stop');
    $('#customersId').buttonLoader('stop');
    $("#divCustomerAddressModal").modal('hide');
};

$(document).delegate("#customersId", "click", function () {
    $('#customersId').buttonLoader('start');
});

$(document).on('submit', '#frmCustomerAddress', function (e) {
    $('.CustomerAddressDetails').buttonLoader('start');
});

$(document).delegate(".btnRemoveAddress", "click", function () {
    if (!$.isEmptyObject(this.id) && this.id > 0) {
        SweetAlert("Home", this.id, DeleteAddress);
    }
    else {
        errorMessage("Oops...", "Something went wrong!", "error");
    }
});

function DeleteAddress(id) {
    if (!$.isEmptyObject(id) && id > 0) {
        $.ajax({
            url: "/Customer/DeleteCustomerAddresses/?Id=" + id,
            type: "GET",
            success: function (response) {
                message("Deleted!", "Your record has been deleted.", "success");
                tblCustomerAddresses.ajax.reload();
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                errorMessage("Oops...", "Something went wrong!", "error");
            }
        });
    } else {
        errorMessage("Oops...", "Something went wrong!", "error");
    }
}