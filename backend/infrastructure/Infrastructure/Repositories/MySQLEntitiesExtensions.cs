using AppointmentScheduler.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentScheduler.Infrastructure.Repositories;

public static class MySQLEntitiesExtensions
{
    internal static void BuildPrescriptionDetailEntity(EntityTypeBuilder<PrescriptionDetail> builder)
    {
        builder.ToTable("prescriptiondetail");
        builder.HasKey(nameof(PrescriptionDetail.Id));
        builder.Property<uint>(nameof(PrescriptionDetail.Id))
            .HasColumnName("Id")
            .HasColumnType("int unsigned");
        builder.Property<uint>(nameof(PrescriptionDetail.PrescriptionId))
            .HasColumnName("PrescriptionId")
            .HasColumnType("int unsigned");
        builder.Property<uint>(nameof(PrescriptionDetail.MedicineId))
            .HasColumnName("MedicineId")
            .HasColumnType("int unsigned");
        builder.Property<string>(nameof(PrescriptionDetail.Description))
            .HasColumnName("Description")
            .HasColumnType("varchar(500)");
    }

    internal static void BuildMedicineEntity(EntityTypeBuilder<Medicine> builder)
    {
        builder.ToTable("medicine");
        builder.HasKey(nameof(Medicine.Id));
        builder.Property<uint>(nameof(Medicine.Id))
            .HasColumnName("Id")
            .HasColumnType("int unsigned");
        builder.Property<string>(nameof(Medicine.Name))
            .HasColumnName("Name")
            .HasColumnType("varchar(100)");
        builder.Property<string>(nameof(Medicine.Unit))
            .HasColumnName("Unit")
            .HasColumnType("varchar(50)");
        // builder.Ignore(nameof(Medicine.Image));
    }

    internal static void BuildPrescriptionEntity(EntityTypeBuilder<Prescription> builder)
    {
        builder.ToTable("prescription");
        builder.HasKey(nameof(Prescription.Id));
        builder.Property<uint>(nameof(Prescription.Id))
            .HasColumnName("Id")
            .HasColumnType("int unsigned");
        builder.Property<uint>(nameof(Prescription.ExaminationId))
            .HasColumnName("ExaminationId")
            .HasColumnType("int unsigned");
        builder.Property<string>(nameof(Prescription.Description))
            .HasColumnName("Description")
            .HasColumnType("varchar(500)");
        builder.Ignore(nameof(Prescription.Document));
    }

    internal static void BuildExaminationServiceEntity(EntityTypeBuilder<ExaminationService> builder)
    {
        builder.ToTable("examinationservice");
        builder.HasKey(nameof(ExaminationService.Id));
        builder.Property<uint>(nameof(ExaminationService.Id))
            .HasColumnName("Id")
            .HasColumnType("int unsigned");
        builder.Property<uint>(nameof(ExaminationService.DoctorId))
            .HasColumnName("DoctorId")
            .HasColumnType("int unsigned");
        builder.Property<uint>(nameof(ExaminationService.DiagnosticServiceId))
            .HasColumnName("DiagnosticServiceId")
            .HasColumnType("int unsigned");
        builder.Property<uint>(nameof(ExaminationService.ExaminationId))
            .HasColumnName("ExaminationDetailId")
            .HasColumnType("int unsigned");
        builder.Ignore(nameof(ExaminationService.Document));
    }

    internal static void BuildDiagnosticServiceEntity(EntityTypeBuilder<DiagnosticService> builder)
    {
        builder.ToTable("diagnosticservice");
        builder.HasKey(nameof(DiagnosticService.Id));
        builder.Property<uint>(nameof(DiagnosticService.Id))
            .HasColumnName("Id")
            .HasColumnType("int unsigned");
        builder.Property<string>(nameof(DiagnosticService.Name))
            .HasColumnName("Name")
            .HasColumnType("varchar(250)");
        builder.Property<double>(nameof(DiagnosticService.Price))
            .HasColumnName("Price")
            .HasColumnType("double");
    }

    internal static void BuildExaminationEntity(EntityTypeBuilder<Examination> builder)
    {
        builder.ToTable("examinationdetail");
        builder.HasKey(nameof(Examination.Id));
        builder.Property<uint>(nameof(Examination.Id))
            .HasColumnName("Id")
            .HasColumnType("int unsigned");
        builder.Property<uint>(nameof(Examination.AppointmentId))
            .HasColumnName("AppointmentId")
            .HasColumnType("int unsigned");
        builder.Property<string>(nameof(Examination.Diagnostic))
            .HasColumnName("Diagnostic")
            .HasColumnType("varchar(50)");
        builder.Property<string>(nameof(Examination.Description))
            .HasColumnName("Description")
            .HasColumnType("varchar(50)");
        builder.Property<uint>(nameof(Examination.State))
            .HasColumnName("State")
            .HasColumnType("int unsigned");
    }

    internal static void BuildAppointmentEntity(EntityTypeBuilder<Appointment> builder)
    {
        builder.ToTable("appointment");
        builder.HasKey(nameof(Appointment.Id));
        builder.Property<uint>(nameof(Appointment.Id))
            .HasColumnName("Id")
            .HasColumnType("int unsigned");
        builder.Property<DateTime>(nameof(Appointment.AtTime))
            .HasColumnName("Time")
            .HasColumnType("datetime");
        builder.Property<uint>(nameof(Appointment.Number))
            .HasColumnName("Number")
            .HasColumnType("int");
        builder.Property<uint>(nameof(Appointment.State))
            .HasColumnName("State")
            .HasColumnType("int unsigned");
        builder.Property<uint>(nameof(Appointment.DoctorId))
            .HasColumnName("DoctorId")
            .HasColumnType("int unsigned");
        builder.Property<uint?>(nameof(Appointment.ProfileId))
            .HasColumnName("ProfileId")
            .HasColumnType("int unsigned");
    }

    internal static void BuildProfileEntity(EntityTypeBuilder<Profile> builder)
    {
        builder.ToTable("profile");
        builder.HasKey(nameof(Profile.Id));
        builder.Property<uint>(nameof(Profile.Id))
            .HasColumnName("Id")
            .HasColumnType("int unsigned");
        builder.Property<uint>(nameof(Profile.PatientId))
            .HasColumnName("PatientId")
            .HasColumnType("int unsigned");
        builder.Property<string>(nameof(Profile.FullName))
            .HasColumnName("Fullname")
            .HasColumnType("varchar(100)");
        builder.Property<DateOnly>(nameof(Profile.DateOfBirth))
            .HasColumnName("BirthDate")
            .HasColumnType("date")
            .HasConversion(d => new DateTime(d, default), dt => DateOnly.FromDateTime(dt));
        builder.Property<char>(nameof(Profile.Gender))
            .HasColumnName("Gender")
            .HasColumnType("char(1)");
    }


    internal static void BuildDoctorEntity(EntityTypeBuilder<Doctor> builder)
    {
        builder.ToTable("doctor");
        builder.HasKey(nameof(Doctor.Id));
        builder.Property<uint>(nameof(Doctor.Id))
            .HasColumnName("Id")
            .HasColumnType("int unsigned");
        builder.Property<string>(nameof(Doctor.Email))
            .HasColumnName("Email")
            .HasColumnType("varchar(50)");
        builder.Property<string>(nameof(Doctor.Phone))
            .HasColumnName("PhoneNumber")
            .HasColumnType("varchar(50)");
        builder.Property<string>(nameof(Doctor.Position))
            .HasColumnName("Position")
            .HasColumnType("varchar(50)");
        builder.Property<string>(nameof(Doctor.Certificate))
            .HasColumnName("Certificate")
            .HasColumnType("varchar(50)");
        // builder.Ignore(nameof(Doctor.Image));
    }

    internal static void BuildPatientEntity(EntityTypeBuilder<Patient> builder)
    {
        builder.ToTable("patient");
        builder.HasKey(nameof(Patient.Id));
        builder.Property<uint>(nameof(Patient.Id))
            .HasColumnName("Id")
            .HasColumnType("int unsigned");
        builder.Property<string>(nameof(Patient.Email))
            .HasColumnName("Email")
            .HasColumnType("varchar(50)");
        builder.Property<string>(nameof(Patient.Phone))
            .HasColumnName("PhoneNumber")
            .HasColumnType("varchar(50)");
        // builder.Ignore(nameof(Patient.Image));
    }

    internal static void BuildUserEntity(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("user");
        builder.HasKey(nameof(User.Id));
        builder.Property<uint>(nameof(User.Id))
            .HasColumnName("Id")
            .HasColumnType("int unsigned");
        builder.Property<string>(nameof(User.UserName))
            .HasColumnName("Username")
            .HasColumnType("varchar(50)");
        builder.Property<string>(nameof(User.FullName))
            .HasColumnName("Fullname")
            .HasColumnType("varchar(50)");
        builder.Property<string>(nameof(User.Password))
            .HasColumnName("Password")
            .HasColumnType("varchar(63)");
        builder.Property<uint>(nameof(User.RoleId))
            .HasColumnName("RoleId")
            .HasColumnType("int unsigned");
    }

    internal static void BuildRoleEntity(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("role");
        builder.HasKey(nameof(Role.Id));
        builder.Property<uint>(nameof(Role.Id))
            .HasColumnName("Id")
            .HasColumnType("int unsigned");
        builder.Property<string>(nameof(Role.Name))
            .HasColumnName("Name")
            .HasColumnType("varchar(50)");
        builder.Property<string>(nameof(Role.Description))
            .HasColumnName("Description")
            .HasColumnType("varchar(250)");
        builder.Property<byte[]>(nameof(Role.Permissions))
            .HasColumnName("Permissions")
            .HasColumnType("binary(60)");
    }
}