﻿onOpenModal.attach('mainModal', function (sender, args) {
    let guid = args[1].id;
    args[0].dataset.guid = guid;
    const player = AllPersonByGuid.get(guid);

    args[0].firstElementChild.firstElementChild.lastElementChild.firstElementChild.src = player.AvatarUrl;
    document.getElementById("nickname").value = player.Nickname;
    document.getElementById("inDrop").checked = player.InDrop;
    document.getElementById("videoLink").value = player.VideoLink;
    document.getElementById("avatarUrl").value = player.AvatarUrl;
});

function playerPropertiesUpdate() {
    const modal = document.getElementById('mainModal');

    const guid = modal.dataset.guid;

    const targetBlock = document.getElementById(guid);

    const player = AllPersonByGuid.get(guid);

    const nickname = document.getElementById("nickname").value;
    const inDrop = document.getElementById("inDrop").checked;
    const videoLink = document.getElementById("videoLink").value;
    const avatarUrl = document.getElementById("avatarUrl").value;

    if (!nickname || !videoLink || !avatarUrl) {
        alertModal("Одно из полей не заполнено!");
        return;
    }

    player.Nickname = nickname;
    player.InDrop = inDrop;
    player.VideoLink = videoLink;
    player.AvatarUrl = avatarUrl;

    const obj = {
        TargetPosition: getIndexAndTier(guid),
        NewProperties: player,
    };

    changeCache.push(CreateChange(obj, "PersonProperties"));

    targetBlock.classList.add('modified');

    setPlayerObject(guid);

    closeModal();
}