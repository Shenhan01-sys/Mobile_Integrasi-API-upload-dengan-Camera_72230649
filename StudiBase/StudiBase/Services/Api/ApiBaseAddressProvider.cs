using System;

namespace StudiBase.Services.Api
{
 public static class ApiBaseAddressProvider
 {
 // TODO: Sesuaikan port di bawah dengan port actual dari StudiBase.Web (lihat Properties/launchSettings.json)
 private const int HttpsPortWindows =7231; // https profile
 private const int HttpPortHttp =5190; // http profile

 public static Uri GetBaseAddress()
 {
#if ANDROID
 // Android emulator mengakses host lewat10.0.2.2 dan lebih mudah gunakan HTTP saat dev
 return new Uri($"http://10.0.2.2:{HttpPortHttp}/");
#elif IOS || MACCATALYST
 // Simulator iOS/MacCatalyst: gunakan localhost dan pastikan port HTTP terbuka
 return new Uri($"http://localhost:{HttpPortHttp}/");
#else
 // Windows (desktop): gunakan HTTPS localhost
 return new Uri($"https://localhost:{HttpsPortWindows}/");
#endif
 }
 }
}
