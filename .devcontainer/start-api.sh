#!/bin/bash
set -e

# Ensure we are in the API directory
cd /workspace/TaskManagement.Api

# Start the .NET API
echo "Starting .NET API..."
dotnet watch run --urls http://0.0.0.0:5000 || echo "API exited with error."

# Keep the container alive even if the API crashes
sleep infinity
