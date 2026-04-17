# ADR 0016: Development Environment Setup Workflow

## Status
Accepted

## Context
Following the 12-Factor App methodology (ADR 0015) and Environment-Driven Secret Management (ADR 0014), the application relies on environment variables for configuration. To ensure a consistent and reproducible developer experience, we need a clear process for setting up these variables in a new development environment.

## Decision
We will establish a formal environment setup workflow using `.env.example` files as the source of truth for required configuration:

1.  **Template-First**: Every project (Backend and Frontend) must maintain a `.env.example` file in its root. This file will contain all required environment variable keys with empty or non-sensitive default values.
2.  **Explicit Setup Step**: The project setup process will require developers to copy `.env.example` to a local `.env` (or `.env.development`) file.
3.  **DevContainer Integration**: The `.devcontainer/docker-compose.yml` will be configured to use these `.env` files if they exist, or provide safe defaults, ensuring the "clone and run" experience is preserved while making the configuration source explicit.
4.  **Documentation**: The `README.md` will explicitly list the environment setup as a required step, reinforcing the 12-Factor principle that configuration is separate from code.

## Consequences
- **Positive**:
    - **Clarity**: New developers can immediately see what configuration is required.
    - **Security**: Prevents sensitive local overrides from being committed, as `.env` files are added to `.gitignore`.
    - **Consistency**: Bridges the gap between local development and production environment variable management.
- **Negative**: Adds one manual step (copying the file) to the initial setup process.
