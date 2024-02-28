#!/bin/bash

cd scripts/Scripts

for file in "./migrations/"/*; do
    if [ -f "$file" ]; then
        /opt/mssql-tools/bin/sqlcmd -S sql-server -U sa -P Passw0rd! -i $file
    fi
done

for file in "./seeds/"/*; do
    if [ -f "$file" ]; then
        /opt/mssql-tools/bin/sqlcmd -S sql-server -U sa -P Passw0rd! -i $file
    fi
done