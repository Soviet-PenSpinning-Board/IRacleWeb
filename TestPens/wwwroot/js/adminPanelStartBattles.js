onOpenModal.attach('mainModal', function (sender, args) {
    const id = args[1].id;
    args[0].dataset.personId = id;
    const player = getPlayerObject(id);
    args[0].firstElementChild.firstElementChild.lastElementChild.firstElementChild.src = player.AvatarUrl;

    document.getElementById("startBattle").checked = battledPlayers.has(id);
});

if (typeof battledPlayers === 'undefined') {
    var battledPlayers = new Set();
}

document.getElementById("startBattle").addEventListener('change', function () {
    const modal = document.getElementsByClassName("modal")[0];
    const id = modal.dataset.personId;
    if (this.checked && !battledPlayers.has(id)) {
        if (battledPlayers.size >= 2) {
            alert("Нельзя за 1 раз указать в параметры баттла больше двух человек!");
            this.checked = false;
            return;
        }
        battledPlayers.add(id);
    }
    else if (!this.checked && battledPlayers.has(id)) {
        battledPlayers.delete(id);
    }

    let elems = Array.from(document.getElementsByClassName("createBattleExample"));

    resetBattles(false);

    let i = 0;
    battledPlayers.forEach(id => {
        let playerObj = getFullPlayerObject(id);

        // мда
        elems[i].children[0].children[0].src = playerObj.AvatarUrl;
        elems[i].children[1].firstChild.dataset.tier = playerObj.Tier;
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
});

function resetBattles(clearCache = true) {
    Array.from(document.getElementsByClassName("createBattleExample")).forEach(elem => {
        // мда
        elem.children[1].firstChild.dataset.tier = "";
        elem.children[1].childNodes[1].textContent = "Участник";
        elem.children[0].children[0].src = "/avatars/Art.png";
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
    battledPlayers.forEach(id => {
        arr.push(getFullPlayerObject(id));
    });

    let videos = document.getElementsByClassName('battle-link');

    let prop = {
        Left: {
            VideoUrl: videos[0].value,
            Guid: arr[0].Guid,
        },
        Right: {
            VideoUrl: videos[1].value,
            Guid: arr[1].Guid,
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