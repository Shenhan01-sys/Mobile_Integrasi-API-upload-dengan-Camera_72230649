using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudiBase.Shared.Services
{
    public interface IGeocodingService
    {
        Task<string?> GetAddressAsync(double lat, double lng);
    }
}