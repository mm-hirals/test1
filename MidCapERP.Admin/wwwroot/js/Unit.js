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
                        '<a id="' + o.lookupValueId + '" class="btn btn-icon btn-outline-danger btnRemoveUnit"><i class="bx bxs-trash"></i></a></div>';
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
$(document).delegate(".btnRemoveUnit", "click", function () {
    if (!$.isEmptyObject(this.id) && this.id > 0) {
        SweetAlert("Home", this.id, DeleteUnit);
    }
    else {
        errorMessage("Oops...", "Something went wrong!", "error");
    }
});

function DeleteUnit(id) {
    if (!$.isEmptyObject(id) && id > 0) {
        $.ajax({
            url: "/Unit/Delete/?Id=" + id,
            type: "GET",
            success: function (response) {
                message("Deleted!", "Your record has been deleted.", "success");
                tblUnit.ajax.reload();
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
    $("#unitName").val('')
    $('#tblUnit').dataTable().fnDraw();
});

$(document).on('click', '#btnReset', function (e) {
    $("#unitName").val('')
    $('#tblUnit').dataTable().fnDraw();
});