using Microsoft.JSInterop;
using StudiBase.Shared.Models;
using StudiBase.Shared.Services;

namespace StudiBase.Web.Services
{
    public class BrowserGeolocationService : IGeolocationService
    {
        private readonly IJSRuntime _js;

        // Inject IJSRuntime untuk komunikasi dengan JavaScript
        public BrowserGeolocationService(IJSRuntime js)
        {
            _js = js;
        }

        public async Task<GeoLocationModel> GetCurrentLocationAsync()
        {
            try
            {
                // Memanggil fungsi 'window.browserLocation.getCurrentPosition' dari file JS
                var result = await _js.InvokeAsync<GeoLocationModel>("browserLocation.getCurrentPosition");
                return result;
            }
            catch (Exception ex)
            {
                // Handle jika user menolak izin lokasi di browser (Block)
                Console.WriteLine($"Error Browser Geo: {ex.Message}");
                return null;
            }
        }
    }
}