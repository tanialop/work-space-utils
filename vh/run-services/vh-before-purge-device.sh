#!/bin/bash
while IFS= read -r line; do echo ">>Number of records to be deleted for: $line<<";
  new_line="'$line'";
mysql -uroot -h 127.0.0.1 -P 3306 -pcontrol123! <<MY_QUERY
SET collation_connection = 'utf8mb4_unicode_ci';
SET @veea_hub_serial_number = ${new_line};
use bootstrap_db;
select count(*) bootstrap_db_certificate from bootstrap_db.certificate where id in (
    select cb.serial_number FROM bootstrap_db.certificate_veea_hub cb WHERE cb.veea_hub_serial_number = @veea_hub_serial_number and (filename = 'device.pem' or filename = 'minimal.pem'));
select count(*) certificate_db_certificate from certificate_db.certificate c where c.serial_number in (
    select cb.serial_number FROM bootstrap_db.certificate_veea_hub cb WHERE cb.veea_hub_serial_number = @veea_hub_serial_number and (filename = 'device.pem' or filename = 'minimal.pem'));
select * from certificate_db.certificate c where c.serial_number in (
    select cb.serial_number FROM bootstrap_db.certificate_veea_hub cb WHERE cb.veea_hub_serial_number = @veea_hub_serial_number and (filename = 'device.pem' or filename = 'minimal.pem'));
select count(*) certificate_veea_hub FROM bootstrap_db.certificate_veea_hub WHERE veea_hub_serial_number = @veea_hub_serial_number;
select count(*) mac_address FROM bootstrap_db.mac_address AS mac WHERE mac.module_id IN (
SELECT id FROM bootstrap_db.module WHERE veea_hub_serial_number = @veea_hub_serial_number
);
select count(*) bootstrap_db_module FROM bootstrap_db.module WHERE veea_hub_serial_number = @veea_hub_serial_number;

select count(*) bootstrap_sim FROM bootstrap_db.sim s WHERE s.module_id  = (SELECT id FROM bootstrap_db.module m WHERE m.type = 'wwan' AND m.veea_hub_serial_number = @veea_hub_serial_number);
select count(*) bootstrap_sim_profile FROM bootstrap_db.sim_profile sp WHERE sp.sim_id = (
    select id AS sim_id FROM bootstrap_db.sim s WHERE s.module_id = (SELECT id FROM bootstrap_db.module m WHERE m.type = 'wwan' AND m.veea_hub_serial_number=@veea_hub_serial_number)
);

select count(*) veea_hub_configuration_data FROM bootstrap_db.veea_hub_configuration_data WHERE veea_hub_serial_number = @veea_hub_serial_number;
select count(*) certificate_veea_hub FROM bootstrap_db.certificate_veea_hub WHERE veea_hub_serial_number = @veea_hub_serial_number;
select count(*) veea_hub_log FROM bootstrap_db.veea_hub_log WHERE veea_hub_serial_number = @veea_hub_serial_number;
select count(*) veea_hub FROM bootstrap_db.veea_hub WHERE serial_number = @veea_hub_serial_number;
select count(*) security_identifier FROM bootstrap_db.security_identifier WHERE id = @veea_hub_serial_number;
MY_QUERY
  
  echo "SQL Complete execution for: $line"
done < "$VH_CURRENT_SCRIPT_PATH/serials.txt"