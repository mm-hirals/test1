'use strict';
var counter = 0;
var ProductModel = {};
var tblProduct;

$(function () {
    tblProduct = $("#tblProduct").DataTable({
        "searching": false,
        "processing": true,
        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": "/Product/GetProductData",
            "type": "POST",
            "datatype": "json"
        },
        "columns": [
            { "data": "categoryName", "name": "CategoryName", "autoWidth": true },
            { "data": "productTitle", "name": "ProductTitle", "autoWidth": true },
            { "data": "modelNo", "name": "ModelNo", "autoWidth": true },
            { "data": "costPrice", "name": "CostPrice", "autoWidth": true },
            { "data": "retailerPrice", "name": "RetailerPrice", "autoWidth": true },
            { "data": "wholesalerPrice", "name": "WholesalerPrice", "autoWidth": true },
            {
                "mData": null, "bSortable": false,
                "mRender": function (o) {
                    return '<div class="c-action-btn-group justify-content-start"><a class="btn btn-icon btn-outline-primary" href="/Product/Update/' + o.productId + '"><i class="bx bxs-pencil"></i></a>' +
                        '<a data-ajax-complete="ProductModel.onDelete" data-ajax="true" class="btn btn-icon btn-outline-danger" data-ajax-mode="replace" href="/Product/Delete/' + o.productId + '"><i class="bx bxs-trash"></i></a></div>';
                }
            }
        ]
    });
});
$("#lnkProductFilter").click(function () {
    $(this).toggleClass("filter-icon");
    $("#FilterCard").slideToggle("slow");
});

$(".add-icon").click(function () {
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
});
function calculateCostPrice() {
    var sum = 0;
    $(".costPrice").each(function () {
        if ($(this).parent().parent().find("input.isDeleted").val() == 'false')
            sum += +this.value;
    });
    $("#ProductRequestDto_CostPrice").val(sum);
}

function emptyFields(trRow) {
    trRow.find("input[type=text]").each(function () {
        $(this).val("")
    });
    trRow.find("select").each(function () {
        $(this).val("")
    });
}
$("input.costPrice").change(function () {
    $(this).attr('value', $(this).val());
})
$("input.quantity").change(function () {
    var qty = $(this).attr('value', $(this).val());
    var unitPrice = $(this).parent().parent().find("input.materialPrice").val();
    var costPrice = qty.val() * unitPrice;
    $(this).parent().parent().find("input.quantity").attr('value', qty.val());
    $(this).parent().parent().find("input.costPrice").val(costPrice);
    $(this).parent().parent().find("input.costPrice").attr('value', costPrice);
})
$("select.material").change(function () {
    var val = $(this).val();
    $("option", this).removeAttr("selected").filter(function () {
        return $(this).attr("value") == val;
    }).first().attr("selected", "selected");

    var unitPrice = $(this).find(':selected').attr('data-unitprice');
    $(this).parent().parent().find("input.materialPrice").val(unitPrice);
    $(this).parent().parent().find("input.materialPrice").attr('value', unitPrice);
    $(this).parent().parent().find("input.quantity").val(1);
    $(this).parent().parent().find("input.quantity").attr('value', 1);
    $(this).parent().parent().find("input.costPrice").val(unitPrice);
    $(this).parent().parent().find("input.costPrice").attr('value', unitPrice);

    $("span.materialErrorMsg").hide();
})
$(document).on("click", ".minus-icon", function () {
    var htmlStringToAppend = $(this).parent().parent();
    htmlStringToAppend.find("[name$='IsDeleted']").val("true");
    htmlStringToAppend.hide();

    calculateCostPrice();
});