using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudiBase.Shared.Models;

namespace StudiBase.Shared.Services
{
    public interface IGeolocationService
    {
        Task<GeoLocationModel> GetCurrentLocationAsync();
    }
}
