#!/bin/bash
set -e

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

echo "Environment initialization complete."
dotnet --version
node --version
