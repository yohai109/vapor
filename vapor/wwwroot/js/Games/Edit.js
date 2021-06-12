let imagesToDelete = []

$(document).ready(function () {
    $("#edit-form").submit(() => {
        imagesToDelete.forEach((imageId) => {
            $("#edit-form").append(`<input type='hidden' name='imagesToDelete[]' value='${imageId}' />`)
        })
    })
});

function taggleDelete(imageId) {
    let deleteClass = "deleted-image"

    // If the game was deleted
    if (imagesToDelete.includes(imageId)) {
        $(`.game-image img[data-image-id='${imageId}']`).removeClass(deleteClass)
        imagesToDelete = imagesToDelete.filter(id => id !== imageId)
    }
    else {
        $(`.game-image img[data-image-id='${imageId}']`).addClass(deleteClass)
        imagesToDelete.push(imageId)
    }
}

