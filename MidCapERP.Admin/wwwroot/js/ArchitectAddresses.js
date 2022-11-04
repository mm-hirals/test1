'use strict';

var ArchitectAddressesModel = {};
var tblArchitectAddresses;

$(function () {
    tblArchitectAddresses = $("#tblArchitectAddresses").DataTable({
        "searching": false,
        "processing": true,
        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": "/Architect/GetArchitectAddressesData",
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
                    return '<div class="c-action-btn-group justify-content-end"><a data-ajax-complete="ArchitectAddressesModel.onComplete" data-ajax="true" class="btn btn-icon btn-outline-primary" data-ajax-mode="replace" data-ajax-update="#divUpdateArchitectAddresses" href="/Architect/UpdateArchitectAddresses/' + o.customerAddressId + '"><i class="bx bxs-pencil"></i></a>' +
                        '<a id="' + o.customerAddressId + '" class="btn btn-icon btn-outline-danger btnRemoveAddress" data-ajax-mode="replace"><i class="bx bxs-trash"></i></a></div>';
                }
            }
        ]
    });
});

$("#lnkArchitectAddressFilter").click(function () {
    $(this).toggleClass("filter-icon");
    $("#FilterCard").slideToggle("slow");
});

ArchitectAddressesModel.onComplete = function () {
    $('#customersId').buttonLoader('stop');
    $("#divArchitectAddressModal").modal('show');
}


ArchitectAddressesModel.onSuccess = function (xhr) {
    $('.btn-architectAddress').buttonLoader('stop');
    $('#customersId').buttonLoader('stop');
    tblArchitectAddresses.ajax.reload(null, false);
    $("#divArchitectAddressModal").modal('hide');
    toastr.success('Information saved successfully.');
};

ArchitectAddressesModel.onFailed = function (xhr) {
    $('.btn-architectAddress').buttonLoader('stop');
    $('#customersId').buttonLoader('stop');
    tblArchitectAddresses.ajax.reload(null, false);
    $("#divArchitectAddressModal").modal('hide');
};

$(document).on('submit', '#frmArchitectAddress', function (e) {
    $('.btn-architectAddress').buttonLoader('start');
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
            url: "/Architect/DeleteArchitectAddresses/?Id=" + id,
            type: "GET",
            success: function (response) {
                debugger;
                message("Deleted!", "Your record has been deleted.", "success");
                tblArchitectAddresses.ajax.reload();
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                errorMessage("Oops...", "Something went wrong!", "error");
            }
        });
    } else {
        errorMessage("Oops...", "Something went wrong!", "error");
    }
}