using System;

namespace StudiBase.Services.Api
{
    public static class ApiBaseAddressProvider
    {
        // Pastikan port ini sama dengan backend
        private const int HttpsPortWindows = 7231;
        private const int HttpPortHttp = 5190;

        public static Uri GetBaseAddress()
        {
#if ANDROID
            // KHUSUS HP FISIK (ADB REVERSE): WAJIB 'localhost', BUKAN '10.0.2.2'
            return new Uri($"https://loraine-seminiferous-snappily.ngrok-free.dev");
#else
            // Windows/iOS Simulator
            return new Uri($"https://localhost:{HttpsPortWindows}/");
#endif
        }
    }
}