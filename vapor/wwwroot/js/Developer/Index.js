/**

function convertImageFrom64(avatar, type) {
    var image = new Image();
    image.src = 'data:' + type + ';base64,' + avatar;
    return image.src;
}

$("body", function insertIntoPage() {
    $.ajax({
        url: '/Developers/All'
    }).done(function (data) {
        $('tbody').html('');
        var template = $('#developer_template').html();

        $.each(data, function (i, val) {
            var temp = template;
            temp = temp.replaceAll("{image}", "<img src='" + convertImageFrom64(val.avatar, val.fileContentType) + "'></img>");
            $.each(val, function (key, value) {
                temp = temp.replaceAll('{' + key + '}', value)
            })

            console.log(temp)

            $('tbody').append(
                temp 
            )
        })
    })
})
*/


$(function () {

    $("#search-container").submit(function (e) {
        
        e.preventDefault();

        var devName = $("#dev-name").val();
        var gameName = $("#game-name").val();
        var numOfGames = $("#num-of-games").val();

        var data = { devName: devName, gameName: gameName, numOfGames: numOfGames };
        console.log(data);

        $.ajax({
            method: "POST",
            url: '/Developers/Search',
            data: data
        }).done(function (data) {
            $('#developer-list').html('');
            var developerTemplate = $('#developer-template').html();

            console.log("received:")
            console.log(data)
            /*var genreTemplate = $('#genre-template').html();*/

            $.each(data, function (i, val) {
                var devTemp = developerTemplate;

                /*var genreFinal = ""

                $.each(val.genres, function (i, currGener) {
                    genreFinal += genreTemplate.replaceAll("{name}", currGener.name);
                })

                gameTemp = gameTemp.replaceAll('{genre}', genreFinal)*/

                $.each(val, function (key, value) {
                    devTemp = devTemp.replaceAll('{' + key + '}', value)
                })

                $('#developer-list').append(devTemp);
            });
        });
    });
});