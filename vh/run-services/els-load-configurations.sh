#!/bin/bash
# ./els-load-configurations.sh "/home/ronald/backups/11-07-2021"

source "$VH_CURRENT_SCRIPT_PATH/els-functions.sh"

path_file=$1

echo "Starting to load configurations documents from $path_file."

load_els_configurations "$path_file"

echo "Configuration documents were loaded."