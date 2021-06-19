$(function () {

    $("#search-container").submit(function (e) {
        console.log("searching");
        e.preventDefault();

        var textQuery = $("#search-box").val();
        var genres = [];
        var developers = [];

        $.each($("#search-container").serializeArray(), function (i, currElement) {
            switch (currElement.name) {
                case "genres-select":
                    genres.push(currElement.value);
                    break;
                case "developer-select":
                    developers.push(currElement.value)
                    break;
                default:
                    break;
            }
        });

        var data = { query: textQuery, developers: developers, genres: genres };
        console.log(data);

        $.ajax({
            method: "POST",
            url: '/Games/Search',
            data: data
        }).done(function (data) {
            $('#game-list').html('');
            var gameTemplate = $('#game_template').html();
            var genreTemplate = $('#genre-template').html();

            $.each(data, function (i, val) {
                var gameTemp = gameTemplate;

                var genreFinal = ""

                $.each(val.genres, function (i, currGener) {
                    genreFinal += genreTemplate.replaceAll("{name}", currGener.name);
                })

                gameTemp = gameTemp.replaceAll('{genre}', genreFinal)

                $.each(val, function (key, value) {
                    gameTemp = gameTemp.replaceAll('{' + key + '}', value)
                })

                $('#game-list').append(gameTemp);
            });
        });
    });
});