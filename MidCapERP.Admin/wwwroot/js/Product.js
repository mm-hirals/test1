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
$("#lnkToProductIndex").click(function () {
    window.location.href = "/Product/Index";
});
$(".add-icon").click(function () {
    debugger;
    var htmlStringToAppend = $(this).parent().parent()[0].outerHTML.replaceAll("{ID}", counter)
    htmlStringToAppend = htmlStringToAppend.replaceAll("add-icon", "minus-icon")
    htmlStringToAppend = htmlStringToAppend.replaceAll("bx-plus", "bx-minus")
    htmlStringToAppend = htmlStringToAppend.replaceAll("data-", "")
    $(this).parent().parent().parent().append(htmlStringToAppend)
    counter++;
});
$("input").change(function () {
    $(this).attr('value', $(this).val());
})
$("select").change(function () {
    var val = $(this).val(); //get new value
    //find selected option
    $("option", this).removeAttr("selected").filter(function () {
        return $(this).attr("value") == val;
    }).first().attr("selected", "selected");
    //$(this).('selected', $(this).val());

    var subjectTypeId = $(this).parent().find('input').val();
    console.log(subjectTypeId);
})
$(document).on("click", ".minus-icon", function () {
    var htmlStringToAppend = $(this).parent().parent();
    htmlStringToAppend.find("[name$='IsDeleted']").val("true");
    htmlStringToAppend.hide();
    //htmlStringToAppend = htmlStringToAppend.replaceAll("add-icon", "minus-icon")
    //htmlStringToAppend = htmlStringToAppend.replaceAll("bx-plus", "bx-minus")
    //htmlStringToAppend = htmlStringToAppend.replaceAll("data-", "")
    //$(this).parent().parent().parent().append(htmlStringToAppend)
    //counter++;
});