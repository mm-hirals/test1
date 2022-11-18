'use strict';

var InteriorModel = {};
var tblInterior;
var value_check = new Array();

$(function () {
    tblInterior = $("#tblInterior").DataTable({
        "searching": false,
        "processing": true,
        "serverSide": true,
        "filter": true,
        "iDisplayLength": 50,
        "ajax": {
            "url": "/Interior/GetInteriorsData",
            "type": "POST",
            "datatype": "json",
            "data": function (d) {
                d.customerName = $("#interiorName").val().trim();
                d.customerMobileNo = $("#interiorMobileNo").val().trim();
                d.customerFromDate = $("#interiorFromDate").val().trim();
                d.customerToDate = $("#interiorToDate").val().trim();
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
            { "data": "emailId", "name": "emailId", "autoWidth": true },
            { "data": "phoneNumber", "name": "phoneNumber", "autoWidth": true },
            { "data": "createdDateFormat", "name": "CreatedDateFormat", "autoWidth": true },
            {
                "bSortable": false,
                "mRender": (data, type, row) => {
                    return '<div class="c-action-btn-group justify-content-end"><a  href="/Interior/Update/' + row.customerId + '" class="btn btn-icon btn-outline-primary"><i class="bx bxs-pencil"></i></a></div>';
                }
            }
        ]
    });
});

$('#interiorName').keyup(function () {
    tblInterior.ajax.reload(null, false);
});

$('#interiorMobileNo').keyup(function () {
    tblInterior.ajax.reload(null, false);
});

$("#interiorFromDate").on("input", function () {
    tblInterior.ajax.reload(null, false);
});

$("#interiorToDate").on("input", function () {
    tblInterior.ajax.reload(null, false);
});

$("#lnkInteriorFilter").click(function () {
    $(this).toggleClass("filter-icon");
    $("#FilterCard").slideToggle("slow");
});

InteriorModel.onSuccess = function (xhr) {
    tblInterior.ajax.reload(null, false);
    $("#divInteriorModal").modal('hide');
};

InteriorModel.onFailed = function (xhr) {
    tblInterior.ajax.reload(null, false);
    $("#divInteriorModal").modal('hide');
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
$('#tblInterior').on('click', 'input[type="checkbox"]', function () {
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
$("#multiSelectInterior").click(function () {
    if (value_check.length > 0) {
        $("#sendSMSModal").modal('show');
    }
    else {
        //Swal.fire('Please select interior to send message.')
        toastr.error('Please select interior to send message.');
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
            var interiorName = $("#interiorName").val().trim();
            var interiorMobileNo = $("#interiorMobileNo").val().trim();
            var interiorFromDate = $("#interiorFromDate").val().trim();
            var interiorToDate = $("#interiorToDate").val().trim();
        }

        var data = {
            CustomerName: interiorName,
            CustomerMobileNo: interiorMobileNo,
            CustomerFromDate: interiorFromDate,
            CustomerToDate: interiorToDate,
            IsCheckedAll: $("#selectall").prop('checked'),
            CustomerList: value_check,
            Message: $("#txtMessage").val(),
            Subject: $("#txtSubject").val()
        };
        console.log(data);
        $.ajax({
            url: "/Interior/MultipleSendInterior",
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

$(document).on('submit', '#frmInteriorEdit', function (e) {
    $('#dataSave').buttonLoader('start');
    toastr.success('Information saved successfully.');
});

$(document).on('click', '#btnReset', function (e) {
    $("#interiorName").val('');
    $("#interiorMobileNo").val('');
    $("#interiorFromDate").val('');
    $("#interiorToDate").val('');
    $('#tblInterior').dataTable().fnDraw();
});