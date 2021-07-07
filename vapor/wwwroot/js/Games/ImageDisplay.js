var curSelectedItem;
var barRowPossition = 0;

function calculateRotateSize() {
    let vm = 15.4

    // Convers vm to pixcels

    let vmSize = document.documentElement.clientWidth / 100
    return vm * vmSize
}

function souldRotateRight(direction) {
    let displayWidth = $(".image-display").width()
    let barWidth = $(".item-bar").width()
    return Math.abs(barRowPossition) + displayWidth < barWidth
}

function souldRotateLeft(direction) {
    return barRowPossition < 0
}


/*
    Rolls the item bar
*/
function rotateItemBar(amount) {
    barRowPossition += amount
    $(".carousel .item-bar").css("transform", `translateX(${barRowPossition}px)`)
}

/*
    Selects another image
*/
function selectItem() {
    let itemBar = $(this).parent()
    let SLECTED_ITEM_CLASS = "item-bar__item--selected"
    let itemId = ""

    let newSelectedItem = $(this).attr("data-image-url")

    // If update should occurre
    if (curSelectedItem === newSelectedItem) {
        return
    }

    // Updates the selected item stlyes
    let allItems = itemBar.children()
    let newItem = itemBar.children(`.item-bar__item[data-image-url='${newSelectedItem}']`)

    allItems.removeClass(SLECTED_ITEM_CLASS)
    newItem.addClass(SLECTED_ITEM_CLASS)

    curSelectedItem = newSelectedItem

    // Updates the selected item display
    $(".image-screen img").attr("src", newSelectedItem)
}


$(document).ready(function () {
    $(".item-bar__item").click(selectItem)
    $(".carousel__arrow--right").click(() => {
        if (souldRotateRight()) {
            let rotateSize = calculateRotateSize()
            rotateItemBar(-rotateSize)
        }
    })
    $(".carousel__arrow--left").click(() => {
        if (souldRotateLeft()) {
            let rotateSize = calculateRotateSize()
            rotateItemBar(rotateSize)
        }
    })
    rotateItemBar(0)
})
