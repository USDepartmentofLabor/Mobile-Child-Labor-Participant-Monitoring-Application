#!/bin/bash
# compare files from 2 directories
# show files not common in both directories and show git diff
DIRA=$1
DIRB=$2
echo "--------------------------"
echo "--- COMPARISON STARTED ---"
echo "--------------------------"
csv_files_a=( $(ls $1) )
csv_files_b=( $(ls $2) )
unmatched_files=(`echo ${csv_files_a[@]} ${csv_files_b[@]} | tr ' ' '\n' | sort | uniq -u `)
files_to_compare=( $(ls $1) )
# remove unmatched files => (files_to_compare = a - unmatched_files)
for i in "${unmatched_files[@]}"; do
         files_to_compare=(${files_to_compare[@]//*$i*})
done
for i in "${unmatched_files[@]}"
  do :    
   echo "Unable to compare file" $i
done
echo "----------------------"
echo "--- CHANGED TABLES ---"
echo "----------------------"
for i in "${files_to_compare[@]}"
  do :
   git diff --no-index --stat $1$i $2$i
done
echo "------------------------"
echo "--- COMPARISON ENDED ---"
echo "------------------------"
