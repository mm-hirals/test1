'use strict';

var ProductModel = {};
var tblProduct;
var value_check = new Array();

$(function () {
    tblProduct = $("#tblProduct").DataTable({
        "searching": false,
        "processing": true,
        "serverSide": true,
        "filter": true,
        "iDisplayLength": 50,
        "ajax": {
            "url": "/Product/GetProductData",
            "type": "POST",
            "datatype": "json",
            "data": function (d) {
                d.categoryName = $("#categoryName").val().trim();
                d.productTitle = $("#productTitle").val().trim();
                d.modelNo = $("#modelNo").val().trim();
                d.publishStatus = $("#publishStatus").val().trim();
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
                    return '<div class="c-action-btn-group justify-content-start"><input type="checkbox" class="case" value="' + row.productId + '" /></div>';
                }
            },
            { "data": "categoryName", "name": "CategoryName", "autoWidth": true },
            { "data": "productTitle", "name": "ProductTitle", "autoWidth": true },
            { "data": "modelNo", "name": "ModelNo", "autoWidth": true },
            /*{ "data": "createdDateFormat", "name": "CreatedDateFormat", "autoWidth": true },*/
            /*{ "data": "createdByName", "name": "CreatedByName", "autoWidth": true },*/
            { "data": "updatedByName", "name": "UpdatedByName", "autoWidth": true },
            { "data": "updatedDateFormat", "name": "UpdatedDateFormat", "autoWidth": true },
            {
                "mData": null, "bSortable": false,
                "mRender": function (x) {
                    if (x.status == 1) {
                        return '<div class="custom-control custom-switch d-flex"><input type="checkbox" class="custom-control-input productStatus" value="' + x.productId + '" id="' + x.productId + '" checked /><label class="custom-control-label" for="' + x.productId + '"></label></div>'
                    }
                    else {
                        return '<div class="custom-control custom-switch d-flex"><input type="checkbox" class="custom-control-input productStatus" value="' + x.productId + '" id="' + x.productId + '" /><label class="custom-control-label" for="' + x.productId + '"></label></div>'
                    }
                }
            },
            {
                "mData": null, "bSortable": false,
                "mRender": function (o) {
                    return '<div class="c-action-btn-group justify-content-end"><a class="btn btn-icon btn-outline-primary" href="/Product/Update/' + o.productId + '"><i class="bx bxs-pencil"></i></a>' +
                        '<a id="' + o.productId + '" class="btn btn-icon btn-outline-danger delete-productDetails" data-ajax-mode="replace"><i class="bx bxs-trash"></i></a></div>';
                }
            }
        ],
    });
});

$("#lnkProductFilter").click(function () {
    $(this).toggleClass("filter-icon");
    $("#FilterCard").slideToggle("slow");
});


$("#categoryName,#productTitle,#modelNo").keyup("input", function () {
    tblProduct.ajax.reload(null, false);
});

$('#publishStatus').change(function () {
    tblProduct.ajax.reload(null, false);
});

// Change Product Status
$('#tblProduct').on('click', 'input.productStatus[type="checkbox"]', function () {
    var data = {
        ProductId: $(this).val(),
        Status: this.checked,
    };
    $.ajax({
        url: "/Product/UpdateProductStatus",
        type: "POST",
        data: data,
        success: function (response) {
            if (response == "success") {
                tblProduct.ajax.reload();
            }
            else
                alert("Error: " + response);
        }
    });
});

// Check all checkbox values and store it in array
$("#selectallProduct").click(function () {
    if (this.checked) {
        if (value_check.length > 0) {
            value_check = [];
        }
        $('.case').prop('checked', true);
        for (var i = 0; i < $(".case:checked").length; i++) {
            value_check.push($(".case:checked")[i].value);
        }
    }
    else {
        $('.case').prop('checked', false);
        value_check = [];
    }
});

// Check single checkbox value and store it in array
$('#tblProduct').on('click', 'input.case[type="checkbox"]', function () {
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

// On button click send checkbox values to controller
$("#multiSelectProduct").click(function () {
    if (value_check.length > 0) {
        if ($("#selectallProduct").prop('checked')) {
            var categoryName = $("#categoryName").val().trim();
            var productTitle = $("#productTitle").val().trim();
            var modelNo = $("#modelNo").val().trim();
            var publishStatus = $("#publishStatus").val().trim();
        }
        var data = {
            CategoryName: categoryName,
            ProductTitle: productTitle,
            ModelNo: modelNo,
            PublishStatus: publishStatus,
            IsCheckedAll: $("#selectallProduct").prop('checked'),
            ProductList: value_check
        };

        $.ajax({
            url: "/Product/PrintProductDetail",
            type: "POST",
            data: { model: data },
            cache: false,
            xhr: function () {
                var xhr = new XMLHttpRequest();
                xhr.onreadystatechange = function () {
                    if (xhr.readyState == 2) {
                        if (xhr.status == 200) {
                            xhr.responseType = "blob";
                        } else {
                            xhr.responseType = "text";
                        }
                    }
                };
                return xhr;
            },
            success: function (data) {
                var blob = data;
                var isIE = false || !!document.documentMode;
                if (isIE) {
                    window.navigator.msSaveBlob(blob, fileName);
                } else {
                    var url = window.URL || window.webkitURL;
                    var link = url.createObjectURL(blob);
                    var a = $("<a />");
                    a.attr("download", "ProductList.pdf");
                    a.attr("href", link);
                    $("body").append(a);
                    a[0].click();
                    $("body").remove(a);
                    toastr.success("PDF is generated successfully.")
                }
            }
        });
    }
    else {
        //Swal.fire('Please select customer to send message.')
        toastr.error('Please select products to print.');
    }
});

$(document).delegate(".delete-productDetails", "click", function () {
    if (!$.isEmptyObject(this.id) && this.id > 0) {
        SweetAlert("Home", this.id, DeleteProduct);
    }
    else {
        errorMessage("Oops...", "Something went wrong!", "error");
    }
});

function DeleteProduct(id) {
    if (!$.isEmptyObject(id) && id > 0) {
        $.ajax({
            url: "/Product/Delete?Id=" + id,
            type: "GET",
            success: function (response) {
                message("Deleted!", "Your record has been deleted.", "success");
                tblProduct.ajax.reload();
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                errorMessage("Oops...", "Something went wrong!", "error");
            }
        });
    } else {
        errorMessage("Oops...", "Something went wrong!", "error");
    }
}