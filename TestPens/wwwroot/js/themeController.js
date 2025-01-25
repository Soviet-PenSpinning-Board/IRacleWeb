if (localStorage.getItem("theme") === "dark") {
    document.documentElement.classList.add("dark-theme");
}

document.addEventListener("DOMContentLoaded", () => {
    let themeToggle = document.getElementById('themeToggle');

    if (document.documentElement.classList.contains("dark-theme")) {
        themeToggle.textContent = "☀️";
    }

    themeToggle.addEventListener("click", () => {
        document.documentElement.classList.toggle("dark-theme");

        if (document.documentElement.classList.contains("dark-theme")) {
            themeToggle.textContent = "☀️";
        } else {
            themeToggle.textContent = "🌙";
        }
    });
});