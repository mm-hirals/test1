var isChecked;
var permissionData = [];

$("input[type='checkbox']").click(function () {
    if ($(this).hasClass("childPermission")) {
        setPermissions(this);
    } else {
        var chkModule = this.checked;
        var allModulePermissions = $(this).parent().parent().next().find('li').find('div.custom-switch').find('input');
        $(allModulePermissions).each(function () {
            if (chkModule) {
                $(this).prop('checked', true);
            } else {
                $(this).prop('checked', false);
            }
            setPermissions(this);
        });
    }
});

function setPermissions(obj) {
    var data = {
        Permission: $(obj).val(),
        IsChecked: obj.checked,
        RoleId: $("#RoleId").val()
    };
    $.ajax({
        url: "/Role/CreateRolePermission",
        type: "POST",
        data: data,
        async: false,
        success: function (response) {
            if (response != "success")
                alert("Error: " + response);
        }
    });
}

$(document).on('submit', '#frmCreateUpdateRolePermission', function (e) {
    $('#btnCreateUpdateRolePermission').buttonLoader('start');
});