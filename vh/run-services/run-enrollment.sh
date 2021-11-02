#!/bin/bash


source $VH_CURRENT_SCRIPT_PATH/export-viewer-certificate.sh

PORT=9022
echo "########### Start up certificate-service in the port $PORT ###########"

sbt clean compile "run -Dhttp.port=$PORT -DbootstrapService.viewer.serialNumber=${VIEWER_CERTIFICATE_SERIAL_NUMBER}"

echo "########### Runnig enrollment-service in the port $PORT ###########"
