#!/bin/bash
set -e

# Ensure we are in the client directory
cd /workspace/TaskManagement.Client

# Install dependencies
echo "Installing frontend dependencies..."
npm install

# Start the dev server
echo "Starting Vite dev server..."
npm run dev -- --host --port 5173 || echo "Vite exited with error."

# Keep the container alive
sleep infinity
