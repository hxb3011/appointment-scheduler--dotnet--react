CREATE DATABASE  IF NOT EXISTS `apomtschedsys` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `apomtschedsys`;
-- MySQL dump 10.13  Distrib 8.0.40, for Win64 (x86_64)
--
-- Host: localhost    Database: apomtschedsys
-- ------------------------------------------------------
-- Server version	8.0.40

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `appointment`
--

DROP TABLE IF EXISTS `appointment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `appointment` (
  `Id` int unsigned NOT NULL,
  `Time` datetime DEFAULT NULL,
  `Number` int unsigned DEFAULT 0,
  `State` int unsigned DEFAULT 0,
  `ProfileId` int unsigned DEFAULT NULL,
  `DoctorId` int unsigned DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `appointment`
--

LOCK TABLES `appointment` WRITE;
/*!40000 ALTER TABLE `appointment` DISABLE KEYS */;
INSERT INTO `appointment` (`Id`, `Time`, `State`, `ProfileId`, `DoctorId`) VALUES (1684590067,'2024-01-01 10:30:00',0,1,1),(1684590068,'2024-02-01 09:00:00',1,2,3),(1684590069,'2024-02-02 10:00:00',2,3,4),(1684590070,'2024-02-03 11:00:00',0,4,1),(1684590071,'2024-02-04 14:00:00',1,5,2),(1684590072,'2024-02-05 15:00:00',2,1,3),(1684590073,'2024-02-06 08:00:00',1,2,4),(1684590074,'2024-02-07 09:30:00',2,3,1),(1684590075,'2024-02-08 13:00:00',0,4,2),(1684590076,'2024-02-09 15:30:00',1,5,3),(1684590077,'2024-02-10 16:00:00',2,1,4),(1684590078,'2024-02-11 17:30:00',1,2,5),(1684590079,'2024-02-12 08:30:00',2,3,2),(1684590080,'2024-02-13 09:45:00',0,4,3),(1684590081,'2024-02-14 10:00:00',1,5,4),(1684590082,'2024-02-15 11:30:00',2,1,5),(1684590083,'2024-02-16 13:45:00',0,2,1),(1684590084,'2024-02-17 14:30:00',1,3,2),(1684590085,'2024-02-18 15:15:00',2,4,3),(1684590086,'2024-02-19 16:30:00',0,5,4),(1684590087,'2024-02-20 08:00:00',1,1,5),(1800891285,'2024-01-01 10:30:00',0,1,1);
/*!40000 ALTER TABLE `appointment` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `diagnosticservice`
--

DROP TABLE IF EXISTS `diagnosticservice`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `diagnosticservice` (
  `Id` int unsigned NOT NULL,
  `Name` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `Price` double DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `diagnosticservice`
--

LOCK TABLES `diagnosticservice` WRITE;
/*!40000 ALTER TABLE `diagnosticservice` DISABLE KEYS */;
INSERT INTO `diagnosticservice` (`Id`, `Name`, `Price`) VALUES (6,'Siêu âm tim',500000),(7,'Xét nghiệm máu',300000),(8,'Chụp X-quang',400000),(9,'Chụp cộng hưởng từ',1200000),(10,'Điện tim',250000),(11,'Đo huyết áp',100000),(12,'Xét nghiệm đường huyết',350000),(13,'Xét nghiệm mỡ máu',400000),(14,'Khám tổng quát',800000),(15,'Khám chuyên khoa mắt',300000),(16,'Khám chuyên khoa tai mũi họng',350000),(17,'Khám chuyên khoa thần kinh',450000),(18,'Khám chuyên khoa da liễu',300000),(19,'Xét nghiệm nước tiểu',200000),(20,'Đo chức năng hô hấp',500000),(21,'Chụp CT scan',2500000),(22,'Khám răng hàm mặt',400000),(23,'Khám nội tiết',600000),(24,'Khám tiêu hóa',500000),(25,'Khám hô hấp',450000);
/*!40000 ALTER TABLE `diagnosticservice` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `doctor`
--

DROP TABLE IF EXISTS `doctor`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `doctor` (
  `Id` int unsigned NOT NULL,
  `Email` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
  `Position` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
  `Certificate` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
  `PhoneNumber` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
  `Image` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `doctor`
--

LOCK TABLES `doctor` WRITE;
/*!40000 ALTER TABLE `doctor` DISABLE KEYS */;
INSERT INTO `doctor` (`Id`, `Email`, `Position`, `Certificate`, `PhoneNumber`, `Image`) VALUES (1,'dafsds','fdsafsd','fdsafsf','9874982','fdsafdsf'),(2,'nguyenvana@hospital.vn','Bác sĩ Nội khoa','Chứng chỉ Nội khoa','0901234567','https://example.com/image1.jpg'),(3,'tranb@hospital.vn','Bác sĩ Nhi khoa','Chứng chỉ Nhi khoa','0912345678','https://example.com/image2.jpg'),(4,'phamc@hospital.vn','Bác sĩ Ngoại khoa','Chứng chỉ Ngoại khoa','0923456789','https://example.com/image3.jpg'),(5,'lethid@hospital.vn','Bác sĩ Sản phụ khoa','Chứng chỉ Sản phụ khoa','0934567890','https://example.com/image4.jpg'),(6,'nguyenlevan@hospital.vn','Bác sĩ Da liễu','Chứng chỉ Da liễu','0945678901','https://example.com/image5.jpg'),(7,'doanhoang@hospital.vn','Bác sĩ Mắt','Chứng chỉ Nhãn khoa','0956789012','https://example.com/image6.jpg'),(8,'tranduong@hospital.vn','Bác sĩ Tai mũi họng','Chứng chỉ Tai mũi họng','0967890123','https://example.com/image7.jpg'),(9,'ngocanh@hospital.vn','Bác sĩ Thần kinh','Chứng chỉ Thần kinh','0978901234','https://example.com/image8.jpg'),(10,'thanhdat@hospital.vn','Bác sĩ Hô hấp','Chứng chỉ Hô hấp','0989012345','https://example.com/image9.jpg'),(11,'hungtran@hospital.vn','Bác sĩ Tiêu hóa','Chứng chỉ Tiêu hóa','0990123456','https://example.com/image10.jpg');
/*!40000 ALTER TABLE `doctor` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `examinationdetail`
--

DROP TABLE IF EXISTS `examinationdetail`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `examinationdetail` (
  `Id` int unsigned NOT NULL,
  `DoctorId` int unsigned DEFAULT NULL,
  `AppointmentId` int unsigned DEFAULT NULL,
  `Diagnostic` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `Description` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `State` int unsigned DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `examinationdetail`
--

LOCK TABLES `examinationdetail` WRITE;
/*!40000 ALTER TABLE `examinationdetail` DISABLE KEYS */;
INSERT INTO `examinationdetail` (`Id`, `DoctorId`, `AppointmentId`, `Diagnostic`, `Description`, `State`) VALUES (2,3,1684590068,'Đau dạ dày','Khuyến nghị uống thuốc dạ dày và ăn uống khoa học.',1),(3,4,1684590069,'Sốt cao','Chỉ định truyền nước và uống thuốc hạ sốt.',2),(4,5,1684590070,'Đau đầu mãn tính','Hướng dẫn nghỉ ngơi và kiểm tra thần kinh.',0),(5,6,1684590071,'Ho và khó thở','Đề nghị chụp X-quang phổi để kiểm tra.',1),(6,2,1684590072,'Viêm họng','Kê thuốc kháng sinh và nghỉ ngơi tại nhà.',2),(7,3,1684590073,'Tiểu đường','Điều chỉnh chế độ ăn và tiêm insulin.',1),(8,4,1684590074,'Huyết áp cao','Theo dõi thường xuyên và uống thuốc.',2),(9,5,1684590075,'Đau khớp','Đề nghị vật lý trị liệu và kiểm tra xương khớp.',0),(10,6,1684590076,'Phát ban','Dùng thuốc chống dị ứng và khám lại sau 1 tuần.',1);
/*!40000 ALTER TABLE `examinationdetail` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `examinationservice`
--

DROP TABLE IF EXISTS `examinationservice`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `examinationservice` (
  `Id` int unsigned NOT NULL,
  `DoctorId` int unsigned DEFAULT NULL,
  `DiagnosticServiceId` int unsigned DEFAULT NULL,
  `ExaminationDetailId` int unsigned DEFAULT NULL,
  `Document` varbinary(10000) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `examinationservice`
--

LOCK TABLES `examinationservice` WRITE;
/*!40000 ALTER TABLE `examinationservice` DISABLE KEYS */;
INSERT INTO `examinationservice` (`Id`, `DoctorId`, `DiagnosticServiceId`, `ExaminationDetailId`, `Document`) VALUES (2,4,7,2,NULL),(3,5,9,3,NULL),(4,6,10,4,NULL),(5,3,8,5,NULL),(6,2,6,6,NULL),(7,4,11,7,NULL),(8,5,13,8,NULL),(9,6,12,9,NULL),(10,3,14,10,NULL);
/*!40000 ALTER TABLE `examinationservice` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `medicine`
--

DROP TABLE IF EXISTS `medicine`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `medicine` (
  `Id` int unsigned NOT NULL,
  `Name` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `Image` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `Unit` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `medicine`
--

LOCK TABLES `medicine` WRITE;
/*!40000 ALTER TABLE `medicine` DISABLE KEYS */;
INSERT INTO `medicine` (`Id`, `Name`, `Image`, `Unit`) VALUES (2,'Thuốc giảm đau','https://example.com/medicine1.jpg','Hộp'),(3,'Thuốc hạ sốt','https://example.com/medicine2.jpg','Vỉ'),(4,'Thuốc kháng sinh','https://example.com/medicine3.jpg','Chai'),(5,'Thuốc ho','https://example.com/medicine4.jpg','Lọ'),(6,'Vitamin C','https://example.com/medicine5.jpg','Hộp'),(7,'Thuốc trị tiểu đường','https://example.com/medicine6.jpg','Lọ'),(8,'Thuốc bổ não','https://example.com/medicine7.jpg','Hộp'),(9,'Thuốc điều trị dạ dày','https://example.com/medicine8.jpg','Gói'),(10,'Thuốc bổ gan','https://example.com/medicine9.jpg','Hộp');
/*!40000 ALTER TABLE `medicine` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `patient`
--

DROP TABLE IF EXISTS `patient`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `patient` (
  `Id` int unsigned NOT NULL,
  `Email` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `PhoneNumber` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `Image` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `patient`
--

LOCK TABLES `patient` WRITE;
/*!40000 ALTER TABLE `patient` DISABLE KEYS */;
INSERT INTO `patient` (`Id`, `Email`, `PhoneNumber`, `Image`) VALUES (1,'kjfhkjsdf','87398273','fsdafds'),(2,'tranthibich@gmail.com','0902345678','https://example.com/patient1.jpg'),(3,'phamminhhoang@gmail.com','0913456789','https://example.com/patient2.jpg'),(4,'lethanhmai@gmail.com','0924567890','https://example.com/patient3.jpg'),(5,'ngocanh@gmail.com','0935678901','https://example.com/patient4.jpg'),(6,'huytran@gmail.com','0946789012','https://example.com/patient5.jpg');
/*!40000 ALTER TABLE `patient` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `prescription`
--

DROP TABLE IF EXISTS `prescription`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `prescription` (
  `Id` int unsigned NOT NULL,
  `ExaminationId` int unsigned DEFAULT NULL,
  `Description` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `prescription`
--

LOCK TABLES `prescription` WRITE;
/*!40000 ALTER TABLE `prescription` DISABLE KEYS */;
INSERT INTO `prescription` (`Id`, `ExaminationId`, `Description`) VALUES (2,2,'Kê thuốc giảm đau và dạ dày, theo dõi sau 1 tuần.'),(3,3,'Sử dụng thuốc hạ sốt, giữ nhiệt độ cơ thể ổn định.'),(4,4,'Đề nghị kiểm tra thần kinh và nghỉ ngơi đủ.'),(5,5,'Kê thuốc kháng sinh và uống đủ nước.'),(6,6,'Tiêm insulin, theo dõi đường huyết hàng ngày.');
/*!40000 ALTER TABLE `prescription` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `prescriptiondetail`
--

DROP TABLE IF EXISTS `prescriptiondetail`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `prescriptiondetail` (
  `Id` int unsigned NOT NULL,
  `PrescriptionId` int unsigned DEFAULT NULL,
  `MedicineId` int unsigned DEFAULT NULL,
  `Description` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `prescriptiondetail`
--

LOCK TABLES `prescriptiondetail` WRITE;
/*!40000 ALTER TABLE `prescriptiondetail` DISABLE KEYS */;
INSERT INTO `prescriptiondetail` (`Id`, `PrescriptionId`, `MedicineId`, `Description`) VALUES (2,2,4,'Uống sau khi ăn 2 viên mỗi ngày.'),(3,3,2,'Uống 1 viên mỗi 6 giờ khi cần.'),(4,4,3,'Uống mỗi tối trước khi đi ngủ.'),(5,5,5,'Sử dụng 5ml siro ho trước bữa ăn.'),(6,6,7,'Tiêm insulin trước bữa ăn sáng.');
/*!40000 ALTER TABLE `prescriptiondetail` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `profile`
--

DROP TABLE IF EXISTS `profile`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `profile` (
  `Id` int unsigned NOT NULL,
  `PatientId` int unsigned DEFAULT NULL,
  `Fullname` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `BirthDate` date DEFAULT NULL,
  `Gender` char(1) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT 'M',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `profile`
--

LOCK TABLES `profile` WRITE;
/*!40000 ALTER TABLE `profile` DISABLE KEYS */;
INSERT INTO `profile` (`Id`, `PatientId`, `Fullname`, `BirthDate`, `Gender`) VALUES (1,1,'lkjdsakfd','2024-01-01','1'),(2,2,'Nguyễn Văn An','1985-01-15','M'),(3,3,'Trần Thị Bích','1990-05-20','F'),(4,4,'Phạm Minh Hoàng','1975-09-10','M'),(5,5,'Lê Thanh Mai','2000-07-18','F'),(6,6,'Vũ Ngọc Hùng','1982-11-25','M'),(7,7,'Nguyễn Văn Dũng','1988-07-15','M'),(8,8,'Lê Thị Hoa','1995-12-10','F'),(9,9,'Vũ Hữu Khánh','1990-03-05','M'),(10,10,'Trần Minh Hà','1980-06-20','F'),(11,11,'Hoàng Đức Anh','1975-11-30','M');
/*!40000 ALTER TABLE `profile` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `role`
--

DROP TABLE IF EXISTS `role`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `role` (
  `Id` int unsigned NOT NULL,
  `Name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `Description` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `Permissions` binary(60) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `role`
--

LOCK TABLES `role` WRITE;
/*!40000 ALTER TABLE `role` DISABLE KEYS */;
INSERT INTO `role` (`Id`, `Name`, `Description`, `Permissions`) VALUES (1,'dfsdf','vcxzvcx',_binary '\0\0\0000000000000000000000000000000000000000000000000000000000'),(2,'Bác sĩ','Người chịu trách nhiệm khám chữa bệnh.',_binary '\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0'),(3,'Y tá','Hỗ trợ bác sĩ trong quá trình điều trị.',_binary '\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0'),(4,'Nhân viên phòng khám','Quản lý hồ sơ bệnh nhân.',_binary '\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0'),(5,'Nhân viên hành chính','Xử lý thủ tục hành chính.',_binary '\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0');
/*!40000 ALTER TABLE `role` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user`
--

DROP TABLE IF EXISTS `user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user` (
  `Id` int unsigned NOT NULL,
  `Username` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `Password` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `Fullname` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `Address` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `RoleId` int unsigned DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user`
--

LOCK TABLES `user` WRITE;
/*!40000 ALTER TABLE `user` DISABLE KEYS */;
INSERT INTO `user` (`Id`, `Username`, `Password`, `Fullname`, `Address`, `RoleId`) VALUES (1,'fdsafds','fdsfsd','fdsafsd','fdsaf',1),(2,'bacsi1','password123','Nguyễn Văn Bác Sĩ','Hà Nội',2),(3,'yta1','secure456','Trần Thị Y Tá','Hồ Chí Minh',3),(4,'nhanvien1','admin789','Lê Phòng Khám','Đà Nẵng',4),(5,'admin1','admin@2024','Vũ Hành Chính','Hải Phòng',5);
/*!40000 ALTER TABLE `user` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-11-19  9:54:00
