echo "insert into sim_cards (iccid,imsi,artworkId,accId,produced,installed,purged,ki1,card_owner,pin1,pin2,puk1,puk2,adm1) VALUES" >../data_load/sim_cards_load.sql;
for _imsi in {255702000000000..255702000001000}; do
echo "('89014${_imsi}', ${_imsi}, 1, 1, TRUE, FALSE, FALSE, 'test_kl1', 'test owner', 1234, 5678, 12345678, 87654321, 9876543210)," >>../data_load/sim_cards_load.sql;
done
