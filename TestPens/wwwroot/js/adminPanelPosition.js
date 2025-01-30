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

            if (fromIndex === toIndex && toTier === fromTier)
                return;

            const obj = {
                NewPosition: {
                    Tier: toTier,
                    TierPosition: toIndex,
                },
                TargetPosition: {
                    Tier: fromTier,
                    TierPosition: fromIndex,
                },
            };

            changeCache.push(CreateChange(obj, "ChangePosition"));

            evt.item.children[0].dataset.tier = toTier;

            evt.item.classList.add('modified');
        },
        onStart: function (evt) {
            evt.item.classList.add('dragging');
        },
    });
});