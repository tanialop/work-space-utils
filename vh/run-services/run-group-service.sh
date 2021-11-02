#!/bin/bash

mvn clean compile package
go generate api/api.go
go generate pkg/keycloak/keycloak.go
go run cmd/service/main.go  -migrate-folder=./compose/db/db_mysql -port 9046

echo "########### Runnig group-service in the port 9046 ###########"
