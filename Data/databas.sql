CREATE DATABASE IF NOT EXISTS `hatebin` DEFAULT CHARACTER SET utf8 COLLATE utf8_swedish_ci;
USE `hatebin`;

DROP TABLE IF EXISTS `hate`;
CREATE TABLE IF NOT EXISTS `hate` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `network` text COLLATE utf8_swedish_ci NOT NULL,
  `networkId` bigint(20) NOT NULL,
  `text` text COLLATE utf8_swedish_ci NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 COLLATE=utf8_swedish_ci;

DROP TABLE IF EXISTS `hate-categories`;
CREATE TABLE IF NOT EXISTS `hate-categories` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `hateId` int(11) NOT NULL,
  `category` text COLLATE utf8_swedish_ci NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 COLLATE=utf8_swedish_ci;

DROP TABLE IF EXISTS `love`;
CREATE TABLE IF NOT EXISTS `love` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `created` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `email` text COLLATE utf8_swedish_ci NOT NULL,
  `reason` text COLLATE utf8_swedish_ci NOT NULL,
  `sent` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 COLLATE=utf8_swedish_ci;

DROP TABLE IF EXISTS `users`;
CREATE TABLE IF NOT EXISTS `users` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` text COLLATE utf8_swedish_ci NOT NULL,
  `email` varchar(50) COLLATE utf8_swedish_ci NOT NULL,
  `password` text COLLATE utf8_swedish_ci NOT NULL,
  `roles` text COLLATE utf8_swedish_ci NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `email` (`email`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 COLLATE=utf8_swedish_ci;