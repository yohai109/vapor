$(function () {

    // TODO get genres and developer names
    $("#search-container", function (e) {
        $.ajax({
            method: "GET",
            url: "/Genres/All"
        }).done(function (data) {
            var options = "";
            $.each(data, function (i, curr) {
                options += "<option>" + curr.name + "</option>";
            });

            $("#genres-select").html(options);
        });
    });

    $("#search-btn").submit(function(e) {
        console.log("searching");
        e.preventDefault();
        var textQuery = $("#search-box").val();
        $.ajax({
            method: "GET",
            url: '/Games/Search',
            data: { query: textQuery }
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