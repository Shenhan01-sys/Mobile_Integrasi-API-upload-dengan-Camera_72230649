window.browserLocation = {
    getCurrentPosition: function () {
        return new Promise((resolve, reject) => {
            if (!navigator.geolocation) {
                resolve(null);
                return;
            }

            navigator.geolocation.getCurrentPosition(
                (position) => {
                    resolve({
                        latitude: position.coords.latitude,
                        longitude: position.coords.longitude
                    });
                },
                (error) => {
                    console.error("Geolocation error:", error);
                    // Opsi: Bisa resolve(null) atau reject(error) tergantung kebutuhan
                    // Di sini kita resolve null agar aplikasi tidak crash
                    resolve(null);
                },
                // --- BAGIAN INI YANG DIREVISI ---
                {
                    enableHighAccuracy: false, // Ubah ke FALSE agar lebih cepat (menggunakan Wi-Fi/IP)
                    timeout: 20000,            // Naikkan ke 20 detik (sebelumnya 5 detik terlalu cepat)
                    maximumAge: 60000          // Boleh pakai cache lokasi s.d 1 menit yang lalu
                }
            );
        });
    }
};