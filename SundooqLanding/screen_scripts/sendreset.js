﻿$(document).ready(function () {
    (function ($, W, D) {
        var JQUERY4U = {};
        JQUERY4U.UTIL =
        {
            setupFormValidation: function () {
                $("#reset-form").validate({
                    rules: {
                        email: {
                            required: true,
                            email: true
                        }
                    },
                    messages: {
                        email: {
                            required: "Please enter your email",
                            email: "Please enter a valid email"
                        }
                    },
                    submitHandler: function (form) {
                        $(function () {
                            //collect values fromt he form
                            $("#msg").addClass("alert-hidden");
                            var email = $('#email').val();
                            var data = {
                                '_email': email
                            };
                            //disable the button 
                            
                            $("#reset-form-btn").attr('disabled', 'disabled');
                            ShowLoader("Please wait, preparing your data to reset your password...");
                            $.ajax({
                                url: '/user/sendreset',
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
                                    $("#reset-form-btn").removeAttr('disabled');
                                    HideLoader();
                                    $('html,body').animate({
                                        scrollTop: $("#msg").offset().top
                                    },
           'slow');
                                    
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