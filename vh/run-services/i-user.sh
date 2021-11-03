#!/bin/bash
# i-user.sh <environment> <user_name> <user_password> <impersonated_user_name> <realm>
# i-user.sh qa support@veea.com 'support123!' 'automated-test-qa@veeasystems.com'
# i-user.sh qa support@veea.com 'support123!' 'automated-test-qa@veeasystems.com' veea

ENV=$1
SUPPORT_USER_NAME=$2
SUPPORT_PASSWORD=$3
IMPERSONATED_USER_NAME=$4
REALM=$5

source "$VH_CURRENT_SCRIPT_PATH/user-functions.sh" "$ENV" "$REALM"

# calling a function.
get_impersonated_token "$SUPPORT_USER_NAME" "$SUPPORT_PASSWORD" "$IMPERSONATED_USER_NAME"
echo ""
echo "$impersonated_token"

