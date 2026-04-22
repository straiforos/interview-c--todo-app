---
name: Task Management App Dev Plan
overview: A production-ready Task Management application using .NET 10 and React 19, featuring a DevContainer setup, clean DTO architecture (demonstrating List vs Detail patterns), robust validation, PostgreSQL Row Level Security (RLS) for data isolation, and comprehensive documentation.
todos:
  - id: docs-vitepress
    content: Setup VitePress documentation and ADRs
    status: completed
  - id: setup-devcontainer
    content: Setup DevContainer with .NET 10, Node.js 24, and PostgreSQL 18 (via docker-compose)
    status: completed
  - id: init-projects
    content: Initialize .NET Web API and Vite React projects
    status: completed
  - id: backend-db-models
    content: "Backend: Setup EF Core PostgreSQL, Models (User, TaskItem), and Initial Migration"
    status: completed
  - id: backend-auth
    content: "Backend: Implement JWT Auth and User Context"
    status: completed
  - id: backend-dtos-validation
    content: "Backend: Implement DTOs (TaskSummaryDto vs TaskDto), AutoMapper, and Data Annotations"
    status: completed
  - id: backend-services-controllers
    content: "Backend: Implement TaskService, SignalR Hub, and thin Controllers"
    status: completed
  - id: backend-rls
    content: "Backend: Configure EF Core Interceptor and PostgreSQL RLS Policies"
    status: completed
  - id: backend-error-swagger
    content: "Backend: Add Global Exception Handling Middleware and Swagger"
    status: completed
  - id: frontend-setup
    content: "Frontend: Setup Tailwind, Radix UI, RxJS, and shared components"
    status: completed
  - id: frontend-auth-ui
    content: "Frontend: Build Auth UI (Login/Register) with validation and error states"
    status: completed
  - id: frontend-todo-ui
    content: "Frontend: Build Task UI (List using Summary DTO, Detail using Full DTO) and loading/empty states"
    status: completed
  - id: testing-backend
    content: "Testing: Add Backend Integration Tests (WebApplicationFactory) with Testcontainers for PostgreSQL"
    status: completed
  - id: testing-frontend
    content: "Testing: Add Frontend Component Tests"
    status: completed
  - id: docs-readme
    content: "Documentation: Write comprehensive README (Setup, Assumptions, Scalability, Future)"
    status: completed
  - id: docs-gh-pages
    content: "CI/CD: Setup GitHub Actions workflow to publish VitePress docs to GitHub Pages"
    status: completed
  - id: docs-bruno
    content: "Documentation: Create Bruno API collection for testing"
    status: completed
isProject: false
---

# Task Management App Implementation Plan

The goal is to build a "small, production-quality project" that demonstrates strong fundamentals without over-architecting. We will focus on clean architecture, robust error handling, proper data isolation (auth), and a polished frontend experience.

## 1. Infrastructure & Setup

- **DevContainer**: Create a `.devcontainer` configuration with .NET 10, Node.js 24 (LTS), and a `docker-compose.yml` to spin up a PostgreSQL 18 database so the project runs out-of-the-box without local installations. The DevContainer will also include recommended VS Code extensions (like Bruno).
- **Project Structure**: A single repository containing a `.NET Web API` backend folder and a `Vite + React` frontend folder. We will avoid multiple backend projects to prevent over-architecting, using clean folder separation instead (Controllers, Services, Models, DTOs).

## 2. Backend (.NET 10)

- **Database**: PostgreSQL 18 with Entity Framework Core.
- **Migrations Strategy**: EF Core Migrations will be used to manage the database schema. To ensure a seamless developer experience, migrations will be automatically applied on application startup in the `Program.cs` development environment block.
- **Authentication & Data Ownership (RLS)**: Implement JWT-based authentication. Data isolation will be enforced at the database level using **PostgreSQL Row Level Security (RLS)**. An EF Core Interceptor will inject the current user's ID into the database session (`SET LOCAL app.current_user_id`), making accidental data leaks impossible.
- **Architecture**:
  - **Models**: `User` and `TaskItem` (with `CreatorId` and audit fields).
  - **DTOs**: Use separate DTOs for different operations (`TaskSummaryDto` for lists, `TaskDto` for details, `CreateTaskDto`, `UpdateTaskDto`) to demonstrate the DTO pattern for performance and data encapsulation. EF entities are never returned.
  - **AutoMapper**: For clean mapping between Entities and DTOs.
  - **Services**: Business logic will live in `TaskService` to keep controllers thin.
- **Validation**: Use built-in **ASP.NET Core Data Annotations** (`[Required]`, `[MaxLength]`) directly on the separate DTOs. This provides standard, zero-dependency input validation that automatically integrates with Swagger and the ASP.NET Core pipeline.
- **Error Handling**: Implement a Global Exception Handling Middleware to return consistent `ProblemDetails` responses (ensuring correct 404s, 400s, etc.).
- **API Docs & Testing**: Configure OpenAPI (Swagger) with JWT support. Create a **Bruno** collection in the repository with pre-configured requests and environment variables for seamless API testing.

## 3. Frontend (React + TypeScript)

- **Tooling**: Vite 8, Tailwind CSS v4.2, Radix UI Primitives, React Router, `@microsoft/signalr`.
- **Architecture & State**: Service-Oriented Architecture (SOA) using **RxJS**. Business logic and API calls are abstracted into singleton services. React components subscribe to RxJS Observables for state updates (loading, error, data).
- **Features**:
  - **Auth**: Login and Registration flows.
  - **Tasks**: List (using `TaskSummaryDto`), Detail/Edit view (fetching full `TaskDto`), Create, Delete, Toggle status.
  - **UX**: Clear loading skeletons/spinners, error toasts, and empty states. Edge cases handled (e.g., keeping form data on submission failure).
- **Modularity**: Build a robust `shared` components library using Radix UI primitives styled with Tailwind CSS, ensuring accessibility and reusability across the app.

## 4. Testing

- **Backend**: Integration tests using `WebApplicationFactory` and Testcontainers to spin up a real PostgreSQL instance for testing the API endpoints end-to-end (Auth + CRUD + Data Isolation).
- **Frontend**: Basic component testing using React Testing Library (e.g., rendering empty states, form validation).

## 5. Documentation & CI/CD

- **README**: A comprehensive runbook detailing setup steps, explanation notes, assumptions, scalability considerations, and future implementations.
- **Bruno Collection**: A plain-text API collection located in the `bruno` directory for easy end-to-end API testing.
- **GitHub Pages**: Create a GitHub Actions workflow (`.github/workflows/deploy-docs.yml`) to automatically build and publish the VitePress documentation to GitHub Pages on every push to `main`.

