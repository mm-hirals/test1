var isChecked;
var permissionData = [];
$("input[type='checkbox']").click(function () {
    var data = {
        Permission: $(this).val(),
        IsChecked: this.checked,
        AspNetRoleName: $("#RoleName").val()
    };
    console.log(data);
    $.ajax({
        url: "/RolePermission/Create",
        type: "POST",
        data: data
    });
});