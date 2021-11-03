#!/bin/bash
# get-service-token.sh <environment> <realm>
# Default value for <environment> <realm> will be "local" and "veea"
# get-service-token.sh
# get-service-token.sh local
# get-service-token.sh qa veea
ENV=$1
REALM=$2
source "$VH_CURRENT_SCRIPT_PATH/export-keycloak-host.sh" "$ENV" "$REALM"

KEYCLOAK_URI="$KEYCLOAK_HOST/auth/realms/$REALM/protocol/openid-connect/token"
echo -e "Getting service-token from $KEYCLOAK_URI \n"
response=$(
	curl --silent \
	--location --request POST "$KEYCLOAK_URI" \
	--header 'Content-Type: application/x-www-form-urlencoded' \
	--data-urlencode "client_id=$CLIENT_ID" \
	--data-urlencode "client_secret=$CLIENT_SECRET" \
	--data-urlencode "grant_type=$CLIENT_TYPE")

#echo "$response" | jq -r '.access_token'
echo "$response" | jq '.access_token'

