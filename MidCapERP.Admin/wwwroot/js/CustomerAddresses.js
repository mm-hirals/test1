'use strict';

var CustomerAddressesModel = {};
var tblCustomerAddresses;

GetCustomerAddressData();

function GetCustomerAddressData() {
    $.ajax({
        url: "/Customer/GetCustomerAddressesData",
        type: "POST",
        dataType: "json",
        data: { customerId: $('#customerId').val().trim() },
        success: function (response) {
            
            if (response.length > 0) {
                var card = '';
                $.each(response, function (i, item) {
                    $.each(item, function () {
                        var isDefault = item.isDefault === true ? 'Default' : '';
                        var addressUrl = "/Customer/UpdateCustomerAddresses/" + item.customerAddressId;
                        card = "<div class='col-md-3 mb-2'><div class='c-card--wrapper c-card-sm--wrapper addressDetails'><div class='c-card--header'><h4 class='c-card--header-title'>" + item.addressType + "&nbsp;<span class='badge bg-primary badge-sm'>" + isDefault + "</span></h4><div class='c-card--header-action'><a data-ajax-complete='CustomerAddressesModel.onComplete' data-ajax='true' class='btn btn-icon btn-outline-primary btn-sm' data-ajax-mode='replace' data-ajax-update='#divUpdateCustomerAddresses' href='" + addressUrl + "'><i class='bx bxs-pencil'></i></a><a class='btn btn-icon btn-outline-danger btnRemoveAddress ms-2 btn-sm' id=" + item.customerAddressId + "><i class='bx bxs-trash'></i></a></div></div><div class='c-card--body'><p class='customer-address-text m-0'>" + item.street1 + ", " + item.street2 + ", " + item.area + " " + "<br />" + item.city + ", " + item.state + "-" + item.zipCode + "</p></div></div></div>"
                    });
                    $('#addressCard').append(card);
                });
            } else {
                card = "<div class='text-center customer-address-text'>No Address Details found!</div>";
                $('#addressCard').append(card);
            }
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
}

$("#lnkCustemerAddressFilter").click(function () {
    $(this).toggleClass("filter-icon");
    $("#FilterCard").slideToggle("slow");
});

CustomerAddressesModel.onComplete = function () {
    $('#customersId').buttonLoader('stop');
    $("#divCustomerAddressModal").modal('show');
}

CustomerAddressesModel.onDelete = function () {
    toastr.error('Data deleted successfully.');
}

CustomerAddressesModel.onSuccess = function (xhr) {
    $("#addressCard").html("");
    GetCustomerAddressData();
    $('.CustomerAddressDetails').buttonLoader('stop');
    $('#customersId').buttonLoader('stop');
    $("#divCustomerAddressModal").modal('hide');
    toastr.success('Information saved successfully.');
};

CustomerAddressesModel.onFailed = function (xhr) {
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
                $("#addressCard").html("");
                GetCustomerAddressData();
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                errorMessage("Oops...", "Something went wrong!", "error");
            }
        });
    } else {
        errorMessage("Oops...", "Something went wrong!", "error");
    }
}