var current = 1;
var scroll = 1;
var _throttleTimer = null;
var _throttleDelay = 100;
$(document).ready(function () {
    $(window)
        .off('scroll', ScrollHandler)
        .on('scroll', ScrollHandler);
});
function ScrollHandler(e) {
    //throttle event:
    clearTimeout(_throttleTimer);
    _throttleTimer = setTimeout(function () {
        console.log('scroll');
        if ($(window).scrollTop() + $(window).height() == $(document).height()) {
            $("#topicframe").focus();
        }
    });
}
$(document).ready(function () {
    $('.btn-color').click(function () {
        $(this).removeClass('btn-color');
        $(this).addClass('btn-outline-color');
        $('.btn-outline-color').click(function () {
            $(this).addClass('btn-color');
            $(this).removeClass('btn-outline-color');
        });
        Manage($(this).text());
    });
    $('.btn-outline-color').click(function () {
        $(this).addClass('btn-color');
        $(this).removeClass('btn-outline-color');
        $('.btn-color').click(function () {
            $(this).removeClass('btn-color');
            $(this).addClass('btn-outline-color');
        });
        Manage($(this).text());
    });
});
function Manage(tag) {
    $.ajax({
        url: '/user/Manage',
        type: 'post',
        data: {'tag':tag},
        success: function (data) {
        },
        error: function (data) {

        }
    });
}
function toggleTags() {
    $("#tagsdiv").toggle();
}
