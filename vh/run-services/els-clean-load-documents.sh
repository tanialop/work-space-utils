#!/bin/bash
if [ $# -lt 1 ]; then
  echo "It require as parameter the path where .json are located."
  echo "Example:"
  echo "./els-clean-load-documents.sh  /home/ronald/backups/11-07-2021"
  exit 255
fi

path_of_documents=$1
echo "####################### Working on User Package Preference documents #######################"
source "$VH_CURRENT_SCRIPT_PATH/els-delete-user-preferences.sh" "$path_of_documents"
source "$VH_CURRENT_SCRIPT_PATH/els-load-user-preferences.sh" "$path_of_documents"
echo ""

echo "####################### Working on Resource documents #######################"
source "$VH_CURRENT_SCRIPT_PATH/els-delete-resources.sh" "$path_of_documents"
source "$VH_CURRENT_SCRIPT_PATH/els-load-resources.sh" "$path_of_documents"
echo ""

echo "####################### Working on Configuration documents #######################"
source "$VH_CURRENT_SCRIPT_PATH/els-delete-configurations.sh" "$path_of_documents"
source "$VH_CURRENT_SCRIPT_PATH/els-load-configurations.sh" "$path_of_documents"
echo ""