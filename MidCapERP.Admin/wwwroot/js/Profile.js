$(document).ready(function () {
    $("#divTenantInfo").load('/Tenant/GetTenantDetail' + "?Id=" + $("#hdnTenantId").val());
    $("#divTenantBankDetailInfo").load('/Tenant/GetTenantBankDetail' + "?Id=" + $("#hdnTenantId").val());
});