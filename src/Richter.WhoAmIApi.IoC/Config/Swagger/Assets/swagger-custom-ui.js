(function () {
    document.addEventListener("DOMContentLoaded", function () {
        let topbar = document.querySelector('.swagger-ui .topbar-wrapper > a');

        if (!topbar) {
            let observer = new MutationObserver((mutations, obs) => {

                let topbar = document.querySelector('.swagger-ui .topbar-wrapper > a');

                if (topbar) {
                    obs.disconnect();
                    substituirLogo(topbar);
                }
            });

            observer.observe(document.body, { childList: true, subtree: true });
        }else{
            substituirLogo(topbar);
        }

        substituirFavicon();
    });

    function substituirLogo(topbar) {

        topbar.innerHTML = '';

        let logoContainer = document.createElement('div');
        logoContainer.classList.add("logo-container");

        let logo = document.createElement('img');
        logo.src = '../swagger/assets/icon.png';
        logo.alt = 'Icons';

        logoContainer.appendChild(logo);
        topbar.appendChild(logoContainer);
    }

    // function substituirFavicon() {

    //     document.querySelectorAll("link[rel~='icon']").forEach(e => e.remove());

    //     const link = document.createElement("link");

    //     link.rel = "icon";
    //     link.type = "image/png";
    //     link.href = "../swagger/assets/favicon.png";

    //     document.head.appendChild(link);
    // }
})();