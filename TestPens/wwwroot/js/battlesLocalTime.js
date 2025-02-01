document.querySelectorAll(".battle-start-time").forEach(elem => {
    const utcDate = elem.dataset.utc;
    if (utcDate) {
        const localDate = moment.utc(utcDate).local();
        elem.textContent = localDate.format("DD MMM YYYY, HH:mm");
    }
});