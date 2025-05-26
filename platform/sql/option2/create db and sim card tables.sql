-- Create database and some users
BEGIN

CREATE Database sdb2;
CREATE USER 'hostmaster'@'localhost' IDENTIFIED WITH mysql_native_password;
GRANT ALL PRIVILEGES ON *.* TO 'hostmaster'@'localhost' WITH GRANT OPTION;
CREATE USER 'apiuser'@'%' IDENTIFIED BY '************';
GRANT ALL PRIVILEGES ON sdb1.* TO 'apiuser'@'%';
FLUSH PRIVILEGES;

SHOW GRANTS FOR 'apiuser'@'%';

use sdb2;

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

CREATE TABLE transportKey (
id int PRIMARY KEY NOT NULL,
kInd VARCHAR(15) NOT NULL DEFAULT 'EMPTY'
);

-- Table for sim cards: ordered, ready to use, in use
CREATE TABLE simCards (
    iccid CHAR(20) UNIQUE NOT NULL,
    imsi CHAR(15) UNIQUE NOT NULL,
    msisdn CHAR(12) UNIQUE NOT NULL,
    kIndId int NOT NULL DEFAULT 0,
    ki CHAR(32) NOT NULL,
    pin1 smallint NOT NULL,
    pin2 smallint NOT NULL,
    puk1 int NOT NULL,
    puk2 int NOT NULL,
    adm1 VARCHAR(255) NOT NULL,
    artworkId tinyint NOT NULL DEFAULT 0,
    accId tinyint NOT NULL DEFAULT 0,
    installed tinyint(1) DEFAULT 0,
    cardOwnerId bigint NOT NULL DEFAULT 0,

    PRIMARY KEY (iccid, imsi, msisdn, kIndId),
    FOREIGN KEY (artworkId) REFERENCES artwork(id),
    FOREIGN KEY (accId) REFERENCES acc(id),
    FOREIGN KEY (cardOwnerId) REFERENCES owners(id),
    FOREIGN KEY (kIndId) REFERENCES transportKey(id)
);

-- Triggers for samCards table: forbid insert if card exists in simCardsDrain table
DELIMITER //
CREATE TRIGGER simCards_insert
BEFORE INSERT ON simCards
FOR EACH ROW
BEGIN
    DECLARE _message_text VARCHAR(255) DEFAULT '';
    IF EXISTS (SELECT 1 FROM simCardsDrain WHERE iccid = NEW.iccid AND imsi = NEW.imsi AND msisdn = NEW.msisdn AND kIndId = NEW.kIndId) THEN
        SET _message_text = CONCAT("Cannot proceed.\nRecord already exists in simCardsDrained table:",
                    "\n  iccid : ", NEW.iccid,
                    "\n  imsi  : ", NEW.imsi,
                    "\n  msisdn: ", NEW.msisdn,
                    "\n  kIndId: ", NEW.kIndId);
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = _message_text;
    END IF;
END;
//
DELIMITER ;

-- Table for drained sim cards
CREATE TABLE simCardsDrain (
    iccid CHAR(20) UNIQUE NOT NULL,
    imsi CHAR(15) NOT NULL,
    msisdn CHAR(12) NOT NULL,
    kIndId int NOT NULL DEFAULT 0,
    ki CHAR(32) NOT NULL,
    pin1 smallint NOT NULL,
    pin2 smallint NOT NULL,
    puk1 int NOT NULL,
    puk2 int NOT NULL,
    adm1 VARCHAR(255) NOT NULL,
    artworkId tinyint NOT NULL DEFAULT 0,
    accId tinyint NOT NULL DEFAULT 0,
    installed tinyint(1) COMMENT 'stores status at the create moment',
    cardOwnerId bigint NOT NULL DEFAULT 0,
    createTimestamp DATETIME,

    PRIMARY KEY (iccid, imsi, msisdn, kIndId),
    FOREIGN KEY (artworkId) REFERENCES artwork(id),
    FOREIGN KEY (accId) REFERENCES acc(id),
    FOREIGN KEY (cardOwnerId) REFERENCES owners(id),
    FOREIGN KEY (kIndId) REFERENCES transportKey(id)
);

-- Trigger for simCardsDrain: on insert add timestamp
DELIMITER //
CREATE TRIGGER simCardsDrain_insert
BEFORE INSERT ON simCardsDrain
FOR EACH ROW
BEGIN
    SET NEW.createTimestamp = NOW ();
END;
//
DELIMITER ;

-- Procedure to move one sim card from simCards table to simCardsDrained. One sim card only can be moved in one call.
DELIMITER //
CREATE PROCEDURE drainOneSim(
    IN _iccid CHAR(20),
    IN _imsi CHAR(15),
    IN _msisdn CHAR(12),
    IN _kIndId int
)
BEGIN
    DECLARE rows_found INT DEFAULT 0 ;
    DECLARE rows_moved INT DEFAULT 0 ;
    DECLARE rows_deleted INT DEFAULT 0 ;
    DECLARE _message_text VARCHAR(255) DEFAULT '';
    IF _kIndId IS NULL THEN
        SET _kIndId = 0;
    END IF;

    START TRANSACTION;
    -- Precheck number of records
    SELECT COUNT(*) INTO rows_found FROM simCards WHERE iccid = _iccid
        AND imsi = _imsi AND msisdn = _msisdn AND kIndId = _kIndId;

    IF rows_found = 0 THEN
        ROLLBACK;
        SET _message_text = CONCAT('ERROR:'
            '\n  iccid : ', _iccid,
            '\n  imsi  : ', _imsi,
            '\n  msisdn: ', _msisdn,
            '\n  kIndId: ', _kIndId,
            '\nno records found; ROLLBACK');
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = _message_text;
    ELSEIF rows_found > 1 THEN
        ROLLBACK;
        SET _message_text = CONCAT('ERROR:'
            '\n  iccid : ', _iccid,
            '\n  imsi  : ', _imsi,
            '\n  msisdn: ', _msisdn,
            '\n  kIndId: ', _kIndId,
            '\nhas been found ', rows_found,' times.',
            '\nMultiple move is forbidder for this procedure. ROLLBACK');
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = _message_text;
    ELSE -- last variant when rows_found == 1
        INSERT INTO simCardsDrain (iccid, imsi, msisdn, kIndId, ki, pin1, pin2, puk1, puk2, adm1, artworkId, accId, installed, cardOwnerId)
        SELECT
            iccid, imsi, msisdn, kIndId, ki, pin1, pin2, puk1, puk2, adm1, artworkId, accId, installed, cardOwnerId
        FROM
            simCards
        WHERE
            iccid = _iccid AND imsi = _imsi AND msisdn = _msisdn AND kIndId = _kIndId;

        -- collect number of inserted rows
        SET rows_moved = ROW_COUNT();

        IF rows_moved != rows_found THEN
            -- if found and inserted resords is not equal. Rollback
            ROLLBACK;
            SET _message_text = CONCAT('ERROR:'
                 '\nFound ', rows_found, ' rows;',
                 '\nMoved ', rows_moved, ' rows;',
                 '\nData inconsistancy suspected. ROLLBACK');
            SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = _message_text;
        ELSE
            -- Delete record from simCards
            DELETE FROM simCards
            WHERE iccid = _iccid AND imsi = _imsi AND msisdn = _msisdn AND kIndId = _kIndId;

            -- save number of deleted records
            SET rows_deleted = ROW_COUNT();

            IF rows_deleted != rows_found THEN
                -- if found and deleted resords is not equal. Rollback
                ROLLBACK;
                SET _message_text = CONCAT('ERROR:'
                     '\nFound ', rows_found, ' rows;',
                     '\nDeleted ', rows_moved, ' rows;',
                     '\nData inconsistancy suspected. ROLLBACK');
                SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = _message_text;
            ELSE
                -- success
                COMMIT;
                SELECT CONCAT('Record [iccid: ', _iccid,', imsi: ', _imsi,', msisdn: ', _msisdn,
                    ', kIndId: ', _kIndId, '] successfully moved.') AS status_message;
            END IF;
        END IF;
    END IF;
END;
//
DELIMITER ;
