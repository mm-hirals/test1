$(document).ready(function () {
    $("#divOrderInfo").load('/Order/GetOrderBasicDetail' + "?OrderId=" + $("#hdnOrderId").val());
    $("#divOrderSetDetailPartial").load('/Order/GetOrderSetDetailData' + "?OrderId=" + $("#hdnOrderId").val());

    //if ($("#hdnOrderId").val() > 0) {
    //    $("#divOrderSetDetailPartial").load('/Order/CreateProductDetail' + "?ProductId=" + $("#hdnProductId").val());
    //    $("#divProductImagePartial").load('/Product/CreateProductImage' + "?ProductId=" + $("#hdnProductId").val());
    //    $("#divProductMaterialPartial").load('/Product/CreateProductMaterial' + "?ProductId=" + $("#hdnProductId").val());
    //    $("#divProductActivityPartial").load('/Product/GetProductActivity' + "?ProductId=" + $("#hdnProductId").val());
    //    $("#divProductWorkflowPartial").load('/Product/CreateProductWorkFlow' + "?ProductId=" + document.getElementById("hdnProductId").value);
    //}
});

$(document).on("shown.bs.tab", 'button[data-bs-toggle="tab"]', function (e) {
    var tabId = $(e.target).attr("id")
    if (tabId == "nav-images-tab") {
        $("#divOrderImagePartial").load();
        //$("#divOrderImagePartial").load('/Order/CreateProductImage' + "?ProductId=" + $("#hdnProductId").val());
    } else if (tabId == "nav-activity-tab") {
        $("#divOrderActivityPartial").load();
        //$("#divOrderActivityPartial").load('/Order/CreateProductMaterial' + "?ProductId=" + $("#hdnProductId").val());
    }
});

$(document).delegate(".saveDiscount", "click", function (e) {
    alert($(e.target).attr("id"));
    //$.ajax({
    //    url: "/Order/SaveDiscount?ProductImageId=" + id,
    //    type: "POST",
    //    success: function (response) {
    //    }
    //});
});