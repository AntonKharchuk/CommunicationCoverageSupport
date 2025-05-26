/*M!999999\- enable the sandbox mode */
-- MariaDB dump 10.19  Distrib 10.11.11-MariaDB, for debian-linux-gnu (x86_64)
--
-- Host: localhost    Database: sdb2
-- ------------------------------------------------------
-- Server version       10.11.11-MariaDB-0+deb12u1-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `acc`
--

DROP TABLE IF EXISTS `acc`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `acc` (
  `id` tinyint(4) NOT NULL AUTO_INCREMENT,
  `name` varchar(255) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `artwork`
--

DROP TABLE IF EXISTS `artwork`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `artwork` (
  `id` tinyint(4) NOT NULL AUTO_INCREMENT,
  `name` varchar(255) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `owners`
--

DROP TABLE IF EXISTS `owners`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `owners` (
  `id` bigint(20) NOT NULL,
  `name` varchar(255) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `simCards`
--

DROP TABLE IF EXISTS `simCards`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `simCards` (
  `iccid` char(20) NOT NULL,
  `imsi` char(15) NOT NULL,
  `msisdn` char(12) NOT NULL,
  `kIndId` tinyint(4) NOT NULL DEFAULT 0,
  `ki` char(32) NOT NULL,
  `pin1` smallint(6) NOT NULL,
  `pin2` smallint(6) NOT NULL,
  `puk1` int(11) NOT NULL,
  `puk2` int(11) NOT NULL,
  `adm1` varchar(255) NOT NULL,
  `artworkId` tinyint(4) NOT NULL DEFAULT 0,
  `accId` tinyint(4) NOT NULL DEFAULT 0,
  `installed` tinyint(1) DEFAULT 0,
  `cardOwnerId` bigint(20) NOT NULL DEFAULT 0,
  PRIMARY KEY (`iccid`,`imsi`,`msisdn`,`kIndId`),
  UNIQUE KEY `iccid` (`iccid`),
  UNIQUE KEY `imsi` (`imsi`),
  UNIQUE KEY `msisdn` (`msisdn`),
  KEY `artworkId` (`artworkId`),
  KEY `accId` (`accId`),
  KEY `cardOwnerId` (`cardOwnerId`),
  KEY `kIndId` (`kIndId`),
  CONSTRAINT `simCards_ibfk_1` FOREIGN KEY (`artworkId`) REFERENCES `artwork` (`id`),
  CONSTRAINT `simCards_ibfk_2` FOREIGN KEY (`accId`) REFERENCES `acc` (`id`),
  CONSTRAINT `simCards_ibfk_3` FOREIGN KEY (`cardOwnerId`) REFERENCES `owners` (`id`),
  CONSTRAINT `simCards_ibfk_4` FOREIGN KEY (`kIndId`) REFERENCES `transportKey` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,ERROR_FOR_DIVISION_BY_ZERO,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`hostmaster`@`localhost`*/ /*!50003 TRIGGER simCards_insert
BEFORE INSERT ON simCards
FOR EACH ROW
BEGIN
    DECLARE _message_text VARCHAR(255) DEFAULT '';
    IF EXISTS (SELECT 1 FROM simCardsDrain WHERE iccid = NEW.iccid AND imsi = NEW.imsi AND msisdn = NEW.msisdn AND kIndId = NEW.kIndId) THEN
        SET _message_text = CONCAT("Cannot proceed.\nRecord already exists in simCardsDrained table:",
                    "\n  iccid : ", NEW.iccid,
                    "\n  imsi  : ", NEW.imsi,
                    "\n  msisdn: ", NEW.msisdn,
                    "\n  kIndId: ", NEW.kIndId);
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = _message_text;
    END IF;
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `simCardsDrain`
--

DROP TABLE IF EXISTS `simCardsDrain`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `simCardsDrain` (
  `iccid` char(20) NOT NULL,
  `imsi` char(15) NOT NULL,
  `msisdn` char(12) NOT NULL,
  `kIndId` tinyint(4) NOT NULL DEFAULT 0,
  `ki` char(32) NOT NULL,
  `pin1` smallint(6) NOT NULL,
  `pin2` smallint(6) NOT NULL,
  `puk1` int(11) NOT NULL,
  `puk2` int(11) NOT NULL,
  `adm1` varchar(255) NOT NULL,
  `artworkId` tinyint(4) NOT NULL DEFAULT 0,
  `accId` tinyint(4) NOT NULL DEFAULT 0,
  `installed` tinyint(1) DEFAULT NULL COMMENT 'stores status at the create moment',
  `cardOwnerId` bigint(20) NOT NULL DEFAULT 0,
  `createTimestamp` datetime DEFAULT NULL,
  PRIMARY KEY (`iccid`,`imsi`,`msisdn`,`kIndId`),
  UNIQUE KEY `iccid` (`iccid`),
  KEY `artworkId` (`artworkId`),
  KEY `accId` (`accId`),
  KEY `cardOwnerId` (`cardOwnerId`),
  KEY `kIndId` (`kIndId`),
  CONSTRAINT `simCardsDrain_ibfk_1` FOREIGN KEY (`artworkId`) REFERENCES `artwork` (`id`),
  CONSTRAINT `simCardsDrain_ibfk_2` FOREIGN KEY (`accId`) REFERENCES `acc` (`id`),
  CONSTRAINT `simCardsDrain_ibfk_3` FOREIGN KEY (`cardOwnerId`) REFERENCES `owners` (`id`),
  CONSTRAINT `simCardsDrain_ibfk_4` FOREIGN KEY (`kIndId`) REFERENCES `transportKey` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,ERROR_FOR_DIVISION_BY_ZERO,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`hostmaster`@`localhost`*/ /*!50003 TRIGGER simCardsDrain_insert
BEFORE INSERT ON simCardsDrain
FOR EACH ROW
BEGIN
    SET NEW.createTimestamp = NOW ();
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `transportKey`
--

DROP TABLE IF EXISTS `transportKey`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `transportKey` (
  `id` tinyint(4) NOT NULL,
  `kInd` varchar(15) NOT NULL DEFAULT 'EMPTY',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-05-26 19:15:15
