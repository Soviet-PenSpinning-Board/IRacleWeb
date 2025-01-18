let authToken = '';

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
        authToken = $("#passwordInput").val();

        $.ajax({
            url: '/admin/checkpassword',
            type: 'POST',
            data: { password: authToken },
            success: function (response) {
                $("#resultContainer").empty().html(response); // Выводим результат в контейнер
                changeCache.length = 0;
                const targButton = document.getElementById("submitButton");
                targButton.classList.add("pressed");
                targButton.innerText = 'Сбросить';
            },
            error: function () {
                alert("Ошибка запроса. Попробуйте позже.");
            }
        });
    });
});

document.getElementById('saveChanges').addEventListener('click', function () {
    if (changeCache.length > 0) {
        // Отправка изменений на сервер через fetch
        fetch(`/api/tierlist/addchanges?token=${authToken}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(changeCache),
        }).then(response => {
            if (response.ok) {
                alert('Изменения успешно сохранены!');
                changeCache.length = 0; // Очищаем кеш после успешной отправки
                document.querySelectorAll('.tier-person').forEach(e => {
                    e.classList.remove("modified")
                });
            } else {
                alert('Произошла ошибка при сохранении.');
            }
        }).catch(error => {
            console.error('Ошибка при отправке данных:', error);
            alert('Произошла ошибка при сохранении.');
        });
    } else {
        alert('Нет изменений для сохранения.');
    }
});