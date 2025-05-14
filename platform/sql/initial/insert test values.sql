-- Insert test values into the artwork table
INSERT INTO artwork (name) VALUES
('Artwork 1'),
('Artwork 2'),
('Artwork 3');

-- Insert test values into the acc table
INSERT INTO acc (name) VALUES
('acc 1'),
('acc 2'),
('acc 3');

insert into sim_cards (iccid, imsi, msisdn, artworkId, accId, produced, installed, purged, kl1, card_owner, pin1, pin2, puk1, puk2, adm1)
VALUES ('89014103279000000001', 123456789012345, 1234567890, 1, 1, TRUE, FALSE, FALSE, 'test_kl1', 'test owner', 1234, 5678, 12345678, 87654321, 9876543210);
