$(function () {
    $("#confirm").click(function (e) {
        var data = {
            gamesId : []
        };

        $.each($(".gamesid"), function(key, val) {
            console.log(key);
            console.log(val.value);

            data.gamesId.push(val.value)
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