#!/bin/bash

location_backup=$1
user="root"
password="control123!"
host="127.0.0.1"
port=3306


load_database() {	
	sql_file_names_with_extension=("$@")
	
	for sql_file in "${sql_file_names_with_extension[@]}"; do

		filename=$(basename -- "$sql_file")		
		database_name="${filename%.*}"

		echo "########## Restoring the file $sql_file in ${database_name}"

		{
			echo "DROP DATABASE IF EXISTS ${database_name};"			
			echo "CREATE DATABASE IF NOT EXISTS ${database_name};"

		} | mysql -u$user -h $host -P $port -p$password --ssl-mode=DISABLED

		mysql -u$user -h $host -P $port -p$password --database=$database_name --ssl-mode=DISABLED<"$sql_file"

		echo "########## The database $database_name was restored"
		echo ""
	done
}

print_options() {
	sql_file_names=("$@")
	echo "Select a option, q in order to quit"
	counter=0
	for file in "${sql_file_names[@]}"; do		
		filename=$(basename -- "$file")
		echo "[$counter] load $filename"		
		((counter=counter+1))
		
	done
	echo "[$counter] load all sql files"
	echo "[q] quit"	
}

if [[ $location_backup == "." ]] || [[ $location_backup == "" ]]	
then
	location_backup=$(pwd)
fi

if [ ! -d $location_backup ] 
then
	echo "Directory ${location_backup} doesn't exist." 
	exit 1
fi

unset sql_files i
while input= read -r -d $'\0' f; do
  sql_files[i++]="$f"  
done < <(find $location_backup -maxdepth 1 -type f -name "*.sql" -print0 )

if [[ ${#sql_files[@]} -eq 0 ]]; then
	echo "The directory ${location_backup} doesn't have any .sql files." 
	exit 1
fi


total_sql_files=${#sql_files[@]}
max_index=${#sql_files[@]}
((max_index--))

option=""
while [[ $option != 'q' ]]; do
	print_options "${sql_files[@]}"

	read -p "Option: " option

	if [[ $option == "q" ]]; then
		continue
	fi

	if [[ $option -ge 0 ]] && [[ $option -le $max_index ]]; then		
		array_files_to_restore=("${sql_files[option]}")		
		load_database "${array_files_to_restore[@]}"
	fi

	if [[ $option -eq $total_sql_files ]]; then		
		load_database "${sql_files[@]}"
	fi
done
