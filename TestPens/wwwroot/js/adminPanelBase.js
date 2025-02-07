var authToken = '';

const changeCache = [];

function CreateChange(properties, type) {
    return {
        ...properties,
        Type: type
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
    sendUpdates();
});

function sendUpdates() {
    if (changeCache.length > 0) {
        fetch(`/api/changes/add?token=${authToken}`, {
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
}

function setPlayerObject(guid) {
    let htmlDoc = document.getElementById(guid);
    let playerData = AllPersonByGuid.get(guid);

    htmlDoc.childNodes[2].nodeValue = playerData.Nickname;
    htmlDoc.dataset.inactive = playerData.InDrop ? "True" : "False";
}

function getIndexAndTier(guid) {
    const targetBlock = document.getElementById(guid);

    const tier = targetBlock.parentElement.dataset.tier;
    const index = $(`#${guid}`).index();

    return {
        Tier: tier,
        TierPosition: index,
    }
}