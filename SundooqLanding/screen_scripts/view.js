var current = 1;
var scroll = 1;
var _throttleTimer = null;
var _throttleDelay = 100;
$(document).ready(function () {
    $(window)
        .off('scroll', ScrollHandler)
        .on('scroll', ScrollHandler);
});
function showPopup(url) {
    newwindow = window.open(url, 'name', 'height=300,width=600,top=200,left=300,resizable');
    if (window.focus) { newwindow.focus() }
}
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
    $('.btn-outline-dark').click(function () {
        $(this).removeClass('btn-outline-dark');
        $(this).addClass('btn-dark');
        $('.btn-dark').click(function () {
            $(this).addClass('btn-outline-dark');
            $(this).removeClass('.btn-dark');
        });
        Manage($(this).text());
    });
    $('.btn-dark').click(function () {
        $(this).addClass('btn-outline-dark');
        $(this).removeClass('btn-dark');
        $('.btn-outline-dark').click(function () {
            $(this).removeClass('btn-outline-dark');
            $(this).addClass('btn-dark');
        });
        Manage($(this).text());
    });
    $('#topicframe').load(function () {
        $(document.getElementById('topicframe').contentWindow.document).keydown(function () {
            if (e.keyCode == 39) {
                ShowLoader("Loading next topic");
                $.ajax({
                    url: '/topics/getNext',
                    type: 'post',
                    data: { '_url': $('#topicframe').attr('src') },
                    success: function (data) {
                        location.href = "/topics/view/" + data;
                    },
                    error: function (data) {
                        console.log(data.responseText);
                        HideLoader();
                    }
                });

                return false;
            }
            else if (e.keyCode == 37) {
                ShowLoader("Loading Previous topic");
                $.ajax({
                    url: '/topics/getprev',
                    type: 'post',
                    data: { '_url': $('#topicframe').attr('src') },
                    success: function (data) {
                        location.href = "/topics/view/" + data;
                    },
                    error: function (data) {
                        console.log(data.responseText);
                        HideLoader();
                    }
                });
            }
        });
    });
});
function Manage(tag) {
    $.ajax({
        url: '/user/Manage',
        type: 'post',
        data: { 'tag': tag },
        success: function (data) {
        },
        error: function (data) {
            console.log(data.responseText);
        }
    });
}
function toggleTags() {
    $("#tagsdiv").toggle();
}
$(document).keydown(function (e) {
    if (e.keyCode == 39) {
        ShowLoader("Loading next topic");
        $.ajax({
            url: '/topics/getNext',
            type: 'post',
            data: { '_url': $('#topicframe').attr('src') },
            success: function (data) {
                location.href = "/topics/view/" + data;
            },
            error: function (data) {
                console.log(data.responseText);
                HideLoader();
            }
        });

        return false;
    }
    else if (e.keyCode == 37) {
        ShowLoader("Loading Previous topic");
        $.ajax({
            url: '/topics/getprev',
            type: 'post',
            data: { '_url': $('#topicframe').attr('src') },
            success: function (data) {
                location.href = "/topics/view/" + data;
            },
            error: function (data) {
                console.log(data.responseText);
                HideLoader();
            }
        });
    }
});

