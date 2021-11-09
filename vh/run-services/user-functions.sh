#!/bin/bash
# user-functions.sh <environment> <realm>
# Default values are: "local" and "veea"
# Example:
# user-functions.sh local veea

source "$VH_CURRENT_SCRIPT_PATH/export-environment.sh" "$ENV" "$REALM"

# Example: how to call this function
# get_user_token_by_credential support@veea.com 'support123!'
get_user_token_by_credential() {
  local SUPPORT_USER_NAME=$1
  local SUPPORT_PASSWORD=$2

  local KEYCLOAK_URI_GET_TOKEN="$KEYCLOAK_HOST/auth/realms/$REALM/protocol/openid-connect/token"
  echo -e "Getting token from $KEYCLOAK_URI_GET_TOKEN for user $SUPPORT_USER_NAME"

  CLIENT_ID='veeahub-cli'
  response=$(
    curl --silent \
    --location --request POST "$KEYCLOAK_URI_GET_TOKEN" \
    --header 'Content-Type: application/x-www-form-urlencoded' \
    --data-urlencode "client_id=$CLIENT_ID" \
    --data-urlencode "grant_type=password" \
    --data-urlencode "username=$SUPPORT_USER_NAME" \
    --data-urlencode "password=$SUPPORT_PASSWORD")

  access_token=$(echo "$response" | jq -r '.access_token')
}

# Example: how to call this function
# get_impersonated_token support@veea.com 'support123!' 'automated-test-qa@veeasystems.com'
get_impersonated_token() {
  local SUPPORT_USER_NAME=$1
  local SUPPORT_PASSWORD=$2
  local IMPERSONATE_USER_NAME=$3

  echo "Impersonating the user $IMPERSONATE_USER_NAME with $SUPPORT_USER_NAME"

  # After calling this function we can take the value of access_token variable
  get_user_token_by_credential "$SUPPORT_USER_NAME" "$SUPPORT_PASSWORD"

  KEYCLOAK_URI_GET_USER="$KEYCLOAK_HOST/auth/admin/realms/$REALM/users"
  echo -e "Retrieving id from $KEYCLOAK_URI_GET_USER for user: $IMPERSONATE_USER_NAME"
  local response=$(
  	curl -G --silent \
  	--location --request GET "$KEYCLOAK_URI_GET_USER" \
  	--header 'Content-Type: application/x-www-form-urlencoded' \
  	--header "Authorization: Bearer $access_token" \
  	--data-urlencode "email=$IMPERSONATE_USER_NAME")

  impersonated_user_id=$(echo "$response" | jq -r '.[0].id')

  local KEYCLOAK_URI_GET_TOKEN="$KEYCLOAK_HOST/auth/realms/$REALM/protocol/openid-connect/token"
  echo -e "Getting token from $KEYCLOAK_URI_GET_TOKEN for user $IMPERSONATE_USER_NAME"
  local impersonated_response=$(
  	curl --silent \
  	--location --request POST "$KEYCLOAK_URI_GET_TOKEN" \
  	--header 'Content-Type: application/x-www-form-urlencoded' \
  	--header 'Accept: application/json' \
  	--data-urlencode "client_id=$CLIENT_ID" \
  	--data-urlencode "grant_type=urn:ietf:params:oauth:grant-type:token-exchange" \
  	--data-urlencode "requested_subject=$impersonated_user_id" \
  	--data-urlencode "subject_token=$access_token")

  # the parameter -r remove double quotes from token.
  impersonated_token=$(echo "$impersonated_response" | jq -r '.access_token')
}