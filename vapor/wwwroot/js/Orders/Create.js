$(function () {
    $("#confirm").click(function (e) {
        var data = {
            gameid : []
        };

        $.each($(".gamesid"), function(key, val) {
            console.log(key);
            console.log(val);
        })

        $.ajax({
            method: "POST",
            url: '/Orders/Order', /*+ $("#gameid").val()*/
            data: data
        }).done(function (data) {
            window.location.href = "/";
        });
    })
})