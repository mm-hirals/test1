'use strict';
$(document).ready(function () {
    $("#divTenantInfo").load('/Profile/GetTenantDetail' + "?Id=" + $("#hdnTenantId").val());
    $("#divTenantBankDetailInfo").load('/Profile/GetTenantBankDetail' + "?Id=" + $("#hdnTenantId").val());
});

var TenantBankDetailModel = {};
var tblTenantBankDetail;

$(function () {
    tblTenantBankDetail = $("#tblTenantBankDetail").DataTable({
        "searching": false,
        "processing": true,
        "serverSide": true,
        "iDisplayLength": 50,
        "filter": true,
        "bLengthChange": false,
        "info": false,
        "ajax": {
            "url": "/Profile/GetFilterTenantBankDetailData",
            "type": "POST",
            "datatype": "json",
            "data": function (d) {
                d.tenantId = $('#hdnTenantId').val().trim()
            }
        },
        "columns": [
            { "data": "bankName", "name": "bankName", "autoWidth": true },
            { "data": "accountName", "name": "accountName", "autoWidth": true },
            { "data": "accountNo", "name": "accountNo", "autoWidth": true },
            { "data": "branchName", "name": "branchName", "autoWidth": true },
            { "data": "accountType", "name": "accountType", "autoWidth": true },
            { "data": "ifscCode", "name": "iFSCCode", "autoWidth": true },
            { "data": "upiId", "name": "uPIId", "autoWidth": true },
            {
                "data": "qrCode",
                "name": "qRCode",
                "autoWidth": true,
                "mRender": function (qrCode) {
                    if (!qrCode) {
                        return 'N/A';
                    } else {
                        return '<img src="' + qrCode + '" height="50px" width="50px" />';
                    }
                }
            },
            {
                "mData": null, "bSortable": false,
                "mRender": function (o) {
                    return '<div class="c-action-btn-group justify-content-start"><a data-ajax-complete="TenantBankDetailModel.onComplete" data-ajax="true" data-ajax-mode="replace" data-ajax-update="#divUpdateTenantBankDetail"  href="/Profile/UpdateTenantBankDetail/' + o.tenantBankDetailId + '" class="btn btn-icon btn-outline-primary"><i class="bx bxs-pencil"></i></a>' + '<a id="' + o.tenantBankDetailId +'" class="btn btn-icon btn-outline-danger btnRemoveBankDetails" data-ajax-mode="replace"><i class="bx bxs-trash"></i></a></div>';
                }
            }
        ]
    });
});

TenantBankDetailModel.onComplete = function () {
    $('#btnCreateBankDetails').buttonLoader('stop');
    $("#divTenantBankDetailModal").modal('show');
}

TenantBankDetailModel.onSuccess = function (xhr) {
    $('#btnCreateBankDetails').buttonLoader('stop');
    tblTenantBankDetail.ajax.reload(null, false);
    $("#divTenantBankDetailModal").modal('hide');
    toastr.success('Information saved successfully.');
};

TenantBankDetailModel.onFailed = function (xhr) {
    $('#btnCreateBankDetails').buttonLoader('stop');
    tblTenantBankDetail.ajax.reload(null, false);
    $("#divTenantBankDetailModal").modal('hide');
};

$(document).on('submit', '#frmProfile', function (e) {
    $('#btnUpdateProfile').buttonLoader('start');
});

$(document).on('submit', '#frmCreateUpdateBankDetails', function (e) {
    $('#btnCreateUpdateBankDetails').buttonLoader('start');
});

$(document).delegate("#btnCreateBankDetails", "click", function () {
    $('#btnCreateBankDetails').buttonLoader('start');
});
$(document).delegate(".btnRemoveBankDetails", "click", function () {
    if (!$.isEmptyObject(this.id) && this.id > 0) {
        SweetAlert("Home", this.id, DeleteBankDetails);
    }
    else {
        errorMessage("Oops...", "Something went wrong!", "error");
    }
});

function DeleteBankDetails(id) {
    if (!$.isEmptyObject(id) && id > 0) {
        $.ajax({
            url: "/Profile/DeleteTenantBankDetail/?Id=" + id,
            type: "GET",
            success: function (response) {
                message("Deleted!", "Your record has been deleted.", "success");
                tblTenantBankDetail.ajax.reload(null, false);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                errorMessage("Oops...", "Something went wrong!", "error");
            }
        });
    } else {
        errorMessage("Oops...", "Something went wrong!", "error");
    }
}