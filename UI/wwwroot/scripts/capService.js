window.CAP = {
    init: function (options) {
        const humand = document.querySelector(options.element);
        if (!humand) {
            console.error("No se encontró el contenedor: ", options.element);
            return;
        }

        const widget = document.createElement('cap-widget');
        widget.setAttribute('site', 'default');
        widget.setAttribute('data-cap-api-endpoint', 'https://localhost:7086/captcha/');

        widget.addEventListener('solve', (e) => {
            const token = e.detail.token;
            DotNet.invokeMethodAsync('UI', 'SetCaptchaToken', token);
        });

        humand.innerHTML = "";
        humand.appendChild(widget);
    }
}
