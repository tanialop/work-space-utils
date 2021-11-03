#!/bin/bash
# get-user-token.sh <environment> <user_name> <user_password> <realm>
# get-user-token.sh qa support@veea.com 'support123!'
# get-user-token.sh qa support@veea.com 'support123!' veea

ENV=$1
USER_NAME=$2
PASSWORD=$3
REALM=$4

source "$VH_CURRENT_SCRIPT_PATH/user-functions.sh" "$ENV" "$REALM"

# calling a function.
get_user_token_by_credential "$USER_NAME" "$PASSWORD"
echo ""
echo "$access_token"

