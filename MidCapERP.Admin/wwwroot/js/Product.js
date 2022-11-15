﻿'use strict';
window.counter = 0;
var ProductModel = {};
var formChangedValue = false;
var productTabId = "nav-detail-tab";
$(document).ready(function () {
    $("#divProductInfo").load('/Product/CreateProductBasicDetail' + "?ProductId=" + $("#hdnProductId").val());
    $("#divProductDetailPartial").load('/Product/CreateProductDetail' + "?ProductId=" + $("#hdnProductId").val());
    //if ($("#hdnProductId").val() > 0) {
    //    $("#divProductDetailPartial").load('/Product/CreateProductDetail' + "?ProductId=" + $("#hdnProductId").val());
    //    $("#divProductImagePartial").load('/Product/CreateProductImage' + "?ProductId=" + $("#hdnProductId").val());
    //    $("#divProductMaterialPartial").load('/Product/CreateProductMaterial' + "?ProductId=" + $("#hdnProductId").val());
    //    $("#divProductActivityPartial").load('/Product/GetProductActivity' + "?ProductId=" + $("#hdnProductId").val());
    //    //$("#divProductWorkflowPartial").load('/Product/CreateProductWorkFlow' + "?ProductId=" + document.getElementById("hdnProductId").value);
    //}
});


$(document).on("shown.bs.tab", 'button[data-bs-toggle="tab"]', function (e) {
    //debugger;
    console.log(productTabId);
    var tabId = $(e.target).attr("id")
    if (tabId == "nav-images-tab" ) {
        if (formChangedValue && confirm('Do you want to save?')) {
            if (productTabId == "nav-rawmaterial-tab") {
                getFormName(productTabId);
                }
            else if (productTabId == "nav-detail-tab") {
                getFormName(productTabId);
            }
        }
        $("#divProductImagePartial").load('/Product/CreateProductImage' + "?ProductId=" + $("#hdnProductId").val());
    } else if (tabId == "nav-rawmaterial-tab") {
        if (formChangedValue && confirm('Do you want to save?')) {

            if (productTabId == "nav-images-tab") {
                    //$("#frmProductMaterial").submit();
            }
            else if (productTabId == "nav-detail-tab") {
                getFormName(productTabId);
            }
        }
        $("#divProductMaterialPartial").load('/Product/CreateProductMaterial' + "?ProductId=" + $("#hdnProductId").val());
    } else if (tabId == "nav-detail-tab") {
        if (formChangedValue && confirm('Do you want to save?')) {
            if (productTabId == "nav-rawmaterial-tab")  {
                getFormName(productTabId);
            }
            else if (productTabId == "nav-images-tab") {
                    getFormName(productTabId);
            }
        } 
        $("#divProductMaterialPartial").load('/Product/CreateProductMaterial' + "?ProductId=" + $("#hdnProductId").val());
    } else if (tabId == "nav-productActivity-tab") {
        if (formChangedValue && confirm('Do you want to save?')) {

            if (productTabId == "nav-rawmaterial-tab") {
                getFormName(productTabId);
            }
            else if (productTabId == "nav-detail-tab") {
                getFormName(productTabId);
            }
            else if (productTabId == "nav-images-tab") {
                    $("#frmProductMaterial").submit();
            }
        }
       $("#divProductActivityPartial").load('/Product/GetProductActivity' + "?ProductId=" + $("#hdnProductId").val());
    }
    formChangedValue = false;
    productTabId = tabId;
});

$(document).on("#lnkProductFilter", "click", (function () {
    $(this).toggleClass("filter-icon");
    $("#FilterCard").slideToggle("slow");
}));

$(document).on("click", ".add-icon", (function () {
    if ($(this).parent().parent().find("select").val() != "") {
        if ($(this).parent().parent().find("input.quantity").val() > 0 && $(this).parent().parent().find("input.quantity").val() < 1000) {
            var htmlStringToAppend = $(this).parent().parent()[0].outerHTML.replaceAll("{ID}", counter)
            htmlStringToAppend = htmlStringToAppend.replaceAll("add-icon", "minus-icon")
            htmlStringToAppend = htmlStringToAppend.replaceAll("bx-plus", "bx-minus")
            htmlStringToAppend = htmlStringToAppend.replaceAll("data-", "")

            $(this).parent().parent().parent().append(htmlStringToAppend)
            counter++;
            emptyFields($(this).parent().parent())
            calculateCostPrice();
        } else {
            toastr.warning("Please enter value between 1-999.");
        }
    }
    else {
        toastr.warning("Please select value.");
    }
}));

$(document).on("change", "#CostPrice", (function () {
    var amount = parseFloat($(this).val());
    calculateRetailerSP(amount);
    calculateWholesalerSP(amount);
}));

function calculateCostPrice() {
    var sum = 0;
    $(".costPrice").each(function () {
        if ($(this).parent().parent().find("input.isDeleted").val() == 'false')
            sum += +this.value;
    });
    var roundedSum = RoundTo(Math.round(sum).toFixed(2));
    sum = roundedSum.toFixed(2);
    $("#CostPrice").val(sum);
    $("#ProductRequestDto_CostPrice-error").hide();
    calculateRetailerSP(roundedSum);
    calculateWholesalerSP(roundedSum);
}

function calculateRetailerSP(costPrice) {
    var retailerSP = $("#hdnRetailerSP").val();
    if (retailerSP > 0) {
        var retailerCostPrice = RoundTo(Math.round((costPrice + (costPrice * retailerSP) / 100)).toFixed(2));
        $("#RetailerPrice").val(retailerCostPrice.toFixed(2));
    }
}

function calculateWholesalerSP(costPrice) {
    var wholesalerSP = $("#hdnWholesalerSP").val();
    if (wholesalerSP > 0) {
        var wholesalerCostPrice = RoundTo(Math.round((costPrice + (costPrice * wholesalerSP) / 100)).toFixed(2));
        $("#WholesalerPrice").val(wholesalerCostPrice.toFixed(2));
    }
}

function RoundTo(number) {
    var roundto = $("#hdnAmountRoundTo").val();
    if (roundto > 0)
        return roundto * Math.round(number / roundto);
    else
        return number;
}

function emptyFields(trRow) {
    trRow.find("input[type=text]").each(function () {
        $(this).val("")
    });
    trRow.find("input[type=number]").each(function () {
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
    if (val == "")
        $(this).parent().parent().find("input.quantity").val("");
}));

$(document).on("click", ".minus-icon", function () {
    var htmlStringToAppend = $(this).parent().parent();
    htmlStringToAppend.find("[name$='IsDeleted']").val("true");
    htmlStringToAppend.hide();

    calculateCostPrice();
});

ProductModel.onSuccess = function (xhr) {
    $('.productDetailsSubmit').buttonLoader('stop');
    toastr.success('Information saved successfully.');
    $("#divProductInfo").load('/Product/CreateProductBasicDetail' + "?ProductId=" + $("#hdnProductId").val());
};

ProductModel.onFailed = function (xhr) {
    $('.productDetailsSubmit').buttonLoader('stop');
};

ProductModel.onProductMaterialSuccess = function (xhr) {
    $('.btnProductMaterial').buttonLoader('stop');
    toastr.success('Information saved successfully.');
    $("#divProductInfo").load('/Product/CreateProductBasicDetail' + "?ProductId=" + $("#hdnProductId").val());
};

ProductModel.onProductMaterialFailed = function (xhr) {
    $('.btnProductMaterial').buttonLoader('stop');
};

$(document).on('submit', '#frmProductInfoForm', function (e) {
    $('.productSubmit').buttonLoader('start');
});

$(document).on('submit', '#frmProductDetail', function (e) {
    $('.productDetailsSubmit').buttonLoader('start');
});

$(document).on('submit', '#frmProductMaterial', function (e) {
    $('.btnProductMaterial').buttonLoader('start');
});

$(document).on("click", ".img-wrap .close", (function () {
    var id = $(this).closest('.img-wrap').find('img').data('imageid');
    $(this).parent().remove();

    $.ajax({
        url: "/Product/DeleteProductImage?ProductImageId=" + id,
        type: "GET",
        success: function (response) {
            $("#divProductInfo").load('/Product/CreateProductBasicDetail' + "?ProductId=" + $("#hdnProductId").val());
        }
    });
}));

$(document).on("click", ".img-wrap .custom-control-input", (function () {
    var id = $(this).closest('.img-wrap').find('img').data('imageid');

    $.ajax({
        url: "/Product/ProductImageMarkAsCover?ProductImageId=" + id + "&IsCover=" + $(this).prop('checked'),
        type: "GET",
        success: function (response) {
            $("#divProductInfo").load('/Product/CreateProductBasicDetail' + "?ProductId=" + $("#hdnProductId").val());
        }
    });
}));

$(function () {
    var myDropzone = $("#filedrop");
    myDropzone.on("success", function (file) {
        alert('Hello World');
    });
})


$('.ProductForm').on('change input', 'input, select, textarea', function () {
    formChangedValue = true;
});

function getFormName(tabNameId) {
    if (productTabId == "nav-rawmaterial-tab") {
        return $("#frmProductMaterial").submit();
    } else if (productTabId == "nav-detail-tab") {
        return $("#frmProductDetail").submit();
    }
    else if (productTabId == "nav-detail-tab") {
        return;
    }
}