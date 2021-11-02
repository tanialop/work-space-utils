#!/bin/bash

PORT=9023
echo "########### Start up certificate-service in the port $PORT ###########"

sbt clean compile "run -Dhttp.port=$PORT"

echo "########### Runnig certificate-service in the port $PORT ###########"
