# ADR 0015: 12-Factor App Methodology

## Status
Accepted

## Context
To ensure the Task Management application is truly production-ready, scalable, and maintainable, we need to follow industry-standard best practices for building software-as-a-service apps. The [12-Factor App](https://12factor.net/) methodology provides a robust framework for this.

## Decision
We will align the application architecture with the 12-Factor methodology:

1.  **Codebase**: One codebase tracked in Git, many deploys.
2.  **Dependencies**: Explicitly declared via `csproj` and `package.json`. Isolation is enforced via Docker/DevContainers.
3.  **Config**: All configuration that varies between deployments (database URLs, JWT secrets, SignalR endpoints) is stored in the **environment** (ref: ADR 0014).
4.  **Backing services**: PostgreSQL and local storage are treated as attached resources, accessed via URLs/connection strings provided in the environment.
5.  **Build, release, run**: Strictly separate stages. We will use multi-stage Docker builds to separate the compilation environment from the execution environment.
6.  **Processes**: The backend is executed as a stateless process. Any persistent data is stored in the backing service (PostgreSQL).
7.  **Port binding**: The application is self-contained and exports services via port binding (Kestrel for .NET, Vite for React).
8.  **Concurrency**: The stateless design allows scaling by adding more concurrent processes.
9.  **Disposability**: Maximize robustness with fast startup and graceful shutdown (handling SIGTERM).
10. **Dev/prod parity**: Development and production environments are kept as similar as possible using Docker.
11. **Logs**: Logs are treated as event streams and written to `stdout`. We will not manage log files within the application.
12. **Admin processes**: Management tasks (like EF migrations) are run as one-off processes.

## Consequences
- **Positive**: High portability between execution environments (Local, AWS, Azure). Simplified scaling and deployment pipelines. Improved reliability and observability.
- **Negative**: Requires more discipline in managing environment variables and container orchestration.
