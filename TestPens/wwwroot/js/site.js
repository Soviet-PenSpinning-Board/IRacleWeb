function ModalEvent(sender) {
    this._sender = sender;
    this._listeners = [];
}

ModalEvent.prototype = {
    attach: function (listener) {
        this._listeners.push(listener);
    },
    notify: function (...args) {
        for (var i = 0; i < this._listeners.length; i++) {
            this._listeners[i](this._sender, args);
        }
    }
};

onOpenModal = new ModalEvent(this);
onCloseModal = new ModalEvent(this);

function openModal(obj, id = 'mainModal') {
    const modal = document.getElementById(id);

    onOpenModal.notify(modal, obj);

    modal.style.display = "flex";
}

function closeModal(id = 'mainModal') {
    const modal = document.getElementById(id)
    modal.style.display = "none";

    onCloseModal.notify(modal);
}


window.onclick = function(event) {
    if (event.target.classList.contains('modal')) {
        closeModal();
    }
};
