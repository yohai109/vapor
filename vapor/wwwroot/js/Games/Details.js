$(function () {
    $("#cartAlert").hide();

    var currentUserRate = $("#currentReviewRatingHidden").val();
    if (typeof currentUserRate !== undefined) {
        $("#currentReviewRating").val(currentUserRate);
    }

    var id = $('#reviews').attr("gameId");
    $.ajax({
        url: '/games/RatingAvarage?gameId=' + id
    }).done(function (data) {
        // $('#avaregeRate').html('').append(data[0].avg.toFixed(2));
    })


    /*$.ajax({
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




    $("#editCurrentReview").click(function (e) {
        var comment = $("#currentReviewTextArea").val()
        var rating = $("#currentReviewRating :selected").val()
        var reviewId = $("#currentReviewIdHidden").val()
        console.log(comment)
        console.log(rating)
        console.log(reviewId)

        $.ajax({
            method: 'POST',
            url: '/games/EditReview',
            data: {
                id: reviewId,
                rating: rating,
                comment: comment
            }
        }).done(function (data) {
            console.log(data)
        })
    })
})