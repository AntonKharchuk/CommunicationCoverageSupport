CREATE Database sdb1;
CREATE USER 'hostmaster'@'localhost' IDENTIFIED WITH mysql_native_password;
GRANT ALL PRIVILEGES ON *.* TO 'hostmaster'@'localhost' WITH GRANT OPTION;
CREATE USER 'apiuser'@'%' IDENTIFIED BY '************';
GRANT ALL PRIVILEGES ON sdb1.* TO 'apiuser'@'%';
FLUSH PRIVILEGES;

SHOW GRANTS FOR 'apiuser'@'%';

use sdb1;
CREATE TABLE artwork (
    id TINYINT PRIMARY KEY auto_increment,
    name VARCHAR(255) NOT NULL
);

CREATE TABLE acc (
    id TINYINT PRIMARY KEY auto_increment,
    name VARCHAR(255) NOT NULL
);

CREATE TABLE owners (
id bigint PRIMARY KEY NOT NULL,
name VARCHAR(255) NOT NULL
);

CREATE TABLE sim_cards (
    iccid VARCHAR(20) PRIMARY KEY NOT NULL,
    imsi VARCHAR(15) NOT NULL,
    artworkId tinyint NOT NULL,
    accId tinyint NOT NULL,
    installed BOOLEAN DEFAULT FALSE,
    purged BOOLEAN DEFAULT FALSE,
    ki1 VARCHAR(255) NOT NULL,
    cardOwnerId bigint NOT NULL DEFAULT 0,
    pin1 smallint NOT NULL,
    pin2 smallint NOT NULL,
    puk1 int NOT NULL,
    puk2 int NOT NULL,
    adm1 VARCHAR(255) NOT NULL,

    FOREIGN KEY (artworkId) REFERENCES artwork(id),
    FOREIGN KEY (accId) REFERENCES acc(id),
    FOREIGN KEY (cardOwnerId) REFERENCES owners(id)
);
DELIMITER //
CREATE TRIGGER sim_cards_chk_insert
BEFORE INSERT ON sim_cards
FOR EACH ROW
BEGIN
    IF NEW.installed = TRUE AND NEW.purged = TRUE THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'A SIM card cannot be both installed and purged.';
    END IF;
END;
//
DELIMITER ;
DELIMITER //
CREATE TRIGGER sim_cards_chk_update
BEFORE UPDATE ON sim_cards
FOR EACH ROW
BEGIN
    IF NEW.installed = TRUE AND NEW.purged = TRUE THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'A SIM card cannot be both installed and purged.';
    END IF;
END;
//
DELIMITER ;

CREATE TABLE msisdn (
    msisdn VARCHAR(12) PRIMARY KEY NOT NULL,
    class tinyint NOT NULL DEFAULT 0,
    prop2 BOOLEAN DEFAULT FALSE
);

CREATE TABLE subscriber (
    msisdn VARCHAR(12) NOT NULL,
    imsi VARCHAR(15) NOT NULL
);

CREATE VIEW imsi_free AS
SELECT free.imsi
FROM sim_cards free
LEFT JOIN subscriber t2 ON free.imsi = t2.imsi
WHERE free.installed = TRUE
AND t2.imsi IS NULL;

CREATE VIEW msisdn_free AS
SELECT free.msisdn
FROM msisdn free
LEFT JOIN subscriber t2 ON free.msisdn = t2.msisdn
WHERE t2.msisdn IS NULL;

DELIMITER //
CREATE TRIGGER subscriber_chk_insert
BEFORE INSERT ON subscriber
FOR EACH ROW
BEGIN
    IF NOT EXISTS (SELECT 1 FROM msisdn_free WHERE msisdn = NEW.msisdn) THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'A MSISDN occupied.';
    END IF;
    IF NOT EXISTS (SELECT 1 FROM imsi_free WHERE imsi = NEW.imsi) THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'A IMSI unavailable.';
    END IF;
END;
//
DELIMITER ;
