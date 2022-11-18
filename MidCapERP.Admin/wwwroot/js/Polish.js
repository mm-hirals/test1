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
                        '<a id="' + o.polishId+'" class="btn btn-icon btn-outline-danger btnRemovePolish" data-ajax-mode="replace" ><i class="bx bxs-trash"></i></a></div>';
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
    $('#btnCreatePolish').buttonLoader('stop');
    $('#btnCreateUpdatePolish').buttonLoader('stop');
    $("#divPolishModal").modal('show');
}

PolishModel.onSuccess = function (xhr) {
    $('#btnCreatePolish').buttonLoader('stop');
    $('#btnCreateUpdatePolish').buttonLoader('stop');
    tblPolish.ajax.reload(null, false);
    $("#divPolishModal").modal('hide');
    toastr.success('Information saved successfully.');
};

PolishModel.onFailed = function (xhr) {
    $('#btnCreatePolish').buttonLoader('stop');
    $('#btnCreateUpdatePolish').buttonLoader('stop');
    tblPolish.ajax.reload(null, false);
    $("#divPolishModal").modal('hide');
};

$(document).delegate("#btnCreatePolish", "click", function () {
    $('#btnCreatePolish').buttonLoader('start');
});

$(document).on('submit', '#frmCreateUpdatePolish', function (e) {
    $('#btnCreateUpdatePolish').buttonLoader('start');
});

$(document).delegate(".btnRemovePolish", "click", function () {
    if (!$.isEmptyObject(this.id) && this.id > 0) {
        SweetAlert("Home", this.id, DeletePolish);
    }
    else {
        errorMessage("Oops...", "Something went wrong!", "error");
    }
});

function DeletePolish(id) {
    if (!$.isEmptyObject(id) && id > 0) {
        $.ajax({
            url: "/Polish/Delete/?Id=" + id,
            type: "GET",
            success: function (response) {
                message("Deleted!", "Your record has been deleted.", "success");
                tblPolish.ajax.reload(null, false);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                errorMessage("Oops...", "Something went wrong!", "error");
            }
        });
    } else {
        errorMessage("Oops...", "Something went wrong!", "error");
    }
}

$(document).on('click', '#btnReset', function (e) {
    $("#title").val('');
    $("#modelNo").val('');
    $("#companyName").val('');
    $('#tblPolish').dataTable().fnDraw();
});