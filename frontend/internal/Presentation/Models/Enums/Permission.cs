using AppointmentScheduler.Presentation.Attributes;

namespace AppointmentScheduler.Presentation.Models.Enums;


[Metadata("DisplayName", "{}", "Badge", "secondary")]
public enum Permission : uint
{
    [Metadata("DisplayName", "Quản trị hệ thống", "Badge", "secondary")]
    SystemPrivilege = 0,
    [Metadata("DisplayName", "Đọc vai trò", "Badge", "secondary")]
    ReadRole,
    [Metadata("DisplayName", "Tạo vai trò", "Badge", "secondary")]
    CreateRole,
    [Metadata("DisplayName", "Sửa vai trò", "Badge", "secondary")]
    UpdateRole,
    [Metadata("DisplayName", "Xoá vai trò", "Badge", "secondary")]
    DeleteRole,
    [Metadata("DisplayName", "Đọc người dùng", "Badge", "secondary")]
    ReadUser,
    [Metadata("DisplayName", "Tạo người dùng", "Badge", "secondary")]
    CreateUser,
    [Metadata("DisplayName", "Sửa người dùng", "Badge", "secondary")]
    UpdateUser,
    [Metadata("DisplayName", "Xoá người dùng", "Badge", "secondary")]
    DeleteUser,
    [Metadata("DisplayName", "Đọc hồ sơ", "Badge", "secondary")]
    ReadProfile,
    [Metadata("DisplayName", "Tạo hồ sơ", "Badge", "secondary")]
    CreateProfile,
    [Metadata("DisplayName", "Sửa hồ sơ", "Badge", "secondary")]
    UpdateProfile,
    [Metadata("DisplayName", "Xoá hồ sơ", "Badge", "secondary")]
    DeleteProfile,
    [Metadata("DisplayName", "Đọc dịch vụ chuẩn đoán", "Badge", "secondary")]
    ReadDiagnosticService,
    [Metadata("DisplayName", "Tạo dịch vụ chuẩn đoán", "Badge", "secondary")]
    CreateDiagnosticService,
    [Metadata("DisplayName", "Sửa dịch vụ chuẩn đoán", "Badge", "secondary")]
    UpdateDiagnosticService,
    [Metadata("DisplayName", "Xoá dịch vụ chuẩn đoán", "Badge", "secondary")]
    DeleteDiagnosticService,
    [Metadata("DisplayName", "Tạo chuẩn đoán", "Badge", "secondary")]
    CreateExaminationDiagnostic,
    [Metadata("DisplayName", "Khoá chuẩn đoán", "Badge", "secondary")]
    SealExaminationDiagnostic,
    [Metadata("DisplayName", "Xoá chuẩn đoán", "Badge", "secondary")]
    DeleteExaminationDiagnostic,
    [Metadata("DisplayName", "Đọc lịch hẹn", "Badge", "secondary")]
    ReadAppointment,
    [Metadata("DisplayName", "Tạo lịch hẹn", "Badge", "secondary")]
    CreateAppointment,
    [Metadata("DisplayName", "Xoá lịch hẹn", "Badge", "secondary")]
    DeleteAppointment,
    [Metadata("DisplayName", "Đọc phiếu khám", "Badge", "secondary")]
    ReadExamination,
    [Metadata("DisplayName", "Tạo phiếu khám", "Badge", "secondary")]
    CreateExamination,
    [Metadata("DisplayName", "Sửa phiếu khám", "Badge", "secondary")]
    UpdateExamination,
    [Metadata("DisplayName", "Xoá phiếu khám", "Badge", "secondary")]
    DeleteExamination,
    [Metadata("DisplayName", "Đọc đơn thuốc", "Badge", "secondary")]
    ReadPrescription,
    [Metadata("DisplayName", "Tạo đơn thuốc", "Badge", "secondary")]
    CreatePrescription,
    [Metadata("DisplayName", "Khoá đơn thuốc", "Badge", "secondary")]
    SealPrescription,
    [Metadata("DisplayName", "Xoá đơn thuốc", "Badge", "secondary")]
    DeletePrescription
}
