# ADR 0005: EF Core Migrations Strategy

## Status
Accepted

## Context
The application relies on a relational database (PostgreSQL) managed by Entity Framework Core. An easy setup and a reproducible environment are critical for a good developer experience. Other developers should not have to manually run CLI commands to create the database schema if it can be avoided.

## Decision
We will use **EF Core Migrations** to manage all schema changes. 
Furthermore, during application startup (specifically in the `Development` environment), the application will automatically apply any pending migrations using `dbContext.Database.MigrateAsync()`.

## Consequences
- **Positive**: Guarantees the database schema is always in sync with the codebase when a developer spins up the DevContainer and runs the API. Eliminates a manual setup step, improving the developer experience.
- **Negative**: Auto-migrating on startup is generally discouraged in production environments with multiple instances (due to race conditions), but it is perfectly acceptable and highly beneficial for a local development/review MVP environment. We will restrict this behavior to the `Development` environment.
