#!/bin/bash
while IFS= read -r line; do echo ">>Start Delete for: $line<<";
  new_line="'$line'";
mysql -uroot -h 127.0.0.1 -P 3306 -pcontrol123! <<MY_QUERY
SET collation_connection = 'utf8mb4_unicode_ci';
SET @veea_hub_serial_number = ${new_line};
use bootstrap_db;
START TRANSACTION;
DELETE from bootstrap_db.certificate where id in (
    select cb.serial_number FROM bootstrap_db.certificate_veea_hub cb WHERE cb.veea_hub_serial_number = @veea_hub_serial_number and (filename = 'device.pem' or filename = 'minimal.pem'));
DELETE from certificate_db.certificate where serial_number in (
    select cb.serial_number FROM bootstrap_db.certificate_veea_hub cb WHERE cb.veea_hub_serial_number = @veea_hub_serial_number and (filename = 'device.pem' or filename = 'minimal.pem'));
DELETE FROM bootstrap_db.certificate_veea_hub WHERE veea_hub_serial_number = @veea_hub_serial_number;
DELETE mac FROM bootstrap_db.mac_address AS mac WHERE mac.module_id IN (
SELECT id FROM bootstrap_db.module WHERE veea_hub_serial_number = @veea_hub_serial_number
);

DELETE FROM bootstrap_db.sim_profile sp WHERE sp.sim_id = (
  SELECT id AS sim_id FROM bootstrap_db.sim s WHERE s.module_id = (SELECT id FROM bootstrap_db.module m WHERE m.type = 'wwan' AND m.veea_hub_serial_number=@veea_hub_serial_number)
);
DELETE FROM bootstrap_db.sim s WHERE s.module_id = (SELECT id FROM bootstrap_db.module m WHERE m.type = 'wwan' AND m.veea_hub_serial_number=@veea_hub_serial_number);

DELETE FROM bootstrap_db.module WHERE veea_hub_serial_number = @veea_hub_serial_number;
DELETE FROM bootstrap_db.veea_hub_configuration_data WHERE veea_hub_serial_number = @veea_hub_serial_number;
DELETE FROM bootstrap_db.certificate_veea_hub WHERE veea_hub_serial_number = @veea_hub_serial_number;
DELETE FROM bootstrap_db.veea_hub_log WHERE veea_hub_serial_number = @veea_hub_serial_number;
DELETE FROM bootstrap_db.veea_hub WHERE serial_number = @veea_hub_serial_number;
DELETE FROM bootstrap_db.security_identifier WHERE id = @veea_hub_serial_number;
COMMIT;
MY_QUERY
  
  echo ">>Complete Delete for: $line<<"
done < "$VH_CURRENT_SCRIPT_PATH/serials.txt"