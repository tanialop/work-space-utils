#!/bin/bash

source "$VH_CURRENT_SCRIPT_PATH/export-mysql-credential.sh"

DATABASE_NAME="bootstrap_db"

query_result=$({
	echo "USE ${DATABASE_NAME};"
	echo "SELECT serial_number FROM viewer v WHERE v.active = true;"			

} | mysql -h $MYSQL_HOST -P $MYSQL_PORT -u$MYSQL_USER -p$MYSQL_PASSWORD --ssl-mode=DISABLED)

# column_name=$(echo $query_result | awk '{print $1}')
viewer_serial_number=$(echo $query_result | awk '{print $2}')


export VIEWER_CERTIFICATE_SERIAL_NUMBER=$viewer_serial_number

echo "The viewer certificate serial number $VIEWER_CERTIFICATE_SERIAL_NUMBER was exported."

