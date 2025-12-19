using Microsoft.JSInterop;
using StudiBase.Shared.Models;
using StudiBase.Shared.Services;

namespace StudiBase.Web.Services
{
    public class BrowserGeolocationService : IGeolocationService
    {
        private readonly IJSRuntime _js;

        // Inject IJSRuntime agar bisa 'bicara' dengan JavaScript
        public BrowserGeolocationService(IJSRuntime js)
        {
            _js = js;
        }

        public async Task<GeoLocationModel> GetCurrentLocationAsync()
        {
            try
            {
                // Panggil fungsi JS 'window.browserLocation.getCurrentPosition'
                var result = await _js.InvokeAsync<GeoLocationModel>("browserLocation.getCurrentPosition");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Browser Geo: {ex.Message}");
                return null;
            }
        }
    }
}