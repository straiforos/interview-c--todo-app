#!/bin/bash
set -e

# 12-Factor App: Configuration in Environment
# This script applies EF Core migrations to the database.
# It relies on the ConnectionStrings__DefaultConnection environment variable
# or the appsettings.json/secrets for connection details.

cd /workspace/TaskManagement.Api

echo "🚀 Applying database migrations..."
dotnet ef database update
echo "✅ Database update complete."
