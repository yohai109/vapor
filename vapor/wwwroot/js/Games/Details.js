$("body", function insertIntoPage() {
    var id = $('#reviews').attr("gameId");
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
    })

    $("#addToCart").click(function (e) {
        var currcart = sessionStorage.getItem("cart").split(",")
        sessionStorage.setItem("cart", currcart.push($("#gameid").val())
    })
})