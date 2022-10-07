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
    calculateUnitPrice();
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