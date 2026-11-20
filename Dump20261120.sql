-- MySQL dump 10.13  Distrib 8.0.46, for Win64 (x86_64)
--
-- Host: 88.218.168.195    Database: isp-41_maryashkin_ad_chudo_obuvv
-- ------------------------------------------------------
-- Server version	8.0.46-0ubuntu0.24.04.5

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
-- Table structure for table `categories`
--

DROP TABLE IF EXISTS `categories`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `categories` (
  `category_id` int NOT NULL AUTO_INCREMENT,
  `category_name` varchar(100) NOT NULL,
  PRIMARY KEY (`category_id`),
  UNIQUE KEY `category_name_UNIQUE` (`category_name`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `categories`
--

LOCK TABLES `categories` WRITE;
/*!40000 ALTER TABLE `categories` DISABLE KEYS */;
INSERT INTO `categories` VALUES (1,'Детская обувь'),(2,'Женская обувь'),(3,'Мужская обувь');
/*!40000 ALTER TABLE `categories` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `manufacturers`
--

DROP TABLE IF EXISTS `manufacturers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `manufacturers` (
  `manufacturer_id` int NOT NULL AUTO_INCREMENT,
  `manufacturer_name` varchar(100) NOT NULL,
  PRIMARY KEY (`manufacturer_id`),
  UNIQUE KEY `manufacturer_name_UNIQUE` (`manufacturer_name`)
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `manufacturers`
--

LOCK TABLES `manufacturers` WRITE;
/*!40000 ALTER TABLE `manufacturers` DISABLE KEYS */;
INSERT INTO `manufacturers` VALUES (15,'Азимут'),(10,'Барбари'),(16,'Берестов'),(6,'Весна'),(7,'Здоровый шаг'),(8,'Карапуз'),(13,'Кожевенные традиции'),(3,'Лапушка'),(9,'Маленький франт'),(1,'Малыш-Спорт'),(14,'Модный силуэт'),(5,'Северята'),(12,'Стиль и комфорт'),(11,'Твой комфорт'),(4,'Тёплые ножки'),(2,'Топ-Топ');
/*!40000 ALTER TABLE `manufacturers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `order_items`
--

DROP TABLE IF EXISTS `order_items`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `order_items` (
  `order_item_id` int NOT NULL AUTO_INCREMENT,
  `order_id` int NOT NULL,
  `stock_item_id` int NOT NULL,
  `quantity` int NOT NULL,
  `unit_price` decimal(10,2) NOT NULL,
  PRIMARY KEY (`order_item_id`),
  KEY `fk_order_items_orders_idx` (`order_id`),
  KEY `fk_order_items_stock_items_idx` (`stock_item_id`),
  CONSTRAINT `fk_order_items_orders` FOREIGN KEY (`order_id`) REFERENCES `orders` (`order_id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_order_items_stock_items` FOREIGN KEY (`stock_item_id`) REFERENCES `stock_items` (`stock_item_id`) ON DELETE RESTRICT ON UPDATE CASCADE,
  CONSTRAINT `chk_order_items_price` CHECK ((`unit_price` >= 0)),
  CONSTRAINT `chk_order_items_qty` CHECK ((`quantity` > 0))
) ENGINE=InnoDB AUTO_INCREMENT=44 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `order_items`
--

LOCK TABLES `order_items` WRITE;
/*!40000 ALTER TABLE `order_items` DISABLE KEYS */;
INSERT INTO `order_items` VALUES (3,2,3,1,25000.00),(4,2,4,1,7500.00),(5,3,5,1,17000.00),(6,3,6,1,8565.00),(7,3,7,1,7795.00),(8,4,8,1,7757.00),(9,4,9,1,12340.00),(10,4,10,1,7130.00),(11,4,11,1,8765.00),(12,4,12,1,15000.00),(13,5,13,1,9567.00),(14,5,14,1,21500.00),(15,5,15,1,7890.00),(16,6,16,1,24569.00),(17,6,17,1,14567.00),(18,6,18,1,15670.00),(19,7,19,1,4290.00),(20,7,20,2,2190.00),(21,7,21,1,1690.00),(22,8,22,1,25625.00),(23,8,23,3,7890.00),(24,8,24,2,6550.00),(25,9,25,1,2490.00),(26,9,26,2,3190.00),(27,10,27,1,7550.00),(28,10,28,1,7130.00),(29,10,29,1,12340.00),(30,10,30,2,8765.00),(31,11,36,1,5625.00),(32,11,42,1,5347.50),(35,13,36,1,5625.00),(36,13,42,1,5347.50),(37,14,45,1,9255.00),(38,14,88,1,10925.25),(39,15,36,1,5625.00),(40,16,36,1,5625.00),(41,16,37,1,5625.00),(42,17,42,1,5347.50),(43,17,90,1,11752.50);
/*!40000 ALTER TABLE `order_items` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `orders`
--

DROP TABLE IF EXISTS `orders`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `orders` (
  `order_id` int NOT NULL AUTO_INCREMENT,
  `order_date` date NOT NULL,
  `user_id` int NOT NULL,
  PRIMARY KEY (`order_id`),
  KEY `fk_orders_users_idx` (`user_id`),
  CONSTRAINT `fk_orders_users` FOREIGN KEY (`user_id`) REFERENCES `users` (`user_id`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `orders`
--

LOCK TABLES `orders` WRITE;
/*!40000 ALTER TABLE `orders` DISABLE KEYS */;
INSERT INTO `orders` VALUES (1,'2026-04-02',3),(2,'2026-04-05',17),(3,'2026-04-08',7),(4,'2026-04-12',15),(5,'2026-04-18',12),(6,'2026-04-22',9),(7,'2026-04-25',14),(8,'2026-04-28',20),(9,'2026-05-05',5),(10,'2026-05-08',17),(11,'2026-11-13',1),(13,'2026-11-19',1),(14,'2026-11-19',1),(15,'2026-11-20',15),(16,'2026-11-20',1),(17,'2026-11-20',2);
/*!40000 ALTER TABLE `orders` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `products`
--

DROP TABLE IF EXISTS `products`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `products` (
  `product_id` int NOT NULL AUTO_INCREMENT,
  `product_name` varchar(150) NOT NULL,
  `category_id` int NOT NULL,
  `subcategory_id` int NOT NULL,
  `manufacturer_id` int NOT NULL,
  `description` text,
  `composition` text,
  `price` decimal(10,2) NOT NULL,
  `image_file` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`product_id`),
  KEY `fk_products_categories_idx` (`category_id`),
  KEY `fk_products_subcategories_idx` (`subcategory_id`),
  KEY `fk_products_manufacturers_idx` (`manufacturer_id`),
  CONSTRAINT `fk_products_categories` FOREIGN KEY (`category_id`) REFERENCES `categories` (`category_id`) ON DELETE RESTRICT ON UPDATE CASCADE,
  CONSTRAINT `fk_products_manufacturers` FOREIGN KEY (`manufacturer_id`) REFERENCES `manufacturers` (`manufacturer_id`) ON DELETE RESTRICT ON UPDATE CASCADE,
  CONSTRAINT `fk_products_subcategories` FOREIGN KEY (`subcategory_id`) REFERENCES `subcategories` (`subcategory_id`) ON DELETE RESTRICT ON UPDATE CASCADE,
  CONSTRAINT `chk_products_price` CHECK ((`price` >= 0))
) ENGINE=InnoDB AUTO_INCREMENT=32 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `products`
--

LOCK TABLES `products` WRITE;
/*!40000 ALTER TABLE `products` DISABLE KEYS */;
INSERT INTO `products` VALUES (1,'Кроссовки детские «Звёздочка» экокожа',1,1,1,'Кроссовки детские «Звёздочка» экокожа. Производитель: ООО «Малыш-Спорт», г. Смоленск','Верх — экокожа (полиуретан на хлопковой основе), подкладка — хлопок 100%, подошва — ПВХ',2190.00,'IMG_KS_195217.png'),(2,'Кроссовки детские «Гонщик» экокожа',1,1,2,'Кроссовки детские «Гонщик» экокожа. Производитель: Фабрика «Топ-Топ», г. Киров','Верх — экокожа (микрофибра), подкладка — текстиль сетчатый (полиэстер), подошва — термопластичная резина (ТЭП)',2490.00,'IMG_KS_195213.png'),(3,'Кроссовки детские «Радуга» экокожа перфорированная',1,1,3,'Кроссовки детские «Радуга» экокожа перфорированная. Производитель: ИП Смирнова Л.В. (бренд «Лапушка»), г. Вологда','Верх — экокожа перфорированная, вставки — нейлон, стелька — ортопедическая с латексной подушкой, подошва — ЭВА',1990.00,'IMG_KS_195222.png'),(4,'Сапожки детские «Снежинка» зимние на меху',1,2,4,'Сапожки детские «Снежинка» зимние на меху. Производитель: ООО «Тёплые ножки», г. Новосибирск','Верх — экокожа морозостойкая, утеплитель — искусственный мех (акрил 70%, полиэстер 30%), подкладка — флис, подошва — термоэластопласт (ТЭП)',3490.00,'IMG_KB_185501.png'),(5,'Сапожки детские «Медвежонок» демисезонные непромокаемые',1,2,5,'Сапожки детские «Медвежонок» демисезонные непромокаемые. Производитель: Компания «Северята», г. Архангельск','Верх — плащёвая ткань с водоотталкивающей пропиткой (полиэстер 100%), отделка — экокожа, подкладка — микрофибра, мембрана — AirTex, подошва — резина',3190.00,'IMG_KB_164816.png'),(6,'Сапожки детские «Принцесса» кожаные',1,2,6,'Сапожки детские «Принцесса» кожаные. Производитель: Мастерская «Весна», г. Тула','Верх — натуральная кожа (КРС), подкладка — шерсть натуральная (70%), байка (30%), стелька — войлочная, подошва — полиуретан',4290.00,'IMG_KB_164811.png'),(7,'Сандалии детские «Пчёлка» ортопедические летние',1,3,7,'Сандалии детские «Пчёлка» ортопедические летние. Производитель: ООО «Здоровый шаг», г. Воронеж','Верх — нубук натуральный, подкладка — кожа растительного дубления, стелька — анатомическая с супинатором (латекс + пробка), подошва — каучук',2790.00,'IMG_KSd_195207.png'),(8,'Сандалии детские «Морячок» текстильные с фиксатором',1,3,8,'Сандалии детские «Морячок» текстильные с фиксатором. Производитель: ИП Кузнецов А.Д. (бренд «Карапуз»), г. Краснодар','Верх — джинсовая ткань (хлопок 100%), окантовка — искусственная кожа, подкладка — хлопок, подошва — ЭВА лёгкая',1690.00,'IMG_KSd_185508.png'),(9,'Туфли детские «Бантик» лаковые праздничные',1,4,9,'Туфли детские «Бантик» лаковые праздничные. Производитель: Дом обуви «Маленький франт», г. Москва','Верх — искусственная лаковая кожа (ПВХ), носок усиленный — термопластичный полиуретан, подкладка — кожа искусственная дышащая, стелька — анатомическая с амортизацией, подошва — полиуретан',2390.00,'IMG_KSho_185512.png'),(10,'Сапоги зимние натуральная кожа',2,2,10,'Сапоги женские зимние, нескользящая, устойчивая подошва, натуральный мех. Производитель: Дом обуви «Барбари», г. Новороссийск','Верх — натуральная кожа, подкладка — евромех, натуральный мех, текстильный утеплитель, подошва — резина',25000.00,'IMG_WB_170905.png'),(11,'Сапоги осенние натуральная кожа',2,2,10,'Сапоги женские демисезонные из высококачественной натуральной кожи. Производитель: Дом обуви «Барбари», г. Новороссийск','Верх — натуральная кожа, подкладка — текстильный утеплитель, стелька — текстиль, подошва — ТПР',15000.00,'IMG_WB_170859.png'),(12,'Сапоги трубы демисезонные замшевые',2,2,11,'Сапоги женские демисезонные замшевые. Производитель: ОАО «Твой комфорт», г. Ярославль','Верх — натуральная замша, утепленные, тракторная подошва, стелька — байка, подошва — резина',17000.00,'IMG_WB_170906.png'),(13,'Босоножки «Песчаный берег» коричневые',2,3,12,'Коричневая кожаная модель с открытым носком, боковой пряжкой и закрытым каблуком. Производитель: ИП «Стиль и комфорт», г. Санкт-Петербург','Верх — 100% козья кожа, подкладка — 100% кожа; подошва: 100% кожа',7500.00,'IMG_WSd_174332.png'),(14,'Женские босоножки «Черный кофе» со скульптурным каблуком',2,3,13,'Открытый носок и ремешок со сборной пряжкой, низкий каблук и резиновая подошва гарантируют комфорт. Производитель: ЗАО «Кожевенные традиции», г. Вятка','Верх — кожа собственного производства, подкладка — кожа; подошва: резина',5600.00,'IMG_WSd_185156.png'),(15,'Босоножки женские «Летний бриз»',2,3,14,'Босоножки женские кожаные белые, регулируемый ремешок, закрытый каблук и кожаная подошва. Производитель: ООО «Модный силуэт», г. Кострома','Верх — натуральная кожа, подкладка — натуральная кожа, подошва — кожа',7130.00,'IMG_WSd_185551.png'),(16,'Демисезонные туфли натуральная кожа',2,4,14,'Комфортные лаконичные лодочки, классический дизайна с современными акцентами. Производитель: ООО «Модный силуэт», г. Кострома','Верх — натуральная овечья кожа, подкладка и стелька — натуральная овечья кожа, подошва — резина',12340.00,'IMG_WSho_174409.png'),(17,'Туфли женские натуральная кожа с металлическими вставками',2,4,13,'Стильные туфли для весеннего и летнего сезона. Производитель: ЗАО «Кожевенные традиции», г. Вятка','Верх — натуральная телячья кожа, металлическая пряжка, подкладка и стелька — натуральная овечья кожа, подошва — резина',7757.00,'IMG_WSho_185202.png'),(18,'Туфли классические лодочки натуральная кожа',2,4,12,'Модель из натуральной кожи, классический острый носок и изящная шпилька. Производитель: ИП «Стиль и комфорт», г. Санкт-Петербург','Верх — натуральная кожа, подкладка и стелька — натуральная кожа, подошва — тунит',7550.00,'IMG_WSho_185545.png'),(19,'Кроссовки белые с оранжевым и красным',2,1,2,'Многослойный верх из кожи и текстиля. Производитель: Фабрика «Топ-Топ», г. Киров','Верх — 70% кожа, 30% текстиль, подкладка — текстиль, подошва — резина',9450.00,'IMG_WS_174110.png'),(20,'Кроссовки летние кожаные',2,1,2,'Уверенность на каждом шагу. Материал обеспечивает долговечность и комфорт при носке. Производитель: Фабрика «Топ-Топ», г. Киров','Верх — 87% кожа, 11% текстиль, 2% синтетика; подкладка: 100% текстиль; подошва: резина, пластик',8565.00,'IMG_WS_185528.png'),(21,'Кроссовки кожаные',2,1,12,'Кроссовки женские из натуральной кожи. Производитель: ИП «Стиль и комфорт», г. Санкт-Петербург','Верх — натуральная кожа, подкладка — кожа, подошва — полиуретан',8765.00,'IMG_WS_185535.png'),(22,'Кроссовки «Стиль и комфорт»',2,1,12,'Кроссовки женские из натуральной кожи и синтетических материалов, сетчатые вставки. Производитель: ИП «Стиль и комфорт», г. Санкт-Петербург','Верх — 70% кожа, 20% полиамид, 10% полиуретан; подкладка — текстиль, подошва — ТПУ',7795.00,'IMG_WS_185538.png'),(23,'Кроссовки для бега «Драйв»',3,1,15,'Сочетание стиля, комфорта и современных технологий. Производитель: Фабрика «Азимут», г. Санкт-Петербург','Верх — сетка, подкладка — текстиль, подошва — резина',6550.00,'IMG_MS_185531.png'),(24,'Кроссовки «Азимут Бег»',3,1,15,'Универсальный выбор для повседневного использования. Производитель: Фабрика «Азимут», г. Санкт-Петербург','Верх — 6% кожа, 29% текстиль, 15% полимерные материалы; подкладка — 100% текстиль; подошва — полимерные материалы, резина',7890.00,'IMG_MS_185532.png'),(25,'Кроссовки',3,1,2,'Кроссовки для мужчин, идеальное сочетание стиля и комфорта. Производитель: Фабрика «Топ-Топ», г. Киров','Верх — 56% кожа, 29% текстиль, 15% полимерные материалы; подкладка — 100% текстиль; подошва — полимерные материалы, резина',9567.00,'IMG_MS_185533.png'),(26,'Черные туфли в классическом стиле — база для деловых образов',3,4,10,'Модель выполнена из натуральной кожи, на резиновой подошве. Производитель: Дом обуви «Барбари», г. Новороссийск','Верх — натуральная кожа, подкладка — текстиль, стелька — натуральная кожа, подошва — резина',24569.00,'IMG_MSho_190514.png'),(27,'Туфли мужские светлые классика демисезонные',3,4,16,'Туфли на низком каблуке кожаные','Верх — натуральная кожа, подкладка — текстиль, стелька — натуральная кожа, подошва — ТЭП',10155.00,'IMG_MSho_190518.png'),(28,'Туфли из натуральной кожи',3,4,16,'Модель выполнена из натуральной кожи, на резиновой подошве, поддерживает классический и деловой стиль образов. Производитель: Дом обуви «Берестов», г. Москва','Верх — натуральная кожа, подкладка — натуральная кожа, стелька — натуральная кожа, подошва — резина',25625.00,'IMG_MSho_191612.png'),(29,'Ботинки мужские демисезонные',3,5,10,'Ботинки из натуральной кожи темно-коричневого и черного цвета на шнурках. Производитель: Дом обуви «Берестов», г. Москва','Верх — натуральная кожа, подкладка — текстиль, подошва — искусственный материал',21500.00,'IMG_MB_174349.png'),(30,'Ботинки черные из натуральной кожи',3,5,13,'Ботинки представляют баланс традиций и современности. Выполнены из натуральной кожи насыщенного черного цвета. Производитель: ЗАО «Кожевенные традиции», г. Вятка','Верх — натуральная кожа, подкладка — ворсин, стелька — ворсин, подошва — тунит',14567.00,'IMG_MB_191200.png'),(31,'Ботинки зимние классические',3,5,10,'Выбор на каждый день — натуральная кожа, натуральный мех. Производитель: Дом обуви «Барбари», г. Новороссийск','Верх — натуральная кожа, подкладка — натуральный мех, стелька — натуральный мех, подошва — ТЭП',15670.00,'IMG_MB_191205.png');
/*!40000 ALTER TABLE `products` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `roles`
--

DROP TABLE IF EXISTS `roles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `roles` (
  `role_id` int NOT NULL AUTO_INCREMENT,
  `role_name` varchar(50) NOT NULL,
  PRIMARY KEY (`role_id`),
  UNIQUE KEY `role_name_UNIQUE` (`role_name`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `roles`
--

LOCK TABLES `roles` WRITE;
/*!40000 ALTER TABLE `roles` DISABLE KEYS */;
INSERT INTO `roles` VALUES (1,'Administrator'),(5,'Customer'),(2,'Manager'),(4,'Order_Processor'),(3,'Stock_Keeper');
/*!40000 ALTER TABLE `roles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sizes`
--

DROP TABLE IF EXISTS `sizes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `sizes` (
  `size_id` int NOT NULL AUTO_INCREMENT,
  `size_value` decimal(3,1) NOT NULL,
  PRIMARY KEY (`size_id`),
  UNIQUE KEY `size_value_UNIQUE` (`size_value`)
) ENGINE=InnoDB AUTO_INCREMENT=314 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sizes`
--

LOCK TABLES `sizes` WRITE;
/*!40000 ALTER TABLE `sizes` DISABLE KEYS */;
INSERT INTO `sizes` VALUES (1,18.0),(2,19.0),(3,20.0),(4,21.0),(5,22.0),(6,23.0),(7,24.0),(8,25.0),(9,26.0),(10,27.0),(11,28.0),(12,29.0),(13,30.0),(14,31.0),(15,32.0),(16,33.0),(17,34.0),(18,35.0),(19,36.0),(20,36.5),(21,37.0),(22,37.5),(23,38.0),(24,38.5),(25,39.0),(26,39.5),(27,40.0),(28,40.5),(29,41.0),(30,41.5),(31,42.0),(32,42.5),(33,43.0),(34,44.0),(35,45.0);
/*!40000 ALTER TABLE `sizes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `stock_items`
--

DROP TABLE IF EXISTS `stock_items`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `stock_items` (
  `stock_item_id` int NOT NULL AUTO_INCREMENT,
  `product_id` int NOT NULL,
  `size_id` int NOT NULL,
  `quantity` int NOT NULL DEFAULT '0',
  PRIMARY KEY (`stock_item_id`),
  UNIQUE KEY `uq_stock_items_product_size` (`product_id`,`size_id`),
  KEY `fk_stock_items_products_idx` (`product_id`),
  KEY `fk_stock_items_sizes_idx` (`size_id`),
  CONSTRAINT `fk_stock_items_products` FOREIGN KEY (`product_id`) REFERENCES `products` (`product_id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_stock_items_sizes` FOREIGN KEY (`size_id`) REFERENCES `sizes` (`size_id`) ON DELETE RESTRICT ON UPDATE CASCADE,
  CONSTRAINT `chk_stock_items_qty` CHECK ((`quantity` >= 0))
) ENGINE=InnoDB AUTO_INCREMENT=93 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `stock_items`
--

LOCK TABLES `stock_items` WRITE;
/*!40000 ALTER TABLE `stock_items` DISABLE KEYS */;
INSERT INTO `stock_items` VALUES (1,1,3,21),(2,1,4,21),(3,1,5,20),(4,1,6,20),(5,1,7,20),(6,1,8,20),(7,2,16,2),(8,2,17,2),(9,3,2,30),(10,3,3,30),(11,4,2,15),(12,4,3,15),(13,5,4,5),(14,5,5,5),(15,5,6,5),(16,6,1,4),(17,6,2,4),(18,6,3,4),(19,7,4,4),(20,7,5,4),(21,7,6,4),(22,7,7,4),(23,8,10,7),(24,8,11,7),(25,8,12,7),(26,8,13,7),(27,9,1,7),(28,9,2,7),(29,9,3,7),(30,10,19,12),(31,10,21,17),(32,11,25,2),(33,11,27,2),(34,12,25,5),(35,12,27,5),(36,13,19,6),(37,13,21,9),(38,13,23,10),(39,13,25,10),(40,13,27,10),(41,14,18,3),(42,15,21,20),(43,15,25,23),(44,15,27,23),(45,16,19,24),(46,16,21,25),(47,16,23,25),(48,17,19,4),(49,17,23,4),(50,17,25,4),(51,17,27,4),(52,18,18,9),(53,18,23,9),(54,19,18,2),(55,19,19,2),(56,20,21,5),(57,20,22,5),(58,20,23,5),(59,20,24,5),(60,21,18,16),(61,21,19,16),(62,21,23,16),(63,21,25,16),(64,22,18,3),(65,22,23,3),(66,23,30,18),(67,23,31,18),(68,23,32,18),(69,23,33,18),(70,24,23,50),(71,24,24,50),(72,24,25,50),(73,24,26,50),(74,24,27,50),(75,24,29,50),(76,25,29,3),(77,25,31,3),(78,25,34,3),(79,26,27,23),(80,26,29,23),(81,26,33,23),(82,27,3,15),(83,27,33,15),(84,27,34,15),(85,28,34,3),(86,29,29,1),(87,29,31,1),(88,30,31,19),(89,30,33,20),(90,31,29,29),(91,31,31,30);
/*!40000 ALTER TABLE `stock_items` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `subcategories`
--

DROP TABLE IF EXISTS `subcategories`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `subcategories` (
  `subcategory_id` int NOT NULL AUTO_INCREMENT,
  `subcategory_name` varchar(100) NOT NULL,
  PRIMARY KEY (`subcategory_id`),
  UNIQUE KEY `subcategory_name_UNIQUE` (`subcategory_name`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `subcategories`
--

LOCK TABLES `subcategories` WRITE;
/*!40000 ALTER TABLE `subcategories` DISABLE KEYS */;
INSERT INTO `subcategories` VALUES (3,'Босоножки'),(5,'Ботинки'),(1,'Кроссовки'),(2,'Сапоги'),(4,'Туфли');
/*!40000 ALTER TABLE `subcategories` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `user_id` int NOT NULL,
  `last_name` varchar(50) NOT NULL,
  `first_name` varchar(50) NOT NULL,
  `patronymic` varchar(50) DEFAULT NULL,
  `login` varchar(50) NOT NULL,
  `role_id` int NOT NULL,
  PRIMARY KEY (`user_id`),
  UNIQUE KEY `login_UNIQUE` (`login`),
  KEY `fk_users_roles_idx` (`role_id`),
  CONSTRAINT `fk_users_roles` FOREIGN KEY (`role_id`) REFERENCES `roles` (`role_id`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (1,'Иванов','Иван','Сергеевич','isivanov',1),(2,'Петров','Пётр','Алексеевич','papetrov',2),(3,'Сидорова','Анна','Дмитриевна','asidorova',3),(4,'Кузнецова','Екатерина','Андреевна','ekuznetsova',2),(5,'Смирнов','Дмитрий','Владимирович','dsmirnov',3),(6,'Попов','Алексей','Николаевич','apopov',1),(7,'Васильева','Ольга','Игоревна','ovasileva',3),(8,'Соколов','Павел','Викторович','psokolov',2),(9,'Михайлова','Татьяна','Юрьевна','tmikhailova',3),(10,'Новикова','Наталья','Сергеевна','nnovikova',2),(11,'Фёдоров','Андрей','Максимович','afedorov',1),(12,'Морозов','Сергей','Павлович','smorozov',3),(13,'Волкова','Юлия','Александровна','jvolkova',2),(14,'Алексеев','Артём','Евгеньевич','aalekseev',3),(15,'Лебедева','Ирина','Владимировна','ilebedeva',3),(16,'Егоров','Никита','Денисович','negorov',2),(17,'Павлова','Елена','Петровна','epavlova',3),(18,'Козлов','Роман','Олегович','rkozlov',1),(19,'Степанова','Виктория','Романовна','vstepanova',2),(20,'Николаев','Владимир','Иванович','vnikolaev',3);
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-11-20 13:37:38
