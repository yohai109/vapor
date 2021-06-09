$(function () {

    // TODO get genres and developer names

    $("#search-btn").click(function(e) {
        console.log("clicked search-btn")
        e.preventDefault()
        $.ajax({
            method: "GET",
            url: '/Games/Search',
            data: { query: $("#search-box").val() }
        }).done(function (data) {
            $('#game-list').html('');
            var gameTemplate = $('#game_template').html();
            var genreTemplate = $('#genre-template').html();

            $.each(data, function (i, val) {
                var gameTemp = gameTemplate;

                var genreFinal = ""

                $.each(val.generes, function(i, currGener) {
                    genreFinal += genreTemplate.replaceAll("{name}", currGener.name);
                })

                gameTemp = gameTemp.replaceAll('{genre}', genreFinal)
                console.log(gameTemp)
                $.each(val, function (key, value) {
                    gameTemp = gameTemp.replaceAll('{' + key + '}', value)
                })

                $('#game-list').append(gameTemp);
            });
        });
    });
});