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
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                    if (status != PermissionStatus.Granted)
                    {
                        throw new Exception("Location permission denied.");
                    }
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
                // Tangani pengecualian yang mungkin terjadi selama pengambilan lokasi
                throw new Exception("Failed to get current location.", ex);
            }

            return null;
        }
    }
}
