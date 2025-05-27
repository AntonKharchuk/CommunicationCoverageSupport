echo "insert into simCards (iccid, imsi, msisdn, kIndId, ki, pin1, pin2, puk1, puk2, adm1, artworkId, accId, installed, cardOwnerId) VALUES" >../data_load/sim_cards_load.sql;
for _imsi in {255702000000000..255702000009999}; do
_iccid="54321${_imsi}";
#_imsi is generated
_msisdn="3807028${_imsi:10:5}"
_kIndId=8
_ki=$(head -c 16 /dev/urandom | xxd -p | tr -d '\n')
_pin1=$((RANDOM % 9999))
_pin2=$((RANDOM % 9999))
_puk1=$((RANDOM % 9999 * 1000 + RANDOM % 9999))
_puk2=$((RANDOM % 9999 * 1000 + RANDOM % 9999))
_adm1="noBodyKnowsWhatIsIt"
_artworkId=0
_accId=0
_installed=0
_cardOwnerId=0
echo "('${_iccid}', '${_imsi}', '${_msisdn}', ${_kIndId}, '${_ki}', ${_pin1}, ${_pin2}, ${_puk1}, ${_puk2}, '${_adm1}', ${_artworkId}, ${_accId}, ${_installed}, ${_cardOwnerId})," >>../data_load/sim_cards_load.sql;
done
sed -i -zE 's/(.*),/\1;\n/' ../data_load/sim_cards_load.sql