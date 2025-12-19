using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudiBase.Shared.Models;
using StudiBase.Shared.Services;
using Microsoft.Maui.Devices.Sensors;

namespace StudiBase.Services
{
    public class NativeGeolocationService : IGeolocationService
    {
        public async Task<GeoLocationModel> GetCurrentLocationAsync()
        {
            // REVISI: Bungkus dengan MainThread agar jalan di Windows
            return await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                try
                {
                    var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
                    if (status != PermissionStatus.Granted)
                    {
                        status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                        if (status != PermissionStatus.Granted) return null;
                    }

                    var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                    var location = await Geolocation.Default.GetLocationAsync(request);

                    if (location != null)
                    {
                        return new GeoLocationModel
                        {
                            Latitude = location.Latitude,
                            Longitude = location.Longitude
                        };
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error GPS: {ex.Message}");
                }
                return null;
            });
        }
    }
}
