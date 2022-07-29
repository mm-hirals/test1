﻿'use strict';

var WoodTypeModel = {};
var tblWoodType;

$(function () {
    tblWoodType = $("#tblWoodType").DataTable({
        "searching": false,
        "processing": true,
        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": "/WoodType/GetWoodTypeData",
            "type": "POST",
            "datatype": "json"
        },
        "columns": [
            { "data": "lookupName", "name": "LookupName", "autoWidth": true },
            { "data": "lookupValueName", "name": "LookupValueName", "autoWidth": true },
            {
                "mData": null, "bSortable": false,
                "mRender": function (o) {
                    return '<a data-ajax-complete="WoodTypeModel.onComplete" data-ajax="true" class="btn btn-icon btn-outline-primary" data-ajax-mode="replace" data-ajax-update="#divUpdateWoodType" href="/WoodType/Update/' + o.lookupValueId + '"><i class="bx bxs-pencil"></i></a>' +
                        '&nbsp<a data-ajax-complete="WoodTypeModel.onDelete" data-ajax="true" class="btn btn-icon btn-outline-danger" data-ajax-mode="replace" href="/WoodType/Delete/' + o.lookupValueId + '"><i class="bx bxs-trash"></i></a>';
                }
            }
        ]
    });
});

$("#lnkWoodTypeFilter").click(function () {
    $(this).toggleClass("filter-icon");
    $("#FilterCard").slideToggle("slow");
});

WoodTypeModel.onComplete = function () {
    $("#divWoodTypeModal").modal('show');
}

WoodTypeModel.onDelete = function () {
    tblWoodType.ajax.reload();
}

WoodTypeModel.onSuccess = function (xhr) {
    tblWoodType.ajax.reload();
    $("#divWoodTypeModal").modal('hide');
};

WoodTypeModel.onFailed = function (xhr) {
    tblWoodType.ajax.reload();
    $("#divWoodTypeModal").modal('hide');
};