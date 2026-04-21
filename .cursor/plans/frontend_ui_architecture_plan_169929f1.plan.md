---
name: Frontend UI Architecture Plan
overview: Implement a snappy, router-driven React frontend using React Router v7 and RxJS, featuring memoized shared components and strict separation of List vs. Detail views to demonstrate the DTO pattern.
todos:
  - id: frontend-init
    content: Initialize Vite React project and install core dependencies (React Router, RxJS, Tailwind, Radix)
    status: pending
  - id: frontend-services
    content: Implement RxJS API Services (AuthService, TaskService) with shareReplay caching
    status: pending
  - id: frontend-shared-components
    content: Build Shared Component Library (Button, Input, Card, Checkbox)
    status: pending
  - id: frontend-routing
    content: Configure React Router with Loaders (Resolvers) for /tasks and /tasks/:id
    status: pending
  - id: frontend-auth-pages
    content: Implement Auth Pages (/login, /register) and protected route wrapper
    status: pending
  - id: frontend-task-list
    content: Implement Task List Page (/tasks) using TaskSummaryDto
    status: pending
  - id: frontend-task-detail
    content: Implement Task Detail Page (/tasks/:id) as a read-only view using TaskDto
    status: pending
  - id: frontend-task-form
    content: Implement Task Form Page (/tasks/new, /tasks/:id/edit) for creating and updating tasks
    status: pending
isProject: false
---

# Frontend UI Architecture & API Integration Plan

This plan aligns the frontend architecture with ADR 0021 (DTO Pattern) and ADR 0022 (Router-Driven Architecture with RxJS).

## 1. Project Initialization & Tooling

- Scaffold the `TaskManagement.Client` directory with Vite, React 19, and TypeScript.
- Install dependencies: `react-router`, `rxjs`, `tailwindcss` (v4.2), Radix UI primitives, `clsx`, `tailwind-merge`, and `lucide-react` for icons.

## 2. RxJS Services (API Integration & Caching)

Create singleton services in `src/services/` to handle all API communication and state:

- `**AuthService**`: Manages JWT in `localStorage`, handles `/api/auth/login` and `/api/auth/register`, and exposes a `currentUser$` BehaviorSubject.
- `**TaskService**`:
  - `getTasks$()`: Fetches `TaskSummaryDto[]` from `GET /api/tasks`. Uses `shareReplay(1)` to cache the list in memory.
  - `getTask$(id)`: Fetches full `TaskDto` from `GET /api/tasks/{id}`. Uses `shareReplay(1)` for caching individual task details.
  - Mutations (`create`, `update`, `delete`): Perform API calls and invalidate/update the cached BehaviorSubjects to trigger UI reactivity.

## 3. Router-Driven Architecture (React Router v7)

Define routes in `src/App.tsx` using the data router API (`createBrowserRouter`):

- **Loaders (Resolvers)**: Route loaders will subscribe to the RxJS services and `await firstValueFrom(Observable)` before rendering the component. This ensures data is ready and eliminates component-level loading spinners.
- **Routes**:
  - `/login` & `/register`: Auth layouts.
  - `/` (Protected): Redirects to `/tasks`.
  - `/tasks` (Protected): Uses `tasksLoader` to fetch `TaskSummaryDto[]`. Renders the List view.
  - `/tasks/new` (Protected): Renders the Create Task form.
  - `/tasks/:id` (Protected): Uses `taskDetailLoader` to fetch `TaskDto`. Renders the read-only Detail view.
  - `/tasks/:id/edit` (Protected): Uses `taskDetailLoader` to fetch `TaskDto`. Renders the Edit Task form populated with existing data.

## 4. Shared Component Library

Build highly optimized, reusable components in `src/components/shared/`:

- `Button`, `Input`, `Checkbox`, `Card`.
- Use Tailwind CSS for styling and Radix UI for accessible interactive elements.

## 5. View Implementations (Demonstrating DTOs)

- **Task List (`/tasks`)**: Displays a clean list of tasks using the lightweight `TaskSummaryDto`. Includes a quick-toggle for completion status (calling `PATCH` or `PUT`) and a "View Details" button.
- **Task Detail (`/tasks/:id`)**: A highly user-friendly, read-only view displaying the full `TaskDto`. Utilizes `lucide-react` icons for visual hierarchy and Radix UI Tooltips to explain metadata (e.g., hovering over "Created At" shows exact timestamps or user info). Includes an "Edit" button navigating to the edit route.
- **Task Form (`/tasks/new` & `/tasks/:id/edit`)**: A reusable form component for both creating and updating tasks. In edit mode, it populates with the `TaskDto` fetched by the loader.
- **Memoization**: Apply `React.memo`, `useMemo`, and `useCallback` at the parent/container component level (e.g., the main List view or Detail view) to prevent unnecessary re-renders of complex views, keeping shared primitive components simple.

## 6. Error Handling

- Implement a global `ErrorBoundary` at the router level to catch and display API errors (e.g., 404 Not Found, 403 Forbidden) gracefully without crashing the app.

