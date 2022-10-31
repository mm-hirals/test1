'use strict';

var ContractorModel = {};
var tblContractor;

$(function () {
    tblContractor = $("#tblContractor").DataTable({
        "searching": false,
        "processing": true,
        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": "/Contractor/GetContractorData",
            "type": "POST",
            "datatype": "json"
        },
        "columns": [
            { "data": "contractorName", "name": "contractorName", "autoWidth": true },
            { "data": "phoneNumber", "name": "phoneNumber", "autoWidth": true },
            { "data": "imei", "name": "imei", "autoWidth": true },
            { "data": "emailId", "name": "emailId", "autoWidth": true },
            { "data": "categoryName", "name": "CategoryName", "autoWidth": true },
            {
                "mData": null, "bSortable": false,
                "mRender": function (o) {
                    return '<div class="c-action-btn-group justify-content-start"><a data-ajax-complete="ContractorModel.onComplete" data-ajax="true" class="btn btn-icon btn-outline-primary" data-ajax-mode="replace" data-ajax-update="#divUpdateContractor" href="/Contractor/Update/' + o.contractorId + '"><i class="bx bxs-pencil"></i></a>' +
                        '<a data-ajax-complete="ContractorModel.onDelete" data-ajax="true" class="btn btn-icon btn-outline-danger" data-ajax-mode="replace" href="/Contractor/Delete/' + o.contractorId + '"><i class="bx bxs-trash"></i></a></div>';
                }
            }
        ]
    });
});

$("#lnkContractorFilter").click(function () {
    $(this).toggleClass("filter-icon");
    $("#FilterCard").slideToggle("slow");
});

ContractorModel.onComplete = function () {
    $("#divContractorModal").modal('show');
}

ContractorModel.onDelete = function () {
    tblContractor.ajax.reload(null, false);
    toastr.error('Delete Data Successfully.');
}

ContractorModel.onSuccess = function (xhr) {
    tblContractor.ajax.reload(null, false);
    $("#divContractorModal").modal('hide');
    toastr.success('Save Data Successfully.');
};

ContractorModel.onFailed = function (xhr) {
    tblContractor.ajax.reload(null, false);
    $("#divContractorModal").modal('hide');
};