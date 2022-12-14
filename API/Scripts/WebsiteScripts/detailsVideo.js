var url = document.location.href;
const videoId = url.substring(url.lastIndexOf('/') + 1);
const creatorId = document.getElementById("creatorId").value


function ToggleComments()
{
    var commentsection = document.getElementById("commentSection")
    var switchtext = document.getElementById("switchtext")

    if (document.getElementById('flexSwitchCheckChecked').checked) {

        switchtext.innerHTML = 'Comments "ON"'
        commentsection.style.display = 'block';
    }
    else {
        switchtext.innerHTML = 'Comments "OFF"'
        commentsection.style.display = 'none';
    }

}
function ShowReply(commentId)
{
    document.getElementById("replyId " + commentId).style.display = 'block';
}


function LikeVideo() {
    var xmlHttp = new XMLHttpRequest();
    xmlHttp.open("POST", document.location.origin + '/Videos/LikeVideo?videoId=' + videoId, true);
    xmlHttp.onload = function () {
        window.location.href = document.location.origin + '/Videos/Details/' + videoId
    }
    xmlHttp.send();
}

function RemoveLikeVideo() {
    var xmlHttp = new XMLHttpRequest();
    xmlHttp.open("POST", document.location.origin + '/Videos/RemoveLikeVideo?videoId=' + videoId, true);
    xmlHttp.onload = function () {
        window.location.href = document.location.origin + '/Videos/Details/' + videoId
    }
    xmlHttp.send();
}

function DislikeVideo() {
    var xmlHttp = new XMLHttpRequest();
    xmlHttp.open("POST", document.location.origin + '/Videos/DislikeVideo?videoId=' + videoId, true);
    xmlHttp.onload = function () {
        window.location.href = document.location.origin + '/Videos/Details/' + videoId
    }
    xmlHttp.send();
}

function RemoveDislikeVideo() {
    var xmlHttp = new XMLHttpRequest();
    xmlHttp.open("POST", document.location.origin + '/Videos/RemoveDislikeVideo?videoId=' + videoId, true);
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
    xhr2.open("POST", "/Comments/CreateComment?videoId=" + videoId, true);
    xhr2.send(formData);
}

function EditComment(commentId) {
    var formData = new FormData();
    formData.append("newcomment", $("#newcomment").val());

    var xhr2 = new XMLHttpRequest();
    xhr2.onload = function () {
        window.location = document.location.origin + '/Videos/Details/' + videoId;
    }
    xhr2.open("POST", "/Comments/EditComment?videoId=" + videoId + "&commentId=" + commentId, true);
    xhr2.send(formData);
}

function DeleteComment(commentId) {
    var xhr2 = new XMLHttpRequest();
    xhr2.onload = function () {
        window.location = document.location.origin + '/Videos/Details/' + videoId;
    }
    xhr2.open("POST", "/Comments/DeleteComment?videoId=" + videoId + "&commentId=" + commentId, true);
    xhr2.send();
}

function LikeComment(commentId) {
    var xhr2 = new XMLHttpRequest();
    xhr2.onload = function () {
        window.location = document.location.origin + '/Videos/Details/' + videoId;
    }
    xhr2.open("POST", "/Comments/LikeComment?commentId=" + commentId, true);
    xhr2.send();
}

function RemoveLikeComment(commentId) {
    var xhr2 = new XMLHttpRequest();
    xhr2.onload = function () {
        window.location = document.location.origin + '/Videos/Details/' + videoId;
    }
    xhr2.open("POST", "/Comments/RemoveLikeComment?commentId=" + commentId, true);
    xhr2.send();
}

function DislikeComment(commentId) {
    var xhr2 = new XMLHttpRequest();
    xhr2.onload = function () {
        window.location = document.location.origin + '/Videos/Details/' + videoId;
    }
    xhr2.open("POST", "/Comments/DislikeComment?commentId=" + commentId, true);
    xhr2.send();
}

function RemoveDislikeComment(commentId) {
    var xhr2 = new XMLHttpRequest();
    xhr2.onload = function () {
        window.location = document.location.origin + '/Videos/Details/' + videoId;
    }
    xhr2.open("POST", "/Comments/RemoveDisikeComment?commentId=" + commentId, true);
    xhr2.send();
}

function CreateReply(commentId) {
    var formData = new FormData();
    formData.append("commentId", commentId)
    formData.append("content", document.getElementById("reply " + commentId).value);

    var xhr2 = new XMLHttpRequest();
    xhr2.onload = function () {
        window.location = document.location.origin + '/Videos/Details/' + videoId;
    }
    xhr2.open("POST", "/Replies/CreateReply?videoId=" + videoId, true);
    xhr2.send(formData);
}

function EditReply(replyId) {
    var formData = new FormData();
    formData.append("newReply", document.getElementById("newreply " + replyId).value);
    formData.append("ReplyId", replyId);

    var xhr2 = new XMLHttpRequest();
    xhr2.onload = function () {
        window.location = document.location.origin + '/Videos/Details/' + videoId;
    }
    xhr2.open("POST", "/Replies/EditReply", true);
    xhr2.send(formData);
}

function DeleteReply(replyId) {
    var xhr2 = new XMLHttpRequest();
    xhr2.onload = function () {
        window.location = document.location.origin + '/Videos/Details/' + videoId;
    }
    xhr2.open("POST", "/Replies/DeleteReply?ReplyId=" + replyId , true);
    xhr2.send();
}

function LikeReply(replyId) {
    var xhr2 = new XMLHttpRequest();
    xhr2.onload = function () {
        window.location = document.location.origin + '/Videos/Details/' + videoId;
    }
    xhr2.open("POST", "/Replies/LikeReply?ReplyId=" + replyId, true);
    xhr2.send();
}

function DislikeReply(replyId) {
    var xhr2 = new XMLHttpRequest();
    xhr2.onload = function () {
        window.location = document.location.origin + '/Videos/Details/' + videoId;
    }
    xhr2.open("POST", "/Replies/DislikeReply?ReplyId=" + replyId, true);
    xhr2.send();
}
