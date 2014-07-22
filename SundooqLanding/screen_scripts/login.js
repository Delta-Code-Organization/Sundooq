$(document).ready(function () {
    (function ($, W, D) {
        var JQUERY4U = {};
        JQUERY4U.UTIL =
        {
            setupFormValidation: function () {
                $("#login-form").validate({
                    rules: {
                        email: {
                            required: true,
                            email: true
                        },
                        password: {
                            required: true,
                            minlength: 5
                        }
                    },
                    messages: {
                        email: {
                            required: "Please enter your e-mail",
                            email: "Please enter a valid e-mail address"
                        },
                        password: {
                            required: "Please enter password",
                            minlength: "Password must be at least 5 characters"
                        }
                    },
                    submitHandler: function (form) {
                        $(function () {
                            //collect values fromt he form
                            $("#msg").addClass("alert-hidden");
                            var mail = $('#email').val();
                            var pass = $('#password').val();
                            //disable the button 
                            var data = { '_mail': mail, '_password': pass };
                            $("#log-form-btn").attr('disabled', 'disabled');
                            ShowLoader('Welcome Back! Please hold on while loading Your Feeds.');
                            $.ajax({
                                url: '/user/login',
                                type: 'post',
                                data: data,
                                success: function (data) {
                                    if (data.result == true) {
                                        $("#msg").removeClass("hidden");
                                        $("#msg").addClass("alert-success");
                                        $("#msg").removeClass("alert-danger");
                                        $("#msg").html(data.Msg);
                                        location.href = "/user/Home";
                                        HideLoader();
                                    }
                                    else {
                                        $("#msg").removeClass("hidden");
                                        $("#msg").removeClass("alert-success");
                                        $("#msg").addClass("alert-danger");
                                        $("#msg").html(data.Msg);
                                        HideLoader();
                                    }
                                    //hide the message slowly than show the button
                                    $("#log-form-btn").removeAttr('disabled');
                                },
                                error: function (data) {
                                    console.log(data.responseText);
                                }
                            });
                        });
                    }
                });
            }
        }
        $(D).ready(function ($) {
            JQUERY4U.UTIL.setupFormValidation();
        });

    })(jQuery, window, document);
});
function statusChangeCallback(response) {
    console.log('statusChangeCallback');
    console.log(response);
    // The response object is returned with a status field that lets the
    // app know the current login status of the person.
    // Full docs on the response object can be found in the documentation
    // for FB.getLoginStatus().
    if (response.status === 'connected') {
        // Logged into your app and Facebook. 
        $("body").append('<img id="loader" class="loaderimg" src="/img/loader.gif"/>');
        $.ajax({
            url: '/User/FacebookLogin',
            type: 'post',
            data: { '_code': response.authResponse.userID },
            success: function (data) {
                location.href = "\\User\\Home";
            },
            error: function (data) {
                console.log("Error while  loging: " + data);
            }
        });
    } else if (response.status === 'not_authorized') {
        // The person is logged into Facebook, but not your app.
        console.log('Please log ' +
          'into this app.');
        FB.login();
        fb_login()
    } else {
        // The person is not logged into Facebook, so we're not sure if
        // they are logged into this app or not.
        console.log('Please log ' +
          'into Facebook.');
        FB.login();
    }
}

// This function is called when someone finishes with the Login
// Button.  See the onlogin handler attached to it in the sample
// code below.
function checkLoginState() {
    FB.getLoginStatus(function (response) {
        statusChangeCallback(response);
    });
}

window.fbAsyncInit = function () {
    FB.init({
        appId: '354831701310539',
        cookie: true,  // enable cookies to allow the server to access 
        // the session
        status: false,
        xfbml: true,  // parse social plugins on this page
        version: 'v2.0' // use version 2.0
    });

    // Now that we've initialized the JavaScript SDK, we call 
    // FB.getLoginStatus().  This function gets the state of the
    // person visiting this page and can return one of three states to
    // the callback you provide.  They can be:
    //
    // 1. Logged into your app ('connected')
    // 2. Logged into Facebook, but not your app ('not_authorized')
    // 3. Not logged into Facebook and can't tell if they are logged into
    //    your app or not.
    //
    // These three cases are handled in the callback function.



};

// Load the SDK asynchronously
(function (d, s, id) {
    var js, fjs = d.getElementsByTagName(s)[0];
    if (d.getElementById(id)) return;
    js = d.createElement(s); js.id = id;
    js.src = "//connect.facebook.net/en_US/sdk.js";
    fjs.parentNode.insertBefore(js, fjs);
}(document, 'script', 'facebook-jssdk'));

// Here we run a very simple test of the Graph API after login is
// successful.  See statusChangeCallback() for when this call is made.
function testAPI() {
    console.log('Welcome!  Fetching your information.... ');
    FB.api('/me', function (response) {
        console.log('Successful login for: ' + response.name);
        alert("Success login");
        console.log(
          'Thanks for logging in, ' + response.name + '!');
    });
}

function fb_login() {
    FB.login(function (response) {
        if (response.status == "connected") {
            ShowLoader("Signing up with facebook...");
            FB.api('/me', function (response) {
                $.ajax({
                    url: '/User/FacebookLogin',
                    type: 'post',
                    data: { '_code': response.id },
                    success: function (data) {
                        location.href = "/User/Home";
                    },
                    error: function (data) {
                        console.log("Error while  loging: " + data);
                    }
                });
                HideLoader();
            });
        } else {
            console.log('User cancelled login or did not fully authorize.');
            FB.login();
        }
    });
}

function tw_login(url) {
    var w = 700;
    var h = 500;
    var left = (screen.width / 2) - (w / 2);
    var top = (screen.height / 2) - (h / 2);
    var myWindow = window.open(url, 'Sundoq login using twitter', 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, resizable=no, copyhistory=no, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);
    var timer = setInterval(function () {
        checkVentana(myWindow);
    }, 500);
}

function checkVentana(myWindow) {
    try {
        var ur = myWindow.location.href;
        if (ur.indexOf('/User/Home') != -1 || ur.indexOf('/User/Activate') != -1) {
            myWindow.close();
            location.href = ur;
        }
    } catch (e) { }
}