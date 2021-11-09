#!/bin/bash
# user-functions.sh <environment> <realm>
# Default values are: "local" and "veea"
# Example:
# functions.sh local veea

source "$VH_CURRENT_SCRIPT_PATH/export-environment.sh" "$ENV" "$REALM"

# Example: how to call this function
# get_service_token local veea
get_service_token() {
#  local ENV=$1
#  local REALM=$2
  KEYCLOAK_URI="$KEYCLOAK_HOST/auth/realms/$REALM/protocol/openid-connect/token"
  echo -e "Getting service-token from $KEYCLOAK_URI \n"
  local response=$(
  	curl --silent \
  	--location --request POST "$KEYCLOAK_URI" \
  	--header 'Content-Type: application/x-www-form-urlencoded' \
  	--data-urlencode "client_id=$CLIENT_ID" \
  	--data-urlencode "client_secret=$CLIENT_SECRET" \
  	--data-urlencode "grant_type=$CLIENT_TYPE")

  keycloak_service_token=$(echo "$response" | jq -r '.access_token')
}

#get_service_token local veea
#echo "$keycloak_service_token"