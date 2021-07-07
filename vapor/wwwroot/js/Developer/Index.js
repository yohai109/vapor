$(function () {
    $("#search-container").submit(function (e) {
        
        e.preventDefault();

        var devName = $("#dev-name").val();
        var gameName = $("#game-name").val();
        var numOfGames = $("#num-of-games").val();

        var data = { devName: devName, gameName: gameName, numOfGames: numOfGames };

        $.ajax({
            method: "POST",
            url: '/Developers/Search',
            data: data
        }).done(function (data) {
            $('#developer-list').html('');
            var developerTemplate = $('#developer-template').html();

            $.each(data, function (i, val) {
                var devTemp = developerTemplate;
                $.each(val, function (key, value) {
                    devTemp = devTemp.replaceAll('{' + key + '}', value)
                })

                $('#developer-list').append(devTemp);
            });
        });
    });
});