'use strict';

var RoleModel = {};
var tblRole;

$(function () {
    tblRole = $("#tblRole").DataTable({
        "searching": false,
        "processing": true,
        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": "/Role/GetRoleData",
            "type": "POST",
            "datatype": "json"
        },
        "columns": [
            { "data": "name", "name": "name", "autoWidth": true },
            {
                "mData": null, "bSortable": false,
                "mRender": function (o) {
                    return '<div class="c-action-btn-group justify-content-start"><a href="/RolePermission/Index/' + o.id + '"><i class="bx bxs-pencil"></i></a></div>';
                }
            }
        ]
    });
});

$("#lnkRoleFilter").click(function () {
    $(this).toggleClass("filter-icon");
    $("#FilterCard").slideToggle("slow");
});

$("#lnkToredirect").click(function () {
    window.location.href = "/RolePermission/Index"
});