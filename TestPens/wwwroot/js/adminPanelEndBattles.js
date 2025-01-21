if (typeof this.lockEndBattles === 'undefined') {
    onOpenModal.attach('battleModal', function (sender, args) {
        console.log(args[1]);
        const guid = args[1].dataset.guid;
        args[0].dataset.guid = guid;
    });

    var lockEndBattles = 'a';
}

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
            alert('Изменения успешно сохранены!');
        } else {
            alert('Произошла ошибка при сохранении.');
        }
    }).catch(error => {
        console.error('Ошибка при отправке данных:', error);
        alert('Произошла ошибка при сохранении.');
    });
}