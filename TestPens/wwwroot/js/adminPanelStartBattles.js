onOpenModal.attach('mainModal', function (sender, args) {
    const id = args[1].id;
    args[0].dataset.personId = id;

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
    elems.forEach(elem => {
        // мда
        elem.children[1].firstChild.dataset.tier = "";
        elem.children[1].childNodes[1].textContent = "Участник";
    });

    let i = 0;
    battledPlayers.forEach(id => {
        let playerObj = getFullPlayerObject(id);

        // мда
        elems[i].children[1].firstChild.dataset.tier = playerObj.Tier;
        elems[i].children[1].childNodes[1].textContent = playerObj.Nickname;
        i++;
    });

    let button = document.getElementById("create-battle-button");

    if (battledPlayers.size == 2) {
        button.dataset.full = "Yes";
        button.addEventListener("click", addBattleChange);
    }
    else {
        button.dataset.full = "None";
        button.removeEventListener("click", addBattleChange);
    }
});

function clearBattleCache() {
    Array.from(document.getElementsByClassName("createBattleExample")).forEach(elem => {
        // мда
        elem.children[1].firstChild.dataset.tier = "";
        elem.children[1].childNodes[1].textContent = "Участник";
    });
    battledPlayers.clear();
}

clearBattleCache();

function addBattleChange() {
    let arr = [];
    battledPlayers.forEach(id => {
        arr.push(getFullPlayerObject(id));
    });

    let prop = {
        Left: {
            Guid: arr[0].Guid,
        },
        Right: {
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
            alert('Изменения успешно сохранены!');
            clearBattleCache();
            ResetMainPage();
        } else {
            alert('Произошла ошибка при сохранении.');
        }
    }).catch(error => {
        console.error('Ошибка при отправке данных:', error);
        alert('Произошла ошибка при сохранении.');
    });
}