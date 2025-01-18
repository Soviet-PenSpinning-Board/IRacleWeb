function openModal(obj) {
    const modal = document.getElementsByClassName("modal")[0];

    onOpenModal(modal, obj);

    modal.style.display = "flex";
}

function closeModal() {
    const modal = document.getElementsByClassName("modal")[0];
    modal.style.display = "none";

    onCloseModal(modal);
}


window.onclick = function(event) {
    const modal = document.getElementsByClassName("modal")[0];
    if (event.target === modal) {
        closeModal();
    }
};