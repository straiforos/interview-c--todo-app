#!/bin/bash
set -e

# 12-Factor App: Configuration in Environment
# This script creates a new EF Core migration.

if [ -z "$1" ]; then
    echo "❌ Error: Migration name is required."
    echo "Usage: ./scripts/db/add-migration.sh <MigrationName>"
    exit 1
fi

MIGRATION_NAME=$1

cd /workspace/TaskManagement.Api

echo "🚀 Adding migration: $MIGRATION_NAME..."
dotnet ef migrations add "$MIGRATION_NAME"
echo "✅ Migration added successfully."
