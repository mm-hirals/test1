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
            { "data": "categoryName", "name": "CategoryName", "autoWidth": true },
            {
                "mData": null, "bSortable": false,
                "mRender": function (o) {
                    return '<div class="c-action-btn-group justify-content-end"><a data-ajax-complete="CategoryModel.onComplete" data-ajax="true" class="btn btn-icon btn-outline-primary" data-ajax-mode="replace" data-ajax-update="#divUpdateCategory" href="/Category/Update/' + o.categoryId + '"><i class="bx bxs-pencil"></i></a>' +
                        '<a data-ajax-complete="CategoryModel.onDelete" data-ajax="true" class="btn btn-icon btn-outline-danger" data-ajax-mode="replace" data-ajax-confirm="Are you sure you want to delete?" href="/Category/Delete/' + o.categoryId + '"><i class="bx bxs-trash"></i></a></div>';
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
        toastr.error('Data deleted successfully.');
  }

CategoryModel.onSuccess = function (xhr) {
    tblCategory.ajax.reload(null, false);
    $("#divCategoryModal").modal('hide');
    toastr.success('Information saved successfully.');
};

CategoryModel.onFailed = function (xhr) {
    tblCategory.ajax.reload(null, false);
    $("#divCategoryModal").modal('hide');
};