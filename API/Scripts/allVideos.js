
$("div #date").get().forEach((element) => {
    var seconds = element.innerHTML;
    var minute = Math.round(seconds / 60);
    var hour = Math.round(seconds / 3600);
    var day = Math.round(seconds / 86400);
    var month = Math.round(seconds / 2630000);
    var year = Math.round(seconds / 31556926);
    if (seconds > 60) {
        if (year > 0) {
            element.innerHTML = year + " years ago";
        }
        else if (month > 0) {
            element.innerHTML = month + " months ago";
        }
        else if (day > 0) {
            element.innerHTML = day + " days ago";
        }
        else if (hour > 0) {
            element.innerHTML = hour + " hours ago";
        }
        else {
            element.innerHTML = minute + " minutes ago";
        }
    }
    else {
        element.innerHTML = seconds + " seconds ago";
    }
});

function OpenVideo(id) {
    window.location = document.location.origin + '/Videos/Details/' + id;
}
function OpenCreatorProfile(id) {
    window.location = document.location.origin + '/Users/Details/' + id;
}