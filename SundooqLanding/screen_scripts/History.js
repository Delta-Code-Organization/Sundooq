var _throttleTimer = null;
var _throttleDelay = 100;
var timer = null;
var StopScroll = false;

$(document).ready(function () {
    $(window)
        .off('scroll', ScrollHandler)
        .on('scroll', ScrollHandler);

    function ScrollHandler(e) {
        //throttle event:
        clearTimeout(_throttleTimer);
        _throttleTimer = setTimeout(function () {
            console.log('scroll');
            if ($(window).scrollTop() + $(window).height() == $(document).height()) {
                if (!StopScroll) {
                    loadMore();
                }
            }
        });
    }


    $('#filter').keydown(function () {
        clearTimeout(timer);
        timer = setTimeout(FilterTopics, 1500)
    });
});

function FilterTopics() {
    var ResultFound = false;
    StopScroll = true;
    ShowLoader('Filtering your history topics...');
    var Keyword = $('#filter').val();
    if (Keyword.trim() == "") {
        Keyword = "sk44@ass-449*as-64900as-x?zc.86!6Q";
        StopScroll = false;
    }
    $.ajax({
        url: '/User/FilterHistory',
        type: 'post',
        data: { 'Keyword': Keyword },
        success: function (data) {
            $('#TopicSection').empty();
            var counter = 0;
            var calc = 0;
            var html = "";
            $.each(data, function (index, History) {
                if (calc == counter - 3) {
                    html += "</div>";
                }
                if (counter % 3 == 0 || counter == 0) {
                    html += "<div class='row'>";
                    calc = counter;
                }
                html += '<a href="/topics/view/' + History.TopicId + '">\
                    <div class="col-md-4 col-lg-4 col-sm-6 feature item yo-anim yo-anim-fade yo-anim-start" data-animation-delay="100">\
                        <img style="width: 100%;max-height:200px;height:200px;" src="' + History.Topics.Img + '" />\
                        <h3 style="color: #000">' + History.Topics.Title + '</h3>\
                    </div>\
                </a>';
                counter++;
                ResultFound = true;
            });
            $('#TopicSection').append(html);
            if (!ResultFound) {
                var noResMsg = '<div id="NoResultFoundMsg" class="alert alert-info"><strong>No Result Found</strong> Please search again, there are no results for your current search criteria.</div>';
                $('#TopicSection').append(noResMsg);
            }
            HideLoader();
        },
        error: function (data) {
            console.log(data.responseText);
        }
    });
}

function loadMore() {
    ShowLoader('Loading more history topics...');
    $.ajax({
        url: '/User/LoadMoreHistory',
        type: 'post',
        data: {},
        success: function (data) {
            var counter = 0;
            var calc = 0;
            var html = "";
            $.each(data, function (index, History) {
                if (calc == counter - 3) {
                    html += "</div>";
                }
                if (counter % 3 == 0 || counter == 0) {
                    html += "<div class='row'>";
                    calc = counter;
                }
                html += '<a href="/topics/view/' + History.TopicId + '">\
                    <div class="col-md-4 col-lg-4 col-sm-6 feature item yo-anim yo-anim-fade yo-anim-start" data-animation-delay="100">\
                        <img style="width: 100%;max-height:200px;height:200px;" src="' + History.Topics.Img + '" />\
                        <h3 style="color: #000">' + History.Topics.Title + '</h3>\
                    </div>\
                </a>';
                counter++;
            });
            $('#TopicSection').append(html);
            HideLoader();
        },
        error: function (data) {
            console.log(data.responseText);
        }
    });
}