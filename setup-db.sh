#!/bin/bash

echo "Starting PostgreSQL database..."
docker compose up -d

echo "Waiting for database to be ready..."
sleep 10

echo "Applying migrations..."
cd MsiaPropertyTransaction
dotnet ef database update
cd ..

echo "Database is ready!"
echo "Connection: Host=localhost;Database=msia_property_db;Username=postgres;Password=password"