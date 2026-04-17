# ADR 0014: Environment-Driven Secret Management

## Status
Accepted

## Context
Hardcoding sensitive information (like JWT keys and database passwords) in configuration files is a significant security risk and prevents effective secret rotation. While .NET provides several mechanisms for secret management (like User Secrets), we need a strategy that is Docker-native for local development and compatible with Infrastructure-as-Code (IaC) and cloud secret managers (like AWS Secrets Manager) for production.

## Decision
We will implement an **Environment-Driven Secret Management** strategy using the **Options Pattern**, and we will **completely eliminate environment-specific appsettings.json files**:

1.  **Environment Variable Overrides**: We will leverage .NET's built-in configuration provider that automatically maps environment variables to configuration keys using the double-underscore delimiter (e.g., `Jwt__Key` maps to `Jwt:Key`).
2.  **No Environment appsettings**: We will delete `appsettings.Development.json` and `appsettings.Production.json`. All configuration that varies by environment (secrets, connection strings, etc.) will be provided via the environment.
3.  **Local Development (Dockerized)**: Sensitive local values will be managed in the `.devcontainer/docker-compose.yml` file and `.env.development` files. This ensures a seamless "clone and run" experience within the DevContainer without committing secrets to the application code.
4.  **Production (IaC)**: Secrets will be managed via IaC (e.g., Terraform) and injected into the container's environment variables at runtime. The IaC layer will be responsible for fetching these from a secure vault like AWS Secrets Manager.
5.  **Strongly-Typed Options**: We will use the **Options Pattern** (`IOptions<T>`) to bind configuration sections to C# classes. This provides a clean, testable, and type-safe interface for the application logic, making it agnostic of where the configuration value originated.

## Consequences
- **Positive**: 
    - **Security**: Sensitive data is never stored in the source code repository.
    - **Simplicity**: Eliminates the confusion of having configuration spread across multiple JSON files.
    - **Consistency**: The same mechanism (Environment Variables) is used across all environments.
    - **Cloud-Agnostic**: The application code does not require a specific cloud SDK (like AWS SDK) just to retrieve its configuration.
    - **Scalability**: Perfectly aligns with modern DevOps practices and automated rotation.
- **Negative**: Requires the infrastructure/deployment pipeline to be responsible for secret injection.
