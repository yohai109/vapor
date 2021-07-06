var curSelectedItem;

$(document).ready(function () {
    $(".item-bar__item").click(selectItem)


})

function selectItem() {
    let itemBar = $(this).parent()
    let SLECTED_ITEM_CLASS = "item-bar__item--selected"
    let itemId = ""

    let newSelectedItem = $(this).attr("data-image-url")
    console.log(newSelectedItem)
    console.log(curSelectedItem)
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

    console.log(curSelectedItem)

    // Updates the selected item display
    $(".image-screen img").attr("src", newSelectedItem)
}
