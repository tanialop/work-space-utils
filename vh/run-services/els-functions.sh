#!/bin/bash

create_document() {
  local index=$1
  local type=$2
  local input_data=$3

  local items=$(echo "$input_data" | jq -r '.hits.hits')

  for row in $(echo "${items}" | jq -r '.[] | @base64'); do
    item=$(echo "${row}" | base64 --decode | jq "._source")
    id=$(echo "${item}" | jq -r ".id")
    body="$item"

    local response=$(curl --silent --location --request POST "http://localhost:9200/$index/$type/$id" \
                         --header 'Content-Type: application/json' \
                         --data-raw "$body")

    created=$(echo "${response}" | jq -r ".created")
    echo "Creating the document with $id in the index $index. created: $created"
  done
}

load_els_resources() {
  path_file=$1
  index="resources"
  type="resource"
  full_json_file_path="$path_file/$index.json"
  input_data=$(cat "$full_json_file_path")
  create_document "$index" "$type" "$input_data"
}

load_els_configurations() {
  path_file=$1
  index="configurations"
  type="configuration"
  full_json_file_path="$path_file/$index.json"
  input_data=$(cat "$full_json_file_path")
  create_document "$index" "$type" "$input_data"
}

load_els_user_package_preferences() {
  path_file=$1
  index="user_base_package_preferences"
  type="user_base_package_preference"
  full_json_file_path="$path_file/$index.json"
  input_data=$(cat "$full_json_file_path")
  create_document "$index" "$type" "$input_data"
}

get_els_document_ids() {
  index=$1

  local response=$(curl --silent --location --request GET "http://localhost:9200/$index/_search" \
                     --header 'Content-Type: application/json' \
                     --data-raw '{
                       "size": 10000,
                       "fields": ["id"]
                     }')


    all_document_ids=()
    local items=$(echo "$response" | jq -r '.hits.hits')
    for row in $(echo "${items}" | jq -r '.[] | @base64'); do
      item=$(echo "${row}" | base64 --decode)
      id=$(echo "${item}" | jq -r ".fields.id[0]")
      all_document_ids+=("$id")
    done
}

get_els_documents_field_ids() {
  index=$1

  local response=$(curl --silent --location --request GET "http://localhost:9200/$index/_search" \
                     --header 'Content-Type: application/json' \
                     --data-raw '{
                       "size": 10000,
                       "fields": ["id"]
                     }')

    document_fields_ids=$(echo "$response" | jq -r '.hits.hits')
}

get_all_els_resources() {
  response=$(curl --silent --location --request GET 'http://localhost:9200/resources/_search' \
                   --header 'Content-Type: application/json' \
                   --data-raw '{
                     "size": 10000,
                     "fields": ["id"]
                   }')

  resource_items=$(echo "$response" | jq '.hits.hits')
}

delete_els_document_by_id() {
  local index=$1
  local type=$2
  local id=$3

  local response=$(curl --silent --location --request DELETE "http://localhost:9200/$index/$type/$id" \
                     --header 'Content-Type: application/json')
  deleted=$(echo "${response}" | jq -r ".found")
}

delete_els_documents_by_field_id() {
  local index=$1
  local type=$2

  echo "Retrieving $type ids from $index to delete them."

  get_els_documents_field_ids "$index"

  echo "Starting to delete the documents from index: $index."

  items="$document_fields_ids"
  for row in $(echo "${items}" | jq -r '.[] | @base64'); do
      item=$(echo "${row}" | base64 --decode)
      id=$(echo "${item}" | jq -r ".fields.id[0]")

      delete_els_document_by_id "$index" "$type" "$id"

      echo "Deleting $type with id $id from index $index. deleted: $deleted"
    done
}

get_all_els_user_base_package_preference_ids() {
  get_els_document_ids "user_base_package_preferences"
}

get_all_els_resource_ids() {
  get_els_document_ids "resources"
}

get_all_els_configurations_ids() {
  get_els_document_ids "configurations"
}

delete_els_configuration_by_id() {
  delete_els_documents_by_field_id "configurations" "configuration"
}

delete_els_resource_by_id() {
  delete_els_documents_by_field_id "resources" "resource"
}

delete_els_user_base_package_preference_by_id() {
  delete_els_documents_by_field_id "user_base_package_preferences" "user_base_package_preference"
}

#load_els_resources "/home/ronald/backups/11-07-2021"
