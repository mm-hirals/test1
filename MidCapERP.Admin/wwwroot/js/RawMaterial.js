'use strict';

var RawMaterialModel = {};
var tblRawMaterial;

$(function () {
    tblRawMaterial = $("#tblRawMaterial").DataTable({
        "searching": false,
        "processing": true,
        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": "/RawMaterial/GetRawMaterialData",
            "type": "POST",
            "datatype": "json"
        },
        "columns": [
            { "data": "title", "name": "title", "autoWidth": true },
            { "data": "unitName", "name": "unitName", "autoWidth": true },
            { "data": "unitPrice", "name": "unitPrice", "autoWidth": true },
            {
                "mData": null, "bSortable": false,
                "mRender": function (o) {
                    return '<a data-ajax-complete="RawMaterialModel.onComplete" data-ajax="true" class="btn btn-icon btn-outline-primary" data-ajax-mode="replace" data-ajax-update="#divUpdateRawMaterial" href="/RawMaterial/Update/' + o.rawMaterialId + '"><i class="bx bxs-pencil"></i></a>' +
                        '<a data-ajax-complete="RawMaterialModel.onDelete" data-ajax="true" class="btn btn-icon btn-outline-danger" data-ajax-mode="replace" href="/RawMaterial/Delete/' + o.rawMaterialId + '"><i class="bx bxs-trash"></i></a>';
                }
            }
        ]
    });
});

$("#lnkRawMaterialFilter").click(function () {
    $(this).toggleClass("filter-icon");
    $("#FilterCard").slideToggle("slow");
});

RawMaterialModel.onComplete = function () {
    $("#divRawMaterialModal").modal('show');
}

RawMaterialModel.onDelete = function () {
    tblRawMaterial.ajax.reload();
}

RawMaterialModel.onSuccess = function (xhr) {
    tblRawMaterial.ajax.reload();
    $("#divRawMaterialModal").modal('hide');
};

RawMaterialModel.onFailed = function (xhr) {
    tblRawMaterial.ajax.reload();
    $("#divRawMaterialModal").modal('hide');
};