-- phpMyAdmin SQL Dump
-- version 5.1.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: May 29, 2025 at 05:46 PM
-- Server version: 10.4.19-MariaDB
-- PHP Version: 8.0.7

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `pbuild_db`
--

-- --------------------------------------------------------

--
-- Table structure for table `pokemons`
--

CREATE TABLE `pokemons` (
  `Id` int(11) NOT NULL,
  `Name` longtext NOT NULL,
  `TeamId` int(11) NOT NULL DEFAULT 0,
  `Attack` int(11) NOT NULL DEFAULT 0,
  `Defense` int(11) NOT NULL DEFAULT 0,
  `HP` int(11) NOT NULL DEFAULT 0,
  `SP_Attack` int(11) NOT NULL DEFAULT 0,
  `SP_Defense` int(11) NOT NULL DEFAULT 0,
  `Speed` int(11) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `teams`
--

CREATE TABLE `teams` (
  `Id` int(11) NOT NULL,
  `Name` longtext NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

CREATE TABLE `users` (
  `Id` int(11) NOT NULL,
  `Name` longtext NOT NULL,
  `Email` longtext NOT NULL,
  `Password` longtext NOT NULL,
  `Role` longtext NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `users`
--

INSERT INTO `users` (`Id`, `Name`, `Email`, `Password`, `Role`) VALUES
(1, 'Denzel Huijbers', 'huijbers15@outlook.com', '$2a$11$Ht7RoI3JinxMvHiJijGJgOkGxgyPuulUWYgWMmK3YdjqMFimJceYm', 'Admin'),
(2, 'John Doe', 'johndoe@example.com', '$2a$11$q0xwlkg1V4O6tejjD2FxuOYzEkQiLS/qT3ikW0x7gLmPPW7wwqhmG', 'User'),
(3, 'dddd', 'test@test.com', '$2a$11$I1PI6KKWecMD32Pn2HNtEO5FtjBY43kkO48XtFe/v4wgiZDTOFofy', 'User');

-- --------------------------------------------------------

--
-- Table structure for table `__efmigrationshistory`
--

CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(150) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `__efmigrationshistory`
--

INSERT INTO `__efmigrationshistory` (`MigrationId`, `ProductVersion`) VALUES
('20250217151725_InitialCreate', '8.0.2'),
('20250217172247_AddUserCreatedRole', '8.0.2'),
('20250219103033_AddTeamAndPokemon', '8.0.2'),
('20250219103516_AddTeamAndPokemonNew', '8.0.2'),
('20250219115857_AnnotatedTest', '8.0.2'),
('20250219121222_AddedNameToTeam', '8.0.2'),
('20250226101138_ImGoingInsane', '8.0.2'),
('20250325103332_AddPokemonStats', '8.0.2'),
('20250402114507_RemovedRequiredTags', '8.0.2');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `pokemons`
--
ALTER TABLE `pokemons`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_Pokemons_TeamId` (`TeamId`);

--
-- Indexes for table `teams`
--
ALTER TABLE `teams`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `__efmigrationshistory`
--
ALTER TABLE `__efmigrationshistory`
  ADD PRIMARY KEY (`MigrationId`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `pokemons`
--
ALTER TABLE `pokemons`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=20;

--
-- AUTO_INCREMENT for table `teams`
--
ALTER TABLE `teams`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=88;

--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `users`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `pokemons`
--
ALTER TABLE `pokemons`
  ADD CONSTRAINT `FK_Pokemons_Teams_TeamId` FOREIGN KEY (`TeamId`) REFERENCES `teams` (`Id`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
