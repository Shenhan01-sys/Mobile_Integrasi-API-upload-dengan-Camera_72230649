using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudiBase.Shared.Services;

namespace StudiBase.Services
{
    public class NativeGeocodingService : IGeocodingService
    {
        public async Task<string?> GetAddressAsync(double lat, double lng)
        {
            try
            {
                // Gunakan fitur bawaan MAUI Essentials
                var placemarks = await Geocoding.Default.GetPlacemarksAsync(lat, lng);
                var placemark = placemarks?.FirstOrDefault();

                if (placemark != null)
                {
                    return $"{placemark.Thoroughfare}, {placemark.Locality}, {placemark.CountryName}";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Geocoding Error: {ex.Message}");
            }
            return "Alamat tidak ditemukan";
        }
    }
}
