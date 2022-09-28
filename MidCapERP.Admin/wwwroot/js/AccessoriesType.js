'use strict';

var AccessoriesTypeModel = {};
var tblAccessoriesType;

$(function () {
    tblAccessoriesType = $("#tblAccessoriesType").DataTable({
        "searching": false,
        "processing": true,
        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": "/AccessoriesType/GetAccessoriesTypeData",
            "type": "POST",
            "datatype": "json"
        },
        "columns": [
            { "data": "categoryName", "name": "Category Name", "autoWidth": true },
            { "data": "typeName", "name": "Accessory Type Name", "autoWidth": true },
            {
                "mData": null, "bSortable": false,
                "mRender": function (o) {
                    return '<div class="c-action-btn-group justify-content-start"><a data-ajax-complete="AccessoriesTypeModel.onComplete" data-ajax="true" class="btn btn-icon btn-outline-primary" data-ajax-mode="replace" data-ajax-update="#divUpdateAccessoriesType" href="/AccessoriesType/Update/' + o.accessoriesTypeId + '"><i class="bx bxs-pencil"></i></a>' +
                        '&nbsp<a data-ajax-complete="AccessoriesTypeModel.onDelete" data-ajax="true" class="btn btn-icon btn-outline-danger" data-ajax-mode="replace" href="/AccessoriesType/Delete/' + o.accessoriesTypeId + '"><i class="bx bxs-trash"></i></a></div>';
                }
            }
        ]
    });
});

$("#lnkAccessoriesTypeFilter").click(function () {
    $(this).toggleClass("filter-icon");
    $("#FilterCard").slideToggle("slow");
});

AccessoriesTypeModel.onComplete = function () {
    $("#divAccessoriesTypeModal").modal('show');
}

AccessoriesTypeModel.onDelete = function () {
    tblAccessoriesType.ajax.reload();
}

AccessoriesTypeModel.onSuccess = function (xhr) {
    tblAccessoriesType.ajax.reload();
    $("#divAccessoriesTypeModal").modal('hide');
};

AccessoriesTypeModel.onFailed = function (xhr) {
    tblAccessoriesType.ajax.reload();
    $("#divAccessoriesTypeModal").modal('hide');
};