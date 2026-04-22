#!/bin/bash
set -e

# Fix potential shell issues
export SHELL=/bin/bash

# Initialize backend .env
if [ ! -f TaskManagement.Api/.env.development ]; then
    echo "Initializing TaskManagement.Api/.env.development"
    cp TaskManagement.Api/.env.example TaskManagement.Api/.env.development
fi

# Initialize frontend .env
if [ ! -f TaskManagement.Client/.env.development ]; then
    echo "Initializing TaskManagement.Client/.env.development"
    cp TaskManagement.Client/.env.example TaskManagement.Client/.env.development
fi

# Initialize frontend config.json
if [ ! -f TaskManagement.Client/public/config.json ]; then
    echo "Initializing TaskManagement.Client/public/config.json"
    cp TaskManagement.Client/public/config.json.example TaskManagement.Client/public/config.json
fi

# Disable SSH commit signing in the container since the macOS 1Password path won't exist
git config --local commit.gpgsign false

# Install Entity Framework Core tools globally
echo "Installing dotnet-ef globally..."
dotnet tool install --global dotnet-ef

# Ensure global tools are in PATH
export PATH="$PATH:$HOME/.dotnet/tools"

# Run database migrations
echo "Running database migrations..."
if [ -f /workspace/scripts/db/update.sh ]; then
    /workspace/scripts/db/update.sh || echo "Warning: Database update failed. The DB might not be ready yet."
fi

echo "Environment initialization complete."
dotnet --version
node --version
