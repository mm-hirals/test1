'use strict';
$(document).ready(function () {
    $("#divTenantInfo").load('/Profile/GetTenantDetail' + "?Id=" + $("#hdnTenantId").val());
    $("#divTenantBankDetailInfo").load('/Profile/GetTenantBankDetail' + "?Id=" + $("#hdnTenantId").val());
});


var TenantBankDetailIModel = {};
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
            "datatype": "json"
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
            //{"mRender": function (o) {
            //    return '<div class="c-action-btn-group justify-content-start"><a  href="/Profile/Update/' + o.tenantBankDetailId + '" class="btn btn-icon btn-outline-primary"><i class="bx bxs-pencil"></i></a></div>';
            //    }
            //}
        ]
    });
});


TenantBankDetailIModel.onComplete = function () {
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