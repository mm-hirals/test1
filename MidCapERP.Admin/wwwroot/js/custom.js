(function ($) {
    $.fn.buttonLoader = function (action) {
        var self = $(this);
        if (action == 'start') {
            if ($(self).attr("disabled") == "disabled") {
                return false;
            }
            $(self).attr("disabled", true);
            $(self).append('<i class="bx bx-loader-alt btn-spinner"></i>');
            $(self).addClass('active');
        }
        if (action == 'stop') {
            $(self).find('.btn-spinner').remove();
            $(self).removeClass('active');
            $(self).attr("disabled", false);
        }
    }
})(jQuery);

function message(title, message, type) {
    swal(title, message, type);
}
function errorMessage(title, message, type) {
    swal(title, message, type);
}

// function
function SweetAlert(title, id, callback) {
    swal({
        title: "Are you sure?",
        text: "You will not be able to recover this record! " + title,
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Yes, delete it!",
        closeOnConfirm: false
    },
        function (isConfirm) {
            if (isConfirm) {
                callback(id);
            }
        }
    );
}
document.addEventListener("DOMContentLoaded", function (event) {
    const showNavbar = (toggleId, navId, bodyId, headerId) => {
        const toggle = document.getElementById(toggleId),
            nav = document.getElementById(navId),
            bodypd = document.getElementById(bodyId),
            headerpd = document.getElementById(headerId)

        // Validate that all variables exist
        if (toggle && nav && bodypd && headerpd) {
            toggle.addEventListener('click', () => {
                // Hide navbar
                nav.classList.toggle('collapsed')
                // show navbar
                nav.classList.toggle('navbar-show')
                // change icon
                toggle.classList.toggle('bx-x')
                // add padding to body
                bodypd.classList.toggle('body-pd')
                // add padding to header
                headerpd.classList.toggle('body-pd')
            })
        }
    }

    showNavbar('header-toggle', 'nav-bar', 'body-pd', 'header')

    /*===== LINK ACTIVE =====*/
    const linkColor = document.querySelectorAll('.nav_link')

    function colorLink() {
        if (linkColor) {
            linkColor.forEach(l => l.classList.remove('active'))
            this.classList.add('active')
        }
    }
    linkColor.forEach(l => l.addEventListener('click', colorLink))

    // Your code to run since DOM is loaded and ready
});


/*$(".submenu-link").click(function () {
    $(".sub-menu").toggle().animate({}, 500);
});*/

$('.sub-menu').hide();
$(".submenu-link").click(function () {
    $(this).parent(".submenu-item").children(".sub-menu").slideToggle("500");
});

/*===== LOGIN PASSWORD SHOW =====*/
$(".toggle-password").click(function () {

    $(this).toggleClass("field-icon bxs-hide");
    var input = $($(this).attr("toggle"));
    if (input.attr("type") == "password") {
        input.attr("type", "text");
    } else {
        input.attr("type", "password");
    }
});


//Chart JS For Dashboard page
var xValues = [100, 200, 300, 400, 500, 600, 700, 800, 900, 1000];

new Chart("myChart", {
    type: "line",
    data: {
        labels: xValues,
        datasets: [{
            data: [860, 1140, 1060, 1060, 1070, 1110, 1330, 2210, 7830, 2478],
            borderColor: "#98bdff",
            fill: false
        }, {
            data: [1600, 1700, 1700, 1900, 2000, 2700, 4000, 5000, 6000, 7000],
            borderColor: "#f3797e",
            fill: false
        }, {
            data: [300, 700, 2000, 5000, 6000, 4000, 2000, 1000, 200, 100],
            borderColor: "#4b49ac",
            fill: false
        }]
    },
    options: {
        legend: { display: false }
    }
});


//Chart JS For Dashboard page

var xValues = ["Italy", "France", "Spain", "USA", "Argentina"];
var yValues = [55, 49, 44, 24, 20];
var barColors = ["#4b49ac", "#606060", "#98bdff", "#f3797e", "brown"];

new Chart("myCharts", {
    type: "bar",
    data: {
        labels: xValues,
        datasets: [{
            backgroundColor: barColors,
            data: yValues
        }]
    },
    options: {
        legend: { display: false },
        title: {
            display: true,
            text: ""
        }
    }
});

function InitSelect2(objParent) {
    if (!$.isEmptyObject(objParent)) {
        $(".drpSelect2").select2({
            placeholder: "Please Select",
            allowClear: false,
            dropdownParent: objParent
        });
    }
    else {
        $(".drpSelect2").select2({
            placeholder: "Please Select",
            allowClear: false,
        });
    }
}

function restrictAlphabets(evt) {
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    return true;
}

function restrictNumber(e) {
    return (e.charCode > 64 && e.charCode < 91) || (e.charCode > 96 && e.charCode < 123) || e.charCode == 32;
}

function limit(element,max) {
    if (element.value.length > max) {
        element.value = element.value.substr(0, max);
    }
}