'use strict';

var PolishModel = {};
var tblPolish;

$(function () {
    tblPolish = $("#tblPolish").DataTable({
        "searching": false,
        "processing": true,
        "serverSide": true,
        "iDisplayLength": 50,
        "filter": true,
        "ajax": {
            "url": "/Polish/GetPolishData",
            "type": "POST",
            "datatype": "json",
            "data": function (d) {
                d.title = $("#title").val().trim();
                d.model = $("#modelNo").val().trim();
                d.company = $("#companyName").val().trim();
            }
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
                    return '<div class="c-action-btn-group justify-content-end"><a data-ajax-complete="PolishModel.onComplete" data-ajax="true" class="btn btn-icon btn-outline-primary" data-ajax-mode="replace" data-ajax-update="#divUpdatePolish" href="/Polish/Update/' + o.polishId + '"><i class="bx bxs-pencil"></i></a>' +
                        '<a data-ajax-complete="PolishModel.onDelete" data-ajax="true" class="btn btn-icon btn-outline-danger" data-ajax-mode="replace" data-ajax-confirm="Are you sure you want to delete?" href="/Polish/Delete/' + o.polishId + '"><i class="bx bxs-trash"></i></a></div>';
                }
            }
        ]
    });
});

$("#lnkPolishFilter").click(function () {
    $(this).toggleClass("filter-icon");
    $("#FilterCard").slideToggle("slow");
});

$("#modelNo,#companyName,#title").keyup("input", function () {
    tblPolish.ajax.reload(null, false);
});

PolishModel.onComplete = function () {
    $("#divPolishModal").modal('show');
}

PolishModel.onDelete = function () {
    tblPolish.ajax.reload(null, false);
    toastr.error('Data deleted successfully.');
}

PolishModel.onSuccess = function (xhr) {
    tblPolish.ajax.reload(null, false);
    $("#divPolishModal").modal('hide');
    toastr.success('Information saved successfully.');
};

PolishModel.onFailed = function (xhr) {
    tblPolish.ajax.reload(null, false);
    $("#divPolishModal").modal('hide');
};