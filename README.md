# Todo App

A production-ready Todo application built with .NET 10 and React 19, designed to demonstrate strong architectural fundamentals, clean code, and robust error handling.

## Overview

This project is structured to provide a clear separation of concerns, proper data isolation, and a polished developer and user experience. It includes a DevContainer setup for immediate productivity without local toolchain installation.

## Documentation

Comprehensive documentation, including Architecture Decision Records (ADRs), is maintained using VitePress in the `docs` directory. 

To view the documentation locally:
```bash
cd docs
npm install
npm run docs:dev
```

## Quick Start

### Prerequisites
- Docker
- VS Code with the "Dev Containers" extension

### Setup
1. Clone the repository and open it in VS Code.
2. When prompted, click **"Reopen in Container"** (or run the command from the Command Palette).
3. The DevContainer will automatically install the .NET SDK, Node.js, and other required tools.

### Running the Application
*(Instructions will be updated once the backend and frontend projects are initialized)*

## Architecture Highlights
- **Backend**: .NET 10 Web API, PostgreSQL 18, Entity Framework Core, AutoMapper, OpenAPI/Swagger.
- **Frontend**: React 19, Vite 8, Tailwind CSS 4.2, Radix UI, RxJS (Service-Oriented Architecture).
- **Data Isolation**: JWT authentication ensures users can only access their own data.
- **API Testing**: Includes a fully configured Bruno collection for seamless endpoint testing.
- **Error Handling**: Global exception handling on the backend and consistent error states on the frontend.
