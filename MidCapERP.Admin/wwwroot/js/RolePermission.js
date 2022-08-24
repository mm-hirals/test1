'use strict';

var RolePermissionModel = {};
var tblRolePermission;

$(function () {
    tblRolePermission = $("#tblRolePermission").DataTable({
        "searching": false,
        "processing": true,
        "serverSide": true,
        "filter": true,
        //"ajax": {
        //    "url": "/User/GetUserData",
        //    "type": "POST",
        //    "datatype": "json"
        //},
        //"columns": [
        //    { "data": "firstName", "name": "FirstName", "autoWidth": true },
        //    { "data": "lastName", "name": "LastName", "autoWidth": true },
        //    { "data": "userName", "name": "UserName", "autoWidth": true },
        //    { "data": "email", "name": "Email", "autoWidth": true },
        //    { "data": "phoneNumber", "name": "PhoneNumber", "autoWidth": true },
        //    {
        //        "mData": null, "bSortable": false,
        //        "mRender": function (o) {
        //            return '<div class="c-action-btn-group justify-content-start"><a data-ajax-complete="RolePermissionModel.onComplete" data-ajax="true" class="btn btn-icon btn-outline-primary" data-ajax-mode="replace" data-ajax-update="#divUpdateUser" href="/User/Update/' + o.userId + '"><i class="bx bxs-pencil"></i></a>' +
        //                '<a data-ajax-complete="RolePermissionModel.onDelete" data-ajax="true" class="btn btn-icon btn-outline-danger" data-ajax-mode="replace" href="/User/Delete/' + o.userId + '"><i class="bx bxs-trash"></i></a></div>';
        //        }
        //    }
        //]
    });
});

//$("#lnkRolePermissionFilter").click(function () {
//    $(this).toggleClass("filter-icon");
//    $("#FilterCard").slideToggle("slow");
//});

RolePermissionModel.onComplete = function () {
    $("#divRolePermissionModal").modal('show');
}

//RolePermissionModel.onDelete = function () {
//    tblRolePermission.ajax.reload(null, false);
//}

RolePermissionModel.onSuccess = function (xhr) {
    tblRolePermission.ajax.reload(null, false);
    $("#divRolePermissionModal").modal('hide');
};

RolePermissionModel.onFailed = function (xhr) {
    tblRolePermission.ajax.reload(null, false);
    $("#divRolePermissionModal").modal('hide');
};