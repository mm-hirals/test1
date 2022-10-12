'use strict';

var RoleModel = {};
var tblRole;

$(function () {
    tblRole = $("#tblRole").DataTable({
        "searching": false,
        "processing": true,
        "serverSide": true,
        "iDisplayLength": 50, 
        "filter": true,
        "ajax": {
            "url": "/Role/GetRoleData",
            "type": "POST",
            "datatype": "json",
            "data": function (d) {
                d.roleName = $("#roleName").val().trim()
            }
        },
        "columns": [
            { "data": "name", "name": "name", "autoWidth": true },
            {
                "mData": null, "bSortable": false,
                "mRender": function (o) {
                    return '<div class="c-action-btn-group justify-content-start"><a class="btn btn-icon btn-outline-primary" href="/Role/RolePermission/' + o.id + '"><i class="bx bxs-pencil"></i></a></div>';
                }
            }
        ]
    });
});

$("#lnkRoleFilter").click(function () {
    $(this).toggleClass("filter-icon");
    $("#FilterCard").slideToggle("slow");
});

$("#roleName").keyup("input", function () {
    tblRole.ajax.reload(null, false);
});

$("#lnkToredirect").click(function () {
    window.location.href = "/Role/RolePermission"
});