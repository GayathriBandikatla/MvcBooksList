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