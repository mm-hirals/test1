var isChecked;
var permissionData = [];
$("input[type='checkbox']").click(function () {
    var data = {
        Permission: $(this).val(),
        IsChecked: this.checked,
        AspNetRoleName: $("#RoleName").val()
    };
    $.ajax({
        url: "/Role/CreateRolePermission",
        type: "POST",
        data: data
    });
});

$("#lnkToRoleIndex").click(function () {
    window.location.href = "/Role/Index"
});