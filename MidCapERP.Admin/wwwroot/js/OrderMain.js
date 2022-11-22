$(document).ready(function () {
    $("#divOrderInfo").load('/Order/GetOrderBasicDetail' + "?OrderId=" + $("#hdnOrderId").val());
    $("#divOrderSetDetailPartial").load('/Order/GetOrderSetDetailData' + "?OrderId=" + $("#hdnOrderId").val());
});

$(document).on("shown.bs.tab", 'button[data-bs-toggle="tab"]', function (e) {
    var tabId = $(e.target).attr("id")
    if (tabId == "nav-activity-tab") {
        $("#divOrderActivityPartial").load('/Order/GetOrderActivity' + "?OrderId=" + $("#hdnOrderId").val());
    }
});

$(document).delegate(".saveDiscount", "click", function (e) {
    var discount = $(this).parent().find("input").val();
    if (discount >= 0 && discount <= 100) {
        var data = {
            DiscountPrice: discount,
            OrderSetItemId: $(this).attr('id')
        };
        $('.saveDiscount').buttonLoader('start');
        $.ajax({
            url: "/Order/SaveDiscount",
            type: "POST",
            data: data,
            success: function (response) {
                $('.saveDiscount').buttonLoader('stop');
                $('#divOrderSetDetailPartial').html(response);
                toastr.success("Discount price updated successfully!");
            }
        });
    } else {
        toastr.error("Please enter discount between 0-100");
        $('.saveDiscount').buttonLoader('stop');
    }
});

$(document).delegate(".approveOrder", "click", function (e) {
    $.ajax({
        url: "/Order/ApproveOrderStatus",
        type: "POST",
        data: { "Id": $('#orderId').val() },
        success: function (response) {
            if (response.status) {
                toastr.success(response.message);
                $("#divOrderInfo").load('/Order/GetOrderBasicDetail' + "?OrderId=" + $("#hdnOrderId").val());
                $("#divOrderSetDetailPartial").load('/Order/GetOrderSetDetailData' + "?OrderId=" + $("#hdnOrderId").val());
            }
            else {
                toastr.error(response.message);
                var errorMessages = "";
                $(response.errorMessages).each(function (i, k) {
                    errorMessages += k + "\n";
                });
                errorMessage("Oops...", errorMessages, "error");
            }
        }
    });
});

$(document).delegate(".declineOrder", "click", function (e) {
    if (!$.isEmptyObject($('#orderId').val()) && $('#orderId').val() > 0) {
        DeclineConfirmation();
    }
    else {
        errorMessage("Oops...", "Something went wrong!", "error");
    }
});

// On modal button click call controller decline method with comment
$(".declineOrderModalBtn").click(function () {
    if ($.isEmptyObject($("#txtComment").val()) == true) {
        $("#errorComment").text("Please enter comment.");
        $("#errorComment").show();
    }
    else {
        $('.declineOrderModalBtn').buttonLoader('start');
        $("#errorComment").hide();

        var data = {
            OrderId: $('#hdnOrderId').val(),
            Comments: $("#txtComment").val()
        };
        $.ajax({
            url: "/Order/DeclineOrderStatus",
            type: "POST",
            data: data,
            success: function (response) {
                $('.declineOrderModalBtn').buttonLoader('stop');
                if (response == "success") {
                    $("#declineOrderModal").modal('hide');
                    $("#divOrderInfo").load('/Order/GetOrderBasicDetail' + "?OrderId=" + $("#hdnOrderId").val());
                    $("#divOrderSetDetailPartial").load('/Order/GetOrderSetDetailData' + "?OrderId=" + $("#hdnOrderId").val());
                }
                else
                    alert("Error: " + response);
            }
        });
    }
});

// Reset modal values after close
$('#declineOrderModal').on('hidden.bs.modal', function () {
    $("#txtComment").val("");
    $("#errorComment").hide();
})