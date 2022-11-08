'use strict';

var InteriorAddressesModel = {};
var tblInteriorAddresses;

$(function () {
    tblInteriorAddresses = $("#tblInteriorAddresses").DataTable({
        "searching": false,
        "processing": true,
        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": "/Interior/GetInteriorAddressesData",
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
                    return '<div class="c-action-btn-group justify-content-end"><a data-ajax-complete="InteriorAddressesModel.onComplete" data-ajax="true" class="btn btn-icon btn-outline-primary" data-ajax-mode="replace" data-ajax-update="#divUpdateInteriorAddresses" href="/Interior/UpdateInteriorAddresses/' + o.customerAddressId + '"><i class="bx bxs-pencil"></i></a>' +
                        '<a id="' + o.customerAddressId + '" class="btn btn-icon btn-outline-danger btnRemoveAddress" data-ajax-mode="replace"><i class="bx bxs-trash"></i></a></div>';
                }
            }
        ]
    });
});

$("#lnkInteriorAddressFilter").click(function () {
    $(this).toggleClass("filter-icon");
    $("#FilterCard").slideToggle("slow");
});

InteriorAddressesModel.onComplete = function () {
    $('#customersId').buttonLoader('stop');
    $("#divInteriorAddressModal").modal('show');
}

InteriorAddressesModel.onSuccess = function (xhr) {
    $('.btn-interiorAddress').buttonLoader('stop');
    $('#customersId').buttonLoader('stop');
    tblInteriorAddresses.ajax.reload(null, false);
    $("#divInteriorAddressModal").modal('hide');
    toastr.success('Information saved successfully.');
};

InteriorAddressesModel.onFailed = function (xhr) {
    $('.btn-interiorAddress').buttonLoader('stop');
    $('#customersId').buttonLoader('stop');
    tblInteriorAddresses.ajax.reload(null, false);
    $("#divInteriorAddressModal").modal('hide');
};

$(document).on('submit', '#frmInteriorAddress', function (e) {
    $('.btn-interiorAddress').buttonLoader('start');
});

$(document).delegate("#customersId", "click", function () {
    $('#customersId').buttonLoader('start');
});

function restrictNumber(e) {
    return (e.charCode > 64 && e.charCode < 91) || (e.charCode > 96 && e.charCode < 123) || e.charCode == 32;
}

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
            url: "/Interior/DeleteInteriorAddresses/?Id=" + id,
            type: "GET",
            success: function (response) {
                message("Deleted!", "Your record has been deleted.", "success");
                tblInteriorAddresses.ajax.reload();
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                errorMessage("Oops...", "Something went wrong!", "error");
            }
        });
    } else {
        errorMessage("Oops...", "Something went wrong!", "error");
    }
}