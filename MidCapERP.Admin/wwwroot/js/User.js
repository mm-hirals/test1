'use strict';

var UserModel = {};
var tblUser;

$(function () {
    tblUser = $("#tblUser").DataTable({
        "searching": false,
        "processing": true,
        "serverSide": true,
        "filter": true,
        "iDisplayLength": 50,
        "ajax": {
            "url": "/User/GetUserData",
            "type": "POST",
            "datatype": "json",
            "data": function (d) {
                d.name = $("#name").val().trim();
                d.email = $("#email").val().trim();
                d.phoneNumber = $("#phoneNumber").val().trim();
            }
        },
        "columns": [
            { "data": "firstName", "name": "FirstName", "autoWidth": true },
            { "data": "lastName", "name": "LastName", "autoWidth": true },
            { "data": "userName", "name": "UserName", "autoWidth": true },
            { "data": "email", "name": "Email", "autoWidth": true },
            { "data": "userRole", "name": "UserRole", "autoWidth": true },
            { "data": "phoneNumber", "name": "PhoneNumber", "autoWidth": true },
            {
                "mData": null, "bSortable": false,
                "mRender": function (o) {
                    return '<div class="c-action-btn-group justify-content-end"><a data-ajax-complete="UserModel.onComplete" data-ajax="true" class="btn btn-icon btn-outline-primary" data-ajax-mode="replace" data-ajax-update="#divUpdateUser" href="/User/Update/' + o.userId + '"><i class="bx bxs-pencil"></i></a>' +
                        '<a data-ajax-complete="UserModel.onDelete" data-ajax="true" data-ajax-confirm="Are you sure you want to delete?" class="btn btn-icon btn-outline-danger" data-ajax-mode="replace" href="/User/Delete/' + o.userId + '"><i class="bx bxs-trash"></i></a></div>';
                }
            }
        ]
    });
});

$("#lnkUserFilter").click(function () {
    $(this).toggleClass("filter-icon");
    $("#FilterCard").slideToggle("slow");
});

$("#name").keyup("input", function () {
    tblUser.ajax.reload(null, false);
});

$("#email").keyup("input", function () {
    tblUser.ajax.reload(null, false);
});

$("#phoneNumber").keyup("input", function () {
    tblUser.ajax.reload(null, false);
});

UserModel.onComplete = function () {
    $("#divUserModal").modal('show');
}

UserModel.onDelete = function () {
    tblUser.ajax.reload(null, false);
    toastr.error('Data deleted successfully.');
}

UserModel.onSuccess = function (xhr) {
    tblUser.ajax.reload(null, false);
    $("#divUserModal").modal('hide');
    toastr.success('Information saved successfully.');
};

UserModel.onFailed = function (xhr) {
    tblUser.ajax.reload(null, false);
    $("#divUserModal").modal('hide');
};