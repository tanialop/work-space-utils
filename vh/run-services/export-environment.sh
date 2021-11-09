#!/bin/bash

ENV=$1
REALM=$2
if [ -z "$REALM" ]; then
  REALM="veea"
fi

KEYCLOAK_HOST="http://localhost:8080"
BOOTSTRAP_SERVICE_HOST="http://localhost:9020"
ENROLLMENT_SERVICE_HOT="http://localhost:9022"
ACTIVATION_SERVICE_HOST="http://localhost:9013"
CERTIFICATE_SERVICE_HOST="http://localhost:9023"
GROUP_SERVICE_HOST="http://localhost:9046/api/v2"
case $ENV in
  local)
    KEYCLOAK_HOST='http://localhost:8080'
    CLIENT_ID='veeahub-services'
    CLIENT_SECRET='kindred-headlock-registrar-hamlet'
    CLIENT_TYPE='client_credentials'

    BOOTSTRAP_SERVICE_HOST="http://localhost:9020"
    ENROLLMENT_SERVICE_HOT="http://localhost:9022"
    ACTIVATION_SERVICE_HOST="http://localhost:9013"
    CERTIFICATE_SERVICE_HOST="http://localhost:9023"
    GROUP_SERVICE_HOST="http://localhost:9046/api/v2"
    ;;
  dev)
    KEYCLOAK_HOST='https://auth.dev.veeaplatform.net'
    CLIENT_ID='veeahub-services'
    CLIENT_SECRET='stomp-justify-unlucky-curse'
    CLIENT_TYPE='client_credentials'

    BOOTSTRAP_SERVICE_HOST="http://localhost:9020"
    ENROLLMENT_SERVICE_HOT="https://dev.veeaplatform.net/enrollment"
    ACTIVATION_SERVICE_HOST="https://dev.veeaplatform.net/ac"
    CERTIFICATE_SERVICE_HOST="http://localhost:9023"
    GROUP_SERVICE_HOST="http://localhost:9046/api/v2"
    ;;
  qa)
    KEYCLOAK_HOST='https://qa-auth.veea.co'
    CLIENT_ID='veeahub-services'
    CLIENT_SECRET='kindred-headlock-registrar-hamlet'
    CLIENT_TYPE='client_credentials'

    BOOTSTRAP_SERVICE_HOST="http://localhost:9020"
    ENROLLMENT_SERVICE_HOT="https://qa.veea.co/enrollment"
    ACTIVATION_SERVICE_HOST="https://qa.veea.co/ac"
    CERTIFICATE_SERVICE_HOST="http://localhost:9023"
    GROUP_SERVICE_HOST="https://qa.veea.co/groupservice/api/v2"
    ;;
  prod)
    KEYCLOAK_HOST='https://auth.veea.co'
    CLIENT_ID='veeahub-services'
    CLIENT_SECRET='stomp-justify-unlucky-curse'
    CLIENT_TYPE='client_credentials'

    BOOTSTRAP_SERVICE_HOST="http://localhost:9020"
    ENROLLMENT_SERVICE_HOT="https://dweb.veea.co/enrollment"
    ACTIVATION_SERVICE_HOST="https://dweb.veea.co/ac"
    CERTIFICATE_SERVICE_HOST="http://localhost:9023"
    GROUP_SERVICE_HOST="http://localhost:9046/api/v2"
    ;;
  *)
    KEYCLOAK_HOST='http://localhost:8080'
    CLIENT_ID='veeahub-services'
    CLIENT_SECRET='kindred-headlock-registrar-hamlet'
    CLIENT_TYPE='client_credentials'

    BOOTSTRAP_SERVICE_HOST="http://localhost:9020"
    ENROLLMENT_SERVICE_HOT="http://localhost:9022"
    ACTIVATION_SERVICE_HOST="http://localhost:9013"
    CERTIFICATE_SERVICE_HOST="http://localhost:9023"
    GROUP_SERVICE_HOST="http://localhost:9046/api/v2"
    ;;
esac

export KEYCLOAK_HOST CLIENT_ID CLIENT_SECRET CLIENT_TYPE
export BOOTSTRAP_SERVICE_HOST ENROLLMENT_SERVICE_HOT ACTIVATION_SERVICE_HOST CERTIFICATE_SERVICE_HOST GROUP_SERVICE_HOST
