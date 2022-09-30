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
        "filter": true,
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
            { "data": "qrCode", "name": "qRCode", "autoWidth": true },
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
    $("#divTenantBankDetailModal").modal('show');
}

TenantBankDetailModel.onDelete = function () {
    tblTenantBankDetail.ajax.reload(null, false);
     $("#divTenantBankDetailModal").modal('hide');
}

TenantBankDetailModel.onSuccess = function (xhr) {
    tblTenantBankDetail.ajax.reload(null, false);
    $("#divTenantBankDetailModal").modal('hide');
};

TenantBankDetailModel.onFailed = function (xhr) {
    tblTenantBankDetail.ajax.reload(null, false);
    $("#divTenantBankDetailModal").modal('hide');
};