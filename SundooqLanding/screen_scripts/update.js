﻿function Manage(tag) {
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
$(document).ready(function () {
    (function ($, W, D) {
        var JQUERY4U = {};
        JQUERY4U.UTIL =
        {
            setupFormValidation: function () {
                $("#update-form").validate({
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
                            var data = {
                                '_mail': mail,
                                '_password': pass,
                                '_gender': gender,
                                '_dob': dob
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
    $('.tab').click(function () {
        $('.tab').removeClass('btn-outline-color');
        $('.tab').addClass('btn-color');
        $(this).addClass('btn-outline-color');
        $(this).removeClass('btn-color');
        $('.contenttab').addClass('hidden');
        $('.' + $(this).data('div')).removeClass('hidden');
    });
});
function filtertags() {
    $('.tagsbtn').each(function () {
        if ($(this).html().toLowerCase().indexOf($('#tagfilter').val().toLowerCase()) > -1) {
            $(this).show();
        }
        else {
            $(this).hide();
        }
    });
}
function filtersources() {
    $('.sourcebtn').each(function () {
        if ($(this).html().toLowerCase().indexOf($('#sourcefilter').val().toLowerCase()) > -1) {
            $(this).show();
        }
        else {
            $(this).hide();
        }
    });
}
function filtermoresources() {
    $('.moresourcebtn').each(function () {
        if ($('#moresourcefilter').val() != "" && $(this).html().toLowerCase().indexOf($('#moresourcefilter').val().toLowerCase()) > -1) {
            $(this).removeClass("hidden");
        }
        else {
            $(this).addClass("hidden");
        }
    });
}

$(document).ready(function () {
    $("#CustomTag").keyup(function (event) {
        if (event.keyCode == 13) {
            var Tagname = $('#CustomTag').val();
            $('#TagsTabToAppendTo').append('<a class="btn btn-dark tagsbtn">' + Tagname + '</a>&nbsp');
        }
    });
});
