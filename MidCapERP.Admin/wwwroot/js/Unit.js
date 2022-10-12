'use strict';

var UnitModel = {};
var tblUnit;

$(function () {
    tblUnit = $("#tblUnit").DataTable({
        "searching": false,
        "processing": true,
        "serverSide": true,
        "filter": true,
        "iDisplayLength": 50, 
        "ajax": {
            "url": "/Unit/GetUnitData",
            "type": "POST",
            "datatype": "json",
            "data": function (d) {
                d.unitName = $("#unitName").val().trim()
            }
        },
        "columns": [
            { "data": "lookupValueName", "name": "LookupValueName", "autoWidth": true },
            {
                "mData": null, "bSortable": false,
                "mRender": function (o) {
                    return '<div class="c-action-btn-group justify-content-start"><a data-ajax-complete="UnitModel.onComplete" data-ajax="true" class="btn btn-icon btn-outline-primary" data-ajax-mode="replace" data-ajax-update="#divUpdateUnit" href="/Unit/Update/' + o.lookupValueId + '"><i class="bx bxs-pencil"></i></a>' +
                        '<a data-ajax-complete="UnitModel.onDelete" data-ajax="true" class="btn btn-icon btn-outline-danger" data-ajax-mode="replace" href="/Unit/Delete/' + o.lookupValueId + '"><i class="bx bxs-trash"></i></a></div>';
                }
            }
        ]
    });
});

$("#lnkUnitFilter").click(function () {
    $(this).toggleClass("filter-icon");
    $("#FilterCard").slideToggle("slow");
});

$("#unitName").on("input", function () {
    tblUnit.ajax.reload(null, false);
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