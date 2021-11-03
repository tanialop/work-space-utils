#!/bin/bash

ENV=$1
REALM=$2
if [ -z "$REALM" ]; then
  REALM="veea"
fi

KEYCLOAK_HOST="http://localhost:8080"
case $ENV in
  local)
    KEYCLOAK_HOST='http://localhost:8080'
    CLIENT_ID='veeahub-services'
    CLIENT_SECRET='kindred-headlock-registrar-hamlet'
    CLIENT_TYPE='client_credentials'
    ;;
  dev)
    KEYCLOAK_HOST='https://auth.dev.veeaplatform.net'
    CLIENT_ID='veeahub-services'
    CLIENT_SECRET='stomp-justify-unlucky-curse'
    CLIENT_TYPE='client_credentials'
    ;;
  qa)
    KEYCLOAK_HOST='https://qa-auth.veea.co'
    CLIENT_ID='veeahub-services'
    CLIENT_SECRET='kindred-headlock-registrar-hamlet'
    CLIENT_TYPE='client_credentials'
    ;;
  prod)
    KEYCLOAK_HOST='https://auth.veea.co'
    CLIENT_ID='veeahub-services'
    CLIENT_SECRET='stomp-justify-unlucky-curse'
    CLIENT_TYPE='client_credentials'
    ;;
  *)
    KEYCLOAK_HOST='http://localhost:8080'
    CLIENT_ID='veeahub-services'
    CLIENT_SECRET='kindred-headlock-registrar-hamlet'
    CLIENT_TYPE='client_credentials'
    ;;
esac

export KEYCLOAK_HOST CLIENT_ID CLIENT_SECRET CLIENT_TYPE
