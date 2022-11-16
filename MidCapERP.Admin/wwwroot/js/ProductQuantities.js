'use strict';

var ProductQuantitiesModel = {};
var tblProductQuantities;

$(function () {
    tblProductQuantities = $("#tblProductQuantities").DataTable({
        "searching": false,
        "processing": true,
        "serverSide": true,
        "filter": true,
        "iDisplayLength": 50,
        "ajax": {
            "url": "/ProductQuantity/GetProductQuantitiesData",
            "type": "POST",
            "datatype": "json",
            "data": function (d) {
                d.categoryId = $("#categoryId").val().trim();
                d.productTitle = $("#productTitle").val().trim();
                d.quantityDate = $("#quantityDate").val().trim();
            }
        },
        "columns": [
            { "data": "categoryName", "name": "CategoryName", "autoWidth": true },
            { "data": "productTitle", "name": "ProductTitle", "autoWidth": true },
            { "data": "quantityDateFormat", "name": "QuantityDateFormat", "autoWidth": true },
            { "data": "quantity", "name": "Quantity", "autoWidth": true },
            { "data": "lastModifiedByName", "name": "LastModifiedByName", "autoWidth": true },
            { "data": "lastModifiedDateFormat", "name": "LastModifiedDateFormat", "autoWidth": true },
            {
                "mData": null, "bSortable": false,
                "mRender": function (o) {
                    return '<div class="c-action-btn-group justify-content-end"><a data-ajax-complete="ProductQuantitiesModel.onComplete" data-ajax="true" class="btn btn-icon btn-outline-primary" data-ajax-mode="replace" data-ajax-update="#divUpdateProductQuantities" href="/ProductQuantity/Update/' + o.productQuantityId + '"><i class="bx bxs-pencil"></i></a></div>';
                }
            }
        ]
    });
});

$("#lnkProductQuantitiesFilter").click(function () {
    $(this).toggleClass("filter-icon");
    $("#FilterCard").slideToggle("slow");
});

$('#categoryId').change(function () {
    tblProductQuantities.ajax.reload(null, false);
});

$('#productTitle').change(function () {
    tblProductQuantities.ajax.reload(null, false);
});

$("#quantityDate").on("input", function () {
    tblProductQuantities.ajax.reload(null, false);
});

ProductQuantitiesModel.onComplete = function () {
    $('#btnUpdateProductQuantities').buttonLoader('stop');
    $("#divProductQuantitiesModal").modal('show');
}

ProductQuantitiesModel.onSuccess = function (xhr) {
    $('#btnUpdateProductQuantities').buttonLoader('stop');
    tblProductQuantities.ajax.reload(null, false);
    $("#divProductQuantitiesModal").modal('hide');
    toastr.success('Information saved successfully.');
};

ProductQuantitiesModel.onFailed = function (xhr) {
    $('#btnUpdateProductQuantities').buttonLoader('stop');
    tblProductQuantities.ajax.reload(null, false);
    $("#divProductQuantitiesModal").modal('hide');
};

$(document).on('submit', '#frmCreateUpdateProductQuantities', function (e) {
    $('#btnUpdateProductQuantities').buttonLoader('start');
});