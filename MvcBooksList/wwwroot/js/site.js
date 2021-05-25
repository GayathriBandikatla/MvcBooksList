// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    // executes when HTML-Document is loaded and DOM is ready
    console.log("document is ready");


    $(".card").hover(
        function () {
            $(this).addClass('shadow  bg-white rounded').css('cursor', 'default');
        }, function () {
            $(this).removeClass('shadow  bg-white rounded');
        }
    );

    // document ready  
});

$('#Category').change(function () {
    document.getElementById("Subcategory").innerHTML = "";
    $.ajax({
        type: "GET",
        url: "https://localhost:44350/Category/" + $('#Category').val(),
        dataType: "text/plain",
        crossDomain: true,
        success: function (a,b,c) {
            console.log(a);
            console.log(b);
            console.log(c);
            
        },
        error: function (e) {
            console.log(e.responseText);
            var list = JSON.parse(e.responseText);
            console.log(typeof (list));
            for (var subcat in list) {
                $('#Subcategory').append('<option value="' + list[subcat] + '" > ' + list[subcat] + '</option > ')

            }
        }

    })
});



//$('#Category').change(function () {
//    $('#Subcategory').remove();
//    $.getJSON('https://localhost:44350/Category/', { categoryName: $('#category').val() }, function (data) {
//        $.each(data, function () {
//            $('#Subcategory').append('<option value=' +
//                this.ProductID + '>' + this.ProductName + '</option>');
//        });
//    }).fail(function (jqXHR, textStatus, errorThrown) {
//        alert('Error getting products!');
//    });
//});