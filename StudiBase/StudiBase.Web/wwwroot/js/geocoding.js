window.geocodingInfo = {
    reverseGeocode: async function (lat, lng) {
        try {
            // Menggunakan API OpenStreetMap (Gratis & No API Key untuk pembelajaran)
            const response = await fetch(`https://nominatim.openstreetmap.org/reverse?format=jsonv2&lat=${lat}&lon=${lng}`);
            const data = await response.json();

            // Ambil alamat lengkap atau bagian tertentu saja
            return data.display_name || "Alamat tidak ditemukan";
        } catch (error) {
            console.error("Geocoding error:", error);
            return "Gagal memuat alamat";
        }
    }
};