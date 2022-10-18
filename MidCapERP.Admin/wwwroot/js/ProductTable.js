'use strict';

var ProductModel = {};
var tblProduct;

$(function () {
    tblProduct = $("#tblProduct").DataTable({
        "searching": false,
        "processing": true,
        "serverSide": true,
        "filter": true,
        "iDisplayLength": 50,
        "ajax": {
            "url": "/Product/GetProductData",
            "type": "POST",
            "datatype": "json",
            "data": function (d) {
                d.categoryName = $("#categoryName").val().trim();
                d.productTitle = $("#productTitle").val().trim();
                d.modelNo = $("#modelNo").val().trim();
                d.publishStatus = $("#publishStatus").val().trim();
            }
        },
        "columns": [
            { "data": "categoryName", "name": "CategoryName", "autoWidth": true },
            { "data": "productTitle", "name": "ProductTitle", "autoWidth": true },
            { "data": "modelNo", "name": "ModelNo", "autoWidth": true },
            /*{ "data": "createdDateFormat", "name": "CreatedDateFormat", "autoWidth": true },*/
            /*{ "data": "createdByName", "name": "CreatedByName", "autoWidth": true },*/
            { "data": "updatedByName", "name": "UpdatedByName", "autoWidth": true },
            { "data": "updatedDateFormat", "name": "UpdatedDateFormat", "autoWidth": true },
            {
                "mData": null, "bSortable": false,
                "mRender": function (x) {
                    if (x.status == 1) {
                        return '<div class="custom-control custom-switch d-flex"><input type="checkbox" class="custom-control-input productStatus" value="' + x.productId + '" id="' + x.productId + '" checked /><label class="custom-control-label" for="' + x.productId + '"></label></div>'
                    }
                    else {
                        return '<div class="custom-control custom-switch d-flex"><input type="checkbox" class="custom-control-input productStatus" value="' + x.productId + '" id="' + x.productId + '" /><label class="custom-control-label" for="' + x.productId + '"></label></div>'
                    }
                }
            },
            {
                "mData": null, "bSortable": false,
                "mRender": function (o) {
                    return '<div class="c-action-btn-group justify-content-end"><a class="btn btn-icon btn-outline-primary" href="/Product/Update/' + o.productId + '"><i class="bx bxs-pencil"></i></a>' +
                        '<a data-ajax-complete="ProductModel.onDelete" data-ajax="true" class="btn btn-icon btn-outline-danger" data-ajax-mode="replace" href="/Product/Delete/' + o.productId + '"><i class="bx bxs-trash"></i></a></div>';
                }
            }
        ],
        //columnDefs: [
        //    {
        //        "type": "unix",
        //        "targets": [3, 4],
        //        "render": function (data) {
        //            return moment(data).format('DD/MM/YYYY HH:MM')
        //        }
        //    },
        //]
    });
});

$("#lnkProductFilter").click(function () {
    $(this).toggleClass("filter-icon");
    $("#FilterCard").slideToggle("slow");
});

ProductModel.onDelete = function () {
    tblProduct.ajax.reload();
}

$("#categoryName,#productTitle,#modelNo").keyup("input", function () {
    tblProduct.ajax.reload(null, false);
});

$('#publishStatus').change(function () {
    tblProduct.ajax.reload(null, false);
});

$('#tblProduct').on('click', 'input[type="checkbox"]', function () {
    var data = {
        ProductId: $(this).val(),
        Status: this.checked,
    };
    $.ajax({
        url: "/Product/UpdateProductStatus",
        type: "POST",
        data: data,
        success: function (response) {
            if (response == "success") {
                tblProduct.ajax.reload();
            }
            else
                alert("Error: " + response);
        }
    });
});