$(document).ready(function () {
    (function ($, W, D) {
        var JQUERY4U = {};
        JQUERY4U.UTIL =
        {
            setupFormValidation: function () {
                $("#activate-form").validate({
                    rules: {
                        email: {
                            required: true,
                            email: true
                        },
                        password: {
                            required: true,
                            minlength: 5
                        },
                        confirm_password: {
                            required: true,
                            minlength: 5,
                            equalTo: password
                        },
                        dob: {
                            required: true
                        },
                        gender: {
                            required: true
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
                        },
                        confirm_password: {
                            required: "Please confirm your password",
                            minlength: "Password must be at least 5 characters",
                            equalTo: "Password does not match"
                        },
                        dob: {
                            required: "Please enter your birthdate"
                        },
                        gender: {
                            required: "Please select your gender"
                        }
                    },
                    submitHandler: function (form) {
                        $(function () {
                            //collect values fromt he form
                            $("#msg").addClass("alert-hidden");
                            var mail = $('#email').val();
                            var pass = $('#password').val();
                            var gender = $('#gender').val();
                            var dob = $('#dob').val();
                            var tags = "";
                            $('.btn-color').each(function () {
                                if ($(this).text().length > 0)
                                tags += "#" + $(this).text().trim();
                            });
                            if (tags.split('#').length < 3) {
                                $("#msg").removeClass("hidden");
                                $("#msg").removeClass("alert-success");
                                $("#msg").addClass("alert-danger");
                                $("#msg").html("You should select 3 tags/sources to build your content");
                                $("#act-form-btn").removeAttr('disabled');
                                $("#loader").remove();
                                return;
                            }
                            var data = {
                                '_mail': mail,
                                '_password': pass,
                                '_gender': gender,
                                '_dob': dob,
                                '_tags': tags
                            };
                            //disable the button 
                            
                            //$("#act-form-btn").attr('disabled', 'disabled');
                            //$("#act-form-btn").after('<img id="loader" src="/img/loader.gif"/>');
                            ShowLoader('Updating Profile ...');
                            $.ajax({
                                url: '/user/Update',
                                type: 'post',
                                data: data,
                                success: function (data) {
                                    if (data.result == true) {
                                        $("#msg").removeClass("alert-danger");
                                        $("#msg").addClass("alert-success");
                                        $("#msg").removeClass("hidden");
                                        $("#msg").html(data.Msg);
                                        location.href = "/user/home";
                                    }
                                    else {
                                        $("#msg").removeClass("hidden");
                                        $("#msg").removeClass("alert-success");
                                        $("#msg").addClass("alert-danger");
                                        $("#msg").html(data.Msg);
                                    }
                                    //hide the message slowly than show the button
                                    //$("#act-form-btn").removeAttr('disabled');
                                    //$("#loader").remove();
                                    $('html,body').animate({
                                        scrollTop: $("#msg").offset().top
                                    },
           'slow');
                                    HideLoader();
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
    $('.btn-default').click(function () {
        $(this).removeClass('btn-default');
        $(this).addClass('btn-color');
        $('.btn-color').click(function () {
            $(this).addClass('btn-default');
            $(this).removeClass('btn-color');
        });
    });

    $('.btn-color').click(function () {
        $(this).addClass('btn-default');
        $(this).removeClass('btn-color');
        $('.btn-default').click(function () {
            $(this).removeClass('btn-default');
            $(this).addClass('btn-color');
        });
    });
});

function ChangeColor(Color,ele)
{
    if (Color == 1) {
        $("#NM" + ele).removeClass('btn-color');
        $("#NM" + ele).addClass('btn-default');
        $("#NM" + ele).attr('href', 'javascript:ChangeColor(0,' + ele + ')');
    }
    else {
        $("#NM" + ele).addClass('btn-color');
        $("#NM" + ele).removeClass('btn-default');
        $("#NM" + ele).attr('href', 'javascript:ChangeColor(1,' + ele + ')');
    }
}