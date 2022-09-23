var isDropZoneInit = true;
Dropzone.autoDiscover = false;
$("#nav-tab").on("shown.bs.tab", function (e) {
    if (isDropZoneInit !== false && e.target.id === "nav-images-tab") {
        Dropzone.autoDiscover = false;
        var myAwesomeDropzone = new Dropzone("#productImagesDropZone", {
            url: "/Product/CreateProductImage",
            paramName: "file",
            autoProcessQueue: false,
            uploadMultiple: true,
            parallelUploads: 100,
            maxFiles: 100,
            acceptedFiles: "image/*",
            init: function () {
                var submitButton = document.querySelector("#submit-all");
                var wrapperThis = this;

                submitButton.addEventListener("click", function () {
                    wrapperThis.processQueue();
                });

                this.on("addedfile", function (file) {
                    var removeButton = Dropzone.createElement("<button class='btn btn-lg dark'>Remove File</button>");

                    removeButton.addEventListener("click", function (e) {
                        e.preventDefault();
                        e.stopPropagation();

                        wrapperThis.removeFile(file);
                    });

                    file.previewElement.appendChild(removeButton);
                });

                this.on("sending", function (file, response, formData) {
                    formData.append("ProductId", $("input.productId").val());
                    formData.append("Files", file);
                });
            }
        });
        myAwesomeDropzone;
        isDropZoneInit = false;
    }
})