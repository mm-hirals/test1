'use strict';

var CustomerModel = {};
var tblCustomer;
var value_check = new Array();

InitSelect2();

$(function () {
    tblCustomer = $("#tblCustomer").DataTable({
        "searching": false,
        "processing": true,
        "serverSide": true,
        "filter": true,
        "iDisplayLength": 50,
        "ajax": {
            "url": "/Customer/GetCustomersData",
            "type": "POST",
            "datatype": "json",
            "data": function (d) {
                d.refferedBy = $("#refferedBy").val();
                d.customerName = $("#customerName").val().trim();
                d.customerMobileNo = $("#customerMobileNo").val().trim();
                d.customerFromDate = $("#customerFromDate").val().trim();
                d.customerToDate = $("#customerToDate").val().trim();
            }
        },
        "columnDefs": [
            {
                "orderable": false,
                "targets": 0
            }
        ],
        "columns": [
            {
                "bSortable": false,
                "mRender": (data, type, row) => {
                    return '<div class="c-action-btn-group justify-content-start"><input type="checkbox" class="case" value="' + row.customerId + '" id="' + row.customerId + '" /></div>';
                }
            },
            {
                "render": (data, type, row) => {
                    return row.firstName + " " + row.lastName;
                }
            },
            { "data": "refferedName", "name": "RefferedName", "autoWidth": true },
            { "data": "emailId", "name": "emailId", "autoWidth": true },
            { "data": "phoneNumber", "name": "phoneNumber", "autoWidth": true },
            { "data": "createdDateFormat", "name": "createdDateFormat", "autoWidth": true },
            {
                "bSortable": false,
                "mRender": (data, type, row) => {
                    return '<div class="c-action-btn-group justify-content-end"><a  href="/Customer/Update/' + row.customerId + '" class="btn btn-icon btn-outline-primary"><i class="bx bxs-pencil"></i></a>' +
                        '<a id="' + row.customerId + '" class="btn btn-icon btn-outline-danger delete-customer" data-ajax-mode="replace"><i class="bx bxs-trash"></i></a></div>';
                    
                }
            }
        ]
    });
});

$('#customerName').keyup(function () {
    tblCustomer.ajax.reload(null, false);
});

$('#customerMobileNo').keyup(function () {
    tblCustomer.ajax.reload(null, false);
});

$("#customerFromDate").on("input", function () {
    tblCustomer.ajax.reload(null, false);
});

$("#customerToDate").on("input", function () {
    tblCustomer.ajax.reload(null, false);
});

$('#refferedBy').change(function () {
    tblCustomer.ajax.reload(null, false);
});

$("#lnkCustomerFilter").click(function () {
    $(this).toggleClass("filter-icon");
    $("#FilterCard").slideToggle("slow");
});

CustomerModel.onSuccess = function (xhr) {
    tblCustomer.ajax.reload(null, false);
    $("#divCustomerModal").modal('hide');
};

CustomerModel.onFailed = function (xhr) {
    tblCustomer.ajax.reload(null, false);
    $("#divCustomerModal").modal('hide');
};

// Check all checkbox values and store it in array
$("#selectall").click(function () {
    if (this.checked) {
        if (value_check.length > 0) {
            value_check = [];
        }
        $('.case').prop('checked', true);
        for (var i = 0; i < $(".case:checked").length; i++) {
            value_check.push($(".case:checked")[i].id);
        }
    }
    else {
        $('.case').prop('checked', false);
        value_check = [];
    }
});

// Check single checkbox value and store it in array
$('#tblCustomer').on('click', 'input[type="checkbox"]', function () {
    if ($(this).prop("checked")) {
        value_check.push($(this).val());
    }
    else {
        var a = value_check.findIndex(q => q == $(this).val());
        if (a > -1) {
            value_check.splice(a, 1);
        }
    }
});

// On button click open modal popup
$("#multiSelectCustomer").click(function () {
    if (value_check.length > 0) {
        $("#sendSMSModal").modal('show');
    }
    else {
        //Swal.fire('Please select customer to send message.')
        toastr.error('Please select customer to send message.');
    }
});

// On button click send checkbox values to controller
$(".sendSMSToClient").click(function () {
    if ($("#txtMessage").val() == '') {
        $("#errorMessage").text("Please enter message.");
    }
    else if ($("#txtSubject").val() == '') {
        $("#errorSubject").text("Please enter subject.");
    }
    else {
        $('.sendSMSToClient').buttonLoader('start');
        $("#errorMessage").hide();

        if ($("#selectall").prop('checked')) {
            var customerName = $("#customerName").val().trim();
            var customerMobileNo = $("#customerMobileNo").val().trim();
            var customerFromDate = $("#customerFromDate").val().trim();
            var customerToDate = $("#customerToDate").val().trim();
        }

        var data = {
            CustomerName: customerName,
            CustomerMobileNo: customerMobileNo,
            CustomerFromDate: customerFromDate,
            CustomerToDate: customerToDate,
            IsCheckedAll: $("#selectall").prop('checked'),
            CustomerList: value_check,
            Message: $("#txtMessage").val(),
            Subject: $("#txtSubject").val()
        };
        $.ajax({
            url: "/Customer/MultipleSendCustomer",
            type: "POST",
            data: { model: data },
            success: function (response) {
                $('.sendSMSToClient').buttonLoader('stop');
                if (response == "success") {
                    toastr.success('Messages sent successfully.');
                    $('#sendSMSModal').modal('hide');
                }
                else
                    toastr.error(response, 'Error in sending messages.');
            }
        });
    }
});

// Reset modal values after close
$('#sendSMSModal').on('hidden.bs.modal', function () {
    $(this).find('form').trigger('reset');
})

$(document).on('submit', '#frmCusomerDetails', function (e) {
    $('#dataSave').buttonLoader('start');
    toastr.success('Information saved successfully.');
}); 


// Delete Customer
$(document).delegate(".delete-customer", "click", function () {
    if (!$.isEmptyObject(this.id) && this.id > 0) {
        SweetAlert("Home", this.id, DeleteCustomer);
    }
    else {
        errorMessage("Oops...", "Something went wrong!", "error");
    }
});

function DeleteCustomer(id) {
    if (!$.isEmptyObject(id) && id > 0) {
        $.ajax({
            url: "/Customer/Delete?Id=" + id,
            type: "GET",
            success: function (response) {
                message("Deleted!", "Your record has been deleted.", "success");
                tblCustomer.ajax.reload();
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                errorMessage("Oops...", "Something went wrong!", "error");
            }
        });
    } else {
        errorMessage("Oops...", "Something went wrong!", "error");
    }
} 

$(document).on('click', '#btnReset', function (e) {
    $("#select2-refferedBy-container").text("Select Reffered");
    $("#refferedBy").val('');
    $("#customerName").val('');
    $("#customerMobileNo").val('');
    $("#customerFromDate").val('');
    $("#customerToDate").val('');
    $('#tblCustomer').dataTable().fnDraw();
});