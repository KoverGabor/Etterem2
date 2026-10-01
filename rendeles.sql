-- phpMyAdmin SQL Dump
-- version 5.2.3
-- https://www.phpmyadmin.net/
--
-- Gép: 127.0.0.1:3306
-- Létrehozás ideje: 2026. Okt 01. 07:13
-- Kiszolgáló verziója: 8.4.7
-- PHP verzió: 8.3.28

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Adatbázis: `etterem`
--

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `rendeles`
--

DROP TABLE IF EXISTS `rendeles`;
CREATE TABLE IF NOT EXISTS `rendeles` (
  `id` int NOT NULL AUTO_INCREMENT,
  `dish` varchar(40) COLLATE utf8mb4_hungarian_ci NOT NULL,
  `description` text COLLATE utf8mb4_hungarian_ci,
  `orderTime` datetime NOT NULL,
  `updateTime` datetime NOT NULL,
  `vendegId` int NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_rendeles_vendeg` (`vendegId`)
) ENGINE=MyISAM AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- A tábla adatainak kiíratása `rendeles`
--

INSERT INTO `rendeles` (`id`, `dish`, `description`, `orderTime`, `updateTime`, `vendegId`) VALUES
(1, 'Gulyásleves', 'Extra csípős kérésre, kenyérrel.', '2025-05-02 12:30:00', '2025-05-02 12:30:00', 1),
(2, 'Rántott sajt', 'Hasábburgonyával és tartármártással.', '2025-05-14 19:15:00', '2025-05-14 19:15:00', 2),
(3, 'Halászlé', 'Szegedi módra, csípős paprikával.', '2025-05-26 13:00:00', '2025-05-26 13:00:00', 3),
(4, 'Túrós csusza', 'Tepertővel, dupla adag tejföllel.', '2025-06-08 20:40:00', '2025-06-08 20:40:00', 5),
(5, 'Somlói galuska', 'Desszert, extra csokiöntettel.', '2025-06-19 21:20:00', '2025-06-19 21:20:00', 6);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
