'use strict';

var CompanyModel = {};
var tblCompany;

$(function () {
    tblCompany = $("#tblCompany").DataTable({
        "searching": false,
        "processing": true,
        "serverSide": true,
        "iDisplayLength": 50,
        "filter": true,
        "ajax": {
            "url": "/Company/GetCompanyData",
            "type": "POST",
            "datatype": "json",
            "data": function (d) {
                d.companyName = $("#companyName").val().trim()
            }
        },
        "columns": [
            { "data": "lookupValueName", "name": "LookupValueName", "autoWidth": true },
            {
                "mData": null, "bSortable": false,
                "mRender": function (o) {
                    return '<div class="c-action-btn-group justify-content-end"><a data-ajax-complete="CompanyModel.onComplete" data-ajax="true" class="btn btn-icon btn-outline-primary" data-ajax-mode="replace" data-ajax-update="#divUpdateCompany" href="/Company/Update/' + o.lookupValueId + '"><i class="bx bxs-pencil"></i></a>' +
                        '<a id="' + o.lookupValueId + '" class="btn btn-icon btn-outline-danger btnRemoveCompany"><i class="bx bxs-trash"></i></a></div>';
                }
            }
        ]
    });
});

$("#lnkCompanyFilter").click(function () {
    $(this).toggleClass("filter-icon");
    $("#FilterCard").slideToggle("slow");
});

$("#companyName").on("input", function () {
    tblCompany.ajax.reload(null, false);
});

CompanyModel.onComplete = function () {
    $('#btnSaveCompany').buttonLoader('stop');
    $('#btnCreateUpdateCompany').buttonLoader('stop');
    $("#divCompanyModal").modal('show');
}


CompanyModel.onSuccess = function (xhr) {
    $('#btnSaveCompany').buttonLoader('stop');
    $('#btnCreateUpdateCompany').buttonLoader('stop');
    tblCompany.ajax.reload(null, false);
    $("#divCompanyModal").modal('hide');
    toastr.success('Information saved successfully.');
};

CompanyModel.onFailed = function (xhr) {
    $('#btnSaveCompany').buttonLoader('stop');
    $('#btnCreateUpdateCompany').buttonLoader('stop');
    tblCompany.ajax.reload(null, false);
    $("#divCompanyModal").modal('hide');
};

$(document).delegate("#btnSaveCompany", "click", function () {
    $('#btnSaveCompany').buttonLoader('start');
});

$(document).on('submit', '#frmSaveCompany', function (e) {
    $('#btnCreateUpdateCompany').buttonLoader('start');
});
$(document).delegate(".btnRemoveCompany", "click", function () {
    if (!$.isEmptyObject(this.id) && this.id > 0) {
        SweetAlert("Home", this.id, DeleteCompany);
    }
    else {
        errorMessage("Oops...", "Something went wrong!", "error");
    }
});

function DeleteCompany(id) {
    if (!$.isEmptyObject(id) && id > 0) {
        $.ajax({
            url: "/Company/Delete/?Id=" + id,
            type: "GET",
            success: function (response) {
                message("Deleted!", "Your record has been deleted.", "success");
                tblCompany.ajax.reload();
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
    $("#companyName").val('')
    $('#tblCompany').dataTable().fnDraw();
});