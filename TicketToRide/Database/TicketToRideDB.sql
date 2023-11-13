-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Vært: localhost:3306
-- Genereringstid: 27. 06 2023 kl. 00:17:59
-- Serverversion: 5.7.23-23
-- PHP-version: 8.1.16

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `bekbekbe_login_sample_db`
--

-- --------------------------------------------------------

--
-- Struktur-dump for tabellen `kort`
--

CREATE TABLE `kort` (
  `Sort kort` int(255) NOT NULL DEFAULT '12',
  `Hvidt kort` int(255) NOT NULL,
  `Blåt kort` int(255) NOT NULL,
  `Lillat kort` int(255) NOT NULL,
  `Brunt kort` int(255) NOT NULL,
  `Gult kort` int(255) NOT NULL,
  `Rødt kort` int(255) NOT NULL,
  `Regnbue kort` int(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Data dump for tabellen `kort`
--

INSERT INTO `kort` (`Sort kort`, `Hvidt kort`, `Blåt kort`, `Lillat kort`, `Brunt kort`, `Gult kort`, `Rødt kort`, `Regnbue kort`) VALUES
(12, 12, 12, 12, 12, 12, 12, 14);

-- --------------------------------------------------------

--
-- Struktur-dump for tabellen `leaderboard`
--

CREATE TABLE `leaderboard` (
  `username` varchar(30) COLLATE utf8_unicode_ci DEFAULT NULL,
  `points` int(10) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Data dump for tabellen `leaderboard`
--

INSERT INTO `leaderboard` (`username`, `points`) VALUES
('Devesh Pratap Singh', 100),
('Emil Brinck Nielsen', 101),
('Tuhu Ghetto', 30);

-- --------------------------------------------------------

--
-- Struktur-dump for tabellen `pwdReset`
--

CREATE TABLE `pwdReset` (
  `pwdResetId` int(11) NOT NULL,
  `pwdResetEmail` text COLLATE utf8_unicode_ci NOT NULL,
  `pwdResetSelector` text COLLATE utf8_unicode_ci NOT NULL,
  `pwdResetToken` longtext COLLATE utf8_unicode_ci NOT NULL,
  `pwdResetExpire` text COLLATE utf8_unicode_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Data dump for tabellen `pwdReset`
--

INSERT INTO `pwdReset` (`pwdResetId`, `pwdResetEmail`, `pwdResetSelector`, `pwdResetToken`, `pwdResetExpire`) VALUES
(2, 'emilbrinck@gmail.com', 'bc3814626f355b1a', '$2y$10$HcMu3PikqE8iXorMDpLTgOU4SHFLEFQzT4YLk/CFKcJYREv2z/0dW', '1670848829');

-- --------------------------------------------------------

--
-- Struktur-dump for tabellen `sc_users`
--

CREATE TABLE `sc_users` (
  `user_id` int(11) NOT NULL,
  `username` varchar(20) CHARACTER SET utf8 NOT NULL,
  `email` varchar(254) CHARACTER SET utf8 NOT NULL,
  `password` char(60) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL,
  `registration_date` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Data dump for tabellen `sc_users`
--

INSERT INTO `sc_users` (`user_id`, `username`, `email`, `password`, `registration_date`) VALUES
(1, 'Olo', 'olo@olo.com', '$2y$10$S5TfBL19uY6BxPNxTLKzxOufAYSmyFzn0CpJMmFhR9MtvdMCRROuu', '2022-10-05 08:26:07'),
(4, 'Olo2', 'olo2@olo.com', '$2y$10$fkV8NaWm3Z/E601YwvjpiucD7azRufCAHkW9tzc469PLwE/tj.JpC', '2022-10-07 11:27:39'),
(5, 'Hej', 'hej@hej.com', '$2y$10$zYOfEtp4yenWtGdb1Nbgn.0JdKqJ5CHkI0NtNJYTezRem5R46gnb6', '2022-10-10 05:37:12'),
(6, 'Tuhu', 'tuhutest@tuhu.com', '$2y$10$tZh6OKUcoCX.I0OVL0w01.V0vSankWe4VEyP0W77unequBPMdWXrS', '2022-10-10 07:40:07'),
(7, 'Tuhu1', 'tuhutest1@tuhu.com', '$2y$10$i8SYqupK/nSZLSKmD3zugOvjjVua0DIXFJIQTR9cLpTSMEXAoRsGO', '2022-10-10 07:41:24'),
(8, 'Laura', 'laura@laura.com', '$2y$10$mvXpKjIhN3kX1ZkEENNpmeSu1mM6Dj3xCgX4xMt2RZQUzIGle7hTu', '2022-10-10 07:44:59'),
(11, 'abc', 'rubbing@gmail.com', '$2y$10$Do7hMkhldtDcEpQ3vXTDwuDkrJ2uGrPGnCSaubg6wdfGCWvF3nrvm', '2022-10-10 09:59:11'),
(12, 'feet', 'feet@gmail.com', '$2y$10$lckeSp5Aqnfsdgfb067CF.Xakc86tTLZHTYkG6OIr0vfPlwAY3SmC', '2022-10-17 06:15:22'),
(13, 'Dunkefar', 'emilbrinck@gmail.com', '$2y$10$G00wOEBoysIODexbOe5bauN8JvC7u2gP7Ikv6VvJKzZBB3mt5GPse', '2022-10-24 11:08:03'),
(14, 'test123', 'malteergey@gmail.com', '$2y$10$PMxkVGpjIttaVFdMDbuFCepVEE.Ggi7ms10l28CsyYSNiBEv/2z7G', '2022-10-24 11:37:36'),
(15, 'admin', 'admin@gmail.com', '$2y$10$rua5XFKg2HPEq8xo9iKlYuJiO7FBDsfp2ZmGElX0bGlA4HE/HML8.', '2022-11-14 09:32:06'),
(18, 'abc123', 'hej@daskdhsakdhsadjsa.com', '$2y$10$AX6InrOLR7xMx2jO0PxEU.pe1X6nq/9noXMCFYjTt4RB7WkkMq6uy', '2022-11-14 09:48:22'),
(19, 'emilbricnk1', 'a@gmail.com', '$2y$10$XWtPgY6fOAGcul.FT7IWIul19qldkgFQsf4jb2nNFYtPGDe.hMEbm', '2022-11-14 09:49:35'),
(20, 'olo3', 'olo3@gmail.com', '$2y$10$RUX1D7oZ94CtHapHXtcukeK/Hy5flRrzxATctCPBt2UGWQO3mStWW', '2023-06-26 10:09:43');

--
-- Begrænsninger for dumpede tabeller
--

--
-- Indeks for tabel `pwdReset`
--
ALTER TABLE `pwdReset`
  ADD PRIMARY KEY (`pwdResetId`);

--
-- Indeks for tabel `sc_users`
--
ALTER TABLE `sc_users`
  ADD PRIMARY KEY (`user_id`),
  ADD UNIQUE KEY `username` (`username`),
  ADD UNIQUE KEY `email` (`email`);

--
-- Brug ikke AUTO_INCREMENT for slettede tabeller
--

--
-- Tilføj AUTO_INCREMENT i tabel `pwdReset`
--
ALTER TABLE `pwdReset`
  MODIFY `pwdResetId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- Tilføj AUTO_INCREMENT i tabel `sc_users`
--
ALTER TABLE `sc_users`
  MODIFY `user_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=21;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
