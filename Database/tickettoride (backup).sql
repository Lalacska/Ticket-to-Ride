-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Vært: 127.0.0.1
-- Genereringstid: 30. 11 2023 kl. 07:41:38
-- Serverversion: 10.4.28-MariaDB
-- PHP-version: 8.2.4

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `tickettoride`
--

-- --------------------------------------------------------

--
-- Struktur-dump for tabellen `kort`
--

CREATE TABLE `kort` (
  `Sort kort` int(255) NOT NULL DEFAULT 12,
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
  `username` varchar(30) DEFAULT NULL,
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
-- Struktur-dump for tabellen `pwdreset`
--

CREATE TABLE `pwdreset` (
  `pwdResetId` int(11) NOT NULL,
  `pwdResetEmail` text NOT NULL,
  `pwdResetSelector` text NOT NULL,
  `pwdResetToken` longtext NOT NULL,
  `pwdResetExpires` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Data dump for tabellen `pwdreset`
--

INSERT INTO `pwdreset` (`pwdResetId`, `pwdResetEmail`, `pwdResetSelector`, `pwdResetToken`, `pwdResetExpires`) VALUES
(20, '', 'f33de5ebe99fbb09', '$2y$10$KlSik0U24uUujFlI1oMkc.XSO7LVWpUVCBN07epSM0v.00S3/00n.', '1700136660'),
(25, 'dhartwich650@gmail.com', '0398d757edb980de', '$2y$10$mRsJr47wPRkzpl/Zwd1GFOPyGV7c.MnwG.R8KsyRpqWw8WNTHo7FW', '1701250518');

-- --------------------------------------------------------

--
-- Struktur-dump for tabellen `sc_users`
--

CREATE TABLE `sc_users` (
  `user_id` int(11) NOT NULL,
  `username` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `email` varchar(254) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `password` char(60) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL,
  `registration_date` timestamp NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Data dump for tabellen `sc_users`
--

INSERT INTO `sc_users` (`user_id`, `username`, `email`, `password`, `registration_date`) VALUES
(1, 'Oldi', 'Ollo@olo.com', '$2y$10$Q45ykXfAZNjKYYPGtTfmQOgrOkePo/.M8KDWvWU.PKl7FsMnXL7z2', '2022-10-05 08:26:07'),
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
(20, 'olo3', 'olo3@gmail.com', '$2y$10$RUX1D7oZ94CtHapHXtcukeK/Hy5flRrzxATctCPBt2UGWQO3mStWW', '2023-06-26 10:09:43'),
(21, 'Hej123', 'hej123@gmail.com', '$2y$10$8XnAoDkeIiPw8gHTe0h8G.vPyuG1NlCY6waE9b6Hb9gqBeDrQEtse', '2023-10-31 07:31:05'),
(22, 'Daniel123', 'daniel@gmail.com', '$2y$10$CaOW5hj5/roKFPH3aOfT.u84QzKhXTctDy6WudVU7gAbnBJMlDlOG', '2023-10-31 07:36:25'),
(23, 'Daniel1234', 'danmail@gmail.com', '$2y$10$XHnrF9O7voIeZ2FcnkrBIekwrcewPZm24lSQBkXFMsBsr.dG1h7aq', '2023-11-01 11:03:03'),
(24, 'DanTheMan', 'dan@dan.com', '$2y$10$H8LNYsBUbJSd2qu1djF76uyoAH0.EMC9fI2o8XoGSDEJ1eUli7o0S', '2023-11-03 09:51:05'),
(25, 'Dr3nzy', 'dhartwich650@gmail.com', '$2y$10$tVceEkFEqBa6M4ZKVwG3mu1BJ4ly0xi4HVjvIYwK13JsThNDZSOD.', '2023-11-07 11:39:58'),
(26, 'Dantis', 'danis@danis.com', '$2y$10$YKJTuz4jWMAVzpkwKenB5OG/o4KEXu176zj7i/hJaRLE462BjFh.e', '2023-11-07 13:42:47');

--
-- Begrænsninger for dumpede tabeller
--

--
-- Indeks for tabel `pwdreset`
--
ALTER TABLE `pwdreset`
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
-- Tilføj AUTO_INCREMENT i tabel `pwdreset`
--
ALTER TABLE `pwdreset`
  MODIFY `pwdResetId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=26;

--
-- Tilføj AUTO_INCREMENT i tabel `sc_users`
--
ALTER TABLE `sc_users`
  MODIFY `user_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=27;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;