$( function () {

    $("#cartAlert").hide();

    /*var id = $('#reviews').attr("gameId");
    console.log(id);
    $.ajax({
        url: '/games/reviews/' + id
    }).done(function (data) {
        console.log(data);
        $('#reviews').html('');
        $.each(data, function (i, review) {
            $('#reviews').append(
                `<h1>${review.comment}</h1>`
            );
        })
        //$('#image_placeholder').html('');
        //$('#image_placeholder').append("<img src='" + convertImageFrom64(data[0].avatar, data[0].fileContentType) + "'></img>");
    })*/

    $("#addToCart").click(function (e) {
        $.ajax({
            url: '/Orders/AddToCart?gameid=' + $("#gameId").val()
        }).done(function () {
            $('#cartAlert').fadeIn(500);

            setTimeout(function () {
                $('#cartAlert').fadeOut(500);
            }, 3000);
        })
    })
})