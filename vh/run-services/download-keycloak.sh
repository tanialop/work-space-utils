#!/bin/bash
# The following keys should have values in files: ~/.aws/credentials or cat ~/.aws/config
# aws_access_key_id
# aws_secret_access_key
# region
# cat ~/.aws/config
# cat ~/.aws/credentials

echo -e "Download keycloak image from aws."
export AWS_DEFAULT_REGION="us-east-2"
docker login -u AWS -p $(aws ecr get-login-password) https://$(aws sts get-caller-identity --query 'Account' --output text).dkr.ecr.${AWS_DEFAULT_REGION}.amazonaws.com
docker pull 065145189516.dkr.ecr.us-east-2.amazonaws.com/keycloak-service:1.1.121-8d8d0e7