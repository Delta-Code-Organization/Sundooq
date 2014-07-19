var current = 1;
var scroll = 1;
var _throttleTimer = null;
var _throttleDelay = 100;
var loading = 0;
var suggested;
$(document).ready(function () {
    $(window)
        .off('scroll', ScrollHandler)
        .on('scroll', ScrollHandler);
    loadSuggested();
    $('.close').click(function () {
        $(this).parent().remove();
    });
});
function update() {
    $.ajax({
        url: '/topics/reload',
        type: 'post',
        data: {},
        success: function (data) {
            console.debug("session updated");
        },
        error: function (data) {

        }
    });
}
function ScrollHandler(e) {
    //throttle event:
    clearTimeout(_throttleTimer);
    _throttleTimer = setTimeout(function () {
        console.log('scroll');
        if ($(window).scrollTop() + $(window).height() >= $(document).height() - 100) {
            loadMore();
        }
    });
}
function loadMore() {
    if (loading == 1)
        return;
    loading = 1;
    ShowLoader('Loading topics...');
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
                        <h4 class="RankBadge" data-fbn="'+ data[0].FB + '" data-twn="' + data[0].TW + '">' + parseInt(data[0].TW) + parseInt(data[0].FB) + '</h4>\
                        <p style="font-size: 14px">' + data[0].ReadyDescription + '</p>\
                        </a>\
                        <p style="font-size: 12px; color: #ddd"><a href="/topics/filter/' + encodeURI(data[0].Source.SourceName) + '">' + data[0].Source.SourceName + '</a> on ' + data[0].formatedDate + '</p>\
                        </div>\
                        <div class="col-sm-5 col-md-5">\
                        <i class="icon-tag tagicon" data-tags="' + data[1].Tags + '"></i>\
                        <a style="color:#000" href="/Topics/View/' + data[1].Id + '">\
                        <img class="timg" src="' + data[1].Img + '" />\
                        <h3>'+ data[1].Title + '</h3>\
                        <h4 class="RankBadge" data-fbn="' + data[1].FB + '" data-twn="' + data[1].TW + '">' + parseInt(data[1].TW) + parseInt(data[1].FB) + '</h4>\
                        <p style="font-size: 14px">' + data[1].ReadyDescription + '</p>\
                        </a>\
                        <p style="font-size: 12px; color: #ddd"><a href="/topics/filter/' + encodeURI(data[1].Source.SourceName) + '">' + data[1].Source.SourceName + '</a> on ' + data[1].formatedDate + '</p>\
                        </div>\
                        </div>';
            }
            html += '<div class="row">';
            if (data.length > 2) {
                html += '<div class="col-md-4 col-lg-4 col-sm-6 feature item yo-anim yo-anim-fade yo-anim-start" data-animation-delay="100">\
                        <i class="icon-tag tagicon" data-tags="' + data[2].Tags + '"></i>\
                        <a style="color:#000" href="/Topics/View/' + data[2].Id + '">\
                        <img class="timg" src="' + data[2].Img + '" />\
                        <h3>'+ data[2].Title + '</h3>\
                        <h4 class="RankBadge" data-fbn="' + data[2].FB + '" data-twn="' + data[2].TW + '">' + parseInt(data[2].TW) + parseInt(data[2].FB) + '</h4>\
                        <p style="font-size: 14px">' + data[2].ReadyDescription + '</p>\
                        </a>\
                        <p style="font-size: 12px; color: #ddd"><a href="/topics/filter/' + encodeURI(data[2].Source.SourceName) + '">' + data[2].Source.SourceName + '</a> on ' + data[2].formatedDate + '</p></div>';
            }
            if (data.length > 3) {
                html += '<div class="col-md-4 col-lg-4 col-sm-6 feature item yo-anim yo-anim-fade yo-anim-start" data-animation-delay="100">\
                        <i class="icon-tag tagicon" data-tags="' + data[3].Tags + '"></i>\
                        <a style="color:#000" href="/Topics/View/' + data[3].Id + '">\
                        <img class="timg" src="' + data[3].Img + '" />\
                        <h3>'+ data[3].Title + '</h3>\
                        <h4 class="RankBadge" data-fbn="' + data[3].FB + '" data-twn="' + data[3].TW + '">' + (parseInt(data[3].TW) + parseInt(data[3].FB)) + '</h4>\
                        <p style="font-size: 14px">' + data[3].ReadyDescription + '</p>\
                        </a>\
                        <p style="font-size: 12px; color: #ddd"><a href="/topics/filter/' + encodeURI(data[3].Source.SourceName) + '">' + data[3].Source.SourceName + '</a> on ' + data[3].formatedDate + '</p></div>';
            }
            if (data.length > 4) {
                html += '<div class="col-md-4 col-lg-4 col-sm-6 feature item yo-anim yo-anim-fade yo-anim-start" data-animation-delay="100">\
                        <i class="icon-tag tagicon" data-tags="' + data[4].Tags + '"></i>\
                        <a style="color:#000" href="/Topics/View/' + data[4].Id + '">\
                        <img class="timg" src="' + data[4].Img + '" />\
                        <h3>'+ data[4].Title + '</h3>\
                        <h4 class="RankBadge" data-fbn="' + data[4].FB + '" data-twn="' + data[4].TW + '">' + parseInt(data[4].TW) + parseInt(data[4].FB) + '</h4>\
                        <p style="font-size: 14px">' + data[4].ReadyDescription + '</p>\
                        </a>\
                        <p style="font-size: 12px; color: #ddd"><a href="/topics/filter/' + encodeURI(data[4].Source.SourceName) + '">' + data[4].Source.SourceName + '</a> on ' + data[4].formatedDate + '</p></div>';
            }
            html += '</div>';
            HideLoader();
            $('#topics').append(html);
            handleTagClick();
            handleBadges();
            $('html,body').animate({
                scrollTop: $("#scroll" + current).offset().top
            },
            'slow');
            current++;
            scroll = current;
            loading = 0;
        },
        error: function (data) {
            console.log(data.responseText);
        }
    });
}
function loadSuggested() {
    $.ajax({
        url: '/User/GetSuggested',
        type: 'post',
        data: {},
        success: function (data) {
            var tags = data.split("#");
            for (var i = 0 ; i < tags.length; i++) {
                if (tags[i].length > 0)
                    $("#tagsbuttons").append('<a style="margin:5px;" class="btn btn-outline-dark">' + tags[i] + '</a>');
            }
            if (tags.length > 1) {
                $("#suggestedTags").removeClass("hidden");
                suggested = data;
                HookupTags();
                $('.icon-close').click(function () {
                    closeSuggested();
                });
            }
        },
        error: function (data) {
            console.debug(data);
        }
    });
}
function closeSuggested() {
    $("#suggestedTags").remove();
    $.ajax({
        url: '/user/ignore',
        type: 'post',
        data: { 'ignored': suggested },
        success: function (data) {
            console.debug("tags ignored: " + suggested);
        },
        error: function (data) {
            console.log(data.responseText);
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
            scroll = scroll + 1;
            $('html,body').animate({
                scrollTop: $("#scroll" + scroll).offset().top
            },
               'slow');
            return false;
        }
    }
    else if (e.keyCode == 37) {
        if (scroll == 0)
            return false;
        if (scroll == current)
            scroll = scroll - 2;
        else
            scroll -= 1;
        $('html,body').animate({
            scrollTop: $("#scroll" + scroll).offset().top
        },
           'slow');
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
                if (currenttags.toLowerCase().indexOf("#" + tagsarray[i].toLowerCase()) > -1)
                    html += '<a class="btn btn-dark" style="margin:5px;" data-hover="Button">' + tagsarray[i] + '</a>';
                else
                    html += '<a class="btn btn-outline-dark" style="margin:5px;" data-hover="Button">' + tagsarray[i] + '</a>';
            }
        }
        $(".tagsdiv").remove();
        $('body').append('<div class="tagsdiv"><i class="icon-close close"></i>' + html + '</div>');
        HookupTags();
        $('.close').click(function () {
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
            console.log(data.responseText);
        }
    });
}
function HookupTags() {
    $('.btn-outline-dark').click(function () {
        $(this).removeClass('btn-outline-dark');
        $(this).addClass('btn-dark');
        $('.btn-dark').click(function () {
            $(this).addClass('btn-outline-dark');
            $(this).removeClass('btn-dark');
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
function reload()
{
    ShowLoader("Loading topics...");
    $('.tagicon').parent().parent().remove();
    $.ajax({
        url: '/user/reload',
        type: 'post',
        data: { },
        success: function (data) {
            current = 0;
            scroll = 0;
            loadMore();

        },
        error: function (data) {
            console.log(data.responseText);
        }
    });
}
