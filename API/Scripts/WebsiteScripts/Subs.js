function Unsubscribe() {
    var url = document.location.href;
    var id = url.substring(url.lastIndexOf('/') + 1);
    console.log(id);
    var xmlHttp = new XMLHttpRequest();
    xmlHttp.open("POST", document.location.origin + '/Users/Unsubscribe?creatorId=' + id);
    xmlHttp.onload = function () {
        window.location.href = document.location.origin + '/Users/Details/' + id
    }
    xmlHttp.send();
}

function Subscribe() {
    var url = document.location.href;
    var id = url.substring(url.lastIndexOf('/') + 1);
    console.log(id);
    var xmlHttp = new XMLHttpRequest();
    xmlHttp.open("POST", document.location.origin + '/Users/Subscribe?creatorId=' + id);
    xmlHttp.onload = function () {
        window.location.href = document.location.origin + '/Users/Details/' + id
    }
    xmlHttp.send();
}
$("#edit").click(() => {
    window.location.href = document.location.origin + '/Users/Edit'
});