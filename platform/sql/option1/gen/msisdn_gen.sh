echo "INSERT INTO msisdn (msisdn,prop2) VALUES" >../data_load/msisdn_load.sql;
for _msisdn in {3807911111110..3807911112110}; do
echo "('${_msisdn}', 0)," >>../data_load/msisdn_load.sql;
done
