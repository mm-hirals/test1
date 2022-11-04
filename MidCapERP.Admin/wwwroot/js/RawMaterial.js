'use strict';

var RawMaterialModel = {};
var tblRawMaterial;

$(function () {
    tblRawMaterial = $("#tblRawMaterial").DataTable({
        "searching": false,
        "processing": true,
        "serverSide": true,
        "filter": true,
        "iDisplayLength": 50, 
        "ajax": {
            "url": "/RawMaterial/GetRawMaterialData",
            "type": "POST",
            "datatype": "json",
            "data": function (d) {
                d.title = $("#title").val().trim(),
                    d.unitName = $("#unitName").val().trim()
            }
        },
        "columns": [
            { "data": "title", "name": "title", "autoWidth": true },
            { "data": "unitName", "name": "unitName", "autoWidth": true },
            { "data": "unitPrice", "name": "unitPrice", "autoWidth": true },
            {
                "mData": null, "bSortable": false,
                "mRender": function (o) {
                    return '<div class="c-action-btn-group justify-content-end"><a data-ajax-complete="RawMaterialModel.onComplete" data-ajax="true" class="btn btn-icon btn-outline-primary" data-ajax-mode="replace" data-ajax-update="#divUpdateRawMaterial" href="/RawMaterial/Update/' + o.rawMaterialId + '"><i class="bx bxs-pencil"></i></a>' +
                        '<a id="' + o.rawMaterialId + '" class="btn btn-icon btn-outline-danger btnRawMaterial" data-ajax-mode="replace"><i class="bx bxs-trash"></i></a></div>';
                }
            }
        ]
    });
});

$("#lnkRawMaterialFilter").click(function () {
    $(this).toggleClass("filter-icon");
    $("#FilterCard").slideToggle("slow");
});

$("#title,#unitName").on("input", function () {
    tblRawMaterial.ajax.reload(null, false);
});

RawMaterialModel.onComplete = function () {
    $('#btnCreateRawMaterial').buttonLoader('stop');
    $('#btnCreateUpdateRawMaterial').buttonLoader('stop');
    $("#divRawMaterialModal").modal('show');
}

RawMaterialModel.onSuccess = function (xhr) {
    $('#btnCreateRawMaterial').buttonLoader('stop');
    $('#btnCreateUpdateRawMaterial').buttonLoader('stop');
    tblRawMaterial.ajax.reload(null, false);
    $("#divRawMaterialModal").modal('hide');
    toastr.success('Information saved successfully.');
};

RawMaterialModel.onFailed = function (xhr) {
    $('#btnCreateRawMaterial').buttonLoader('stop');
    $('#btnCreateUpdateRawMaterial').buttonLoader('stop');
    tblRawMaterial.ajax.reload(null, false);
    $("#divRawMaterialModal").modal('hide');
};

$(document).delegate("#btnCreateRawMaterial", "click", function () {
    $('#btnCreateRawMaterial').buttonLoader('start');
});

$(document).on('submit', '#frmCreateUpdateRawMaterial', function (e) {
    $('#btnCreateUpdateRawMaterial').buttonLoader('start');
});

$(document).delegate(".btnRawMaterial", "click", function () {
    if (!$.isEmptyObject(this.id) && this.id > 0) {
        SweetAlert("Home", this.id, DeleteRawMaterial);
    }
    else {
        errorMessage("Oops...", "Something went wrong!", "error");
    }
});

function DeleteRawMaterial(id) {
    if (!$.isEmptyObject(id) && id > 0) {
        $.ajax({
            url: "/RawMaterial/Delete/?Id=" + id,
            type: "GET",
            success: function (response) {
                message("Deleted!", "Your record has been deleted.", "success");
                tblRawMaterial.ajax.reload(null, false);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                errorMessage("Oops...", "Something went wrong!", "error");
            }
        });
    } else {
        errorMessage("Oops...", "Something went wrong!", "error");
    }
}