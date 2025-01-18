// Получаем все списки людей

function getPlayerObject(htmlDoc) {
    return {
        Nickname: htmlDoc.childNodes[2].nodeValue,
        InDrop: htmlDoc.dataset.inactive === "True",
        VideoLink: htmlDoc.dataset.video,
        AvatarUrl: htmlDoc.dataset.avatar,
    };
}

function setPlayerObject(htmlDoc, playerData) {
    htmlDoc.childNodes[2].nodeValue = playerData.Nickname;
    htmlDoc.dataset.inactive = playerData.InDrop ? "True" : "False";
    htmlDoc.dataset.video = playerData.VideoLink;
    htmlDoc.dataset.avatar = playerData.AvatarUrl;
}

function playerPropertiesUpdate() {
    const modal = document.getElementsByClassName("modal")[0];

    const id = modal.dataset.personId;

    const targetBlock = document.getElementById(id);

    console.log(targetBlock);

    const tier = targetBlock.dataset.tier;
    const index = $(`#${id}`).index();

    const nickname = document.getElementById("nickname").value;
    const inDrop = document.getElementById("inDrop").checked;
    const videoLink = document.getElementById("videoLink").value;
    const avatarUrl = document.getElementById("avatarUrl").value;

    const player = {
        Nickname: nickname,
        InDrop: inDrop,
        VideoLink: videoLink,
        AvatarUrl: avatarUrl,
    }

    const obj = {
        Position: {
            Tier: tier,
            TierPosition: index,
        },
        NewProperties: player,
    };

    changeCache.push(CreateChange(obj, "PersonProperties"));

    targetBlock.classList.add('modified');

    setPlayerObject(targetBlock, player);

    closeModal();
}