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
    ELSE rows_found = 1 THEN
        INSERT INTO simCardsDrain (iccid, imsi, msisdn, kIndId, ki1, pin1, pin2, puk1, puk2, adm1, artworkId, accId, installed, cardOwnerId)
        SELECT
            iccid, imsi, msisdn, kIndId, ki1, pin1, pin2, puk1, puk2, adm1, artworkId, accId, installed, cardOwnerId
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
            WHERE iccid = _iccid
                AND imsi = _imsi AND msisdn = _msisdn AND kIndId = _kIndId;

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
