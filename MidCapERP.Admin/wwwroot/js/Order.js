'use strict';

var OrderModel = {};
var tblOrder;

$(function () {
    tblOrder = $("#tblOrder").DataTable({
        "searching": false,
    });
});

$("#lnkOrderFilter").click(function () {
    $(this).toggleClass("filter-icon");
    $("#FilterCard").slideToggle("slow");
});