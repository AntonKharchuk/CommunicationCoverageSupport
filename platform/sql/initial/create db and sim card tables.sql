CREATE Database sim_card_db;
use sim_card_db;
CREATE TABLE artwork (
    id TINYINT PRIMARY KEY auto_increment,
    name VARCHAR(255) NOT NULL
);

CREATE TABLE acc (
    id TINYINT PRIMARY KEY auto_increment,
    name VARCHAR(255) NOT NULL
);

CREATE TABLE sim_cards (
    iccid VARCHAR(20) PRIMARY KEY NOT NULL,
    imsi bigint UNIQUE NOT NULL,
    msisdn bigint UNIQUE NOT NULL,
    artworkId tinyint NOT NULL,
    accId tinyint NOT NULL,
    produced BOOLEAN DEFAULT FALSE,
    installed BOOLEAN DEFAULT FALSE,
    purged BOOLEAN DEFAULT FALSE,
	kl1 VARCHAR(255) NOT NULL,
    card_owner VARCHAR(255) NOT NULL,
    pin1 smallint NOT NULL,
    pin2 smallint NOT NULL,
    puk1 int NOT NULL,
	puk2 int NOT NULL,
    adm1 bigint NOT NULL,

    FOREIGN KEY (artworkId) REFERENCES artwork(id),
    FOREIGN KEY (accId) REFERENCES acc(id)
);

DELIMITER //
CREATE TRIGGER sim_cards_check_install_purge
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
CREATE TRIGGER sim_cards_check_install_purge_update
BEFORE UPDATE ON sim_cards
FOR EACH ROW
BEGIN
    IF NEW.installed = TRUE AND NEW.purged = TRUE THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'A SIM card cannot be both installed and purged.';
    END IF;
END;
//
DELIMITER ;

