#!/bin/bash
# get-user-token.sh <user_name> <user_password> <environment> <realm>
# get-user-token.sh support@veea.com 'support123!'
# get-user-token.sh support@veea.com 'support123!' qa veea

USER_NAME=$1
PASSWORD=$2
ENV=$3
REALM=$4

source "$VH_CURRENT_SCRIPT_PATH/user-functions.sh" "$ENV" "$REALM"

# calling a function.
get_user_token_by_credential "$USER_NAME" "$PASSWORD"
echo ""
echo "$access_token"

