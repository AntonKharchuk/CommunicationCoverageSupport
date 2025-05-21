mariadb -h <hostIp> -u apiuser -p sdb1 --ssl=0;


-- Insert test values into the artwork table
INSERT INTO artwork (name) VALUES
('RED'),
('ORANGE'),
('YELLOW'),
('GREEN'),
('BLUE'),
('INDIGO'),
('VIOLET');
INSERT INTO artwork (id,name) VALUES (0, 'NOT_USED');

-- Insert test values into the acc table
INSERT INTO acc (name) VALUES
('SNAIL'),
('CHEETAH');
INSERT INTO acc (id,name) VALUES (0, 'NOT_USED');

-- Insert test values into the owners table
INSERT INTO owners (id, name) VALUES
(0, 'NOBODY');

-- Insert test values into the transportKey table
INSERT INTO transportKey (id, kInd) VALUES
(8, 'CONCRETE');

SELECT GROUP_CONCAT(column_name) FROM information_schema.columns WHERE table_name = 'simCards';
SELECT GROUP_CONCAT(column_name) FROM information_schema.columns WHERE table_name = 'simCardsDrain';

#TMP
(iccid, imsi, msisdn, kIndId, ki1, pin1, pin2, puk1, puk2, adm1, artworkId, accId, installed, cardOwnerId)
update simCards set installed = 1 where imsi REGEXP '1[0,1,2,3,4][0-9]$';
