window.browserCamera = {
    takePhoto: function () {
        return new Promise((resolve, reject) => {
            // 1. Cek apakah browser support kamera
            if (!navigator.mediaDevices || !navigator.mediaDevices.getUserMedia) {
                alert("Kamera tidak didukung di browser ini.");
                resolve(null);
                return;
            }

            // 2. Buat elemen UI secara dinamis (Modal, Video, Canvas, Tombol)
            let overlay = document.createElement("div");
            overlay.style.cssText = "position:fixed;top:0;left:0;width:100%;height:100%;background:rgba(0,0,0,0.9);z-index:9999;display:flex;flex-direction:column;align-items:center;justify-content:center;";

            let video = document.createElement("video");
            video.style.cssText = "width:100%;max-width:640px;border:2px solid white;border-radius:8px;margin-bottom:20px;";
            video.autoplay = true;
            video.playsInline = true; // Penting untuk iOS/Mobile Web

            let btnContainer = document.createElement("div");

            let btnCapture = document.createElement("button");
            btnCapture.innerText = "📸 Ambil Foto";
            btnCapture.className = "btn btn-success btn-lg me-2"; // Pake class bootstrap

            let btnCancel = document.createElement("button");
            btnCancel.innerText = "❌ Batal";
            btnCancel.className = "btn btn-secondary btn-lg";

            btnContainer.appendChild(btnCapture);
            btnContainer.appendChild(btnCancel);
            overlay.appendChild(video);
            overlay.appendChild(btnContainer);
            document.body.appendChild(overlay);

            let streamRef = null;

            // 3. Nyalakan Kamera
            navigator.mediaDevices.getUserMedia({ video: { facingMode: "user" } })
                .then(stream => {
                    streamRef = stream;
                    video.srcObject = stream;
                })
                .catch(err => {
                    console.error("Camera Error:", err);
                    alert("Gagal akses kamera: " + err.message);
                    closeCamera();
                    resolve(null);
                });

            // 4. Fungsi Bersih-bersih
            function closeCamera() {
                if (streamRef) {
                    streamRef.getTracks().forEach(track => track.stop());
                }
                document.body.removeChild(overlay);
            }

            // 5. Handle Tombol Capture
            btnCapture.onclick = () => {
                let canvas = document.createElement("canvas");
                canvas.width = video.videoWidth;
                canvas.height = video.videoHeight;
                let ctx = canvas.getContext("2d");
                ctx.drawImage(video, 0, 0, canvas.width, canvas.height);

                // Konversi ke Base64 (Format: "data:image/jpeg;base64,.....")
                let dataUrl = canvas.toDataURL("image/jpeg", 0.8);

                closeCamera();
                resolve(dataUrl); // Kirim balik ke C#
            };

            // 6. Handle Tombol Batal
            btnCancel.onclick = () => {
                closeCamera();
                resolve(null);
            };
        });
    }
};