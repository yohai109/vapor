$(function () {

    $("#search-btn").click(function (e) {
        console.log("searching");
        e.preventDefault();

        var textQuery = $("#search-box").val();
        var genres = $("#genres-select").val() || [];
        var developers = $("#developer-select").val() || [];

        console.log(genres);

        $.ajax({
            method: "POST",
            url: '/Games/Search',
            data: { query: textQuery, developers: developers, genres: genres }
        }).done(function (data) {
            $('#game-list').html('');
            var gameTemplate = $('#game_template').html();
            var genreTemplate = $('#genre-template').html();

            $.each(data, function (i, val) {
                var gameTemp = gameTemplate;

                var genreFinal = ""

                $.each(val.generes, function (i, currGener) {
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