$(document).ready(function () {
    debugger;
    var tblCategory = $("#tblActivitylog").DataTable({
        "searching": false,
        "processing": true,
        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": "/Product/GetProductActivityDataById",
            "type": "POST",
            "datatype": "json",
            "data": function (d) {
                d.productId = $("#hdnProductId").val().trim()
            }
        },
        "columns": [
            { "data": "description", "name": "Description", "autoWidth": true },
            { "data": "action", "name": "Action", "autoWidth": true },
            { "data": "createdByName", "name": "CreatedByName", "autoWidth": true },
            { "data": "createdDate", "name": "CreatedDate", "autoWidth": true }
        ]
    });
});