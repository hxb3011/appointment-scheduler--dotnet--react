CREATE TABLE `appointment` (
  `Id` int UNSIGNED NOT NULL,
  `Time` datetime DEFAULT NULL,
  `State` int UNSIGNED DEFAULT NULL,
  `ProfileId` int UNSIGNED DEFAULT NULL,
  `DoctorId` int UNSIGNED NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

CREATE TABLE `patient` (
  `Id` int UNSIGNED NOT NULL,
  `Email` varchar(50) DEFAULT NULL,
  `PhoneNumber` varchar(50) DEFAULT NULL,
  `Image` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

CREATE TABLE `diagnosticservice` (
  `Id` int UNSIGNED NOT NULL,
  `Name` varchar(250) DEFAULT NULL,
  `Price` double DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

CREATE TABLE `doctor` (
  `Id` int UNSIGNED NOT NULL,
  `Email` varchar(50) DEFAULT '',
  `Position` varchar(50) DEFAULT '',
  `Certificate` varchar(50) DEFAULT '',
  `PhoneNumber` varchar(50) DEFAULT '',
  `Image` varchar(50) DEFAULT '',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

CREATE TABLE `examinationdetail` (
  `Id` int UNSIGNED NOT NULL,
  `DoctorId` int UNSIGNED DEFAULT NULL,
  `AppointmentId` int UNSIGNED DEFAULT NULL,
  `Diagnostic` varchar(50) DEFAULT NULL,
  `Description` varchar(50) DEFAULT NULL,
  `State` int UNSIGNED DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

CREATE TABLE `examinationservice` (
  `Id` int UNSIGNED NOT NULL,
  `DoctorId` int UNSIGNED DEFAULT NULL,
  `DiagnosticServiceId` int UNSIGNED DEFAULT NULL,
  `ExaminationDetailId` int UNSIGNED DEFAULT NULL,
  `Document` varbinary(10000) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

CREATE TABLE `medicine` (
  `Id` int UNSIGNED NOT NULL,
  `Name` varchar(100) DEFAULT NULL,
  `Image` varchar(1000) DEFAULT NULL,
  `Unit` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

CREATE TABLE `prescription` (
  `Id` int UNSIGNED NOT NULL,
  `ExaminationId` int UNSIGNED DEFAULT NULL,
  `Description` varchar(500) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

CREATE TABLE `prescriptiondetail` (
  `Id` int UNSIGNED NOT NULL,
  `PrescriptionId` int UNSIGNED DEFAULT NULL,
  `MedicineId` int UNSIGNED DEFAULT NULL,
  `Description` varchar(500) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

CREATE TABLE `profile` (
  `Id` int UNSIGNED NOT NULL,
  `PatientId` int UNSIGNED DEFAULT NULL,
  `Fullname` varchar(100) DEFAULT NULL,
  `BirthDate` date DEFAULT NULL,
  `Gender` char(1) DEFAULT 'M',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

CREATE TABLE `role` (
  `Id` int UNSIGNED NOT NULL,
  `Name` varchar(50) DEFAULT NULL,
  `Description` varchar(250) DEFAULT NULL,
  `Permissions` binary(60) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

CREATE TABLE `user` (
  `Id` int UNSIGNED NOT NULL,
  `Username` varchar(50) DEFAULT NULL,
  `Password` varchar(50) DEFAULT NULL,
  `Fullname` varchar(50) DEFAULT NULL,
  `Address` varchar(50) DEFAULT NULL,
  `RoleId` int UNSIGNED DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;