'use strict';

var OrderModel = {};
var tblOrder;

InitSelect2();

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
                d.refferedBy = $("#refferedBy").val();
                d.customerName = $("#customerName").val();
                d.phoneNumber = $("#phoneNumber").val();
                d.status = $("#status").val();
                d.orderFromDate = $("#orderFromDate").val();
                d.orderToDate = $("#orderToDate").val();
                d.deliveryFromDate = $("#deliveryFromDate").val();
                d.deliveryToDate = $("#deliveryToDate").val();
            }
        },
        "columns": [
            { "data": "orderNo", "name": "OrderNo", "autoWidth": true },
            { "data": "createdByName", "name": "CreatedByName", "autoWidth": true },
            { "data": "customerName", "name": "CustomerName", "autoWidth": true },
            { "data": "refferedName", "name": "RefferedName", "autoWidth": true },
            { "data": "phoneNumber", "name": "PhoneNumber", "autoWidth": true },
            { "data": "payableAmount", "name": "PayableAmount", "autoWidth": true },
            {
                "data": "status", "name": "Status", "autoWidth": true,
                "mRender": function (o) {
                    if (o == 0) {
                        status = '<span class="form-label badge bg-warning">Inquiry<span>';
                    } else if (o == 1) {
                        status = '<span class="form-label badge bg-secondary">Pending For Approval<span>';
                    }
                    else if (o == 2) {
                        status = '<span class="form-label badge bg-secondary">Approved</span>';
                    }
                    else if (o == 3) {
                        status = '<span class="form-label badge bg-secondary">In Progress</span>';
                    }
                    else if (o == 4) {
                        status = '<span class="form-label badge bg-success">Completed</span>';
                    }
                    else if (o == 5) {
                        status = '<span class="form-label badge bg-secondary">Delivered</span>';
                    }
                    else if (o == 6) {
                        status = '<span class="form-label badge bg-danger">Cancelled</span>';
                    }
                    else if (o == 7) {
                        status = '<span class="form-label badge bg-danger">Material Receive</span>';
                    }
                    else if (o == 8) {
                        status = '<span class="form-label badge bg-danger">Declined</span>';
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
                    return '<div class="c-action-btn-group justify-content-end"><a class="btn btn-icon btn-outline-primary" href="/Order/OrderDetail/' + o.orderId + '"><i class="bx bxs-show"></i></a></div> ';
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

$(document).on('click', '#btnReset', function (e) {
    $("#select2-refferedBy-container").text("Select Reffered");
    $("#select2-status-container").text("Select Reffered");
    $("#refferedBy").val(null);
    $("#customerName").val('');
    $("#phoneNumber").val('');
    $("#status").val(null);
    $("#orderFromDate").val('');
    $("#orderToDate").val('');
    $("#deliveryFromDate").val('');
    $("#deliveryToDate").val('');
    $('#tblOrder').dataTable().fnDraw();
});