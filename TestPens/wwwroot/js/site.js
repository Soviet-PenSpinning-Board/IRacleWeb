function openModal(videoLink) {
    const modal = document.getElementById("videoModal");
    const iframe = document.getElementById("videoFrame");

    if (!videoLink)
        return;

    iframe.src = videoLink;

    modal.style.display = "flex";
}

function closeModal() {
    const modal = document.getElementById("videoModal");
    const iframe = document.getElementById("videoFrame");

    modal.style.display = "none";

    iframe.src = "";
}

window.onclick = function(event) {
    const modal = document.getElementById("videoModal");
    if (event.target === modal) {
        closeModal();
    }
};