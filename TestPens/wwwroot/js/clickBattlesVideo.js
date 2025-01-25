Array.from(document.getElementsByClassName('participant')).forEach(elem => {
    elem.addEventListener('click', (t) => {
        let iframe = t.currentTarget.children[2];

        if (iframe.style.display != 'none') {
            iframe.style.display = 'none';
            iframe.src = '';

        }
        else {
            iframe.src = t.currentTarget.dataset.video;
            iframe.style.display = '';
        }
    })
})