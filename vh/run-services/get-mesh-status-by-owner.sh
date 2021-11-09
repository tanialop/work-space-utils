#!/bin/bash

ENV=$1
REALM=$2
source "$VH_CURRENT_SCRIPT_PATH/functions.sh" "$ENV" "$REALM"

get_service_token
export JWT_TOKEN="$keycloak_service_token"

URL="$ENROLLMENT_SERVICE_HOT/enroll/owner/2a4cd523-75a2-40c9-a0c0-0de1f9715198/config"

curl --location --request GET "$URL" \
--header 'Content-Type: application/json' \
--header "Authorization: Bearer $JWT_TOKEN" \
--data-raw ''