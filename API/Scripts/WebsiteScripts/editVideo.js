const videoId = window.location.pathname.substring(window.location.pathname.lastIndexOf('/') + 1);
var progressBarStart = function () {
    $("#progressbar_container").show();
}

var progressBarUpdate = function (percentage) {
    $('#progressbar_label').html(percentage + "%");
    $("#progressbar").width(percentage + "%");
}

var progressBarComplete = function () {
    $("#progressbar_container").fadeOut(500);
}
function randomName(length) {
    var result = '';
    var characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
    var charactersLength = characters.length;
    for (var i = 0; i < length; i++) {
        result += characters.charAt(Math.floor(Math.random() *
            charactersLength));
    }
    return result;
}

var file;

$('#VideoInput').change(function (e) {
    file = e.target.files[0];
    var blob = file.slice(0, file.size, 'video/mp4');
    var name = randomName(10) + '.mp4';
    file = new File([blob], name, { type: 'video/mp4' });
});

var uploadCompleted = function ()
{
    var formData = new FormData();
    formData.append('id', videoId);
    formData.append('fileName', file.name);
    formData.append('completed', true);
    formData.append('videoName', $('#VideoName').val());
    formData.append('description', escape($('#froala-editor').val()));
    if (document.getElementById("Thumbnail").value !== "") {
        console.log("tes")
        var file_data = $("#Thumbnail").prop("files")[0];
        var blob = file_data.slice(0, file_data.size, 'image/png');
        var newFile = new File([blob], randomName(10) + '.png', { type: 'image/png' });
        formData.append("thumbnail", newFile);
    }


    var xhr2 = new XMLHttpRequest();
    xhr2.onload = function () {
        progressBarUpdate(100);
        progressBarComplete();
        window.location = document.location.origin +'/Videos/Index';
    }
    xhr2.open("POST", "/Videos/Edit?fileName=" + file.name + "&complete=" + 1, true);
    xhr2.send(formData);


}

var multiUpload = function (count, counter, blob, completed, start, end, bytesPerChunk) {
    counter = counter + 1;
    if (counter <= count) {
        var chunk = blob.slice(start, end);
        var xhr = new XMLHttpRequest();
        xhr.onload = function () {
            start = end;
            end = start + bytesPerChunk;
            if (count == counter) {
                uploadCompleted();
            } else {
                var percentage = (counter / count) * 100;
                progressBarUpdate(percentage);
                multiUpload(count, counter, blob, completed, start, end, bytesPerChunk);
            }
        }
        xhr.open("POST", "/Videos/MultiUpload?id=" + counter.toString() + "&fileName=" + file.name, true);
        xhr.send(chunk);
    }
}


$("#VideoDiv").on("click", "#btnUpload", function () {
    var interupt = false;
    var formdata = [$("#VideoName"), $("#froala-editor")]
    for (const x of formdata)
    {
        if (x.val() === "")
        {
            $("#" + x.attr("id") + "Validation").removeClass("d-none")
            interupt = true;
        }
        else {
            $("#" + x.attr("id") + "Validation").addClass("d-none")
        }
    }
    if (!interupt) {
        if (document.getElementById("VideoInput").value !== "")
        {
            var blob = file;
            var bytesPerChunk = 3757000;
            var size = blob.size;

            var start = 0;
            var end = bytesPerChunk;
            var completed = 0;
            var count = size % bytesPerChunk == 0 ? size / bytesPerChunk : Math.floor(size / bytesPerChunk) + 1;
            var counter = 0;
            progressBarStart();
            multiUpload(count, counter, blob, completed, start, end, bytesPerChunk);
        }
        else {

            var formData = new FormData();
            formData.append('id', videoId);
            formData.append('videoName', $('#VideoName').val());
            formData.append('description', escape($('#froala-editor').val()));
            if (document.getElementById("Thumbnail").value !== "") {
                console.log("tes")
                var file_data = $("#Thumbnail").prop("files")[0];
                var blob = file_data.slice(0, file_data.size, 'image/png');
                var newFile = new File([blob], randomName(10) + '.png', { type: 'image/png' });
                formData.append("thumbnail", newFile);
            }


            var xhr2 = new XMLHttpRequest();
            xhr2.onload = function () {
                progressBarUpdate(100);
                progressBarComplete();
                window.location = document.location.origin +'/Videos/Index';
            }
            xhr2.open("POST", "/Videos/Edit", true);
            xhr2.send(formData);

        }


    }

});