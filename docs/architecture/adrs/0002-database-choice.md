# ADR 0002: Use PostgreSQL 18 with EF Core

## Status
Accepted

## Context
While SQLite or EF Core in-memory are common for small projects or prototypes, a true "Production MVP" requires a robust, scalable relational database that supports concurrent connections and advanced data types. To ensure the project remains easy to run for other developers without manual database installation, we will package it within a DevContainer using Docker Compose.

## Decision
We will use **PostgreSQL 18** managed via **Entity Framework (EF) Core** with migrations. The local development environment will spin up a Postgres instance automatically via a `.devcontainer/docker-compose.yml` setup.

## Consequences
- **Positive**: Delivers on the requirement for a Production MVP architecture with a production-grade RDBMS. Other developers can still clone the repo and run the app effortlessly thanks to the DevContainer/Docker Compose setup.
- **Negative**: Slightly more complex infrastructure setup compared to a simple SQLite file, requiring Docker Compose. Integration tests will likely need Testcontainers to spin up a real Postgres instance instead of using an in-memory provider.
