// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
formatMoney();
function formatMoney(){
    $('.vnMoney').each(function () {
        var item = $(this).text();
        item = item.replace(",","");
        var num = Number(item).toLocaleString('en');    
        
        $(this).text(num);
    });
}
function hasClass(element, cls) {
    return (' ' + element.className + ' ').indexOf(' ' + cls + ' ') > -1;
}