$(function () {
    $("#register-user").submit(function (e) {
        console.log("registering");
        // Attach file
        e.preventDefault();
        var form = $(this);
        $.ajax({
            method: "POST",
            url: '/Users/Register',
            data: form.serialize(),
        }).done(function () {
            window.location.href = "/";
        });
    });
});