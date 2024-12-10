function loadAppointmentInfo(appointmentId) {
    console.log("Selected appointment Id:", appointmentId);
    if (!appointmentId) {
        // Xóa thông tin nếu không có ID được chọn
        document.querySelector('#appointment-info').innerHTML = `<p>Chưa có thông tin lịch đặt được chọn.</p>`;
        return;
    }
    const baseUrl = window.location.origin; // Lấy gốc URL, ví dụ: https://localhost:8080
    const url = `${baseUrl}/appointment/AppointmentInfo/${appointmentId}`;
    console.log("url", url);

    fetch(url)
        .then(response => {
            if (!response.ok) {
                throw new Error('Appointment not found');
            }
            return response.json();
        })
        .then(data => {
            // Hiển thị thông tin bác sĩ
            document.querySelector('#appointment-info').innerHTML = `
                            <div class="d-flex">
                                <p class="text-start me-1">Số: </p>
                                <p class="text-dark text-bold">${data.number}</p>
                            </div>

                            <div class="d-flex">
                                <p class="text-start me-1">Mã hồ sơ: </p>
                                <p class="text-dark text-bold">${data.profile}</p>
                            </div>
                            <div class="d-flex">
                                <p class="text-start me-1">Mã bác sĩ: </p>
                                <p class="text-dark text-bold">${data.doctor}</p>
                            </div>
                            <div class="d-flex">
                                <p class="text-start me-1">Mã lịch đặt: </p>
                                <p class="text-dark">${data.id}</p>
                            </div>
                            <div class="d-flex">
                                <p class="text-start me-1">Thời gian đặt: </p>
                                <p class="text-dark">${data.at}</p>
                            </div>
                        `;
        })
        .catch(error => {
            console.error('Error:', error);
            document.querySelector('#doctor-info').innerHTML = `<p class="text-danger">Không tìm thấy thông tin bác sĩ.</p>`;
        });
}