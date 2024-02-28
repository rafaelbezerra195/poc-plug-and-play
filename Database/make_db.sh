#!/bin/bash

cd scripts

# /opt/mssql-tools/bin/sqlcmd -S sql-server -U sa -P Passw0rd! -i Scripts/2.BMA_APPROVAL_RULES.sql

for file in "./Scripts/"/*; do
    if [ -f "$file" ]; then
        /opt/mssql-tools/bin/sqlcmd -S sql-server -U sa -P Passw0rd! -i $file
    fi
done
