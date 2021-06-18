
var id = $('#games-added-graph');

try {

}

$(document).ready(function () {
    $.ajax({
        type: "GET",
        url: '/analysis/index/',
        success: function (data) {
            console.log(data)
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(jqXHR.status);
        },
    });
})
