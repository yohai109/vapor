$("body", function insertIntoPage() {


    var id = $('#reviews').attr("gameId");
    $.ajax({
        url: '/games/RatingAvarage?gameId=' + id
    }).done(function (data) {
        $('#avaregeRate').html('').append(data[0].avg.toFixed(2));
    })


    $.ajax({
        url: '/games/reviews?gameId=' + id
    }).done(function (data) {
        $('#reviews').html('');
        var reviewTemplate = $('#all_users_review_template').html();
        $.each(data, function (i, review) {
            var reviewTemp = reviewTemplate;

            reviewTemp = reviewTemp.replaceAll('{userName}',review.cusotmer.name)
            reviewTemp = reviewTemp.replaceAll('{comment}', '"' + review.comment + '"')
            reviewTemp = reviewTemp.replaceAll('{rating}', review.rating)

            $('#reviews').append(reviewTemp);
        });
    })
})
