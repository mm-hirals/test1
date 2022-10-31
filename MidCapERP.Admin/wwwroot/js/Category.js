'use strict';

var CategoryModel = {};
var tblCategory;

$(function () {
    tblCategory = $("#tblCategory").DataTable({
        "searching": false,
        "processing": true,
        "serverSide": true,
        "iDisplayLength": 50,
        "filter": true,
        "ajax": {
            "url": "/Category/GetCategoryData",
            "type": "POST",
            "datatype": "json",
            "data": function (d) {
                d.categoryName = $("#categoryName").val().trim()
            }
        },
        "columns": [
            { "data": "lookupValueName", "name": "LookupValueName", "autoWidth": true },
            {
                "mData": null, "bSortable": false,
                "mRender": function (o) {
                    return '<div class="c-action-btn-group justify-content-end"><a data-ajax-complete="CategoryModel.onComplete" data-ajax="true" class="btn btn-icon btn-outline-primary" data-ajax-mode="replace" data-ajax-update="#divUpdateCategory" href="/Category/Update/' + o.lookupValueId + '"><i class="bx bxs-pencil"></i></a>' +
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

$("#categoryName").on("input", function () {
    tblCategory.ajax.reload(null, false);
});

CategoryModel.onComplete = function () {
    $("#divCategoryModal").modal('show');
}

CategoryModel.onDelete = function () {
    tblCategory.ajax.reload(null, false);
    toastr.error('Delete Data Successfully.');
}

CategoryModel.onSuccess = function (xhr) {
    tblCategory.ajax.reload(null, false);
    $("#divCategoryModal").modal('hide');
    toastr.success('Save Data Successfully.');
};

CategoryModel.onFailed = function (xhr) {
    tblCategory.ajax.reload(null, false);
    $("#divCategoryModal").modal('hide');
};