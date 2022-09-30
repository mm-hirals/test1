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
                    return '<div class="c-action-btn-group justify-content-start"><a data-ajax-complete="ArchitectAddressesModel.onComplete" data-ajax="true" class="btn btn-icon btn-outline-primary" data-ajax-mode="replace" data-ajax-update="#divUpdateArchitectAddresses" href="/Architect/UpdateArchitectAddresses/' + o.customerAddressId + '"><i class="bx bxs-pencil"></i></a>' +
                        '<a data-ajax-complete="ArchitectAddressesModel.onDelete" data-ajax="true" class="btn btn-icon btn-outline-danger" data-ajax-mode="replace" href="/Architect/DeleteArchitectAddresses/' + o.customerAddressId + '"><i class="bx bxs-trash"></i></a></div>';
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
    $("#divArchitectAddressModal").modal('show');
}

ArchitectAddressesModel.onDelete = function () {
    tblArchitectAddresses.ajax.reload(null, false);
}

ArchitectAddressesModel.onSuccess = function (xhr) {
    tblArchitectAddresses.ajax.reload(null, false);
    $("#divArchitectAddressModal").modal('hide');
};

ArchitectAddressesModel.onFailed = function (xhr) {
    tblArchitectAddresses.ajax.reload(null, false);
    $("#divArchitectAddressModal").modal('hide');
};