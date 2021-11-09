#!/bin/bash
# ./els-load-resources.sh "/home/ronald/backups/11-07-2021"
source "$VH_CURRENT_SCRIPT_PATH/els-functions.sh"

path_file=$1

echo "Starting to load user package preference documents from $path_file."

load_els_user_package_preferences "$path_file"

echo "User package preference documents were loaded."