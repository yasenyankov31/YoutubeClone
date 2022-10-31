const videoId = window.location.pathname.substring(window.location.pathname.lastIndexOf('/') + 1);
function EditComment() {
    var formData = new FormData();
    formData.append("newcomment", $("#newcomment").val());

    var xhr2 = new XMLHttpRequest();
    xhr2.onload = function () {
        window.location = document.location.origin + '/Videos/Details/' + videoId;
    }
    xhr2.open("POST", "/Videos/EditComment?videoId=" + videoId, true);
    xhr2.send(formData);
}
function DeleteComment() {
    var xhr2 = new XMLHttpRequest();
    xhr2.onload = function () {
        window.location = document.location.origin + '/Videos/Details/' + videoId;
    }
    xhr2.open("POST", "/Videos/DeleteComment?videoId=" + videoId, true);
    xhr2.send();
}


$("#form-body").on("click", "#btnUpload", function () {
    var formData = new FormData();
    formData.append("content", $("#comment").val());

    var xhr2 = new XMLHttpRequest();
    xhr2.onload = function () {
        window.location = document.location.origin + '/Videos/Details/' + videoId;
    }
    xhr2.open("POST", "/Videos/CreateComment?videoId=" + videoId, true);
    xhr2.send(formData);
});