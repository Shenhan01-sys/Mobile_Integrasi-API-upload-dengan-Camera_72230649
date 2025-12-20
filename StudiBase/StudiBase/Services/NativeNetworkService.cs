using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudiBase.Shared.Services;
using Microsoft.Maui.Networking;

namespace StudiBase.Services
{
    public class NativeNetworkService : INetworkService, IDisposable
    {
        public bool IsOnline => Connectivity.Current.NetworkAccess == NetworkAccess.Internet;

        public event Action? StatusChanged;

        public NativeNetworkService()
        {
            Connectivity.Current.ConnectivityChanged += OnConnectivityChanged;
        }

        private void OnConnectivityChanged(object? sender, ConnectivityChangedEventArgs e)
        {
            StatusChanged?.Invoke();
        }

        public void Dispose()
        {
            Connectivity.Current.ConnectivityChanged -= OnConnectivityChanged;
        }
    }
}