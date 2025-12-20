using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using StudiBase.Shared.Services;

namespace StudiBase.Web.Services
{
    public class BrowserCameraService : ICameraService
    {
        private readonly IJSRuntime _js;

        public BrowserCameraService(IJSRuntime js)
        {
            _js = js;
        }

        public async Task<string> TakePhotoAsync()
        {
            try
            {
                // Panggil fungsi 'takePhoto' milik 'browserCamera' (sesuai file JS Anda)
                return await _js.InvokeAsync<string>("browserCamera.takePhoto");
            }
            catch
            {
                // Return null jika error atau user membatalkan
                return null!;
            }
        }
    }
}