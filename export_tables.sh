#!/bin/bash
# Export all tables in a sqlite database to csv files using sqlite3
DBNAME=$1
DIR=$2
echo 'started export'
tables=( $(sqlite3 $1 .tables) )
for i in "${tables[@]}"
  do : 
   sqlite3 -header -csv $1 "select * from $i;" > $2/$i.csv
   echo "exported table " $i
done
echo 'completed export'
