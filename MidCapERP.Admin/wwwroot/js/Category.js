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
            { "data": "lookupValueName", "name": "LookupValueName", "autoWidth": true },
            {
                "mData": null, "bSortable": false,
                "mRender": function (o) {
                    return '<div class="c-action-btn-group justify-content-start"><a data-ajax-complete="CategoryModel.onComplete" data-ajax="true" class="btn btn-icon btn-outline-primary" data-ajax-mode="replace" data-ajax-update="#divUpdateCategory" href="/Category/Update/' + o.lookupValueId + '"><i class="bx bxs-pencil"></i></a>' +
                        '<a data-ajax-complete="CategoryModel.onDelete" data-ajax="true" class="btn btn-icon btn-outline-danger" data-ajax-mode="replace" href="/Category/Delete/' + o.lookupValueId + '"><i class="bx bxs-trash"></i></a></div>';
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
    tblCategory.ajax.reload(null, false);
}

CategoryModel.onSuccess = function (xhr) {
    tblCategory.ajax.reload(null, false);
    $("#divCategoryModal").modal('hide');
};

CategoryModel.onFailed = function (xhr) {
    tblCategory.ajax.reload(null, false);
    $("#divCategoryModal").modal('hide');
};