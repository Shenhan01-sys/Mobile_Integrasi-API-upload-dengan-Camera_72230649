using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudiBase.Shared.Services // Sesuaikan namespace Anda
{
    public interface ICameraService
    {
        Task<string> TakePhotoAsync();
    }
}