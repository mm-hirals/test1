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
                    return '<div class="c-action-btn-group justify-content-end"><a data-ajax-complete="UnitModel.onComplete" data-ajax="true" class="btn btn-icon btn-outline-primary" data-ajax-mode="replace" data-ajax-update="#divUpdateUnit" href="/Unit/Update/' + o.lookupValueId + '"><i class="bx bxs-pencil"></i></a>' +
                        '<a data-ajax-complete="UnitModel.onDelete" data-ajax="true" class="btn btn-icon btn-outline-danger" data-ajax-confirm="Are you sure you want to delete?"  data-ajax-mode="replace" href="/Unit/Delete/' + o.lookupValueId + '"><i class="bx bxs-trash"></i></a></div>';
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
    $('#btnCreateUnit').buttonLoader('stop');
    $('#btnCreateUpdateUnit').buttonLoader('stop');
}

UnitModel.onDelete = function () {
    tblUnit.ajax.reload(null, false);
    toastr.error('Data deleted successfully.');
}

UnitModel.onSuccess = function (xhr) {
    $('#btnCreateUnit').buttonLoader('stop');
    $('#btnCreateUpdateUnit').buttonLoader('stop');
    tblUnit.ajax.reload(null, false);
    $("#divUnitModal").modal('hide');
    toastr.success('Information saved successfully.');
};

UnitModel.onFailed = function (xhr) {
    $('#btnCreateUnit').buttonLoader('stop');
    $('#btnCreateUpdateUnit').buttonLoader('stop');
    tblUnit.ajax.reload(null, false);
    $("#divUnitModal").modal('hide');
};

$(document).delegate("#btnCreateUnit", "click", function () {
    $('#btnCreateUnit').buttonLoader('start');
});

$(document).on('submit', '#frmCreateUpdateUnit', function (e) {
    $('#btnCreateUpdateUnit').buttonLoader('start');
});