$(function () {
    $('#SelectAll').change(function () {
        if ($(this).prop('checked')) {
            $("input[name='mails[]']").prop('checked', true);
        } else {
            $("input[name='mails[]']").prop('checked', false);
        }
        //$("input[name='mails[]']").each(function () {
        //    alert($(this).val());
        //});
    });

    $('#InviteFriendsBtn').click(function () {
        var AllMails = "";
        var NoMailSelected = true;
        $("input[name='mails[]']").each(function () {
            if ($(this).prop('checked')) {
                AllMails += $(this).val() + "#";
                NoMailSelected = false;
            }
        });
        if (NoMailSelected == true) {
            alert("Please select at least one mail to invite !");
            return false;
        }
        ShowLoader("Inviting your friends ...");
        $.ajax({
            url: '/User/InviteGmailFriendsNow',
            type: 'post',
            data: { 'Mails': AllMails },
            success: function (data) {
                $("#msg").removeClass("hidden");
                $("#msg").addClass("alert-success");
                $("#msg").removeClass("alert-danger");
                HideLoader();
            },
            error: function (data) { console.log(data.responseText); }
        });
    });
});