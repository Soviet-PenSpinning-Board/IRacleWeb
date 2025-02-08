onOpenModal.attach('mainModal', function (sender, args) {
    const guid = args[1].id;
    args[0].dataset.guid = guid;
    const player = AllPersonByGuid.get(guid);
    args[0].firstElementChild.firstElementChild.lastElementChild.firstElementChild.src = player.AvatarUrl;

    document.getElementById("startBattle").checked = battledPlayers.has(guid);
});

if (typeof battledPlayers === 'undefined') {
    var battledPlayers = new Set();
}

document.getElementById("startBattle").addEventListener('change', function () {
    const modal = document.getElementsByClassName("modal")[0];
    const guid = modal.dataset.guid;
    if (this.checked && !battledPlayers.has(guid)) {
        if (battledPlayers.size >= 2) {
            alert("Нельзя за 1 раз указать в параметры баттла больше двух человек!");
            this.checked = false;
            return;
        }
        battledPlayers.add(guid);
    }
    else if (!this.checked && battledPlayers.has(guid)) {
        battledPlayers.delete(guid);
    }

    let elems = Array.from(document.getElementsByClassName("createBattleExample"));

    resetBattles(false);

    let i = 0;
    battledPlayers.forEach(guid => {
        let playerObj = AllPersonByGuid.get(guid);

        // мда
        elems[i].children[0].children[0].src = playerObj.AvatarUrl;
        elems[i].children[1].firstChild.dataset.tier = getIndexAndTier(guid).Tier;
        elems[i].children[1].childNodes[1].textContent = playerObj.Nickname;
        i++;
    });

    let button = document.getElementById("create-battle-button");

    if (battledPlayers.size == 2) {
        button.classList.add("save-button");
        button.addEventListener("click", addBattleChange);
    }
    else {
        button.classList.remove("save-button");
        button.removeEventListener("click", addBattleChange);
    }

    closeModal(modal.id);
});

function resetBattles(clearCache = true) {
    Array.from(document.getElementsByClassName("createBattleExample")).forEach(elem => {
        // мда
        elem.children[1].firstChild.dataset.tier = "";
        elem.children[1].childNodes[1].textContent = "Участник";
        elem.children[0].children[0].src = "/avatars/default.png";
    });

    if (clearCache) {
        let button = document.getElementById("create-battle-button");
        button.classList.remove("save-button");
        battledPlayers.clear();
    }
}

resetBattles();

function addBattleChange() {
    let arr = [];
    battledPlayers.forEach(guid => {
        arr.push(guid);
    });

    let videos = document.getElementsByClassName('battle-link');

    let prop = {
        Left: {
            VideoUrl: videos[0].value,
            Guid: arr[0],
        },
        Right: {
            VideoUrl: videos[1].value,
            Guid: arr[1],
        },
    };

    fetch(`/api/battles/add?token=${authToken}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(prop),
    }).then(response => {
        if (response.ok) {
            alertModal('Изменения успешно сохранены!');
            resetBattles();
            ResetMainPage();
        } else {
            alertModal('Произошла ошибка при сохранении.');
        }
    }).catch(error => {
        console.error('Ошибка при отправке данных:', error);
        alertModal('Произошла ошибка при сохранении.');
    });
}