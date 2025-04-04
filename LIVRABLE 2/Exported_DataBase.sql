-- MySQL dump 10.13  Distrib 8.0.41, for Win64 (x86_64)
--
-- Host: localhost    Database: livinparis
-- ------------------------------------------------------
-- Server version	8.0.41

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
-- Table structure for table `client`
--

DROP TABLE IF EXISTS `client`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `client` (
  `id_client` varchar(50) NOT NULL,
  PRIMARY KEY (`id_client`),
  CONSTRAINT `client_ibfk_1` FOREIGN KEY (`id_client`) REFERENCES `utilisateur` (`id_utilisateur`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `client`
--

LOCK TABLES `client` WRITE;
/*!40000 ALTER TABLE `client` DISABLE KEYS */;
INSERT INTO `client` VALUES ('U10'),('U12'),('U14'),('U16'),('U18'),('U2'),('U20'),('U22'),('U24'),('U26'),('U28'),('U30'),('U4'),('U6'),('U8');
/*!40000 ALTER TABLE `client` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `commande`
--

DROP TABLE IF EXISTS `commande`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `commande` (
  `id_commande` varchar(50) NOT NULL,
  `date_commande` date DEFAULT NULL,
  `montant_total` decimal(10,2) DEFAULT NULL,
  `id_client` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id_commande`),
  KEY `id_client` (`id_client`),
  CONSTRAINT `commande_ibfk_1` FOREIGN KEY (`id_client`) REFERENCES `client` (`id_client`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `commande`
--

LOCK TABLES `commande` WRITE;
/*!40000 ALTER TABLE `commande` DISABLE KEYS */;
INSERT INTO `commande` VALUES ('C1','2025-02-15',22.50,'U2'),('C2','2025-02-14',14.90,'U4'),('C3','2025-02-16',10.50,'U6'),('C4','2025-02-13',8.90,'U8'),('C5','2025-02-12',12.00,'U10');
/*!40000 ALTER TABLE `commande` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `cuisinier`
--

DROP TABLE IF EXISTS `cuisinier`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `cuisinier` (
  `id_cuisinier` varchar(50) NOT NULL,
  PRIMARY KEY (`id_cuisinier`),
  CONSTRAINT `cuisinier_ibfk_1` FOREIGN KEY (`id_cuisinier`) REFERENCES `utilisateur` (`id_utilisateur`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cuisinier`
--

LOCK TABLES `cuisinier` WRITE;
/*!40000 ALTER TABLE `cuisinier` DISABLE KEYS */;
INSERT INTO `cuisinier` VALUES ('U1'),('U11'),('U13'),('U15'),('U17'),('U19'),('U21'),('U23'),('U25'),('U27'),('U29'),('U3'),('U5'),('U7'),('U9');
/*!40000 ALTER TABLE `cuisinier` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `entreprise_locale`
--

DROP TABLE IF EXISTS `entreprise_locale`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `entreprise_locale` (
  `id_entreprise` varchar(50) NOT NULL,
  `nom_entreprise` varchar(100) DEFAULT NULL,
  `referent` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`id_entreprise`),
  CONSTRAINT `entreprise_locale_ibfk_1` FOREIGN KEY (`id_entreprise`) REFERENCES `client` (`id_client`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `entreprise_locale`
--

LOCK TABLES `entreprise_locale` WRITE;
/*!40000 ALTER TABLE `entreprise_locale` DISABLE KEYS */;
INSERT INTO `entreprise_locale` VALUES ('U10','Le Délice Urbain','Lilia Charpentier'),('U8','Bistro Parisien','Maëlys Reynaud');
/*!40000 ALTER TABLE `entreprise_locale` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ligne_commande`
--

DROP TABLE IF EXISTS `ligne_commande`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ligne_commande` (
  `id_commande_ligne` varchar(50) NOT NULL,
  `id_commande` varchar(50) DEFAULT NULL,
  `id_plat` varchar(50) DEFAULT NULL,
  `quantite` int DEFAULT NULL,
  `date_livraison` date DEFAULT NULL,
  `lieu_livraison` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id_commande_ligne`),
  KEY `id_commande` (`id_commande`),
  KEY `id_plat` (`id_plat`),
  CONSTRAINT `ligne_commande_ibfk_1` FOREIGN KEY (`id_commande`) REFERENCES `commande` (`id_commande`) ON DELETE CASCADE,
  CONSTRAINT `ligne_commande_ibfk_2` FOREIGN KEY (`id_plat`) REFERENCES `plat` (`id_plat`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ligne_commande`
--

LOCK TABLES `ligne_commande` WRITE;
/*!40000 ALTER TABLE `ligne_commande` DISABLE KEYS */;
INSERT INTO `ligne_commande` VALUES ('L1','C1','P2',1,'2025-02-16','34 avenue Montaigne, 75008'),('L2','C2','P1',1,'2025-02-15','78 rue Lafayette, 75010'),('L3','C3','P3',1,'2025-02-17','99 rue de la Paix, 75002'),('L4','C4','P4',1,'2025-02-18','53 avenue des Ternes, 75017'),('L5','C5','P5',1,'2025-02-14','7 place de la Bastille, 75004');
/*!40000 ALTER TABLE `ligne_commande` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `livraison`
--

DROP TABLE IF EXISTS `livraison`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `livraison` (
  `id_livraison` varchar(50) NOT NULL,
  `date_livraison` date DEFAULT NULL,
  `zone` varchar(50) DEFAULT NULL,
  `id_cuisinier` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id_livraison`),
  KEY `id_cuisinier` (`id_cuisinier`),
  CONSTRAINT `livraison_ibfk_1` FOREIGN KEY (`id_cuisinier`) REFERENCES `cuisinier` (`id_cuisinier`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `livraison`
--

LOCK TABLES `livraison` WRITE;
/*!40000 ALTER TABLE `livraison` DISABLE KEYS */;
INSERT INTO `livraison` VALUES ('L1','2025-02-16','Paris Centre','U3'),('L2','2025-02-15','Paris Est','U1'),('L3','2025-02-17','Paris Ouest','U5'),('L4','2025-02-18','Paris Nord','U7'),('L5','2025-02-14','Paris Sud','U9');
/*!40000 ALTER TABLE `livraison` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `particulier`
--

DROP TABLE IF EXISTS `particulier`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `particulier` (
  `id_particulier` varchar(50) NOT NULL,
  PRIMARY KEY (`id_particulier`),
  CONSTRAINT `particulier_ibfk_1` FOREIGN KEY (`id_particulier`) REFERENCES `client` (`id_client`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `particulier`
--

LOCK TABLES `particulier` WRITE;
/*!40000 ALTER TABLE `particulier` DISABLE KEYS */;
INSERT INTO `particulier` VALUES ('U2'),('U4'),('U6');
/*!40000 ALTER TABLE `particulier` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `plat`
--

DROP TABLE IF EXISTS `plat`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `plat` (
  `id_plat` varchar(50) NOT NULL,
  `nom` varchar(100) DEFAULT NULL,
  `type` varchar(50) DEFAULT NULL,
  `date_fabrication` date DEFAULT NULL,
  `date_peremption` date DEFAULT NULL,
  `nb_personnes` int DEFAULT NULL,
  `prix` decimal(10,2) DEFAULT NULL,
  `nationalite` varchar(50) DEFAULT NULL,
  `regime` varchar(50) DEFAULT NULL,
  `ingredients` text,
  `photo` varchar(255) DEFAULT NULL,
  `id_cuisinier` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id_plat`),
  KEY `id_cuisinier` (`id_cuisinier`),
  CONSTRAINT `plat_ibfk_1` FOREIGN KEY (`id_cuisinier`) REFERENCES `cuisinier` (`id_cuisinier`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `plat`
--

LOCK TABLES `plat` WRITE;
/*!40000 ALTER TABLE `plat` DISABLE KEYS */;
INSERT INTO `plat` VALUES ('P1','Bœuf Bourguignon','Plat principal','2025-02-15','2025-02-18',4,14.90,'Française','Classique','Bœuf, Vin rouge, Carottes, Pommes de terre','boeuf_bourguignon.jpg','U1'),('P2','Sushis Variés','Plat principal','2025-02-14','2025-02-16',2,22.50,'Japonaise','Sans gluten','Saumon, Riz, Algues, Wasabi','sushis.jpg','U3'),('P3','Ratatouille Provençale','Plat principal','2025-02-13','2025-02-17',3,10.50,'Française','Végétarien','Tomates, Courgettes, Aubergines, Poivrons','ratatouille.jpg','U5'),('P4','Mousse au Chocolat','Dessert','2025-02-12','2025-02-19',6,8.90,'Française','Classique','Chocolat noir, Œufs, Sucre','mousse_chocolat.jpg','U7'),('P5','Pad Thaï au Poulet','Plat principal','2025-02-11','2025-02-15',2,12.00,'Thaïlandaise','Classique','Nouilles, Poulet, Sauce soja, Cacahuètes','pad_thai.jpg','U9');
/*!40000 ALTER TABLE `plat` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `transaction_`
--

DROP TABLE IF EXISTS `transaction_`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `transaction_` (
  `id_transaction` varchar(50) NOT NULL,
  `montant` decimal(10,2) DEFAULT NULL,
  `statut_paiement` varchar(50) DEFAULT NULL,
  `id_commande` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id_transaction`),
  KEY `id_commande` (`id_commande`),
  CONSTRAINT `transaction__ibfk_1` FOREIGN KEY (`id_commande`) REFERENCES `commande` (`id_commande`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `transaction_`
--

LOCK TABLES `transaction_` WRITE;
/*!40000 ALTER TABLE `transaction_` DISABLE KEYS */;
INSERT INTO `transaction_` VALUES ('T1',22.50,'Payé','C1'),('T2',14.90,'En attente','C2'),('T3',10.50,'Payé','C3'),('T4',8.90,'Annulé','C4'),('T5',12.00,'Payé','C5');
/*!40000 ALTER TABLE `transaction_` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `utilisateur`
--

DROP TABLE IF EXISTS `utilisateur`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `utilisateur` (
  `id_utilisateur` varchar(50) NOT NULL,
  `nom` varchar(50) DEFAULT NULL,
  `prenom` varchar(50) DEFAULT NULL,
  `adresse` varchar(255) DEFAULT NULL,
  `telephone` varchar(20) DEFAULT NULL,
  `email` varchar(100) DEFAULT NULL,
  `mot_de_passe` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id_utilisateur`),
  UNIQUE KEY `email` (`email`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `utilisateur`
--

LOCK TABLES `utilisateur` WRITE;
/*!40000 ALTER TABLE `utilisateur` DISABLE KEYS */;
INSERT INTO `utilisateur` VALUES ('U1','Leclerc','Basile','12 rue de Rivoli, 75001','0678543210','basile.leclerc@email.com','philou7'),('U10','Charpentier','Lilia','7 place de la Bastille, 75004','0712365478','lilia.charpentier@email.com','nasdas44'),('U11','Nom11','Prenom11','Adresse 11 Rue','0102030411','user11@mail.com','mdp11'),('U12','Nom12','Prenom12','Adresse 12 Rue','0102030412','user12@mail.com','mdp12'),('U13','Nom13','Prenom13','Adresse 13 Rue','0102030413','user13@mail.com','mdp13'),('U14','Nom14','Prenom14','Adresse 14 Rue','0102030414','user14@mail.com','mdp14'),('U15','Nom15','Prenom15','Adresse 15 Rue','0102030415','user15@mail.com','mdp15'),('U16','Nom16','Prenom16','Adresse 16 Rue','0102030416','user16@mail.com','mdp16'),('U17','Nom17','Prenom17','Adresse 17 Rue','0102030417','user17@mail.com','mdp17'),('U18','Nom18','Prenom18','Adresse 18 Rue','0102030418','user18@mail.com','mdp18'),('U19','Nom19','Prenom19','Adresse 19 Rue','0102030419','user19@mail.com','mdp19'),('U2','Garnier','Lina','34 avenue Montaigne, 75008','0765124789','lina.garnier@email.com','bfqdskjqf43z'),('U20','Nom20','Prenom20','Adresse 20 Rue','0102030420','user20@mail.com','mdp20'),('U21','Nom21','Prenom21','Adresse 21 Rue','0102030421','user21@mail.com','mdp21'),('U22','Nom22','Prenom22','Adresse 22 Rue','0102030422','user22@mail.com','mdp22'),('U23','Nom23','Prenom23','Adresse 23 Rue','0102030423','user23@mail.com','mdp23'),('U24','Nom24','Prenom24','Adresse 24 Rue','0102030424','user24@mail.com','mdp24'),('U25','Nom25','Prenom25','Adresse 25 Rue','0102030425','user25@mail.com','mdp25'),('U26','Nom26','Prenom26','Adresse 26 Rue','0102030426','user26@mail.com','mdp26'),('U27','Nom27','Prenom27','Adresse 27 Rue','0102030427','user27@mail.com','mdp27'),('U28','Nom28','Prenom28','Adresse 28 Rue','0102030428','user28@mail.com','mdp28'),('U29','Nom29','Prenom29','Adresse 29 Rue','0102030429','user29@mail.com','mdp29'),('U3','Dumont','Noé','5 boulevard Haussmann, 75009','0612349875','noe.dumont@email.com','jaca43di'),('U30','Nom30','Prenom30','Adresse 30 Rue','0102030430','user30@mail.com','mdp30'),('U4','Perrot','Salomé','78 rue Lafayette, 75010','0754986352','salome.perrot@email.com','adit34'),('U5','Bouvier','Elias','42 quai de Seine, 75019','0687654321','elias.bouvier@email.com','afoufou'),('U6','Marchal','Inès','99 rue de la Paix, 75002','0798541236','ines.marchal@email.com','tiplouf'),('U7','Lemoine','Esteban','18 rue Oberkampf, 75011','0721456983','esteban.lemoine@email.com','pikachu'),('U8','Reynaud','Maëlys','53 avenue des Ternes, 75017','0678954123','maelys.reynaud@email.com','saltamenco6549'),('U9','Foucher','Axel','21 rue du Faubourg, 75020','0698542136','axel.foucher@email.com','gouzougouzou12');
/*!40000 ALTER TABLE `utilisateur` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-04-04 17:12:10
