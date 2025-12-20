window.networkInfo = {
    initialize: function (dotNetHelper) {
        function updateStatus() {
            dotNetHelper.invokeMethodAsync('OnConnectionStatusChanged', navigator.onLine);
        }
        window.addEventListener('online', updateStatus);
        window.addEventListener('offline', updateStatus);
        return navigator.onLine;
    }
};