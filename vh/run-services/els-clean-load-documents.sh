#!/bin/bash
# ./els-clean-load-documents.sh "/home/ronald/backups/11-07-2021"
path_of_documents=$1

source "$VH_CURRENT_SCRIPT_PATH/els-delete-user-preferences.sh" "$path_of_documents"
source "$VH_CURRENT_SCRIPT_PATH/els-load-user-preferences.sh" "$path_of_documents"

source "$VH_CURRENT_SCRIPT_PATH/els-delete-resources.sh" "$path_of_documents"
source "$VH_CURRENT_SCRIPT_PATH/els-load-resources.sh" "$path_of_documents"

source "$VH_CURRENT_SCRIPT_PATH/els-delete-configurations.sh" "$path_of_documents"
source "$VH_CURRENT_SCRIPT_PATH/els-load-configurations.sh" "$path_of_documents"