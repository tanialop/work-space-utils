#!/bin/bash
# ./els-load-resources.sh "/home/ronald/backups/11-07-2021"
source "$VH_CURRENT_SCRIPT_PATH/els-functions.sh"

path_file=$1

echo "Starting to load resource documents from $path_file."

load_els_resources "$path_file"

echo "Resource documents were loaded."