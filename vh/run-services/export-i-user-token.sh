#!/bin/bash
# export-i-user-token.sh <user_name> <user_password> <impersonated_user_name> <environment> <realm>
# export-i-user-token.sh support@veea.com 'support123!' 'automated-test-qa@veeasystems.com'
# export-i-user-token.sh support@veea.com 'support123!' 'automated-test-qa@veeasystems.com' qa veea

SUPPORT_USER_NAME=$1
SUPPORT_PASSWORD=$2
IMPERSONATED_USER_NAME=$3
ENV=$4
REALM=$5

source "$VH_CURRENT_SCRIPT_PATH/user-functions.sh" "$ENV" "$REALM"

# calling a function.
get_impersonated_token "$SUPPORT_USER_NAME" "$SUPPORT_PASSWORD" "$IMPERSONATED_USER_NAME"

export JWT_TOKEN="$impersonated_token"
echo "Token was exported into JWT_TOKEN."
#echo "$JWT_TOKEN"
