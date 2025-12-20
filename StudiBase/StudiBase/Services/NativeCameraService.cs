using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Media;
using Microsoft.Maui.Storage;
using Microsoft.Maui.ApplicationModel;
using StudiBase.Shared.Services;

namespace StudiBase.Services
{
    public class NativeCameraService : ICameraService
    {
        public async Task<string?> TakePhotoAsync()
        {
            try
            {
                // Cek apakah perangkat mendukung ambil foto
                if (MediaPicker.Default.IsCaptureSupported)
                {
                    // 1. Buka Aplikasi Kamera Bawaan (Windows Camera / Android Camera)
                    FileResult? photo = await MediaPicker.Default.CapturePhotoAsync();

                    if (photo != null)
                    {
                        // 2. Baca file hasil foto
                        using var stream = await photo.OpenReadAsync();
                        using var memoryStream = new MemoryStream();
                        await stream.CopyToAsync(memoryStream);
                        
                        // 3. Konversi ke Base64 agar bisa ditampilkan di Blazor (sama seperti Web)
                        var bytes = memoryStream.ToArray();
                        var base64 = Convert.ToBase64String(bytes);
                        
                        // Format data URI agar <img> bisa membacanya
                        return $"data:image/jpeg;base64,{base64}";
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error jika kamera gagal dibuka
                System.Diagnostics.Debug.WriteLine($"Camera Error: {ex.Message}");
            }

            return null;
        }
    }
}