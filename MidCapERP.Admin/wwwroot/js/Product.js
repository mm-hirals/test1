'use strict';

var ProductModel = {};
var tblProduct;

$(function () {
    tblProduct = $("#tblProduct").DataTable({
        "searching": false,
        //"processing": true,
        //"serverSide": true,
        //"filter": true,
        //"ajax": {
        //    "url": "/Product/GetProductData",
        //    "type": "POST",
        //    "datatype": "json"
        //},
        //"columns": [
        //    { "data": "lookupValueName", "name": "LookupValueName", "autoWidth": true },
        //    {
        //        "mData": null, "bSortable": false,
        //        "mRender": function (o) {
        //            return '<div class="c-action-btn-group justify-content-start"><a data-ajax-complete="ProductModel.onComplete" data-ajax="true" class="btn btn-icon btn-outline-primary" data-ajax-mode="replace" data-ajax-update="#divUpdateProduct" href="/Product/Update/' + o.lookupValueId + '"><i class="bx bxs-pencil"></i></a>' +
        //                '<a data-ajax-complete="ProductModel.onDelete" data-ajax="true" class="btn btn-icon btn-outline-danger" data-ajax-mode="replace" href="/Product/Delete/' + o.lookupValueId + '"><i class="bx bxs-trash"></i></a></div>';
        //        }
        //    }
        //]
    });
});

$("#lnkProductFilter").click(function () {
    $(this).toggleClass("filter-icon");
    $("#FilterCard").slideToggle("slow");
});

ProductModel.onComplete = function () {
    $("#divProductModal").modal('show');
}

ProductModel.onDelete = function () {
    tblProduct.ajax.reload();
}

ProductModel.onSuccess = function (xhr) {
    tblProduct.ajax.reload();
    $("#divProductModal").modal('hide');
};

ProductModel.onFailed = function (xhr) {
    tblProduct.ajax.reload();
    $("#divProductModal").modal('hide');
};