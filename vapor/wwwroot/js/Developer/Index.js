function convertImageFrom64( avatar, type) {
    var image = new Image();
    image.src = 'data:' + type + ';base64,' + avatar;
    return image.src;
}

$("body", function insertIntoPage() {
    $.ajax({
        url: '/Developers/All'
    }).done(function (data) {
        $('tbody').html('');
        var template = $('#developer_template').html();

        $.each(data, function (i, val) {
            var temp = template;
            temp = temp.replaceAll("{image}", "<img src='" + convertImageFrom64(val.avatar, val.fileContentType) + "'></img>");
            $.each(val, function (key, value) {
                temp = temp.replaceAll('{' + key + '}', value)
            })

            console.log(temp)

            $('tbody').append(
                temp 
            )
        })
    })
})