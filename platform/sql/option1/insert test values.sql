mariadb -h <hostIp> -u apiuser -p sdb1 --ssl=0

-- Insert test values into the artwork table
INSERT INTO artwork (id,name) VALUES (0, 'NOT_USED');
INSERT INTO artwork (name) VALUES
('NOT_USED'),
('Artwork 2'),
('Artwork 3');

-- Insert test values into the acc table
INSERT INTO acc (id,name) VALUES (0, 'NOT_USED');
INSERT INTO acc (name) VALUES
('random'),
('random 15');

-- Insert test values into the acc table
INSERT INTO owners (id, name) VALUES
(0, 'DEFAULT');

SELECT GROUP_CONCAT(column_name) FROM information_schema.columns WHERE table_name = 'sim_cards';

iccid,imsi,artworkId,accId,installed,purged,ki1,pin1,pin2,puk1,puk2,adm1
for _imsi in {255702000000000..255702000001000}; do
echo "('89014${_imsi}', '${_imsi}', 1, 1, 0, 0, 'test_ki1', 1234, 5678, 12345678, 87654321, 9876543210)," >>./data_load/sim_cards_load.sql;
done

insert into sim_cards (iccid,imsi,artworkId,accId,produced,installed,purged,ki1,card_owner,pin1,pin2,puk1,puk2,adm1) VALUES
('89014103279000000001', 255702000000001, 1, 1, TRUE, FALSE, FALSE, 'test_kl1', 'test owner', 1234, 5678, 12345678, 87654321, 9876543210),
('89014103279000000002', 255702000000002, 1, 1, TRUE, FALSE, FALSE, 'test_kl1', 'test owner', 1234, 5678, 12345678, 87654321, 9876543210),
('89014103279000000003', 255702000000003, 1, 1, TRUE, FALSE, FALSE, 'test_kl1', 'test owner', 1234, 5678, 12345678, 87654321, 9876543210),
('89014103279000000004', 255702000000004, 1, 1, TRUE, FALSE, FALSE, 'test_kl1', 'test owner', 1234, 5678, 12345678, 87654321, 9876543210);

update sim_cards set installed = 1 where imsi REGEXP '11[0-9]$';

SELECT GROUP_CONCAT(column_name) FROM information_schema.columns WHERE table_name = 'msisdn';
INSERT INTO msisdn (msisdn,prop1,prop2) VALUES
(3807911111110,0,1),
(3807922222220,0,1),
(3807933333330,0,1),
(3807944444440,0,1);

SELECT GROUP_CONCAT(column_name) FROM information_schema.columns WHERE table_name = 'subscriber';
INSERT INTO subscriber (msisdn,imsi) VALUES
(3807911111111,255702000000009);
