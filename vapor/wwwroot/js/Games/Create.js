let gameImageUrls = []

$(document).ready(function () {
    $("#image-url-input button").click(() => {
        let imageUrl = $("#image-url-input input").val()

        if (!imageUrl) {
            $("#image-url-error").text("Game image url cannot be empty")
        }
        else if (gameImageUrls.includes(imageUrl)) {
            $("#image-url-error").text("The input url already specified")
        }
        else {
            gameImageUrls.push(imageUrl)

            $("#image-url-input input").val("")
            $("#image-url-error").text("")

            RerenderAllImageUrls()
        }
    });
    RerenderAllImageUrls()

});

function RerenderAllImageUrls() {
    let imagesURLContent = ""
    if (gameImageUrls.length > 0) {
        for (let i = 0; i < gameImageUrls.length; i++) {
            imagesURLContent += `   <div class='row'>
                                        <div class='col-md-8'>${gameImageUrls[i]}</div>
                                        <div class='col-md-4'>
                                            <i class='bi bi-x-octagon bi-x-lg'  onClick='RemoveGameImage(\"${gameImageUrls[i]}"\)'> </i>
                                        </div>
                                    </div>`

        }
        $("#game-urls").html("<div class='container'>" + imagesURLContent + "</div>")
    }
    else {
        $("#game-urls").html("<h4>No image URLs specified</h4>")
    }
}
/*        const RemoveGameImage = (imageUrl) => */
function RemoveGameImage(imageUrl) {
    gameImageUrls = gameImageUrls.filter((url) => url !== imageUrl)
    RerenderAllImageUrls()

}

function AlterFormOutput() {
    for (let i = 0; i < gameImageUrls.length; i++) {
        $("#game-form").append('<input type="hidden" name="gameImageUrls[]" value="' + gameImageUrls[i] + '" /> ')
    }
}