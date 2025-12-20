using Microsoft.JSInterop;
using StudiBase.Shared.Services;

namespace StudiBase.Web.Services
{
    public class BrowserGeocodingService : IGeocodingService
    {
        private readonly IJSRuntime _js;
        public BrowserGeocodingService(IJSRuntime js) => _js = js;

        public async Task<string?> GetAddressAsync(double lat, double lng)
        {
            return await _js.InvokeAsync<string>("geocodingInfo.reverseGeocode", lat, lng);
        }
    }
}
