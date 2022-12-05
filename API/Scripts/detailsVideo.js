var url = document.location.href;
const videoId = url.substring(url.lastIndexOf('/') + 1);
const creatorId = document.getElementById("creatorId").value

function reply(commentId) {
    var xmlHttp = new XMLHttpRequest();
    var formData = new FormData();
    formData.append("content", $("#reply").val());
    formData.append("commentId", commentId);
    xmlHttp.open("POST", document.location.origin + '/Videos/CreateReply?videoId=' + videoId, true);
    xmlHttp.onload = function () {
        window.location.href = document.location.origin + '/Videos/Details/' + videoId
    }
    xmlHttp.send(formData);
}

function like() {
    var xmlHttp = new XMLHttpRequest();
    xmlHttp.open("POST", document.location.origin + '/Videos/LikeVideo?videoId=' + videoId, true);
    xmlHttp.onload = function () {
        window.location.href = document.location.origin + '/Videos/Details/' + videoId
    }
    xmlHttp.send();
}
function removelike() {
    var xmlHttp = new XMLHttpRequest();
    xmlHttp.open("POST", document.location.origin + '/Videos/RemoveLike?videoId=' + videoId, true);
    xmlHttp.onload = function () {
        window.location.href = document.location.origin + '/Videos/Details/' + videoId
    }
    xmlHttp.send();
}
function dislike() {
    var xmlHttp = new XMLHttpRequest();
    xmlHttp.open("POST", document.location.origin + '/Videos/DislikeVideo?videoId=' + videoId, true);
    xmlHttp.onload = function () {
        window.location.href = document.location.origin + '/Videos/Details/' + videoId
    }
    xmlHttp.send();
}
function removedislike() {
    var xmlHttp = new XMLHttpRequest();
    xmlHttp.open("POST", document.location.origin + '/Videos/RemoveDislike?videoId=' + videoId, true);
    xmlHttp.onload = function () {
        window.location.href = document.location.origin + '/Videos/Details/' + videoId
    }
    xmlHttp.send();
}
function Unsubscribe() {
    var xmlHttp = new XMLHttpRequest();
    xmlHttp.open("POST", document.location.origin + '/Users/Unsubscribe?creatorId=' + creatorId);
    xmlHttp.onload = function () {
        window.location.href = document.location.origin + '/Videos/Details/' + videoId
    }
    xmlHttp.send();
}

function Subscribe() {
    var xmlHttp = new XMLHttpRequest();
    xmlHttp.open("POST", document.location.origin + '/Users/Subscribe?creatorId=' + creatorId);
    xmlHttp.onload = function () {
        window.location.href = document.location.origin + '/Videos/Details/' + videoId
    }
    xmlHttp.send();
}
function CreateComment()
{
    var formData = new FormData();
    formData.append("content", $("#comment").val());

    var xhr2 = new XMLHttpRequest();
    xhr2.onload = function () {
        window.location = document.location.origin + '/Videos/Details/' + videoId;
    }
    xhr2.open("POST", "/Videos/CreateComment?videoId=" + videoId, true);
    xhr2.send(formData);
}
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
