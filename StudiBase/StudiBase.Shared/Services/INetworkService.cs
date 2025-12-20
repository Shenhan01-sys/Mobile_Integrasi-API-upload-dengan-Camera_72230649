using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudiBase.Shared.Services
{
    public interface INetworkService
    {
        bool IsOnline { get; }
        event Action StatusChanged; // Memberitahu UI saat koneksi berubah
    }
}
