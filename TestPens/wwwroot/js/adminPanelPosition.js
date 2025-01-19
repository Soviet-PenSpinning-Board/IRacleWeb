// Получаем все списки людей
document.querySelectorAll('.tier-people').forEach(tier => {
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
            evt.item.children[0].dataset.tier = toTier;

            evt.item.classList.add('modified');
        },
        onStart: function (evt) {
            evt.item.classList.add('dragging');
        },
    });
});