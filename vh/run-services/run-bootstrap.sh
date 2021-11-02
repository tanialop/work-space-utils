#!/bin/bash

PORT=9020
echo "########### Start up bootstrap-service in the port $PORT ###########"

sbt clean compile "run -Dhttp.port=$PORT"

echo "########### Runnig bootstrap-service in the port $PORT ###########"
