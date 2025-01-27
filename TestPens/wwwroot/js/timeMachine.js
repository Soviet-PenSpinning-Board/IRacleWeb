function toLocalISOString(date) {
    const localDate = new Date(date - date.getTimezoneOffset() * 60000); //offset in milliseconds. Credit https://stackoverflow.com/questions/10830357/javascript-toisostring-ignores-timezone-offset

    // Optionally remove second/millisecond if needed
    localDate.setSeconds(null);
    localDate.setMilliseconds(null);
    return localDate.toISOString().slice(0, -1);
}

document.addEventListener("DOMContentLoaded", () => {
    document.getElementById("time-machine-destination").value = toLocalISOString(new Date());
});

function SumbitTimeMachine() {
    let selectedTime = document.getElementById("time-machine-destination").value;
    let formattedTime = moment(selectedTime).utc().format();

    $.ajax({
        url: `/tierlist/TimeMachineMain?dateTime=${formattedTime}`,
        type: 'POST',
        success: function (response) {
            $("#resultContainer").empty().html(response);
        },
        error: function () {
            alertModal("Ошибка запроса. Посмотрите в консоль на F12 и сообщите");
        }
    });
}