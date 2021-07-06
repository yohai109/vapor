$(function () {
    $("#cartAlert").hide();
    $('#reviewUpdateAlert').hide();
    $('#reviewCreateAlert').hide();
    $('#reviewDeleteAlert').hide();



    var currentUserRate = $("#currentReviewRatingHidden").val();
    if (typeof currentUserRate !== "undefined") {
        $("#currentReviewRating").val(currentUserRate);
        $("#createANewReview").hide();

        $.ajax({
            url: '/Customers/CustomerUserName'
        }).done(function (usernameData) {
            console.log(usernameData)
            $("#currentReviewName").html('').html('<h5>' + usernameData.username + '</h5>')
        })
    } else {
        $("#editCurrentReview").hide();
        $("#deleteCurrentReview").hide();

        $.ajax({
            url: '/Customers/CustomerUserName'
        }).done(function (usernameData) {
            console.log(usernameData)
            $("#currentReviewName").html('').html('<h5>' + usernameData.username + '</h5>')
        })
    }






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
            $.ajax({
                url: '/Customers/CustomerUserName'
            }).done(function (usernameData) {
                console.log(usernameData)
                $("#currentReviewName").html('').html('<h5>' + usernameData.username + '</h5>')
            })
            /*$.ajax({
                url: '/reviews/ReviewUserName?id=' + data.review.customerId
            }).done(function (usernameData) {
                console.log(usernameData)
                $("#currentReviewName").html('').html('<h5>' + usernameData.username + '</h5>')
            })*/

            $('#reviewUpdateAlert').fadeIn(500);
            console.log(data)
            $("#currentReviewLastUpdated").html('').html("Last Update " + data.time)
            setTimeout(function () {
                $('#reviewUpdateAlert').fadeOut(500);
            }, 3000);

            setAvaregeRating()
        })
    })

    $("#createANewReview").click(function (e) {
        var comment = $("#currentReviewTextArea").val()
        var rating = $("#currentReviewRating :selected").val()
        var gameId = $("#hiddenGameId").val()
        console.log(comment)
        console.log(rating)
        console.log(gameId)
        /*if (comment === '') {
            comment = "";
        }
        console.log(comment)*/
        $.ajax({
            method: 'POST',
            url: '/games/CreateReview',
            data: {
                rating: rating,
                comment: comment,
                gameId: gameId
            }
        }).done(function (data) {

            // remaking the write new review into a edit one
            console.log(data)
            $("#createANewReview").hide();
            $("#editCurrentReview").show();
            $("#deleteCurrentReview").show();

            //$("#currentReviewLastUpdated").html('').html("Last Update " + data)
            $("#WrittenTime").html('').html("<i class='bi bi-clock'> Written at " + data.time + "</i> <i class='bi bi-clock-fill' id='currentReviewLastUpdated'> Last update " + data.time + "</i >")
            $("#reviewRatingAndId").html('').html("<input id='currentReviewRatingHidden' hidden value=" + data.review.rating + "> <input id='currentReviewIdHidden' hidden value = " + data.review.id + " >")
            $("#currentReviewName").html('').html('<h5>' + data.username + '</h5>')


            $('#reviewCreateAlert').fadeIn(500);
            console.log(data)
            setTimeout(function () {
                $('#reviewCreateAlert').fadeOut(500);
            }, 3000);

            setAvaregeRating();
        })
    })


    $("#deleteCurrentReview").click(function (e) {
        var reviewId = $("#currentReviewIdHidden").val()
        console.table("id", reviewId)

        $.ajax({
            method: 'POST',
            url: '/games/DeleteReview',
            data: {
                id: reviewId
            }
        }).done(function (data) {
            console.log("deleted")
            console.log(data)

            if (data === true) {
                $("#WrittenTime").html('')
                $("#commentArea").html('').html('<textarea class="form-control" id="currentReviewTextArea" rows="3"></textarea>')
                //$("#currentReviewRating")
                $("#editCurrentReview").hide();
                $("#deleteCurrentReview").hide();
                $("#createANewReview").show();

                $('#reviewDeleteAlert').fadeIn(500);

                setTimeout(function () {
                    $('#reviewDeleteAlert').fadeOut(500);
                }, 3000);

                setAvaregeRating();
            }
        })
    })


    /*$("#createANewReview").click(function (e) {
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
            console.log("done")

            //window.Location.reload(true) //
            //window.location.replace("https://localhost:44334/Games/Details/a6129515-22e5-4c38-8f1e-0564291307c6");
            console.log(data)
            $("#newReview").html('').html(

            )
        })
    })*/

    //$("#deleteCurrentReview").click(location.reload())
})

function setAvaregeRating() {
    var id = $('#gameId').val();
    $.ajax({
        url: '/games/RatingAvarage?gameId=' + id
    }).done(function (data) {
        console.log(data)
        if (data.length === 0) {
            $('#avgRate').html('').html('<span> Avarege Rating: <i id="avaregeRate" class="bi bi-star-fill">0.00</i></span>')
        } else {
            $('#avgRate').html('').html('<span> Avarege Rating: <i id="avaregeRate" class="bi bi-star-fill">' + data[0].avg.toFixed(2) + '</i></span>')
        }
        //$('#avaregeRate').html('').append(data[0].avg.toFixed(2));
    })
}

function deleteReview(id) {
    $.ajax({
        method: 'POST',
        url: '/games/DeleteReview',
        data: {
            id: id
        }
    }).done(function (data) {
        if (data === true) {
            $("#" + id).html('')

            $('#reviewDeleteAlert').fadeIn(500);

            setTimeout(function () {
                $('#reviewDeleteAlert').fadeOut(500);
            }, 3000);

            setAvaregeRating();
        }
    })
}