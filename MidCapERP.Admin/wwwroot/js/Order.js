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
            "datatype": "json"
        },
        "columns": [
            { "data": "orderNo", "name": "OrderNo", "autoWidth": true },
            { "data": "createdDate", "name": "CreatedDate", "autoWidth": true },
            { "data": "customerName", "name": "CustomerName", "autoWidth": true },
            { "data": "status", "name": "Status", "autoWidth": true },
            { "data": "grossTotal", "name": "GrossTotal", "autoWidth": true },
            { "data": "discount", "name": "Discount", "autoWidth": true },
            { "data": "totalAmount", "name": "TotalAmount", "autoWidth": true },
            { "data": "createdByName", "name": "CreatedByName", "autoWidth": true },
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