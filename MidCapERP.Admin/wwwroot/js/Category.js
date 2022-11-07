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
    $('#btnCreateCategory').buttonLoader('stop');
    $('#btnSaveCategory').buttonLoader('stop');
    $("#divCategoryModal").modal('show');
}

  
CategoryModel.onSuccess = function (xhr) {
    $('#btnCreateCategory').buttonLoader('stop');
    $('#btnSaveCategory').buttonLoader('stop');
    tblCategory.ajax.reload(null, false);
    $("#divCategoryModal").modal('hide');
    toastr.success('Information saved successfully.');
};

CategoryModel.onFailed = function (xhr) {
    $('#btnCreateCategory').buttonLoader('stop');
    $('#btnSaveCategory').buttonLoader('stop');
    tblCategory.ajax.reload(null, false);
    $("#divCategoryModal").modal('hide');
};

$(document).delegate("#btnCreateCategory", "click", function () {
    $('#btnCreateCategory').buttonLoader('start');
});

$(document).on('submit', '#frmCreateCategory', function (e) {
    $('#btnSaveCategory').buttonLoader('start');
});

$(document).delegate(".btnRemoveCategory", "click", function () {
    if (!$.isEmptyObject(this.id) && this.id > 0) {
        SweetAlert("Home", this.id, DeleteCategory);
    }
    else {
        errorMessage("Oops...", "Something went wrong!", "error");
    }
});

function DeleteCategory(id) {
    if (!$.isEmptyObject(id) && id > 0) {
        $.ajax({
            url: "/Category/Delete/?Id=" + id,
            type: "GET",
            success: function (response) {
                message("Deleted!", "Your record has been deleted.", "success");
                tblCategory.ajax.reload();
            }
        });
    } else {
        errorMessage("Oops...", "Something went wrong!", "error");
    }
}