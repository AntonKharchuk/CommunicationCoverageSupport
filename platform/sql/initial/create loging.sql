CREATE TABLE log_sim_cards (
    log_id INT AUTO_INCREMENT PRIMARY KEY,
    table_name VARCHAR(20) NOT NULL,
    record_id VARCHAR(20) NOT NULL, 
    column_name VARCHAR(50) NOT NULL,
    old_value VARCHAR(250),
    new_value VARCHAR(250),
    user VARCHAR(255),
    change_time TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Trigger for UPDATE operations
DELIMITER //
CREATE TRIGGER sim_cards_update_log
AFTER UPDATE ON sim_cards
FOR EACH ROW
BEGIN
    IF OLD.iccid <> NEW.iccid THEN
        INSERT INTO log_sim_cards (table_name, record_id, column_name, old_value, new_value, user)
        VALUES ('sim_cards', OLD.iccid, 'iccid', OLD.iccid, NEW.iccid, USER());
    END IF;
    IF OLD.imsi <> NEW.imsi THEN
        INSERT INTO log_sim_cards (table_name, record_id, column_name, old_value, new_value, user)
        VALUES ('sim_cards', OLD.iccid, 'imsi', OLD.imsi, NEW.imsi, USER());
    END IF;
    IF OLD.msisdn <> NEW.msisdn THEN
        INSERT INTO log_sim_cards (table_name, record_id, column_name, old_value, new_value, user)
        VALUES ('sim_cards', OLD.iccid, 'msisdn', OLD.msisdn, NEW.msisdn, USER());
    END IF;
    IF OLD.artworkId <> NEW.artworkId THEN
        INSERT INTO log_sim_cards (table_name, record_id, column_name, old_value, new_value, user)
        VALUES ('sim_cards', OLD.iccid, 'artworkId', OLD.artworkId, NEW.artworkId, USER());
    END IF;
    IF OLD.accId <> NEW.accId THEN
        INSERT INTO log_sim_cards (table_name, record_id, column_name, old_value, new_value, user)
        VALUES ('sim_cards', OLD.iccid, 'accId', OLD.accId, NEW.accId, USER());
    END IF;
    IF OLD.produced <> NEW.produced THEN
        INSERT INTO log_sim_cards (table_name, record_id, column_name, old_value, new_value, user)
        VALUES ('sim_cards', OLD.iccid, 'produced', OLD.produced, NEW.produced, USER());
    END IF;
    
    IF OLD.installed <> NEW.installed THEN
        INSERT INTO log_sim_cards (table_name, record_id, column_name, old_value, new_value, user)
        VALUES ('sim_cards', OLD.iccid, 'installed', OLD.installed, NEW.installed, USER());
    END IF;
    IF OLD.purged <> NEW.purged THEN
        INSERT INTO log_sim_cards (table_name, record_id, column_name, old_value, new_value, user)
        VALUES ('sim_cards', OLD.iccid, 'purged', OLD.purged, NEW.purged, USER());
    END IF;
    IF OLD.kl1 <> NEW.kl1 THEN
        INSERT INTO log_sim_cards (table_name, record_id, column_name, old_value, new_value, user)
        VALUES ('sim_cards', OLD.iccid, 'kl1', OLD.kl1, NEW.kl1, USER());
    END IF;
    IF OLD.card_owner <> NEW.card_owner THEN
        INSERT INTO log_sim_cards (table_name, record_id, column_name, old_value, new_value, user)
        VALUES ('sim_cards', OLD.iccid, 'card_owner', OLD.card_owner, NEW.card_owner, USER());
    END IF;
    IF OLD.pin1 <> NEW.pin1 THEN
        INSERT INTO log_sim_cards (table_name, record_id, column_name, old_value, new_value, user)
        VALUES ('sim_cards', OLD.iccid, 'pin1', OLD.pin1, NEW.pin1, USER());
    END IF;
    IF OLD.pin2 <> NEW.pin2 THEN
        INSERT INTO log_sim_cards (table_name, record_id, column_name, old_value, new_value, user)
        VALUES ('sim_cards', OLD.iccid, 'pin2', OLD.pin2, NEW.pin2, USER());
    END IF;
    IF OLD.puk1 <> NEW.puk1 THEN
        INSERT INTO log_sim_cards (table_name, record_id, column_name, old_value, new_value, user)
        VALUES ('sim_cards', OLD.iccid, 'puk1', OLD.puk1, NEW.puk1, USER());
    END IF;
    IF OLD.puk2 <> NEW.puk2 THEN
        INSERT INTO log_sim_cards (table_name, record_id, column_name, old_value, new_value, user)
        VALUES ('sim_cards', OLD.iccid, 'puk2', OLD.puk2, NEW.puk2, USER());
    END IF;
    IF OLD.adm1 <> NEW.adm1 THEN
        INSERT INTO log_sim_cards (table_name, record_id, column_name, old_value, new_value, user)
        VALUES ('sim_cards', OLD.iccid, 'adm1', OLD.adm1, NEW.adm1, USER());
    END IF;
END //
DELIMITER ;


-- Trigger to log INSERT operations with user information
DELIMITER //
CREATE TRIGGER sim_cards_insert_log
AFTER INSERT ON sim_cards
FOR EACH ROW
BEGIN
    INSERT INTO log_sim_cards (table_name, record_id, column_name, old_value, new_value, user)
    VALUES ('sim_cards', NEW.iccid, 'iccid', NULL, NEW.iccid, USER()),
           ('sim_cards', NEW.iccid, 'imsi', NULL, NEW.imsi, USER()),
           ('sim_cards', NEW.iccid, 'msisdn', NULL, NEW.msisdn, USER()),
           ('sim_cards', NEW.iccid, 'artworkId', NULL, NEW.artworkId, USER()),
           ('sim_cards', NEW.iccid, 'accId', NULL, NEW.accId, USER()),
           ('sim_cards', NEW.iccid, 'produced', NULL, NEW.produced, USER()),
           ('sim_cards', NEW.iccid, 'installed', NULL, NEW.installed, USER()),
           ('sim_cards', NEW.iccid, 'purged', NULL, NEW.purged, USER()),
           ('sim_cards', NEW.iccid, 'kl1', NULL, NEW.kl1, USER()),
           ('sim_cards', NEW.iccid, 'card_owner', NULL, NEW.card_owner, USER()),
           ('sim_cards', NEW.iccid, 'pin1', NULL, NEW.pin1, USER()),
           ('sim_cards', NEW.iccid, 'pin2', NULL, NEW.pin2, USER()),
           ('sim_cards', NEW.iccid, 'puk1', NULL, NEW.puk1, USER()),
           ('sim_cards', NEW.iccid, 'puk2', NULL, NEW.puk2, USER()),
           ('sim_cards', NEW.iccid, 'adm1', NULL, NEW.adm1, USER());
END //
DELIMITER ;

-- Trigger to log DELETE operations on sim_cards

DELIMITER //

CREATE TRIGGER sim_cards_delete_log
AFTER DELETE ON sim_cards
FOR EACH ROW
BEGIN
    INSERT INTO log_sim_cards (table_name, record_id, column_name, old_value, new_value, user)
    VALUES ('sim_cards', OLD.iccid, 'iccid', OLD.iccid, NULL, USER()),
           ('sim_cards', OLD.iccid, 'imsi', OLD.imsi, NULL, USER()),
           ('sim_cards', OLD.iccid, 'msisdn', OLD.msisdn, NULL, USER()),
           ('sim_cards', OLD.iccid, 'artworkId', OLD.artworkId, NULL, USER()),
           ('sim_cards', OLD.iccid, 'accId', OLD.accId, NULL, USER()),
           ('sim_cards', OLD.iccid, 'produced', OLD.produced, NULL, USER()),
           ('sim_cards', OLD.iccid, 'installed', OLD.installed, NULL, USER()),
           ('sim_cards', OLD.iccid, 'purged', OLD.purged, NULL, USER()),
           ('sim_cards', OLD.iccid, 'card_owner', OLD.card_owner, NULL, USER()),
           ('sim_cards', OLD.iccid, 'kl1', OLD.kl1, NULL, USER()),
           ('sim_cards', OLD.iccid, 'pin1', OLD.pin1, NULL, USER()),
           ('sim_cards', OLD.iccid, 'pin2', OLD.pin2, NULL, USER()),
           ('sim_cards', OLD.iccid, 'puk1', OLD.puk1, NULL, USER()),
           ('sim_cards', OLD.iccid, 'puk2', OLD.puk2, NULL, USER()),
           ('sim_cards', OLD.iccid, 'adm1', OLD.adm1, NULL, USER());
END //

DELIMITER ;
-- Triggers for acc table

DELIMITER //

-- Trigger for INSERT operations on acc
CREATE TRIGGER acc_insert_log
AFTER INSERT ON acc
FOR EACH ROW
BEGIN
    INSERT INTO log_sim_cards (table_name, record_id, column_name, old_value, new_value, user) -- Include user
    VALUES ('acc', NEW.id, 'id', NULL, NEW.id, USER()),
           ('acc', NEW.id, 'name', NULL, NEW.name, USER());
END //

-- Trigger for UPDATE operations on acc
CREATE TRIGGER acc_update_log
AFTER UPDATE ON acc
FOR EACH ROW
BEGIN
    IF OLD.name <> NEW.name THEN
        INSERT INTO log_sim_cards (table_name, record_id, column_name, old_value, new_value, user) -- Include user
        VALUES ('acc', NEW.id, 'name', OLD.name, NEW.name, USER());
    END IF;
END //

-- Trigger for DELETE operations on acc
CREATE TRIGGER acc_delete_log
AFTER DELETE ON acc
FOR EACH ROW
BEGIN
    INSERT INTO log_sim_cards (table_name, record_id, column_name, old_value, new_value, user) -- Include user
    VALUES ('acc', OLD.id, 'id', OLD.id, NULL, USER()),
           ('acc', OLD.id, 'name', OLD.name, NULL, USER());
END //

DELIMITER ;

-- Triggers for artwork table

DELIMITER //

-- Trigger for INSERT operations on artwork
CREATE TRIGGER artwork_insert_log
AFTER INSERT ON artwork
FOR EACH ROW
BEGIN
    INSERT INTO log_sim_cards (table_name, record_id, column_name, old_value, new_value, user) -- Include user
    VALUES ('artwork', NEW.id, 'id', NULL, NEW.id, USER()),
           ('artwork', NEW.id, 'name', NULL, NEW.name, USER());
END //

-- Trigger for UPDATE operations on artwork
CREATE TRIGGER artwork_update_log
AFTER UPDATE ON artwork
FOR EACH ROW
BEGIN
    IF OLD.name <> NEW.name THEN
        INSERT INTO log_sim_cards (table_name, record_id, column_name, old_value, new_value, user) -- Include user
        VALUES ('artwork', NEW.id, 'name', OLD.name, NEW.name, USER());
    END IF;
END //

-- Trigger for DELETE operations on artwork
CREATE TRIGGER artwork_delete_log
AFTER DELETE ON artwork
FOR EACH ROW
BEGIN
    INSERT INTO log_sim_cards (table_name, record_id, column_name, old_value, new_value, user) -- Include user
    VALUES ('artwork', OLD.id, 'id', OLD.id, NULL, USER()),
           ('artwork', OLD.id, 'name', OLD.name, NULL, USER());
END //

DELIMITER ;