CREATE DATABASE  IF NOT EXISTS `apomtschedsys` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `apomtschedsys`;
-- MySQL dump 10.13  Distrib 8.0.38, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: apomtschedsys
-- ------------------------------------------------------
-- Server version	8.3.0

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
  `Number` int unsigned DEFAULT '0',
  `State` int unsigned DEFAULT '0',
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
INSERT INTO `appointment` VALUES (256856054,'2024-12-11 15:34:00',58,3,1348295798,2360840730),(654492766,'2024-12-18 15:34:00',58,0,2882583130,375836206),(1079873202,'2024-12-19 15:34:00',58,0,2882583130,4231085890),(1083649749,'2024-12-19 13:00:00',36,2,350316586,3525831068),(3811652854,'2024-12-19 14:31:00',49,2,3415383611,2394356537);
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
INSERT INTO `diagnosticservice` VALUES (1,'X-Quang',50000),(2431368585,'Siêu âm',400000),(3439917793,'Đo điện tâm đồ',100000);
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
INSERT INTO `doctor` VALUES (375836206,'taolao@gmail.com','Tao lao','Tao lao bac nhat','0954413453',''),(853295561,'ductindang1009.13@gmail.com','hiệu trưởng quét rác','quét rác trong nhà vệ sinh','0335637514',''),(1658405388,'ductin449@gmail.com','Bác sĩ chuyên khoa','Chứng chỉ Bác sĩ chuyên khoa','0946789012',''),(1801467038,'ductindang1009.13@gmail.com','hiệu trưởng quét rác','quét rác trong nhà trường','0335637514',''),(2037631953,'ductindang1009.13@gmail.com','hiệu trưởng quét rác','quét rác trong nhà vệ sinh','0335637514',''),(2360840730,'khiemboy@gmail.com','Bác sĩ nội khoa','Chứng chỉ bác sĩ nội khoa','0946789012',''),(2394356537,'ductindang1009.13@gmail.com','hiệu trưởng quét rác','quét rác trong nhà vệ sinh','0335637514',''),(3525831068,'admin@scheduler-appointment.localhost','Bác sĩ gây mê','Gây mê nhanh chóng','0987654321','jhkdsahfkh'),(4231085890,'vantiennst@gmail.com','Bác sĩ ngoại khoa','Chứng chỉ bác sĩ ngoại khoa','0396875451',''),(4285648994,'ductindang1009@gmail.com','hiệu trưởng quét rác','quét rác trong nhà vệ sinh','0335637514','');
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
INSERT INTO `examinationdetail` VALUES (1682478445,NULL,256856054,'bij vieem bnang',NULL,2),(2887861765,NULL,3811652854,NULL,NULL,1),(3572665544,NULL,1083649749,NULL,NULL,0);
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
INSERT INTO `examinationservice` VALUES (2510870277,7,1,960182766,NULL),(3207229325,4231085890,2431368585,1682478445,NULL);
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
INSERT INTO `patient` VALUES (42633337,'ductindang1009@gmail.com','0335637514',NULL),(1867225021,'ductindang1009.13@gmail.com','0335637514',NULL),(1902110849,'patient@gmail.com','0789428734',NULL),(1935419821,'johndoe@example.com','0673947192',NULL),(1941319795,'ductindang1009.13@gmail.com','0335637514',NULL),(2609335914,'linh@gmail.com','0396875451',NULL),(2619398381,'ductindang1009.13@gmail.com','0335637514',NULL),(2650443752,'tung@gmail.com','0789428734',NULL),(3127843709,'ductindang1009@gmail.com','0335637514',NULL),(3798546515,'abc@gmail.com','0946789012',NULL),(4154607017,'truc@gmail.com','0396875451',NULL);
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
INSERT INTO `prescription` VALUES (552618792,960182766,'jhkdjhskfdsk'),(2673859725,3982404318,'fdsafdsfasd'),(3839595477,2887861765,'jhkjhkchvzxcx');
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
INSERT INTO `profile` VALUES (350316586,2650443752,'Xét nghiệm tổng quát Tùng','0001-01-01','M'),(1348295798,1935419821,'Tra cứu bệnh John','0001-01-01','M'),(1461962011,2619398381,'Cuối cùng','0001-01-01','M'),(2882583130,4154607017,'Kiểm tra toàn diện','0001-01-01','F'),(3415383611,1867225021,'Dang Duc Tin','2020-01-01','D'),(4048486444,4154607017,'Kiểm tra toàn thân Trúc','0001-01-01','F'),(4086797343,2609335914,'Tra cứu Linh','0001-01-01','F');
/*!40000 ALTER TABLE `profile` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `property`
--

DROP TABLE IF EXISTS `property`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `property` (
  `Key` varchar(100) COLLATE utf8mb4_general_ci NOT NULL,
  `Value` varchar(100) COLLATE utf8mb4_general_ci NOT NULL,
  PRIMARY KEY (`Key`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `property`
--

LOCK TABLES `property` WRITE;
/*!40000 ALTER TABLE `property` DISABLE KEYS */;
INSERT INTO `property` VALUES ('config.default.role','880477644'),('config.preloader.state','AppointmentScheduler.Infrastructure.Preloader.Success');
/*!40000 ALTER TABLE `property` ENABLE KEYS */;
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
INSERT INTO `role` VALUES (880477644,'User','Created by Preloader',_binary 'E|\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0'),(3284472258,'Doctor','Created by Preloader',_binary '|�\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0'),(3743722430,'Doctor Administrator Role','Created by Preloader',_binary '����\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0');
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
INSERT INTO `user` VALUES (42633337,'abcdef','AQAAAAIAAYagAAAAEEkVwb/DpBE7Vl3C6Y3hcu/elkkBw7zGQtpE1oEHlBBFejryUs0ljNa1zVA41eu5cw==','human-resource',NULL,0),(375836206,'root00','AQAAAAIAAYagAAAAED+Ht+lxUwYRN5qVJqz7Ij8v7nc1v48wo0YXo58FmrfSO+Uy5gqubxqpkN+RpsCQDg==','bac si tao lao',NULL,3743722430),(853295561,'yenxao2','AQAAAAIAAYagAAAAEBvRc84ilMLXOqmUvnCjcU54pUlJKrqicrERWqpNjDlfWEL+D1vNyri62b4hU9NRAA==','abcdeeff',NULL,3284472258),(1658405388,'ductin449','AQAAAAIAAYagAAAAECQSIkz+pMwNQVkpU0EUknyW0umgYLDhJ6aeVD0uffFCI3YWznHKp36sJIjhqCJRVg==','Đặng Đức Tin',NULL,0),(1801467038,'user546452','AQAAAAIAAYagAAAAEMHiZ1h3lmBB+wsNj6YV05X7sA/ACkAVQkBSi+7D2ZHlf1Y+4KvhpnMtbCiLakjBig==','Huỳnh Xuân Bách',NULL,0),(1867225021,'fdsffff','AQAAAAIAAYagAAAAENiaREmB/mFNffMEZX3nLN7NnwMZRXDEWQ1lxxdf8U2L21ArrA2JWUuMrHJx7XX5pQ==','Actor chan123',NULL,0),(1902110849,'yenxao12','AQAAAAIAAYagAAAAEDn9YTrktRVIlY/t5m9nKfnMXgf3CEwg+OS94FxSayP1/eQjvaqv6cOGixc4yMGB6g==','Cuối cùng',NULL,0),(1935419821,'fdsafs','AQAAAAIAAYagAAAAED+Ht+lxUwYRN5qVJqz7Ij8v7nc1v48wo0YXo58FmrfSO+Uy5gqubxqpkN+RpsCQDg==','John Doe',NULL,0),(1941319795,'fdsafsdtrew123','AQAAAAIAAYagAAAAEHqt90Y2eFqM3LNBxRztiMMwTx1iYf5J5mkjfMv7MBxmq9B5mckxrmZKRCQEf6hpDw==','ggg đi m ơi',NULL,0),(2037631953,'abcdee','AQAAAAIAAYagAAAAEDxnvoFH5TTeR46zwl8DW/3/4W96HeEfHdVFBTUUEPg/iHOBDaGst6zdbQ+7+VjIbQ==','Actor chan123',NULL,0),(2360840730,'KhiemJimi','AQAAAAIAAYagAAAAEMZKBSQwXPi69kn5mVK/yflvDkgfDaTPA1IEdy5h4W/UnuIEWxova6KMfzh8I6E8ow==','Hoàng Sỹ Khiêm',NULL,0),(2394356537,'user123','AQAAAAIAAYagAAAAENvioFiFWrVe8xddWgZTBWUJLx5OiOle021BE+wrJ2RET88i8Rx2UE+tAd4ORwvuqw==','Actor chan chan',NULL,0),(2609335914,'linksource','AQAAAAIAAYagAAAAELJdiimdxTeG/IDHYP5w/h6dovFN7y5tYSAb2MgcmLrJl2+GaIvtrfwTBAsxZUZ+HA==','Linh',NULL,0),(2619398381,'user2342','AQAAAAIAAYagAAAAECXKk88g2Wo7+tB7FC0eP3zBq9ow6/K+j5oW1Cin9j39m2wKyEyL7QXaMVwk5Yhg2g==','Actor chan123',NULL,0),(2650443752,'tung123','AQAAAAIAAYagAAAAEMdC6uWdSYXCfbbd/JoBkDTmxBJ0e/VeeuB0O5WTNaFipPpNknBlgLuPbGeP0PfSew==','Tùng',NULL,0),(3127843709,'user546452feqr','AQAAAAIAAYagAAAAEDj3ywTtT3Hf8C6LJUpHN0Il4Mmaa8IMShPECKgv+XGNdydR9OpMI7IEJBGRadwC+Q==','human-resource',NULL,0),(3525831068,'nguyenvanaaa','AQAAAAIAAYagAAAAEIqqjt0ZuTVOR5qV6B7SGF6YLzFRZCmp++2EzPv/yM5jziQ3J5YVhyQVnBscsr7EFw==','Nguyễn Văn A',NULL,3284472258),(3798546515,'1234777','AQAAAAIAAYagAAAAEGe65vWXBBQrKtRCxbxdn6TTLqz6AtppDOsrleHsQgA6dDDEC05/ZJgLoVoRS7JU6A==','Cuối cùng',NULL,0),(4154607017,'trucxinh','AQAAAAIAAYagAAAAEMT8+QN2nRSI2O+3G+OFo00tN7uequhLie+E1UThPYDJxhtgm8mautm9RBF9WSCfuw==','Trúc',NULL,0),(4231085890,'yenxao23123','AQAAAAIAAYagAAAAEGkzXfLUeAW/xwXbe1tIYmB/60XsqcLFhUKSw4l6Dij3xucJTmrxpKo+Gt8RGBveJA==','Bùi Văn Tiến',NULL,3284472258),(4285648994,'ggswrtt123','AQAAAAIAAYagAAAAEBOTKfoGVEATJBipoS8f45ZhPESlCcvBVe9t0kq+da40Fgt380fMRYKYuGJ7zDYaTQ==','human-resource abc',NULL,0);
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

-- Dump completed on 2024-12-10 14:45:22
