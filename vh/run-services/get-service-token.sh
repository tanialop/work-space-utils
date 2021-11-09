#!/bin/bash
# get-service-token.sh <environment> <realm>
# Default value for <environment> <realm> will be "local" and "veea"
# get-service-token.sh
# get-service-token.sh local
# get-service-token.sh qa veea
ENV=$1
REALM=$2
source "$VH_CURRENT_SCRIPT_PATH/export-service-token.sh" "$ENV" "$REALM"

echo "$JWT_TOKEN"


