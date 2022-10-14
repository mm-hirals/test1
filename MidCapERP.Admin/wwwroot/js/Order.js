'use strict';

var OrderModel = {};
var tblOrder;

$(function () {
    tblOrder = $("#tblOrder").DataTable({
        "searching": false,
        "processing": true,
        "serverSide": true,
        "filter": true,
        "iDisplayLength": 50,
        "ajax": {
            "url": "/Order/GetOrderData",
            "type": "POST",
            "datatype": "json",
            "data": function (d) {
                d.refferedBy = $("#refferedBy").val().trim();
                d.customerName = $("#customerName").val().trim();
                d.phoneNumber = $("#phoneNumber").val().trim();
                d.status = $("#status").val().trim();
                d.orderFromDate = $("#orderFromDate").val().trim();
                d.orderToDate = $("#orderToDate").val().trim();
                d.deliveryFromDate = $("#deliveryFromDate").val().trim();
                d.deliveryToDate = $("#deliveryToDate").val().trim();
            }
        },
        "columns": [
            { "data": "orderNo", "name": "OrderNo", "autoWidth": true },
            { "data": "createdByName", "name": "CreatedByName", "autoWidth": true },
            { "data": "customerName", "name": "CustomerName", "autoWidth": true },
            { "data": "payableAmount", "name": "PayableAmount", "autoWidth": true },
            {
                "data": "status", "name": "Status", "autoWidth": true,
                "mRender": function (o) {
                    if (o == 0) {
                        status = "Pending";
                    } else if (o == 1) {
                        status = "In Progress";
                    }
                    else if (o == 2) {
                        status = "Completed";
                    }
                    else if (o == 3) {
                        status = "Cancelled";
                    }
                    else if (o == 4) {
                        status = "Archieved";
                    }
                    return status;
                }
            },
            { "data": "createdDateFormat", "name": "CreatedDateFormat", "autoWidth": true },
            { "data": "deliveryDateFormat", "name": "DeliveryDateFormat", "autoWidth": true },
            //{ "data": "grossTotal", "name": "GrossTotal", "autoWidth": true },
            //{ "data": "discount", "name": "Discount", "autoWidth": true },
            //{ "data": "totalAmount", "name": "TotalAmount", "autoWidth": true },
            //{ "data": "gstTaxAmount", "name": "GSTTaxAmount", "autoWidth": true },
            {
                "mData": null, "bSortable": false,
                "mRender": function (o) {
                    return '<div class="c-action-btn-group justify-content-start"><a class="btn btn-icon btn-outline-primary" href="/Order/OrderDetail/' + o.orderId + '"><i class="bx bxs-show"></i></a></div> ';
                }
            }
        ]
    });
});

$("#lnkOrderFilter").click(function () {
    $(this).toggleClass("filter-icon");
    $("#FilterCard").slideToggle("slow");
});

$('#refferedBy').change(function () {
    tblOrder.ajax.reload(null, false);
});

$('#customerName,#phoneNumber').keyup(function () {
    tblOrder.ajax.reload(null, false);
});

$('#status').change(function () {
    tblOrder.ajax.reload(null, false);
});

$("#orderFromDate").on("input", function () {
    tblOrder.ajax.reload(null, false);
});

$("#orderToDate").on("input", function () {
    tblOrder.ajax.reload(null, false);
});

$("#deliveryFromDate,#deliveryToDate").on("input", function () {
    tblOrder.ajax.reload(null, false);
});