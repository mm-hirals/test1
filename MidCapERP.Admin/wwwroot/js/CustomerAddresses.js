'use strict';

var CustomerAddresses = {};
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
            "datatype": "json"
        },
        "columns": [
            { "data": "addressTypeId", "name": "AddressTypeId", "autoWidth": true },
            { "data": "street1", "name": "Street1", "autoWidth": true },
            { "data": "street2", "name": "Street2", "autoWidth": true },
            { "data": "landmark", "name": "Landmark", "autoWidth": true },
            { "data": "area", "name": "Area", "autoWidth": true },
            { "data": "city", "name": "City", "autoWidth": true },
            { "data": "state", "name": "State", "autoWidth": true },
            { "data": "zipCode", "name": "ZipCode", "autoWidth": true },
            { "data": "isDefault", "name": "IsDefault", "autoWidth": true },
        ]
    });
});
