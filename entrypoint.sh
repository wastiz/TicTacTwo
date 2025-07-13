#!/bin/bash

# Wait for database to be ready
echo "Waiting for database to be ready..."
until pg_isready -h db -p 5432 -U postgres; do
    echo "Database is not ready yet. Waiting..."
    sleep 2
done

echo "Database is ready!"

# Run migrations (if you have them)
# dotnet ef database update --no-build

# Start the application
echo "Starting the application..."
dotnet Api.dll