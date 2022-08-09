'use strict';

var FabricModel = {};
var tblFabric;

$(function () {
    tblFabric = $("#tblFabric").DataTable({
        "searching": false,
        "processing": true,
        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": "/Fabric/GetFabricData",
            "type": "POST",
            "datatype": "json"
        },
        "columns": [
            { "data": "title", "name": "title", "autoWidth": true },
            { "data": "modelNo", "name": "modelNo", "autoWidth": true },
            { "data": "companyName", "name": "companyName", "autoWidth": true },
            { "data": "unitName", "name": "unitName", "autoWidth": true },
            { "data": "unitPrice", "name": "unitPrice", "autoWidth": true },
            {
                "mData": null, "bSortable": false,
                "mRender": function (o) {
                    console.log(o);
                    return '<div class="c-action-btn-group justify-content-start"><a data-ajax-complete="FabricModel.onComplete" data-ajax="true" class="btn btn-icon btn-outline-primary" data-ajax-mode="replace" data-ajax-update="#divUpdateFabric" href="/Fabric/Update/' + o.fabricId + '"><i class="bx bxs-pencil"></i></a>' +
                        '<a data-ajax-complete="FabricModel.onDelete" data-ajax="true" class="btn btn-icon btn-outline-danger" data-ajax-mode="replace" href="/Fabric/Delete/' + o.fabricId + '"><i class="bx bxs-trash"></i></a></div>';
                }
            }
        ]
    });
});

$("#lnkFabricFilter").click(function () {
    $(this).toggleClass("filter-icon");
    $("#FilterCard").slideToggle("slow");
});

FabricModel.onComplete = function () {
    $("#divFabricModal").modal('show');
}

FabricModel.onDelete = function () {
    tblFabric.ajax.reload(null, false);
}

FabricModel.onSuccess = function (xhr) {
    tblFabric.ajax.reload(null, false);
    $("#divFabricModal").modal('hide');
};

FabricModel.onFailed = function (xhr) {
    tblFabric.ajax.reload(null, false);
    $("#divFabricModal").modal('hide');
};