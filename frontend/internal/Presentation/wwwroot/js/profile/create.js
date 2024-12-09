function loadPatientInfo(patientId) {
    console.log("Selected PatientId:", patientId);
    if (!patientId) {
        // Xóa thông tin nếu không có ID được chọn
        document.querySelector('#patient-info').innerHTML = `<p>Chưa có thông tin bệnh nhân được chọn.</p>`;
        return;
    }
    const baseUrl = window.location.origin; // Lấy gốc URL, ví dụ: https://localhost:8080
    const url = `${baseUrl}/patient/PatientInfo/${patientId}`;

    fetch(url)
        .then(response => {
            if (!response.ok) {
                console.log("Loi khong vao duoc");
                throw new Error('Patient not found');
            }
            return response.json();
        })
        .then(data => {
            // Hiển thị thông tin bác sĩ
            document.querySelector('#patient-info').innerHTML = `
                            <div class="d-flex">
                                <p class="text-start me-1">Mã bệnh nhân: </p>
                                <p class="text-dark">${data.id}</p>
                            </div>
                            <div class="d-flex">
                                <p class="text-start me-1">Họ tên: </p>
                                <p class="text-dark">${data.full_name}</p>
                            </div>
                            <div class="d-flex">
                                <p class="text-start me-1">Email: </p>
                                <p class="text-dark text-bold">${data.email}</p>
                            </div>
                            <div class="d-flex">
                                <p class="text-start me-1">Số điện thoại: </p>
                                <p class="text-dark text-bold">${data.phone}</p>
                            </div>
                        `;
        })
        .catch(error => {
            console.error('Error:', error);
            document.querySelector('#patient-info').innerHTML = `<p class="text-danger">Không tìm thấy thông tin bệnh nhân.</p>`;
        });
}