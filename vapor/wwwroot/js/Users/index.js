$(function () {
    $("#register-user").submit(function (e) {
        console.log("registering");
        var form = $('#register-user')[0]; // You need to use standard javascript object here
        var formData = new FormData(form);
        // Attach file
        formData.append('register-avatar', $('input[type=file]')[0].files[0]); 
        e.preventDefault();
        $.ajax({
            method: "POST",
            url: '/Users/Register',
            data: formData,
            contentType: false, // NEEDED, DON'T OMIT THIS (requires jQuery 1.6+)
            processData: false, 
        }).done(function () {
            window.location.href = "/";
        });
    });
});