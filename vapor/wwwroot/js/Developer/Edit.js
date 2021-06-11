function convertImageFrom64(avatar, type) {
    var image = new Image();
    image.src = 'data:' + type + ';base64,' + avatar;
    return image.src;
}

$("body", function insertIntoPage() {
    var id = $('#developer_id_holder').val();
    $.ajax({
        url: '/Developers/id/' + id
    }).done(function (data) {
        $('#image_placeholder').html('');
        $('#image_placeholder').append("<img src='" + convertImageFrom64(data[0].avatar, data[0].fileContentType) + "'></img>");
    })
})