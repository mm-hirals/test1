'use strict';
window.counter = 0;
var ProductModel = {};

$(document).ready(function () {
    $("#divProductInfo").load('/Product/CreateProductBasicDetail' + "?ProductId=" + $("#hdnProductId").val());

    if ($("#hdnProductId").val() > 0) {
        $("#divProductDetailPartial").load('/Product/CreateProductDetail' + "?ProductId=" + $("#hdnProductId").val());
        $("#divProductImagePartial").load('/Product/CreateProductImage' + "?ProductId=" + $("#hdnProductId").val());
        $("#divProductMaterialPartial").load('/Product/CreateProductMaterial' + "?ProductId=" + $("#hdnProductId").val());
        $("#divProductActivityPartial").load('/Product/GetProductActivity' + "?ProductId=" + $("#hdnProductId").val());
        //$("#divProductWorkflowPartial").load('/Product/CreateProductWorkFlow' + "?ProductId=" + document.getElementById("hdnProductId").value);
    }
});

$(document).on("#lnkProductFilter", "click", (function () {
    $(this).toggleClass("filter-icon");
    $("#FilterCard").slideToggle("slow");
}));

$(document).on("click", ".add-icon", (function () {
    if ($(this).parent().parent().find("select").val() != "") {
        var htmlStringToAppend = $(this).parent().parent()[0].outerHTML.replaceAll("{ID}", counter)
        htmlStringToAppend = htmlStringToAppend.replaceAll("add-icon", "minus-icon")
        htmlStringToAppend = htmlStringToAppend.replaceAll("bx-plus", "bx-minus")
        htmlStringToAppend = htmlStringToAppend.replaceAll("data-", "")

        $(this).parent().parent().parent().append(htmlStringToAppend)
        counter++;
        emptyFields($(this).parent().parent())
        calculateCostPrice();
    } else {
        alert("Please select value");
        //$("span.materialErrorMsg").text("Please select value");
    }
}));

function calculateCostPrice() {
    var sum = 0;
    $(".costPrice").each(function () {
        if ($(this).parent().parent().find("input.isDeleted").val() == 'false')
            sum += +this.value;
    });
    sum = RoundTo(Math.round(sum).toFixed(2));
    $("#CostPrice").val(sum);
    $("#ProductRequestDto_CostPrice-error").hide();
    calculateRetailerSP(sum);
    calculateWholesalerSP(sum);
}

function calculateRetailerSP(costPrice) {
    var retailerSP = $("#hdnRetailerSP").val();
    if (retailerSP > 0) {
        $("#RetailerPrice").val(RoundTo(Math.round((costPrice + (costPrice * retailerSP) / 100))));
    }
}

function calculateWholesalerSP(costPrice) {
    var retailerSP = $("#hdnWholesalerSP").val();
    if (retailerSP > 0) {
        $("#WholesalerPrice").val(RoundTo(Math.round((costPrice + (costPrice * retailerSP) / 100))));
    }
}

function RoundTo(number, roundto) {
    roundto = 50;
    return (roundto * Math.round(number / roundto)).toFixed(2);
}

function emptyFields(trRow) {
    trRow.find("input[type=text]").each(function () {
        $(this).val("")
    });
    trRow.find("select").each(function () {
        $(this).val("")
    });
}
$(document).on("change", "input.costPrice", (function () {
    $(this).attr('value', $(this).val());
}));
$(document).on("change", "input.quantity", (function () {
    var qty = $(this).attr('value', $(this).val());
    var unitPrice = $(this).parent().parent().find("input.materialPrice").val();
    var costPrice = qty.val() * unitPrice;
    $(this).parent().parent().find("input.quantity").attr('value', qty.val());
    $(this).parent().parent().find("input.costPrice").val(costPrice.toFixed(2));
    $(this).parent().parent().find("input.costPrice").attr('value', costPrice.toFixed(2));
}));
$(document).on("change", "select.material", (function () {
    var val = $(this).val();
    $("option", this).removeAttr("selected").filter(function () {
        return $(this).attr("value") == val;
    }).first().attr("selected", "selected");

    var unitPrice = $(this).find(':selected').attr('data-unitprice');
    var unit = $(this).find(':selected').attr('data-unitname');

    $(this).parent().parent().find("input.materialPrice").val(unitPrice);
    $(this).parent().parent().find("input.materialPrice").attr('value', unitPrice);
    $(this).parent().parent().find("input.quantity").val(1);
    $(this).parent().parent().find("input.quantity").attr('value', 1);
    $(this).parent().parent().find("input.costPrice").val(unitPrice);
    $(this).parent().parent().find("input.costPrice").attr('value', unitPrice);
    $(this).parent().parent().find("input.Unit").val(unit);
    $(this).parent().parent().find("input.Unit").attr('value', unit);

    $("span.materialErrorMsg").hide();
}));

$(document).on("click", ".minus-icon", function () {
    var htmlStringToAppend = $(this).parent().parent();
    htmlStringToAppend.find("[name$='IsDeleted']").val("true");
    htmlStringToAppend.hide();

    calculateCostPrice();
});

ProductModel.onSuccess = function (xhr) {
};

ProductModel.onFailed = function (xhr) {
};

ProductModel.onProductMaterialSuccess = function (xhr) {
};

ProductModel.onProductMaterialFailed = function (xhr) {
};