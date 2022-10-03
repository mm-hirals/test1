'use strict';

var OrderModel = {};
var tblOrder;

$(function () {
    tblOrder = $("#tblOrder").DataTable({
        "searching": false,
        "processing": true,
        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": "/Order/GetOrderData",
            "type": "POST",
            "datatype": "json",
            "data": function (d) {
                d.customerName = $("#customerName").val().trim(),
                d.phoneNumber = $("#phoneNumber").val().trim(),
                d.status = $("#status").val().trim(),
                d.orderDate = $("#orderDate").val().trim(),
                d.deliveryDate = $("#deliveryDate").val().trim()
            }
        },
        "columns": [
            { "data": "orderNo", "name": "OrderNo", "autoWidth": true },
            { "data": "createdByName", "name": "CreatedByName", "autoWidth": true },
            { "data": "customerName", "name": "CustomerName", "autoWidth": true },
            { "data": "payableAmount", "name": "PayableAmount", "autoWidth": true },
            { "data": "status", "name": "Status", "autoWidth": true },
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

$('#reffereBy').keyup(function () {
    tblOrder.ajax.reload(null, false);
});

$('#customerName').keyup(function () {
    tblOrder.ajax.reload(null, false);
});

$('#phoneNumber').keyup(function () {
    tblOrder.ajax.reload(null, false);
});

$('#status').keyup(function () {
    tblOrder.ajax.reload(null, false);
});

$("#orderDate").on("input", function () {
    tblOrder.ajax.reload(null, false);
});

$("#deliveryDate").on("input", function () {
    tblOrder.ajax.reload(null, false);
});

$(document).ready(function () {
    $("#divCustomerInfo").load('/Order/CustomerDetail' + "?CustomerId=" + $("#hdnCustomerId").val());
});