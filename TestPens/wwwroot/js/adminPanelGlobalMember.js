function CreateNewPerson() {
    let nickname = document.getElementById('nickname-new-person').value;
    let videoLink = document.getElementById('videoLink-new-person').value;
    let avatarUrl = document.getElementById('avatarUrl-new-person').value;

    if (!nickname || !videoLink) {
        alertModal("Одно из полей не заполнено!");
        return;
    }

    if (!avatarUrl) {
        avatarUrl = "/avatars/default.png";
    }

    changeCache.push(CreateChange({
        NewPerson: {
            Nickname: nickname,
            VideoLink: videoLink,
            AvatarUrl: avatarUrl,
            InDrop: false,
            Description: "",
        },
        TargetPosition: {
            Tier: "E",
            TierPosition: -1,
        },
        IsNew: true,
    }, "GlobalPerson"));

    sendUpdates();
}