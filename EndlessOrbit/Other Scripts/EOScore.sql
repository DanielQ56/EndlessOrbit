use `heroku_f9bc1069544552f`;


DROP TABLE IF EXISTS `Normal`;

CREATE TABLE `Normal`(
	`username` varchar(50) NOT NULL,
    `score` int NOT NULL
);

DROP TABLE IF EXISTS `Unstable`;
CREATE TABLE `Unstable`(
	`username` varchar(50) NOT NULL,
    `score` int NOT NULL
);

INSERT INTO `Normal` (username, score)
VALUES ("Phae", 2300);

INSERT INTO `Normal` (username, score)
VALUES ("Egg", 2000);

INSERT INTO `Normal` (username, score)
VALUES ("DenC", 1800);

INSERT INTO `Normal` (username, score)
VALUES ("Phi", 1800);

INSERT INTO `Normal` (username, score)
VALUES ("Al", 1700);

INSERT INTO `Normal` (username, score)
VALUES ("Chef", 1500);

INSERT INTO `Normal` (username, score)
VALUES ("ChanPark", 1500);

INSERT INTO `Normal` (username, score)
VALUES ("Tunky", 1500);

INSERT INTO `Normal` (username, score)
VALUES ("PanPan", 1500);

INSERT INTO `Normal` (username, score)
VALUES ("Sleeper", 1400);


INSERT INTO `Unstable` (username, score)
VALUES ("Boonga", 2300);

INSERT INTO `Unstable` (username, score)
VALUES ("Phi", 2000);

INSERT INTO `Unstable` (username, score)
VALUES ("Dennis", 1800);

INSERT INTO `Unstable` (username, score)
VALUES ("Phae", 1800);

INSERT INTO `Unstable` (username, score)
VALUES ("Alpaca", 1700);

INSERT INTO `Unstable` (username, score)
VALUES ("Chef", 1500);

INSERT INTO `Unstable` (username, score)
VALUES ("Park", 1500);

INSERT INTO `Unstable` (username, score)
VALUES ("Funky", 1500);

INSERT INTO `Unstable` (username, score)
VALUES ("Pan", 1500);

INSERT INTO `Unstable` (username, score)
VALUES ("Sleeper", 1400);
