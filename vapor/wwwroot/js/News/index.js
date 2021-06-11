
function convertImageFrom64(avatar, type) {
    var image = new Image();

    console.log(avatar);
    console.log(type);
    image.src = 'data:' + type + ';base64,' + avatar;
    return image.src;
}

$("body", function doNothing() {
    $.ajax({
        url: '/games/getNews'
    }).done(function (data) {
        var template = $('#news').html();
        $.each(data, function (i, str) {
            var temp = template;
            temp = temp.replaceAll("{title}", '<div class="ticker-item">' + str + '</div>');
            $('#treadmill').append(temp)
        })
    })
})