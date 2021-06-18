$(function () {
    $("#confirm").click(function (e) {

        var data = {
            gameid : $("#gameid").val()
        };
        $.ajax({
            method: "POST",
            url: '/Orders/Order', /*+ $("#gameid").val()*/
            data: data
        }).done(function (data) {
            window.location.href = "/";
        });
    })
})