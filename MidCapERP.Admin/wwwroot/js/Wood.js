'use strict';

var WoodModel = {};
var tblWood;

$(function () {
    tblWood = $("#tblWood").DataTable({
        "searching": false,
        "processing": true,
        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": "/Wood/GetWoodData",
            "type": "POST",
            "datatype": "json"
        },
        "columns": [
            { "data": "woodTypeName", "name": "WoodTypeName", "autoWidth": true },
            { "data": "title", "name": "Title", "autoWidth": true },
            { "data": "modelNo", "name": "ModelNo", "autoWidth": true },
            { "data": "companyName", "name": "CompanyName", "autoWidth": true },
            { "data": "unitName", "name": "UnitName", "autoWidth": true },
            { "data": "unitPrice", "name": "UnitPrice", "autoWidth": true },
            {
              "mData": null, "bSortable": false,
                "mRender": function (o) {
                    return '<div class="c-action-btn-group justify-content-start"><a data-ajax-complete="WoodModel.onComplete" data-ajax="true" class="btn btn-icon btn-outline-primary" data-ajax-mode="replace" data-ajax-update="#divUpdateWood" href="/Wood/Update/' + o.woodId + '"><i class="bx bxs-pencil"></i></a>' +
                        '<a data-ajax-complete="WoodModel.onDelete" data-ajax="true" class="btn btn-icon btn-outline-danger" data-ajax-mode="replace" href="/Wood/Delete/' + o.woodId + '"><i class="bx bxs-trash"></i></a></div>';
                }
            }
        ]
    });
});

$("#lnkWoodFilter").click(function () {
    $(this).toggleClass("filter-icon");
    $("#FilterCard").slideToggle("slow");
});

WoodModel.onComplete = function () {
    $("#divWoodModal").modal('show');
}

WoodModel.onDelete = function () {
    tblWood.ajax.reload(null, false);
}

WoodModel.onSuccess = function (xhr) {
    tblWood.ajax.reload(null, false);
    $("#divWoodModal").modal('hide');
};

WoodModel.onFailed = function (xhr) {
    tblWood.ajax.reload(null, false);
    $("#divWoodModal").modal('hide');
};