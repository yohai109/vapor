$(function () {
    $("#confirm").click(function (e) {
        var Pic = $("#can")[0].toDataURL("image/png");
        Pic = Pic.replace(/^data:image\/(png|jpg);base64,/, "");

        if (hasPic !== false) {
            $.ajax({
                method: "POST",
                url: '/Orders/Order'
            }).done(function (data) {
                window.location.href = "/";
            });
        } else {
            $('#signAlert').fadeIn(500);

            setTimeout(function () {
                $('#signAlert').fadeOut(500);
            }, 3000);
        }
        
    })

    initCanvas()

    $("#clear-btn").click(function () {
        hasPic = false;
        ctx.setTransform(1, 0, 0, 1, 0, 0);
        ctx.clearRect(0, 0, ctx.canvas.width, ctx.canvas.height);
    });

    
})

var hasPic = false;

var mousePressed = false;
var lastX, lastY;
var ctx;

function initCanvas() {
    ctx = document.getElementById('can').getContext("2d");

    $('#can').mousedown(function (e) {
        mousePressed = true;
        Draw(e.pageX - $(this).offset().left, e.pageY - $(this).offset().top, false);
    });

    $('#can').mousemove(function (e) {
        if (mousePressed) {
            Draw(e.pageX - $(this).offset().left, e.pageY - $(this).offset().top, true);
        }
    });

    $('#can').mouseup(function (e) {
        mousePressed = false;
    });
    $('#can').mouseleave(function (e) {
        mousePressed = false;
    });
}

function Draw(x, y, isDown) {
    if (isDown) {
        hasPic = true;
        ctx.beginPath();
        ctx.strokeStyle = "black";
        ctx.lineWidth = 3;
        ctx.lineJoin = "round";
        ctx.moveTo(lastX, lastY);
        ctx.lineTo(x, y);
        ctx.closePath();
        ctx.stroke();
    }
    lastX = x; lastY = y;
}

function UploadPic() {

    // Generate the image data
    

    // Sending the image data to Server
    $.ajax({
        type: 'POST',
        url: 'Save_Picture.aspx/UploadPic',
        data: '{ "imageData" : "' + Pic + '" }',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (msg) {
            alert("Done, Picture Uploaded.");
        }
    });
}