function ModalEvent(sender) {
    this._sender = sender;
    this._listeners = [];
}

ModalEvent.prototype = {
    attach: function (id, listener) {
        this._listeners.push(
            {
                Listener: listener,
                Id: id,
            });
    },
    notify: function (id, ...args) {
        for (var i = 0; i < this._listeners.length; i++) {
            if (this._listeners[i].Id == id)
                this._listeners[i].Listener(this._sender, args);
        }
    }
};

onOpenModal = new ModalEvent(this);
onCloseModal = new ModalEvent(this);

function openModal(obj, id = 'mainModal') {
    const modal = document.getElementById(id);

    onOpenModal.notify(id, modal, obj);

    modal.style.display = "flex";
}

function closeModal(id = 'mainModal') {
    const modal = document.getElementById(id)
    modal.style.display = "none";

    onCloseModal.notify(id, modal);
}


window.onclick = function(event) {
    if (event.target.classList.contains('modal')) {
        closeModal(event.target.id);
    }
};
