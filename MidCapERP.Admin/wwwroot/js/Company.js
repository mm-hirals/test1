'use strict';

var CompanyModel = {};
var tblCompany;

$(function () {
    tblCompany = $("#tblCompany").DataTable({
        "searching": false,
        "processing": true,
        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": "/Company/GetCompanyData",
            "type": "POST",
            "datatype": "json"
        },
        "columns": [
            { "data": "lookupValueName", "name": "LookupValueName", "autoWidth": true },
            {
                "mData": null, "bSortable": false,
                "mRender": function (o) {
                    return '<div class="c-action-btn-group justify-content-start"><a data-ajax-complete="CompanyModel.onComplete" data-ajax="true" class="btn btn-icon btn-outline-primary" data-ajax-mode="replace" data-ajax-update="#divUpdateCompany" href="/Company/Update/' + o.lookupValueId + '"><i class="bx bxs-pencil"></i></a>' +
                        '<a data-ajax-complete="CompanyModel.onDelete" data-ajax="true" class="btn btn-icon btn-outline-danger" data-ajax-mode="replace" href="/Company/Delete/' + o.lookupValueId + '"><i class="bx bxs-trash"></i></a></div>';
                }
            }
        ]
    });
});

$("#lnkCompanyFilter").click(function () {
    $(this).toggleClass("filter-icon");
    $("#FilterCard").slideToggle("slow");
});

CompanyModel.onComplete = function () {
    $("#divCompanyModal").modal('show');
}

CompanyModel.onDelete = function () {
    tblCompany.ajax.reload();
}

CompanyModel.onSuccess = function (xhr) {
    tblCompany.ajax.reload();
    $("#divCompanyModal").modal('hide');
};

CompanyModel.onFailed = function (xhr) {
    tblCompany.ajax.reload();
    $("#divCompanyModal").modal('hide');
};