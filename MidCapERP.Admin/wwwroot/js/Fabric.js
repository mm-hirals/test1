'use strict';

var FabricModel = {};
var tblFabric;

$(function () {
    tblFabric = $("#tblFabric").DataTable({
        "searching": false,
        "processing": true,
        "serverSide": true,
        "filter": true,
        "iDisplayLength": 50,
        "ajax": {
            "url": "/Fabric/GetFabricData",
            "type": "POST",
            "datatype": "json",
            "data": function (d) {
                d.title = $("#title").val().trim();
                d.model = $("#modelNo").val().trim();
                d.company = $("#companyName").val().trim();
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
                    console.log(o);
                    return '<div class="c-action-btn-group justify-content-end"><a data-ajax-complete="FabricModel.onComplete" data-ajax="true" class="btn btn-icon btn-outline-primary" data-ajax-mode="replace" data-ajax-update="#divUpdateFabric" href="/Fabric/Update/' + o.fabricId + '"><i class="bx bxs-pencil"></i></a>' +
                        '<a id="' + o.fabricId +'" class="btn btn-icon btn-outline-danger btnRemoveFabric"><i class="bx bxs-trash"></i></a></div>';
                }
            }
        ]
    });
});

$("#lnkFabricFilter").click(function () {
    $(this).toggleClass("filter-icon");
    $("#FilterCard").slideToggle("slow");
});

$("#title,#modelNo,#companyName").keyup(function () {
    tblFabric.ajax.reload(null, false);
});

FabricModel.onComplete = function () {
    calculateUnitPrice();
    $("#divFabricModal").modal('show');
    $('#btnCreateFabric').buttonLoader('stop');
    $('#btnCreateUpdateFabric').buttonLoader('stop');
}

FabricModel.onSuccess = function (xhr) {
    $('#btnCreateFabric').buttonLoader('stop');
    $('#btnCreateUpdateFabric').buttonLoader('stop');
    tblFabric.ajax.reload(null, false);
    $("#divFabricModal").modal('hide');
    toastr.success('Information saved successfully.');
};

FabricModel.onFailed = function (xhr) {
    $('#btnCreateFabric').buttonLoader('stop');
    $('#btnCreateUpdateFabric').buttonLoader('stop');
    tblFabric.ajax.reload(null, false);
    $("#divFabricModal").modal('hide');
};

$(document).on("change", "input#UnitPrice", (function () {
    $(this).attr('value', $(this).val());
    calculateUnitPrice();
}));

function calculateUnitPrice() {
    var unitPrice = parseInt($("#UnitPrice").val());
    if (unitPrice > 0) {
        calculateRetailerSP(unitPrice);
        calculateWholesalerSP(unitPrice);
    } else {
        $("#RetailerPrice").val("");
        $("#WholesalerPrice").val("");
    }
}

function calculateRetailerSP(unitPrice) {
    var retailerSP = parseInt($("#hdnRetailerSP").val());
    if (retailerSP > 0) {
        var retailerUnitPrice = RoundTo(Math.round(unitPrice + ((unitPrice * retailerSP) / 100)).toFixed(2));
        $("#RetailerPrice").val(retailerUnitPrice.toFixed(2));
    }
}

function calculateWholesalerSP(unitPrice) {
    var wholesalerSP = $("#hdnWholesalerSP").val();
    if (wholesalerSP > 0) {
        var wholesalerUnitPrice = RoundTo(Math.round(unitPrice + ((unitPrice * wholesalerSP) / 100)).toFixed(2));
        $("#WholesalerPrice").val(wholesalerUnitPrice.toFixed(2));
    }
}

function RoundTo(number) {
    var roundto = $("#hdnAmountRoundTo").val();
    if (roundto > 0)
        return roundto * Math.round(number / roundto);
    else
        return number;
}

$(document).delegate("#btnCreateFabric", "click", function () {
    $('#btnCreateFabric').buttonLoader('start');
});

$(document).on('submit', '#frmCreateUpdateFabric', function (e) {
    $('#btnCreateUpdateFabric').buttonLoader('start');
});

$(document).delegate(".btnRemoveFabric", "click", function () {
    if (!$.isEmptyObject(this.id) && this.id > 0) {
        SweetAlert("Home", this.id, DeleteFabric);
    }
    else {
        errorMessage("Oops...", "Something went wrong!", "error");
    }
});

function DeleteFabric(id) {
    if (!$.isEmptyObject(id) && id > 0) {
        $.ajax({
            url: "/Fabric/Delete/?Id=" + id,
            type: "GET",
            success: function (response) {
                message("Deleted!", "Your record has been deleted.", "success");
                tblFabric.ajax.reload(null, false);
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
    $("#companyName").val('');
    $('#tblFabric').dataTable().fnDraw();
});