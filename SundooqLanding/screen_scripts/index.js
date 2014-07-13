$(document).ready(function () {
    (function ($, W, D) {
        var JQUERY4U = {};
        JQUERY4U.UTIL =
        {
            setupFormValidation: function () {
                $("#reg-form").validate({
                    rules: {
                        email: {
                            required: true,
                            email: true
                        },
                        confirm_email: {
                            required: true,
                            email: true,
                            equalTo:email
                        },
                        confirm_password: {
                            required: true,
                            minlength: 5,
                            equalTo: password
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
                        confirm_email: {
                            required: "Please confirm you email",
                            email: "Please enter a valid e-mail address",
                            equalTo: "Email address does not match"
                        },
                        password: {
                            required: "Please enter password",
                            minlength: "Password must be at least 5 characters"
                        },
                        confirm_password: {
                            required: "Please confirm your password",
                            minlength: "Password must be at least 5 characters",
                            equalTo: "Password does not match"
                        }
                    },
                    submitHandler: function (form) {
                        $(function () {
                            //collect values fromt he form
                            var mail = $('#email').val();
                            var pass = $('#password').val();
                            var data = { '_mail': mail, '_password': pass };
                            //disable the button 
                            $("#reg-form-btn").attr('disabled', 'disabled');
                            ShowLoader("Please wait...");
                            $.ajax({
                                url: '/home/index',
                                type: 'post',
                                data: data,
                                success: function (data) {
                                    if (data.result == true) {
                                        $("#msg").removeClass("alert-danger");
                                        $("#msg").addClass("alert-success");
                                        $("#msg").removeClass("hidden");
                                        $("#msg").html(data.Msg);
                                    }
                                    else {
                                        $("#msg").removeClass("hidden");
                                        $("#msg").removeClass("alert-success");
                                        $("#msg").addClass("alert-danger");
                                        $("#msg").html(data.Msg);
                                    }
                                    //hide the message slowly than show the button
                                    $("#reg-form-btn").removeAttr('disabled');
                                    HideLoader();
                                    $('html,body').animate({
                                        scrollTop: $("#msg" + scroll).offset().top
                                    },
           'slow');

                                },
                                error: function (data) {

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
//facebook login code
// This is called with the results from from FB.getLoginStatus().
function statusChangeCallback(response) {
    console.log('statusChangeCallback');
    console.log(response);
    // The response object is returned with a status field that lets the
    // app know the current login status of the person.
    // Full docs on the response object can be found in the documentation
    // for FB.getLoginStatus().
    if (response.status === 'connected') {
        // Logged into your app and Facebook. 
        ShowLoader("Singing In, Please wait...");
        $.ajax({
            url: '/User/FacebookLogin',
            type: 'post',
            data: { '_code': response.authResponse.userID },
            success: function (data) {
                location.href = "\\User\\Home";
            },
            error: function (data) {
                console.log("Error while  loging: " +data);
            }
        });
    } else if (response.status === 'not_authorized') {
        // The person is logged into Facebook, but not your app.
        console.log('Please log ' +
          'into this app.');
        FB.login();
    } else {
        // The person is not logged into Facebook, so we're not sure if
        // they are logged into this app or not.
        console.log('Please log ' +
          'into Facebook.');
        FB.login();
    }
    HideLoader();
}

// This function is called when someone finishes with the Login
// Button.  See the onlogin handler attached to it in the sample
// code below.
function checkLoginState() {
    FB.getLoginStatus(function(response) {
        statusChangeCallback(response);
    });
}

window.fbAsyncInit = function() {
    FB.init({
        appId:      '511493132284905',
        cookie     : true,  // enable cookies to allow the server to access 
        // the session
        status: false,
        xfbml      : true,  // parse social plugins on this page
        version    : 'v2.0' // use version 2.0
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
(function(d, s, id) {
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
    FB.api('/me', function(response) {
        console.log('Successful login for: ' + response.name);
        console.log(
          'Thanks for logging in, ' + response.name + '!');
    });
}
function fb_login() {
    checkLoginState();
}