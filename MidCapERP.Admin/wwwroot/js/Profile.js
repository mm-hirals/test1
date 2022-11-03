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
                    return '<div class="c-action-btn-group justify-content-start"><a data-ajax-complete="TenantBankDetailModel.onComplete" data-ajax="true" data-ajax-mode="replace" data-ajax-update="#divUpdateTenantBankDetail"  href="/Profile/UpdateTenantBankDetail/' + o.tenantBankDetailId + '" class="btn btn-icon btn-outline-primary"><i class="bx bxs-pencil"></i></a>' + '<a data-ajax-complete="TenantBankDetailModel.onDelete" data-ajax="true" class="btn btn-icon btn-outline-danger" data-ajax-mode="replace" href="/Profile/DeleteTenantBankDetail/' + o.tenantBankDetailId + '"><i class="bx bxs-trash"></i></a></div>';
                }
            }
        ]
    });
});

TenantBankDetailModel.onComplete = function () {
    $('#btnCreateBankDetails').buttonLoader('stop');
    $("#divTenantBankDetailModal").modal('show');
}

TenantBankDetailModel.onDelete = function () {
    tblTenantBankDetail.ajax.reload(null, false);
    $("#divTenantBankDetailModal").modal('hide');
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