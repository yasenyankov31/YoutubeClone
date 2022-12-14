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