#!/bin/bash
# ./els-delete-resources.sh "/home/ronald/backups/11-07-2021"
echo "Start"
names=""
for name in joey suzy bobby;do
  names="${names}${name}\n"
done
echo -e "${names}" > names.txt
echo "End"