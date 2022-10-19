function openProfileSelect() {
    document.getElementById("ProfileImageInput").click()
}
function openBackgroundSelect() {
    document.getElementById("BackgroundImageInput").click()
}
const image_input1 = document.querySelector("#ProfileImageInput");

image_input1.addEventListener("change", function () {
    const reader = new FileReader();
    reader.addEventListener("load", () => {
        const uploaded_image = reader.result;
        document.querySelector("#ProfileImage").src = uploaded_image;
    });
    reader.readAsDataURL(this.files[0]);
});

const image_input2 = document.querySelector("#BackgroundImageInput");

image_input2.addEventListener("change", function () {
    const reader = new FileReader();
    reader.addEventListener("load", () => {
        const uploaded_image = reader.result;
        document.querySelector("#BackgroundImage").style.backgroundImage = `url(${uploaded_image})`;
    });
    reader.readAsDataURL(this.files[0]);
});

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
$('#ProfilePicture').change(function (e) {
    var file = e.target.files[0];
    var blob = file.slice(0, file.size, 'image/png');
    var name = randomName(10) + '.png';
    $("#ProfilePicture").prop("files")[0] = new File([blob], name, { type: 'image/png' });
});
$('#BackgroundPicture').change(function (e) {
    var file = e.target.files[0];
    var blob = file.slice(0, file.size, 'image/png');
    var name = randomName(10) + '.png';
    $("#BackgroundPicture").prop("files")[0] = new File([blob], name, { type: 'image/png' });
});