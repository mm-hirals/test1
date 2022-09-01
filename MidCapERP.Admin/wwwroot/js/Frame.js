'use strict';

var FrameModel = {};
var tblFrame;

$(function () {
    tblFrame = $("#tblFrame").DataTable({
        "searching": false,
        "processing": true,
        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": "/Frame/GetFrameData",
            "type": "POST",
            "datatype": "json"
        },
        "columns": [
            { "data": "frameTypeName", "name": "FrameTypeName", "autoWidth": true },
            { "data": "title", "name": "Title", "autoWidth": true },
            { "data": "modelNo", "name": "ModelNo", "autoWidth": true },
            { "data": "companyName", "name": "CompanyName", "autoWidth": true },
            { "data": "unitName", "name": "UnitName", "autoWidth": true },
            { "data": "unitPrice", "name": "UnitPrice", "autoWidth": true },
            {
                "mData": null, "bSortable": false,
                "mRender": function (o) {
                    return '<div class="c-action-btn-group justify-content-start"><a data-ajax-complete="FrameModel.onComplete" data-ajax="true" class="btn btn-icon btn-outline-primary" data-ajax-mode="replace" data-ajax-update="#divUpdateFrame" href="/Frame/Update/' + o.frameId + '"><i class="bx bxs-pencil"></i></a>' +
                        '<a data-ajax-complete="FrameModel.onDelete" data-ajax="true" class="btn btn-icon btn-outline-danger" data-ajax-mode="replace" href="/Frame/Delete/' + o.frameId + '"><i class="bx bxs-trash"></i></a></div>';
                }
            }
        ]
    });
});

$("#lnkFrameFilter").click(function () {
    $(this).toggleClass("filter-icon");
    $("#FilterCard").slideToggle("slow");
});

FrameModel.onComplete = function () {
    $("#divFrameModal").modal('show');
}

FrameModel.onDelete = function () {
    tblFrame.ajax.reload(null, false);
}

FrameModel.onSuccess = function (xhr) {
    tblFrame.ajax.reload(null, false);
    $("#divFrameModal").modal('hide');
};

FrameModel.onFailed = function (xhr) {
    tblFrame.ajax.reload(null, false);
    $("#divFrameModal").modal('hide');
};