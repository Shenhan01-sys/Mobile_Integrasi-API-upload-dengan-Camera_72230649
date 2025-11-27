using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Media;
using Microsoft.Maui.Storage;
using Microsoft.Maui.ApplicationModel;
using StudiBase.Shared.Services; // Panggil Interface dari Shared

namespace StudiBase.Services
{
    public class NativeCameraService : ICameraService
    {
        public async Task<string> TakePhotoAsync()
        {
            try
            {
                // 1. Cek Permission
                var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.Camera>();
                    if (status != PermissionStatus.Granted) return null;
                }

                // 2. Ambil Foto
                var photo = await MediaPicker.Default.CapturePhotoAsync();
                if (photo == null) return null;

                // 3. Convert ke Base64
                using var stream = await photo.OpenReadAsync();
                using var ms = new MemoryStream();
                await stream.CopyToAsync(ms);

                return $"data:image/jpeg;base64,{Convert.ToBase64String(ms.ToArray())}";
            }
            catch (Exception)
            {
                return null; // Atau handle error sesuai kebutuhan
            }
        }
    }
}