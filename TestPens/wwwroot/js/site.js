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

function alertModal(text) {
    openModal(text, 'alertModal')
}

function initModals() {
    onOpenModal = new ModalEvent(this);
    onOpenModal.attach('alertModal', (sender, args) => {
        args[0].firstElementChild.firstElementChild.innerText = args[1];
    })
    onCloseModal = new ModalEvent(this);
}

initModals();

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

document.addEventListener("DOMContentLoaded", () => {
    let themeToggle = document.getElementById('themeToggle');
    themeToggle.addEventListener("click", () => {
        document.body.classList.toggle("dark-theme");

        if (document.body.classList.contains("dark-theme")) {
            themeToggle.textContent = "☀️";
        } else {
            themeToggle.textContent = "🌙";
        }
    });
});