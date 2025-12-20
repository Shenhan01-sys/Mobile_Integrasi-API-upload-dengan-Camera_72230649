using Microsoft.Extensions.Logging;
using StudiBase.Services;
using StudiBase.Shared.Services;
using StudiBase.Services.Api;
using StudiBase.Shared.Clients;

namespace StudiBase
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            // Add device-specific services used by the StudiBase.Shared project
            builder.Services.AddSingleton<IFormFactor, FormFactor>();

            // Service Kamera Native
            builder.Services.AddTransient<ICameraService, NativeCameraService>();
            builder.Services.AddScoped<ICameraService, NativeCameraService>();

            builder.Services.AddMauiBlazorWebView();

            builder.Services.AddSingleton<INetworkService, NativeNetworkService>();

            builder.Services.AddTransient<IGeocodingService, NativeGeocodingService>();

            // Daftarkan Service Geolocation Native
            builder.Services.AddTransient<IGeolocationService, NativeGeolocationService>();

            // HttpClient + API clients
            builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = ApiBaseAddressProvider.GetBaseAddress() });

            // >>> PASTIKAN KETIGA CLIENT INI ADA <<<
            builder.Services.AddScoped<TrainerApiClient>();
            builder.Services.AddScoped<CourseApiClient>();
            builder.Services.AddScoped<FileApiClient>(); // <-- INI YANG TADI HILANG
            // >>> SELESAI <<<

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}