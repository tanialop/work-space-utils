#!/bin/bash

ENV=$1
REALM=$2
source "$VH_CURRENT_SCRIPT_PATH/functions.sh" "$ENV" "$REALM"

get_service_token
export JWT_TOKEN="$keycloak_service_token"