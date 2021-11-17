#!/bin/bash

create_document() {
  local index=$1
  local type=$2
  local input_data=$3

  local items=$(echo "$input_data" | jq -r '.hits.hits')

  for row in $(echo "${items}" | jq -r '.[] | @base64'); do
    item=$(echo "${row}" | base64 --decode | jq -c "._source")
#    id=$(echo "${item}" | jq -r ".id")
#    body="$item"
#    echo "$item"
    local bulk_file="/tmp/.bulk_data"
    echo "$item" >> "$bulk_file"
#    local response=$(curl --silent --location --request POST "http://localhost:9200/$index/$type/$id" \
#                         --header 'Content-Type: application/json' \
#                         --data-raw "$body")
#
#    created=$(echo "${response}" | jq -r ".created")
#    echo "Creating the document with $id in the index $index. created: $created"
  done
}

bulk_create_documents() {
  local index=$1
  local type=$2
  local input_data=$3

  local bulk_file="/tmp/.bulk_data"
  rm -f "$bulk_file"

  echo "Preparing bulk file $bulk_file to create all documents into index \"$index\"."

  local items=$(echo "$input_data" | jq -r '.hits.hits')
  local total=$(echo "$items" | jq '.|length')
  local num_row=0

  for row in $(echo "${items}" | jq -r '.[] | @base64'); do
    item=$(echo "${row}" | base64 --decode | jq -c "._source")
    id=$(echo "${item}" | jq -r ".id")
    echo "{\"index\": {\"_id\": \"$id\"}}" >> "$bulk_file"
    echo "$item" >> "$bulk_file"

    ((num_row++))
    progress=$((100*num_row/total))
    echo -ne "\r... $progress%"
  done

  if [ "$num_row" -gt 0 ]; then
    echo ""
      echo "Executing bulk creation of documents into index \"$index\"."
      response=$(curl --silent --location --request POST "http://localhost:9200/$index/$type/_bulk" \
                   --header 'Content-Type: application/octet-stream' \
                   --data-binary "@$bulk_file")

      errors=$(echo "${response}" | jq -r ".errors")

      if [ "$errors" = false ]; then
        echo "All documents were created successfully."
      else
        echo "There was errors trying to create documents. $response"
      fi
  else
    echo "There are not documents to load into index \"$index\""
  fi

  rm -f "$bulk_file"
}

load_els_resources() {
  path_file=$1
  index="resources"
  type="resource"
  full_json_file_path="$path_file/$index.json"
  input_data=$(cat "$full_json_file_path")
  bulk_create_documents "$index" "$type" "$input_data"
}

load_els_configurations() {
  path_file=$1
  index="configurations"
  type="configuration"
  full_json_file_path="$path_file/$index.json"
  input_data=$(cat "$full_json_file_path")
  bulk_create_documents "$index" "$type" "$input_data"
}

load_els_user_package_preferences() {
  path_file=$1
  index="user_base_package_preferences"
  type="user_base_package_preference"
  full_json_file_path="$path_file/$index.json"
  input_data=$(cat "$full_json_file_path")
#  create_document "$index" "$type" "$input_data"
  bulk_create_documents "$index" "$type" "$input_data"
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

function_test() {

    local index=$1
    local type=$2

    echo "Retrieving \"$type\" ids from index \"$index\" to delete them."

    get_els_documents_field_ids "$index"

    local bulk_file="/tmp/.doc_ids"
    rm -f "$bulk_file"

    echo "Preparing bulk file $bulk_file to delete all documents from index \"$index\"."

    local items="$document_fields_ids"
    local total=$(echo "$items" | jq '.|length')
    local num_row=0

    local tmp=""
    for row in $(echo "${items}" | jq -c '.[]'); do
        item=$(echo "${row}" | jq -r ".fields.id[0]")
#        echo "$item"
#        id=$(echo "${item}" | jq -r ".fields.id[0]")
#        record="{\"delete\": {\"_id\": \"$id\"}}"
#        tmp="${tmp}${record}\n"
#        echo "$record" >> "$bulk_file"

        ((num_row++))
        progress=$((100*num_row/total))
        echo -ne "\r... $progress%"
    done
    echo -e "$tmp" > "$bulk_file"
}

bulk_delete_els_documents_by_field_id() {
  local index=$1
  local type=$2

  echo "Retrieving \"$type\" ids from index \"$index\" to delete them."

  get_els_documents_field_ids "$index"

  local bulk_file="/tmp/.doc_ids"
  rm -f "$bulk_file"

  echo "Preparing bulk file $bulk_file to delete all documents from index \"$index\"."

  local items="$document_fields_ids"
  local total=$(echo "$items" | jq '.|length')
  local num_row=0

  for row in $(echo "${items}" | jq -r '.[] | @base64'); do
      item=$(echo "${row}" | base64 --decode)
      id=$(echo "${item}" | jq -r ".fields.id[0]")
      record="{\"delete\": {\"_id\": \"$id\"}}"
      echo "$record" >> "$bulk_file"

      ((num_row++))
      progress=$((100*num_row/total))
      echo -ne "\r... $progress%"
  done
  if [ "$num_row" -gt 0 ]; then
      echo ""
      echo "Executing bulk deleting of documents from index \"$index\"."
      response=$(curl --silent --location --request POST "http://localhost:9200/$index/$type/_bulk" \
                   --header 'Content-Type: application/octet-stream' \
                   --data-binary "@$bulk_file")

      errors=$(echo "${response}" | jq -r ".errors")

      if [ "$errors" = false ]; then
        echo "All documents were deleted successfully."
      else
        echo "There was errors trying to delete documents. $response"
      fi
  else
    echo "There are not documents in the index \"$index\" to delete."
  fi

  rm -f "$bulk_file"
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
  bulk_delete_els_documents_by_field_id "configurations" "configuration"
#  delete_els_documents_by_field_id "configurations" "configuration"
}

delete_els_resource_by_id() {
  bulk_delete_els_documents_by_field_id "resources" "resource"
#  delete_els_documents_by_field_id "resources" "resource"
}

delete_els_user_base_package_preference_by_id() {
  bulk_delete_els_documents_by_field_id "user_base_package_preferences" "user_base_package_preference"
  # It takes long time.
  #  delete_els_documents_by_field_id "user_base_package_preferences" "user_base_package_preference"
}

#bulk_delete_els_documents_by_field_id "resources" "resource"
#load_els_resources "/home/ronald/backups/11-07-2021"

#bulk_delete_els_documents_by_field_id "configurations" "configuration"
#load_els_configurations "/home/ronald/backups/11-07-2021"

#delete_els_user_base_package_preference_by_id
#load_els_user_package_preferences "/home/ronald/backups/11-07-2021"

#function_test "user_base_package_preferences" "user_base_package_preference"
#function_test "resources" "resource"
#function_test "configurations" "configuration"

