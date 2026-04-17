# Architecture Overview

This section outlines the high-level architecture of the Todo application. The system is divided into a frontend React application and a backend .NET 10 Web API.

## System Context

The application follows a standard client-server architecture:

1. **Frontend (React SPA)**: Handles the user interface, routing, and state management using a Service-Oriented Architecture with RxJS, communicating with the backend via REST APIs.
2. **Backend (.NET 10 Web API)**: Manages business logic, authentication, validation, and data persistence.
3. **Database (PostgreSQL 18)**: A robust relational database used for persistent storage, managed via Entity Framework Core.

## Backend Architecture

The backend is structured to prevent "over-architecting" while maintaining clean separation of concerns:

- **Controllers**: Thin entry points that handle HTTP requests/responses and route them to services.
- **Services**: Contain the core business logic and enforce data ownership (e.g., ensuring users only access their own todos).
- **DTOs (Data Transfer Objects)**: Define the API contracts. EF Core entities are *never* exposed directly to the frontend.
- **AutoMapper**: Handles mapping between internal domain entities and external DTOs.
- **Data Annotations**: Enforces strict input validation rules directly on the DTOs before requests reach the service layer.

## Frontend Architecture

- **Shared Components**: A library of reusable, accessible UI elements built using **Radix UI Primitives** and styled with Tailwind CSS v4.2.
- **State & Data Fetching**: A Service-Oriented Architecture (SOA) is used. Frontend services encapsulate API calls and manage state using **RxJS** (BehaviorSubjects/Observables). React components simply subscribe to these services.
- **Routing**: React Router handles client-side navigation.

## Architecture Decision Records (ADRs)

We use ADRs to document significant architectural decisions. See the [ADRs section](./adrs/) for details.
