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
                        Fullname: {
                            required: true,
                            minlength: 5
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
                        Fullname: {
                            required: "Please enter your full name",
                            minlength: "your full name can't be less than 5 characters"
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
                            var fn = $('#Fullname').val();
                            var tags = "";
                            $('.btn-dark').each(function () {
                                if ($(this).text().length > 0)
                                    tags += "#" + $(this).text().trim();
                            });
                            if (tags.split('#').length < 10) {
                                $("#msg").removeClass("hidden");
                                $("#msg").removeClass("alert-success");
                                $("#msg").addClass("alert-danger");
                                $("#msg").html("What do you want to Stay Updated about? Follow at least 10 sources/tags");
                                $("#act-form-btn").removeAttr('disabled');
                                $("#loader").remove();
                                $('html,body').animate({
                                    scrollTop: $("#msg").offset().top
                                },
           'slow');
                                return;
                            }
                            var data = {
                                '_mail': mail,
                                '_password': pass,
                                '_gender': gender,
                                '_dob': dob,
                                '_tags': tags,
                                '_Fullname': fn
                            };
                            //disable the button 

                            //$("#act-form-btn").attr('disabled', 'disabled');
                            //$("#act-form-btn").after('<img id="loader" src="/img/loader.gif"/>');
                            ShowLoader('Please wait, Updating your account...');
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
    $('.btn-outline-dark').click(function () {
        $(this).removeClass('btn-outline-dark');
        $(this).addClass('btn-dark');
        $('.btn-dark').click(function () {
            $(this).addClass('btn-outline-dark');
            $(this).removeClass('btn-dark');
        });
    });
    $('.btn-dark').click(function () {
        $(this).addClass('btn-outline-dark');
        $(this).removeClass('btn-dark');
        $('.btn-outline-dark').click(function () {
            $(this).removeClass('btn-outline-dark');
            $(this).addClass('btn-dark');
        });
    });
});


function CollapseCont(Detector) {
    $('#TagsCont' + Detector).slideToggle(400);
    if ($('#' + Detector).hasClass('arrow-right-open')) {
        $('#' + Detector).removeClass('arrow-right-open');
        $('#' + Detector).addClass('arrow-down-open');
    }
    else {
        $('#' + Detector).removeClass('arrow-down-open');
        $('#' + Detector).addClass('arrow-right-open');
    }

}