#!/bin/bash

source $VH_CURRENT_SCRIPT_PATH/export-viewer-certificate.sh

PORT=9013
echo "########### Start up acs-service in the port $PORT ###########"

sbt clean compile "run -Dhttp.port=$PORT -DbootstrapServer.viewer.certificate.serialNumber=${VIEWER_CERTIFICATE_SERIAL_NUMBER}"

echo "########### Runnig acs-service in the port $PORT ###########"
