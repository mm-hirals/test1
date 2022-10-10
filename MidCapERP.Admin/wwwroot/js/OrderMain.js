//$(document).ready(function () {
//    $("#divOrderInfo").load('/Order/CreateProductBasicDetail' + "?ProductId=" + $("#hdnProductId").val());
//    $("#divOrderSetDetailPartial").load();

//    //if ($("#hdnOrderId").val() > 0) {
//    //    $("#divOrderSetDetailPartial").load('/Order/CreateProductDetail' + "?ProductId=" + $("#hdnProductId").val());
//    //    $("#divProductImagePartial").load('/Product/CreateProductImage' + "?ProductId=" + $("#hdnProductId").val());
//    //    $("#divProductMaterialPartial").load('/Product/CreateProductMaterial' + "?ProductId=" + $("#hdnProductId").val());
//    //    $("#divProductActivityPartial").load('/Product/GetProductActivity' + "?ProductId=" + $("#hdnProductId").val());
//    //    $("#divProductWorkflowPartial").load('/Product/CreateProductWorkFlow' + "?ProductId=" + document.getElementById("hdnProductId").value);
//    //}
//});

//$(document).on("shown.bs.tab", 'button[data-bs-toggle="tab"]', function (e) {
//    var tabId = $(e.target).attr("id")
//    if (tabId == "nav-images-tab") {
//        $("#divOrderImagePartial").load();
//        //$("#divOrderImagePartial").load('/Order/CreateProductImage' + "?ProductId=" + $("#hdnProductId").val());
//    } else if (tabId == "nav-rowmaterial-tab") {
//        $("#divOrderActivityPartial").load();
//        //$("#divOrderActivityPartial").load('/Order/CreateProductMaterial' + "?ProductId=" + $("#hdnProductId").val());
//    }
//});