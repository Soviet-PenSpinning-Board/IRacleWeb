var authToken = '';

const changeCache = [];

function CreateChange(properties, type) {
    return {
        ...properties,
        Type: type,
        UtcNow: new Date().getTime()
    };
}

$(document).ready(function () {
    $("#submitButton").click(function () {
        ResetMainPage();
    });
});

function ResetMainPage() {
    authToken = $("#passwordInput").val();

    $.ajax({
        url: '/admin/checkpassword',
        type: 'POST',
        data: { password: authToken },
        success: function (response) {
            initModals();
            $("#resultContainer").empty().html(response);
            changeCache.length = 0;
            const targButton = document.getElementById("submitButton");
            targButton.classList.add("cancel-button");
            targButton.innerText = 'Сбросить';
        },
        error: function () {
            alertModal("Ошибка запроса. Посмотрите в консоль на F12 и сообщите");
        }
    });
}

document.getElementById('saveChanges').addEventListener('click', function () {
    if (changeCache.length > 0) {
        fetch(`/api/tierlist/addchanges?token=${authToken}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(changeCache),
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
    } else {
        alertModal('Нет изменений для сохранения.');
    }
});

function getPlayerObject(id) {
    let htmlDoc = document.getElementById(id);
    return {
        Nickname: htmlDoc.childNodes[2].nodeValue,
        InDrop: htmlDoc.dataset.inactive === "True",
        VideoLink: htmlDoc.dataset.video,
        AvatarUrl: htmlDoc.dataset.avatar,
    };
}

function getFullPlayerObject(id) {
    let htmlDoc = document.getElementById(id);
    return {
        Guid: htmlDoc.dataset.guid,
        Nickname: htmlDoc.childNodes[2].nodeValue,
        InDrop: htmlDoc.dataset.inactive === "True",
        VideoLink: htmlDoc.dataset.video,
        AvatarUrl: htmlDoc.dataset.avatar,
        Tier: htmlDoc.dataset.tier,
        Index: $(`#${id}`).index(),
    };
}

function setPlayerObject(id, playerData) {
    let htmlDoc = document.getElementById(id);

    htmlDoc.childNodes[2].nodeValue = playerData.Nickname;
    htmlDoc.dataset.inactive = playerData.InDrop ? "True" : "False";
    htmlDoc.dataset.video = playerData.VideoLink;
    htmlDoc.dataset.avatar = playerData.AvatarUrl;
}