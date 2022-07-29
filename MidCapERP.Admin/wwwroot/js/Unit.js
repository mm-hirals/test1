'use strict';

var UnitModel = {};
var tblUnit;

$(function () {
    tblUnit = $("#tblUnit").DataTable({
        "searching": false,
        "processing": true,
        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": "/Unit/GetUnitData",
            "type": "POST",
            "datatype": "json"
        },
        "columns": [
            { "data": "lookupName", "name": "LookupName", "autoWidth": true },
            { "data": "lookupValueName", "name": "LookupValueName", "autoWidth": true },
            {
                "mData": null, "bSortable": false,
                "mRender": function (o) {
                    return '<a data-ajax-complete="UnitModel.onComplete" data-ajax="true" class="btn btn-icon btn-outline-primary" data-ajax-mode="replace" data-ajax-update="#divUpdateUnit" href="/Unit/Update/' + o.lookupValueId + '"><i class="bx bxs-pencil"></i></a>' +
                        '&nbsp<a data-ajax-complete="UnitModel.onDelete" data-ajax="true" class="btn btn-icon btn-outline-danger" data-ajax-mode="replace" href="/Unit/Delete/' + o.lookupValueId + '"><i class="bx bxs-trash"></i></a>';
                }
            }
        ]
    });
});

$("#lnkUnitFilter").click(function () {
    $(this).toggleClass("filter-icon");
    $("#FilterCard").slideToggle("slow");
});

UnitModel.onComplete = function () {
    $("#divUnitModal").modal('show');
}

UnitModel.onDelete = function () {
    tblUnit.ajax.reload();
}

UnitModel.onSuccess = function (xhr) {
    tblUnit.ajax.reload();
    $("#divUnitModal").modal('hide');
};

UnitModel.onFailed = function (xhr) {
    tblUnit.ajax.reload();
    $("#divUnitModal").modal('hide');
};