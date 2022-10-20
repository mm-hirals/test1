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
                        '<a data-ajax-complete="RawMaterialModel.onDelete" data-ajax="true" class="btn btn-icon btn-outline-danger" data-ajax-mode="replace" href="/RawMaterial/Delete/' + o.rawMaterialId + '"><i class="bx bxs-trash"></i></a></div>';
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
    $("#divRawMaterialModal").modal('show');
}

RawMaterialModel.onDelete = function () {
    tblRawMaterial.ajax.reload(null, false);
}

RawMaterialModel.onSuccess = function (xhr) {
    tblRawMaterial.ajax.reload(null, false);
    $("#divRawMaterialModal").modal('hide');
};

RawMaterialModel.onFailed = function (xhr) {
    tblRawMaterial.ajax.reload(null, false);
    $("#divRawMaterialModal").modal('hide');
};