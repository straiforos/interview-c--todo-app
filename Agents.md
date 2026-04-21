# Agent Instructions & Project Standards

This document outlines the architectural standards, code quality requirements, and operational constraints for AI agents working on this project.

## 🚀 Operational Constraints
- **Human-in-the-Loop (HITL)**: AI agents must **NEVER** perform a `git commit`. All changes must be staged and left for manual review and commitment by a human developer.
- **Environment**: All development must occur within the provided DevContainer.
- **Methodology**: Follow **12-Factor App** principles (Config in Env, Logs to Stdout, Stateless processes).

## 🏗️ Project Structure
```text
.
├── TaskManagement.Api/        # .NET 10 Web API (Backend)
│   ├── Controllers/           # Thin controllers, DTO-based, Happy-path only
│   ├── Data/                  # EF Core Context, Migrations, Interceptors, Options
│   ├── DTOs/                  # Request/Response contracts (Separate for Create/Update)
│   ├── Models/                # EF Core Entities (Internal only, implement IBaseEntity)
│   ├── Services/              # Business logic, SOA, Data Isolation (RLS)
│   ├── Interceptors/          # AOP: RLS, Notifications, Exception Filters
│   └── wwwroot/               # Local blob storage for MVP media
├── TaskManagement.Client/     # React 19 + Vite 8 (Frontend)
│   ├── src/
│   │   ├── components/        # Presentational components
│   │   │   └── shared/        # Reusable Radix UI + Tailwind components
│   │   ├── services/          # RxJS-based singleton services (SOA)
│   │   ├── hooks/             # Custom React hooks (e.g., useObservable)
│   │   └── lib/               # Utilities (clsx, tailwind-merge)
├── TaskManagement.Tests/      # Test Suites
│   ├── Integration/           # Backend API tests (Testcontainers + Postgres)
│   └── ...                    # Frontend Vitest suites
├── docs/                      # VitePress Documentation & ADRs
├── bruno/                     # API Testing Collection
└── .devcontainer/             # Dockerized development environment
```

## 💎 Code Quality & Architectural Standards
- **Data Isolation**: Must use **PostgreSQL Row Level Security (RLS)**. Accidental data leaks must be prevented at the database level using `app.current_user_id`.
- **DTO Architecture**: Never return EF Entities. Use separate DTOs for `Create`, `Update`, and `Response` operations.
- **Service-Oriented Architecture (SOA)**: Controllers must **NEVER** access repositories directly. All logic and data access must flow through a dedicated Service layer.
- **Generic CRUD**: Leverage the `ICrudRepository` and `ICrudService` patterns to minimize boilerplate while maintaining consistency.
- **Aspect-Oriented Programming (AOP)**: 
    - Use **EF Core Interceptors** for side-effects (e.g., `NotificationInterceptor` for SignalR).
    - Use **Action Filters** for cross-cutting concerns (e.g., `ApiExceptionFilter` for declarative error handling).
- **Validation**: Use built-in **Data Annotations** directly on DTOs.
- **Frontend State**: Use **RxJS** in services. Components should subscribe to Observables, not manage complex state or API calls directly.
- **Frontend Types**: 
    - Maintain a clean `src/types/` directory.
    - Use `index.ts` as a **barrel file** (entry point) to export all types.
    - Each major type category should reside in its own file (e.g., `task.types.ts`, `auth.types.ts`).
    - All types and interfaces must be documented with **JSDoc** for better developer experience and IntelliSense.
- **Frontend Configuration**:
    - Follow the **12-Factor App** principle of "Build once, deploy anywhere."
    - Use a `ConfigService` to fetch environment-specific settings from `/config.json` at runtime.
    - Never use `import.meta.env` for environment-specific values like API URLs.
- **Shared Components**: Use the **shadcn CLI** (`npx shadcn@latest add ...`) to manage Radix UI primitives in `src/components/ui`.
- **Real-time**: Use **SignalR** for all real-time notifications.
- **Secret Management**: Follow **Environment-Driven Secret Management**. No secrets in `appsettings.json`. Use the **Options Pattern** (`IOptions<T>`).
- **Documentation**: All major decisions must be recorded in the `docs/architecture/adrs` directory.
