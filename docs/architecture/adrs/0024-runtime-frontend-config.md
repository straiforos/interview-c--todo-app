# ADR 0024: Runtime Frontend Configuration (Build Once, Deploy Anywhere)

## Status
Accepted

## Context
Standard Vite/React builds often bake environment variables (e.g., `VITE_API_URL`) into the static assets at build time. This violates the **12-Factor App** principle of "Build once, deploy anywhere," as it requires a separate build for every environment (staging, production, etc.). We need a way to inject environment-specific configuration into the frontend at runtime without rebuilding the application.

## Decision
We will implement a **Runtime Configuration Service**:

1.  **External Config File**: The frontend will fetch a `config.json` file from the root directory (`/config.json`) at startup. This file is NOT part of the build artifact; instead, it is generated or mounted by **Infrastructure as Code (IaC)** or the deployment pipeline (e.g., a Kubernetes ConfigMap or an S3 upload).
2.  **Config Service**: A singleton `ConfigService` using **RxJS** will be responsible for:
    *   Fetching the `/config.json` file.
    *   Exposing a `config$` BehaviorSubject.
    *   Providing a `loadConfig()` method that returns a Promise, allowing the application to block rendering until the configuration is available.
3.  **Service Integration**: All other services (e.g., `AuthService`, `TaskService`) will depend on the `ConfigService` to retrieve the `API_URL` and other environment-specific settings, rather than using `import.meta.env`.
4.  **Local Development**: A `public/config.json` file will be maintained for local development (and added to `.gitignore`), with a `public/config.json.example` provided for reference.

## Consequences
- **Positive**: 
    - **12-Factor Compliance**: Strictly follows the "Build once, deploy anywhere" principle.
    - **Operational Efficiency**: DevOps can change API URLs or feature flags without requiring a developer to trigger a new frontend build.
    - **Consistency**: The exact same Docker image/artifact is promoted through all environments.
- **Negative**: 
    - **Startup Latency**: Adds one small HTTP request at application startup before the UI renders.
    - **Complexity**: Requires services to handle the asynchronous nature of configuration loading.
