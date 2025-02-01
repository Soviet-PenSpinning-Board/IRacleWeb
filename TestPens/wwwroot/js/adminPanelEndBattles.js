onOpenModal.attach('battleModal', function (sender, args) {
    const guid = args[1].dataset.guid;
    args[0].dataset.guid = guid;
    document.getElementsByClassName("left-win")[0].innerText = args[1].children[1].children[0].children[1].innerText;
    document.getElementsByClassName("right-win")[0].innerText = args[1].children[1].children[2].children[1].innerText;
});

Array.from(document.getElementsByClassName('battle-card')).forEach(elem => {
    elem.addEventListener('click', function (obj) {
        openModal(obj.currentTarget, 'battleModal');
    });
});

function setBattleResult(result) {
    let modal = document.getElementById('battleModal');
    const guid = modal.dataset.guid;

    fetch(`/api/battles/update?token=${authToken}&guid=${guid}&result=${result}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
    }).then(response => {
        if (response.ok) {
            alertModal('Изменения успешно сохранены!');
            ResetMainPage();
        } else {
            alertModal('Произошла ошибка при сохранении.');
        }
    }).catch(error => {
        console.error('Ошибка при отправке данных:', error);
        alertModal('Произошла ошибка при сохранении.');
    });
}