function loadDoctorInfo(doctorId) {
    console.log("Selected DoctorId:", doctorId);
    if (!doctorId) {
        // Xóa thông tin nếu không có ID được chọn
        document.querySelector('#doctor-info').innerHTML = `<p>Chưa có thông tin bác sĩ được chọn.</p>`;
        return;
    }
    const baseUrl = window.location.origin; // Lấy gốc URL, ví dụ: https://localhost:8080
    const url = `${baseUrl}/doctor/DoctorInfo/${doctorId}`;

    fetch(url)
        .then(response => {
            if (!response.ok) {
                throw new Error('Doctor not found');
            }
            return response.json();
        })
        .then(data => {
            // Hiển thị thông tin bác sĩ
            document.querySelector('#doctor-info').innerHTML = `
                            <div class="d-flex">
                                <p class="text-start me-1">Mã bác sĩ: </p>
                                <p class="text-dark">${data.id}</p>
                            </div>
                            <div class="d-flex">
                                <p class="text-start me-1">Tên: </p>
                                <p class="text-dark">${data.certificate}</p>
                            </div>
                            <div class="d-flex">
                                <p class="text-start me-1">Email: </p>
                                <p class="text-dark text-bold">${data.email}</p>
                            </div>
                            <div class="d-flex">
                                <p class="text-start me-1">Vị trí: </p>
                                <p class="text-dark text-bold">${data.position}</p>
                            </div>
                            <div class="d-flex">
                                <p class="text-start me-1">Chứng chỉ: </p>
                                <p class="text-dark text-bold">${data.certificate}</p>
                            </div>
                        `;
        })
        .catch(error => {
            console.error('Error:', error);
            document.querySelector('#doctor-info').innerHTML = `<p class="text-danger">Không tìm thấy thông tin bác sĩ.</p>`;
        });
}


function loadProfileInfo(profileId) {
    console.log("Selected profileId:", profileId);
    if (!profileId) {
        // Xóa thông tin nếu không có ID được chọn
        document.querySelector('#profile-info').innerHTML = `<p>Chưa có thông tin hồ sơ được chọn.</p>`;
        return;
    }
    const baseUrl = window.location.origin; // Lấy gốc URL, ví dụ: https://localhost:8080
    const url = `${baseUrl}/profile/ProfileInfo/${profileId}`;

    fetch(url)
        .then(response => {
            if (!response.ok) {
                throw new Error('Profile not found');
            }
            return response.json();
        })
        .then(data => {
            // Hiển thị thông tin bác sĩ
            const genderText = data.gender === 'M' ? 'Nam' : data.gender === 'F' ? 'Nữ' : 'Không xác định';

            document.querySelector('#profile-info').innerHTML = `
                            <div class="d-flex">
                                <p class="text-start me-1">Mã hồ sơ: </p>
                                <p class="text-dark">${data.id}</p>
                            </div>
                            <div class="d-flex">
                                <p class="text-start me-1">Mã bệnh nhân: </p>
                                <p class="text-dark">${data.patientId}</p>
                            </div>
                            <div class="d-flex">
                                <p class="text-start me-1">Họ tên: </p>
                                <p class="text-dark text-bold">${data.fullName}</p>
                            </div>
                            <div class="d-flex">
                                <p class="text-start me-1">Ngày sinh: </p>
                                <p class="text-dark text-bold">${data.dateOfBirth}</p>
                            </div>
                            <div class="d-flex">
                                <p class="text-start me-1">Giới tính: </p>
                                <p class="text-dark text-bold">${genderText}</p>
                            </div>
                        `;
        })
        .catch(error => {
            console.error('Error:', error);
            document.querySelector('#profile-info').innerHTML = `<p class="text-danger">Không tìm thấy thông tin hồ sơ.</p>`;
        });
}

