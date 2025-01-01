#!/bin/bash

SQL_SCRIPT="add_api_user.sql"

if [ ! -f "$SQL_SCRIPT" ]; then
  echo "Error: SQL script '$SQL_SCRIPT' not found!"
  exit 1
fi

docker compose exec -T lego-db_dev /opt/mssql-tools18/bin/sqlcmd \
  -C -S localhost -U sa -P "2fYFkj0sz8n4rQQlP9lruMgqY2hsDI" -d master < "$SQL_SCRIPT"

if [ $? -eq 0 ]; then
  echo "SQL script executed successfully."
else
  echo "Error: Failed to execute the SQL script."
  exit 1
fi

echo "Done!"