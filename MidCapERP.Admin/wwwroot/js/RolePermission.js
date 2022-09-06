var isChecked;
var permissionData = [];
$("input[type='checkbox']").click(function () {
    var data = {
        Permission: $(this).val(),
        IsChecked: this.checked,
        RoleId: $("#RoleId").val()
    };
    $.ajax({
        url: "/Role/CreateRolePermission",
        type: "POST",
        data: data,
        success: function (response) {
            if (response == "sucess")
                window.location.href = "/Role/Index"
            else
                alert("Error: " + response);
        }
    });
});