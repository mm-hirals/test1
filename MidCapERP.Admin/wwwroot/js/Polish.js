'use strict';

var PolishModel = {};
var tblPolish;

$(function () {
    tblPolish = $("#tblPolish").DataTable({
        "searching": false,
        "processing": true,
        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": "/Polish/GetPolishData",
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
                    return '<div class="c-action-btn-group justify-content-start"><a data-ajax-complete="PolishModel.onComplete" data-ajax="true" class="btn btn-icon btn-outline-primary" data-ajax-mode="replace" data-ajax-update="#divUpdatePolish" href="/Polish/Update/' + o.polishId + '"><i class="bx bxs-pencil"></i></a>' +
                        '<a data-ajax-complete="PolishModel.onDelete" data-ajax="true" class="btn btn-icon btn-outline-danger" data-ajax-mode="replace" href="/Polish/Delete/' + o.polishId + '"><i class="bx bxs-trash"></i></a></div>';
                }
            }
        ]
    });
});

$("#lnkPolishFilter").click(function () {
    $(this).toggleClass("filter-icon");
    $("#FilterCard").slideToggle("slow");
});

PolishModel.onComplete = function () {
    $("#divPolishModal").modal('show');
}

PolishModel.onDelete = function () {
    tblPolish.ajax.reload(null, false);
}

PolishModel.onSuccess = function (xhr) {
    tblPolish.ajax.reload(null, false);
    $("#divPolishModal").modal('hide');
};

PolishModel.onFailed = function (xhr) {
    tblPolish.ajax.reload(null, false);
    $("#divPolishModal").modal('hide');
};