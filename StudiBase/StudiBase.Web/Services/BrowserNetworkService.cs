using Microsoft.JSInterop;
using StudiBase.Shared.Services;

namespace StudiBase.Web.Services
{
    public class BrowserNetworkService : INetworkService
    {
        private readonly IJSRuntime _js;
        private bool _isOnline = true;

        public bool IsOnline => _isOnline;
        public event Action? StatusChanged;

        public BrowserNetworkService(IJSRuntime js)
        {
            _js = js;
            _ = Initialize();
        }

        private async Task Initialize()
        {
            var dotNetHelper = DotNetObjectReference.Create(this);
            _isOnline = await _js.InvokeAsync<bool>("networkInfo.initialize", dotNetHelper);
            StatusChanged?.Invoke();
        }

        [JSInvokable]
        public void OnConnectionStatusChanged(bool isOnline)
        {
            _isOnline = isOnline;
            StatusChanged?.Invoke();
        }
    }
}