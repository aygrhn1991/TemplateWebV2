$(function () {
    $('#menu').metisMenu();
    $('.sidebar-nav').slimScroll({
        height: '100%',
    });
    $('#menu>li>ul>li').click(function (e) {
        e.preventDefault();
        var src = $(this).children('a').attr('href');
        $('#iframe').attr('src', src);
    });
});
