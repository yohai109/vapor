$(function () {
    $("#cartAlert").hide();
    $('#reviewUpdateAlert').hide();


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
            method: 'PUT',
            url: '/games/EditReview',
            data: {
                id: reviewId,
                rating: rating,
                comment: comment
            }
        }).done(function (data) {
            $('#reviewUpdateAlert').fadeIn(500);
            console.log(data)
            $("#currentReviewLastUpdated").html('').html("Last Update " + data)
            setTimeout(function () {
                $('#reviewUpdateAlert').fadeOut(500);
            }, 3000);
        })
    })

    $("#createANewReview").click(function (e) {
        var comment = $("#newReviewTextArea").val()
        var rating = $("#newReviewRating :selected").val()
        var gameId = $("#hiddenGameId").val()
        console.log(comment)
        console.log(rating)
        console.log(gameId)

        $.ajax({
            method: 'POST',
            url: '/games/CreateReview',
            data: {
                rating: rating,
                comment: comment,
                gameId: gameId
            }
        }).done(function (data) {
            console.log(data)
            //window.Location.reload(true) //
            //window.location.replace("https://localhost:44334/Games/Details/a6129515-22e5-4c38-8f1e-0564291307c6");
            setTimeout(function () {
            }, 10000);
        })//.then(window.location.replace("https://localhost:44334/Games/Details/a6129515-22e5-4c38-8f1e-0564291307c6"))
    })

    //$("#deleteCurrentReview").click(location.reload())
})