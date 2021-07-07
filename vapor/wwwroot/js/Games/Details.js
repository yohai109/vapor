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
            $("#currentReviewName").html('').html('<h5>' + usernameData.username + '</h5>')
        })
    } else {
        $("#editCurrentReview").hide();
        $("#deleteCurrentReview").hide();

        $.ajax({
            url: '/Customers/CustomerUserName'
        }).done(function (usernameData) {
            $("#currentReviewName").html('').html('<h5>' + usernameData.username + '</h5>')
        })
    }

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
                $("#currentReviewName").html('').html('<h5>' + usernameData.username + '</h5>')
            })

            $('#reviewUpdateAlert').fadeIn(500);
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
        var gameId = $("#gameId").val()

        $.ajax({
            method: 'POST',
            url: '/games/CreateReview',
            data: {
                rating: rating,
                comment: comment,
                gameId: gameId
            }
        }).done(function (data) {

            $("#createANewReview").hide();
            $("#editCurrentReview").show();
            $("#deleteCurrentReview").show();
            $("#WrittenTime").html('').html("<i class='bi bi-clock'> Written at " + data.time + "</i> <i class='bi bi-clock-fill' id='currentReviewLastUpdated'> Last update " + data.time + "</i >")
            $("#reviewRatingAndId").html('').html("<input id='currentReviewRatingHidden' hidden value=" + data.review.rating + "> <input id='currentReviewIdHidden' hidden value = " + data.review.id + " >")
            $("#currentReviewName").html('').html('<h5>' + data.username + '</h5>')


            $('#reviewCreateAlert').fadeIn(500);
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

            if (data === true) {
                $("#WrittenTime").html('')
                $("#commentArea").html('').html('<textarea class="form-control" id="currentReviewTextArea" rows="3"></textarea>')
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


    $("#buyNow").click(function (e) {
        $.ajax({
            url: '/Orders/AddToCart?gameid=' + $("#gameId").val()   
        }).done(function () {
            /*$('#cartAlert').fadeIn(500);

            setTimeout(function () {
                $('#cartAlert').fadeOut(500);
            }, 3000);*/

            window.location.href = "/Orders/create";
        });

    })

})

function setAvaregeRating() {
    var id = $('#gameId').val();
    $.ajax({
        url: '/games/RatingAvarage?gameId=' + id
    }).done(function (data) {
        if (data.length === 0) {
            $('#avgRate').html('').html('<span> Avarege Rating: <i id="avaregeRate" class="bi bi-star-fill">0.00</i></span>')
        } else {
            $('#avgRate').html('').html('<span> Avarege Rating: <i id="avaregeRate" class="bi bi-star-fill">' + data[0].avg.toFixed(2) + '</i></span>')
        }
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