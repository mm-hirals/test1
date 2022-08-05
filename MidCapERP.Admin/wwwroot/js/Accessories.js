'use strict';

var AccessoriesModel = {};
var tblAccessories;

$(function () {
    tblAccessories = $("#tblAccessories").DataTable({
        "searching": false,
        "processing": true,
        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": "/Accessories/GetAccessoriesData",
            "type": "POST",
            "datatype": "json"
        },
        "columns": [
            { "data": "categoryName", "name": "CategoryName", "autoWidth": true },
            { "data": "accessoriesTypeName", "name": "TypeName", "autoWidth": true },
            { "data": "title", "name": "Title", "autoWidth": true },
            { "data": "unitName", "name": "UnitName", "autoWidth": true },
            { "data": "unitPrice", "name": "UnitPrice", "autoWidth": true },
            {
                "mData": null, "bSortable": false,
                "mRender": function (o) {
                    return '<div class="c-action-btn-group justify-content-start"><a data-ajax-complete="AccessoriesModel.onComplete" data-ajax="true" class="btn btn-icon btn-outline-primary" data-ajax-mode="replace" data-ajax-update="#divUpdateAccessories" href="/Accessories/Update/' + o.accessoriesId + '"><i class="bx bxs-pencil"></i></a>' +
                        '<a data-ajax-complete="AccessoriesModel.onDelete" data-ajax="true" class="btn btn-icon btn-outline-danger" data-ajax-mode="replace" href="/Accessories/Delete/' + o.accessoriesId + '"><i class="bx bxs-trash"></i></a></div>';
                }
            }
        ]
    });
});

$("#lnkAccessoriesFilter").click(function () {
    $(this).toggleClass("filter-icon");
    $("#FilterCard").slideToggle("slow");
});

AccessoriesModel.onComplete = function () {
    $("#divAccessoriesModal").modal('show');
}

AccessoriesModel.onDelete = function () {
    tblAccessories.ajax.reload(null, false);
}

AccessoriesModel.onSuccess = function (xhr) {
    tblAccessories.ajax.reload(null, false);
    $("#divAccessoriesModal").modal('hide');
};

AccessoriesModel.onFailed = function (xhr) {
    tblAccessories.ajax.reload(null, false);
    $("#divAccessoriesModal").modal('hide');
};