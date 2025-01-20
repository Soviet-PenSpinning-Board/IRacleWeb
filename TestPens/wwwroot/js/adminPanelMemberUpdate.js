if (typeof this.lockMemberUpdate === 'undefined') {

    onOpenModal.attach('mainModal', function (sender, args) {
        args[0].dataset.personId = args[1].id;

        const player = getPlayerObject(args[1].id);

        document.getElementById("nickname").value = player.Nickname;
        document.getElementById("inDrop").checked = player.InDrop;
        document.getElementById("videoLink").value = player.VideoLink;
        document.getElementById("avatarUrl").value = player.AvatarUrl;
    });

    var lockMemberUpdate = 'a';
}

function playerPropertiesUpdate() {
    const modal = document.getElementsByClassName("modal")[0];

    const id = modal.dataset.personId;

    const targetBlock = document.getElementById(id);

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

    setPlayerObject(id, player);

    closeModal();
}