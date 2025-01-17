function CreateChange(properties, type) {
    return {
        ...properties,
        Type: type,
        UtcNow: new Date().getTime()
    };
}

const changeCache = [];

// Получаем все списки людей
const tierLists = document.querySelectorAll('.tier-people');

tierLists.forEach(tier => {
    new Sortable(tier, {
        group: 'shared', // Позволяет перетаскивать между списками
        animation: 150,
        onEnd: function (evt) {
            evt.item.classList.remove('dragging');
            const fromIndex = evt.oldIndex; // Элемент, который перетащили
            const fromTier = evt.from.dataset.tier; // Откуда перетащили
            const toTier = evt.to.dataset.tier; // Куда перетащили
            const toIndex = evt.newIndex; // Новая позиция элемента

            const obj = {
                NewPosition: {
                    Tier: toTier,
                    TierPosition: toIndex,
                },
                OldPosition: {
                    Tier: fromTier,
                    TierPosition: fromIndex,
                },
            };

            if (fromIndex === toIndex && toTier === fromTier)
                return;

            changeCache.push(CreateChange(obj, "ChangePosition"));

            evt.item.dataset.tier = toTier;

            evt.item.classList.add('modified');
        },
        onStart: function (evt) {
            evt.item.classList.add('dragging');
        },
    });
});

const saveButton = document.getElementById('saveChanges');
    saveButton.addEventListener('click', function () {
    if (changeCache.length > 0) {
        console.log('Отправка данных на сервер:', changeCache);

        // Отправка изменений на сервер через fetch
        fetch(`/api/v1/tierlist/addchanges?token=${authToken}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(changeCache),
        }).then(response => {
            if (response.ok) {
                alert('Изменения успешно сохранены!');
                changeCache.length = 0; // Очищаем кеш после успешной отправки
                document.querySelectorAll('.tier-person').forEach(e => {
                    console.log(e);
                    e.classList.remove("modified")
                });
            } else {
                alert('Произошла ошибка при сохранении.');
            }
        }).catch(error => {
            console.error('Ошибка при отправке данных:', error);
            alert('Произошла ошибка при сохранении.');
        });
    } else {
        alert('Нет изменений для сохранения.');
    }
});