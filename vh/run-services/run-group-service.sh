#!/bin/bash

PORT=9046
echo "########### Start up group-service in the port $PORT ###########"

mvn clean compile package
go generate api/api.go
go generate pkg/keycloak/keycloak.go
go run cmd/service/main.go  -migrate-folder=./compose/db/db_mysql -port "$PORT"

echo "########### Runnig group-service in the port 9046 ###########"
