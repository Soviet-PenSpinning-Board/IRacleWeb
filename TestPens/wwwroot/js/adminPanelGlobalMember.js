function CreateNewPerson() {
    let nickname = document.getElementById('nickname-new-person').value;
    let videoLink = document.getElementById('videoLink-new-person').value;
    let avatarUrl = document.getElementById('avatarUrl-new-person').value;

    if (!nickname || !videoLink || !avatarUrl) {
        alertModal("Одно из полей не заполнено!");
        return;
    }

    changeCache.push(CreateChange({
        TargetPerson: {
            Nickname: nickname,
            VideoLink: videoLink,
            AvatarUrl: avatarUrl,
            InDrop: false,
        },
        TargetPosition: {
            Tier: "E",
            TierPosition: -1,
        },
        IsNew: true,
    }, "GlobalPerson"));

    sendUpdates();
}