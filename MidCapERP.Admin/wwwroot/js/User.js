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
            { "data": "email", "name": "Email", "autoWidth": true },
            { "data": "userRole", "name": "UserRole", "autoWidth": true },
            { "data": "phoneNumber", "name": "PhoneNumber", "autoWidth": true },
            {
                "mData": null, "bSortable": false,
                "mRender": function (o) {
                    return '<div class="c-action-btn-group justify-content-end"><a data-ajax-complete="UserModel.onComplete" data-ajax="true" class="btn btn-icon btn-outline-primary" data-ajax-mode="replace" data-ajax-update="#divUpdateUser" href="/User/Update/' + o.userId + '"><i class="bx bxs-pencil"></i></a>' +
                        '<a id="' + o.userId +'" class="btn btn-icon btn-outline-danger btnRemoveUser"><i class="bx bxs-trash"></i></a></div>';
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
    $('#btnCreateUser').buttonLoader('stop');
    $('#btnCreateUpdateUser').buttonLoader('stop');
    $("#divUserModal").modal('show');
}

UserModel.onDelete = function () {
    tblUser.ajax.reload(null, false);
    toastr.error('Data deleted successfully.');
}

UserModel.onSuccess = function (xhr) {
    $('#btnCreateUser').buttonLoader('stop');
    $('#btnCreateUpdateUser').buttonLoader('stop');
    tblUser.ajax.reload(null, false);
    $("#divUserModal").modal('hide');
    toastr.success('Information saved successfully.');
};

UserModel.onFailed = function (xhr) {
    $('#btnCreateUser').buttonLoader('stop');
    $('#btnCreateUpdateUser').buttonLoader('stop');
    tblUser.ajax.reload(null, false);
    $("#divUserModal").modal('hide');
};

$(document).delegate("#btnCreateUser", "click", function () {
    $('#btnCreateUser').buttonLoader('start');
});

$(document).on('submit', '#frmCreateUpdateUser', function (e) {
    $('#btnCreateUpdateUser').buttonLoader('start');
});
$(document).delegate(".btnRemoveUser", "click", function () {
    if (!$.isEmptyObject(this.id) && this.id > 0) {
        SweetAlert("Home", this.id, DeleteUser);
    }
    else {
        errorMessage("Oops...", "Something went wrong!", "error");
    }
});

function DeleteUser(id) {
    if (!$.isEmptyObject(id) && id > 0) {
        $.ajax({
            url: "/User/Delete?Id=" + id,
            type: "GET",
            success: function (response) {
                message("Deleted!", "Your record has been deleted.", "success");
                tblUser.ajax.reload(null, false);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                errorMessage("Oops...", "Something went wrong!", "error");
            }
        });
    } else {
        errorMessage("Oops...", "Something went wrong!", "error");
    }
}
