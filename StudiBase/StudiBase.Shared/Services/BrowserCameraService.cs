using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudiBase.Shared.Services;

namespace StudiBase.Web.Services
{
    // Service ini khusus untuk Project Web agar tidak Error DI
    public class BrowserCameraService : ICameraService
    {
        public Task<string> TakePhotoAsync()
        {
            // Web tidak bisa akses kamera native via MAUI.
            // Kita return null saja atau bisa implementasi JSInterop nanti jika mau.
            Console.WriteLine("Fitur kamera native tidak tersedia di Web Browser.");
            return Task.FromResult<string>(null!);
        }
    }
}