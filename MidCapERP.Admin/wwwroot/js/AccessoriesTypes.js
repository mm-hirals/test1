'use strict';

var AccessoriesTypesModel = {};
var tblAccessoriesTypes;

$(function () {
    tblAccessoriesTypes = $("#tblAccessoriesTypes").DataTable({
        "searching": false,
        "processing": true,
        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": "/AccessoriesTypes/GetAccessoriesTypesData",
            "type": "POST",
            "datatype": "json"
        },
        "columns": [
            { "data": "categoryName", "name": "CategoryName", "autoWidth": true },
            { "data": "typeName", "name": "TypeName", "autoWidth": true },   
            {
                "mData": null, "bSortable": false,
                "mRender": function (o) {
                    return '<a data-ajax-complete="AccessoriesTypesModel.onComplete" data-ajax="true" class="btn btn-icon btn-outline-primary" data-ajax-mode="replace" data-ajax-update="#divUpdateAccessoriesTypes" href="/AccessoriesTypes/Update/' + o.accessoriesTypeId + '"><i class="bx bxs-pencil"></i></a>' +
                        '&nbsp<a data-ajax-complete="AccessoriesTypesModel.onDelete" data-ajax="true" class="btn btn-icon btn-outline-danger" data-ajax-mode="replace" href="/AccessoriesTypes/Delete/' + o.accessoriesTypeId + '"><i class="bx bxs-trash"></i></a>';
                }
            }
        ]
    });
});

$("#lnkAccessoriesTypesFilter").click(function () {
    $(this).toggleClass("filter-icon");
    $("#FilterCard").slideToggle("slow");
});

AccessoriesTypesModel.onComplete = function () {
    $("#divAccessoriesTypesModal").modal('show');
}

AccessoriesTypesModel.onDelete = function () {
    tblAccessoriesTypes.ajax.reload();
}

AccessoriesTypesModel.onSuccess = function (xhr) {
    tblAccessoriesTypes.ajax.reload();
    $("#divAccessoriesTypesModal").modal('hide');
};

AccessoriesTypesModel.onFailed = function (xhr) {
    tblAccessoriesTypes.ajax.reload();
    $("#divAccessoriesTypesModal").modal('hide');
};