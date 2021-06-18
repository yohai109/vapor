$("body", function insertIntoPage() {
    var id = $('#reviews').attr("gameId");
    console.log(id);
    $.ajax({
        url: '/games/reviews/' + id
    }).done(function (data) {
        console.log(data);
        $('#reviews').html('');
        var reviewTemplate = $('#all_users_review_template').html();
        $.each(data, function (i, review) {
            var reviewTemp = reviewTemplate;

            /*var genreFinal = ""

            $.each(val.generes, function (i, currGener) {
                genreFinal += genreTemplate.replaceAll("{name}", currGener.name);
            })*/

            reviewTemp = reviewTemp.replaceAll('{userName}', '"' + review.comment + '"')
            reviewTemp = reviewTemp.replaceAll('{comment}', '"' + review.comment + '"')

            /*$.each(data, function (key, value) {
                reviewTemp = reviewTemp.replaceAll('{' + key + '}', value)
            })*/

            $('#reviews').append(reviewTemp);
        });
        /*$('#reviews').append(
            '<div class="reviews-members-body">' +
            `<p>${review.comment}</p>` +
            '</div >'
        );*/
    })
    //$('#image_placeholder').html('');
    //$('#image_placeholder').append("<img src='" + convertImageFrom64(data[0].avatar, data[0].fileContentType) + "'></img>");
})
