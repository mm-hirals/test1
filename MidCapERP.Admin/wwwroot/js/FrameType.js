'use strict';

var FrameTypeModel = {};
var tblFrameType;

$(function () {
    tblFrameType = $("#tblFrameType").DataTable({
        "searching": false,
        "processing": true,
        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": "/FrameType/GetFrameTypeData",
            "type": "POST",
            "datatype": "json"
        },
        "columns": [
            { "data": "lookupValueName", "name": "LookupValueName", "autoWidth": true },
            {
                "mData": null, "bSortable": false,
                "mRender": function (o) {
                    return '<div class="c-action-btn-group justify-content-start"><a data-ajax-complete="FrameTypeModel.onComplete" data-ajax="true" class="btn btn-icon btn-outline-primary" data-ajax-mode="replace" data-ajax-update="#divUpdateFrameType" href="/FrameType/Update/' + o.lookupValueId + '"><i class="bx bxs-pencil"></i></a>' +
                        '<a data-ajax-complete="FrameTypeModel.onDelete" data-ajax="true" class="btn btn-icon btn-outline-danger" data-ajax-mode="replace" href="/FrameType/Delete/' + o.lookupValueId + '"><i class="bx bxs-trash"></i></a></div>';
                }
            }
        ]
    });
});

$("#lnkFrameTypeFilter").click(function () {
    $(this).toggleClass("filter-icon");
    $("#FilterCard").slideToggle("slow");
});

FrameTypeModel.onComplete = function () {
    $("#divFrameTypeModal").modal('show');
}

FrameTypeModel.onDelete = function () {
    tblFrameType.ajax.reload(null, false);
}

FrameTypeModel.onSuccess = function (xhr) {
    tblFrameType.ajax.reload(null, false);
    $("#divFrameTypeModal").modal('hide');
};

FrameTypeModel.onFailed = function (xhr) {
    tblFrameType.ajax.reload(null, false);
    $("#divFrameTypeModal").modal('hide');
};