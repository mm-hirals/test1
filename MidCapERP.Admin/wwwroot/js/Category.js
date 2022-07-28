'use strict';

var CategoryModel = {};
var tblCategory;

$(function () {
    tblCategory = $("#tblCategory").DataTable({
        "searching": false,
        "processing": true,
        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": "/Category/GetCategoryData",
            "type": "POST",
            "datatype": "json"
        },
        "columns": [
            { "data": "lookupName", "name": "LookupName", "autoWidth": true },
            { "data": "lookupValueName", "name": "LookupValueName", "autoWidth": true },
            {
                "mData": null, "bSortable": false,
                "mRender": function (o) {
                    return '<a data-ajax-complete="CategoryModel.onComplete" data-ajax="true" class="btn btn-icon btn-outline-primary" data-ajax-mode="replace" data-ajax-update="#divUpdateCategory" href="/Category/Update/' + o.lookupValueId + '"><i class="bx bxs-pencil"></i></a>' +
                        '&nbsp<a data-ajax-complete="CategoryModel.onDelete" data-ajax="true" class="btn btn-icon btn-outline-danger" data-ajax-mode="replace" href="/Category/Delete/' + o.lookupValueId + '"><i class="bx bxs-trash"></i></a>';
                }
            }
        ]
    });
});

$("#lnkCategoryFilter").click(function () {
    $(this).toggleClass("filter-icon");
    $("#FilterCard").slideToggle("slow");
});

CategoryModel.onComplete = function () {
    $("#divCategoryModal").modal('show');
}

CategoryModel.onDelete = function () {
    tblCategory.ajax.reload();
}

CategoryModel.onSuccess = function (xhr) {
    tblCategory.ajax.reload();
    $("#divCategoryModal").modal('hide');
};

CategoryModel.onFailed = function (xhr) {
    tblCategory.ajax.reload();
    $("#divCategoryModal").modal('hide');
};