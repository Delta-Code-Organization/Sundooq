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
            loadMore();
        }
    });
}
function loadMore() {
    $("body").append('<img id="loader" class="loaderimg" src="/img/loader.gif"/>');
    $.ajax({
        url: '/Topics/load',
        type: 'post',
        data: { 'current': current },
        success: function (data) {
            var html = "";
            if (data.length > 1) {
                html += '<div class="row">\
                        <div id="scroll'+ scroll + '" class="col-sm-7 col-md-7">\
                        <i class="icon-tag tagicon" data-tags="' + data[0].Tags + '"></i>\
                        <a style="color:#000" href="/Topics/View/' + data[0].Id + '">\
                        <img class="timg" src="' + data[0].Img + '" />\
                        <h3>'+ data[0].Title + '</h3>\
                        <h4 class="RankBadge" data-fbn="'+data[0].FB+'" data-twn="'+data[0].TW+'">'+data[0].TW+data[0].FB+'</h4>\
                        <p style="font-size: 14px">'+ data[0].Descr + '</p>\
                        </a>\
                        <p style="font-size: 12px; color: #ddd"><a href="/topics/filter/' + encodeURI(data[0].Source.SourceName) + '">' + data[0].Source.SourceName + '</a> on ' + data[0].formatedDate + '</p>\
                        </div>\
                        <div class="col-sm-5 col-md-5">\
                        <i class="icon-tag tagicon" data-tags="' + data[1].Tags + '"></i>\
                        <a style="color:#000" href="/Topics/View/' + data[1].Id + '">\
                        <img class="timg" src="' + data[1].Img + '" />\
                        <h3>'+ data[1].Title + '</h3>\
                        <h4 class="RankBadge" data-fbn="' + data[1].FB + '" data-twn="' + data[1].TW + '">' + data[1].TW + data[1].FB + '</h4>\
                        <p style="font-size: 14px">'+ data[1].Descr + '</p>\
                        </a>\
                        <p style="font-size: 12px; color: #ddd"><a href="/topics/filter/' + encodeURI(data[1].Source.SourceName) + '">' + data[1].Source.SourceName + '</a> on ' + data[1].formatedDate + '</p>\
                        </div>\
                        </div>';
            }
            if (data.length > 2) {
                html += '<div class="row">\
                        <div class="col-md-4 col-lg-4 col-sm-6 feature item yo-anim yo-anim-fade yo-anim-start" data-animation-delay="100">\
                        <i class="icon-tag tagicon" data-tags="' + data[2].Tags + '"></i>\
                        <a style="color:#000" href="/Topics/View/' + data[2].Id + '">\
                        <img class="timg" src="' + data[2].Img + '" />\
                        <h3>'+ data[2].Title + '</h3>\
                        <h4 class="RankBadge" data-fbn="' + data[2].FB + '" data-twn="' + data[2].TW + '">' + data[2].TW + data[2].FB + '</h4>\
                        <p style="font-size: 14px">'+ data[2].Descr + '</p>\
                        </a>\
                        <p style="font-size: 12px; color: #ddd"><a href="/topics/filter/' + encodeURI(data[2].Source.SourceName) + '">' + data[2].Source.SourceName + '</a> on ' + data[2].formatedDate + '</p>\
                        </div>';
            }
            if (data.length > 3) {
                html += '<div class="row">\
                        <div class="col-md-4 col-lg-4 col-sm-6 feature item yo-anim yo-anim-fade yo-anim-start" data-animation-delay="100">\
                        <i class="icon-tag tagicon" data-tags="' + data[3].Tags + '"></i>\
                        <a style="color:#000" href="/Topics/View/' + data[3].Id + '">\
                        <img class="timg" src="' + data[3].Img + '" />\
                        <h3>'+ data[3].Title + '</h3>\
                        <h4 class="RankBadge" data-fbn="' + data[3].FB + '" data-twn="' + data[3].TW + '">' + data[3].TW + data[3].FB + '</h4>\
                        <p style="font-size: 14px">'+ data[3].Descr + '</p>\
                        </a>\
                        <p style="font-size: 12px; color: #ddd"><a href="/topics/filter/' + encodeURI(data[3].Source.SourceName) + '">' + data[3].Source.SourceName + '</a> on ' + data[3].formatedDate + '</p>\
                        </div>';
            }
            if (data.length > 4) {
                html += '<div class="row">\
                        <div class="col-md-4 col-lg-4 col-sm-6 feature item yo-anim yo-anim-fade yo-anim-start" data-animation-delay="100">\
                        <i class="icon-tag tagicon" data-tags="' + data[4].Tags + '"></i>\
                        <a style="color:#000" href="/Topics/View/' + data[4].Id + '">\
                        <img class="timg" src="' + data[4].Img + '" />\
                        <h3>'+ data[4].Title + '</h3>\
                        <h4 class="RankBadge" data-fbn="' + data[4].FB + '" data-twn="' + data[4].TW + '">' + data[4].TW + data[4].FB + '</h4>\
                        <p style="font-size: 14px">'+ data[4].Descr + '</p>\
                        </a>\
                        <p style="font-size: 12px; color: #ddd"><a href="/topics/filter/' + encodeURI(data[4].Source.SourceName) + '">' + data[4].Source.SourceName + '</a> on ' + data[4].formatedDate + '</p>\
                        </div>';
            }
            $("#loader").remove();
            $('#topics').append(html);
            handleTagClick();
            handleBadges();
            $('html,body').animate({
                scrollTop: $("#scroll" + current).offset().top
            },
            'slow');
            current++;
            scroll = current;
        },
        error: function (data) {
            alert(data);
        }
    });
}
$(document).keydown(function (e) {
    if (e.keyCode == 39) {
        if (scroll == current) {
            loadMore();
            return false;
        }
        else {
            $("body").append('<img id="loader" class="loaderimg" src="/img/loader.gif"/>');
            scroll = scroll + 1;
            $('html,body').animate({
                scrollTop: $("#scroll" + scroll).offset().top
            },
               'slow');
            $("#loader").remove();
            return false;
        }
    }
    else if (e.keyCode == 37) {
        if (scroll == 0)
            return false;
        $("body").append('<img id="loader" class="loaderimg" src="/img/loader.gif"/>');
        if (scroll == current)
            scroll = scroll - 2;
        else
            scroll -= 1;
        $('html,body').animate({
            scrollTop: $("#scroll" + scroll).offset().top
        },
           'slow');
        $("#loader").remove();
        return false;
    }
});
$(document).ready(function () {
    handleTagClick();
    handleBadges();
});
function handleTagClick() {
    $('.tagicon').click(function () {
        var tags = $(this).data('tags');
        var tagsarray = tags.split("#");
        var html = "";
        var currenttags = $('#currenttags').text();
        for (var i = 0; i < tagsarray.length; i++) {
            if (tagsarray[i].trim() != "") {
                if (currenttags.indexOf("#" + tagsarray[i]) > -1)
                    html += '<a class="btn btn-color" style="margin:5px;" data-hover="Button">' + tagsarray[i] + '</a>';
                else
                    html += '<a class="btn btn-outline-color" style="margin:5px;" data-hover="Button">' + tagsarray[i] + '</a>';
            }
        }
        $(".tagsdiv").remove();
        $('body').append('<div class="tagsdiv"><i class="icon-close close"></i>' + html + '</div>');
        HookupTags();
        $('.icon-close').click(function () {
            $(this).parent().remove();
        });
    });
}
function Manage(tag) {
    $.ajax({
        url: '/user/Manage',
        type: 'post',
        data: { 'tag': tag },
        success: function (data) {
        },
        error: function (data) {

        }
    });
}
function HookupTags() {
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
}
function handleBadges() {
    $('.RankBadge').click(function () {
        var old = $(this).text();
        var fb = parseInt($(this).data('fbn'));
        var tw = parseInt($(this).data('twn'));
        $(this).text('Facebook: ' + fb + " Twitter: " + tw);
        $(this).css('width', 'auto');
        return false;
    });
    $('.RankBadge').mouseleave(function () {
        var fb = parseInt($(this).data('fbn')) + parseInt($(this).data('twn'));
        $(this).text(fb);
    });
}
