# 25. Roles and Permissions Enhancements

Date: 2026-04-22

## Status

Accepted

## Context

Our application currently utilizes a robust Policy-Based Authorization system on the backend and enforces data isolation via PostgreSQL Row-Level Security (RLS). However, all users are currently assigned a single default `"User"` role with all permissions, and the frontend has no knowledge of what a user is actually allowed to do.

To support a production-ready feature set, we need to introduce distinct access levels (e.g., `Admin`, `User`, `ReadOnly`) and surface these permissions to the frontend. This will allow the UI to adapt dynamically (hiding buttons the user cannot use) and protect routes from unauthorized access before an API call is even made.

## Decision

We will implement the following enhancements across the stack:

### 1. Backend Role Definitions
We will update the database seeder (`DbSeeder.cs`) to define three distinct roles with granular permissions:
*   **`Admin`**: Granted all permissions (`Tasks.Read`, `Tasks.Create`, `Tasks.Update`, `Tasks.Delete`).
*   **`User`**: Granted standard permissions (`Tasks.Read`, `Tasks.Create`, `Tasks.Update`), but restricted from hard-deleting tasks.
*   **`ReadOnly`**: Granted only read access (`Tasks.Read`).

### 2. RLS Policy Updates
Currently, our RLS policy strictly isolates tasks to the `CreatorId`. We will update the PostgreSQL RLS policy to allow users with the `Admin` role to bypass this restriction, enabling them to view and manage all tasks in the system.

### 3. Surfacing Permissions to the Frontend
We will update the authentication flow to include the user's role and a flat list of their granted permissions in the JWT login/register response (`AuthResponseDto`). 
```json
{
  "token": "...",
  "email": "user@example.com",
  "userId": "123",
  "role": "ReadOnly",
  "permissions": ["Tasks.Read"]
}
```

### 4. Frontend Route Guards (React Router Native)
Instead of using wrapper components (HOCs) for route protection, we will embrace the idiomatic React Router v6 Data API. We will implement permission checks inside our route `loader` functions. This acts as the React Router equivalent to Angular's `canActivate`, preventing waterfall requests and stopping unauthorized route transitions before the component renders.

### 5. UI Conditional Rendering
We will introduce a `<HasPermission>` component (or a `usePermission` hook) in the React frontend. This will evaluate the current user's permissions array and conditionally render UI elements (e.g., completely removing the "New Task" button for `ReadOnly` users).

## Consequences

### Positive
*   **Defense in Depth**: Security is enforced at the UI level (hiding actions), the Route level (loaders), the API level (`[HasPermission]`), and the Database level (RLS).
*   **Better UX**: Users will not see buttons or routes they are not allowed to access, preventing frustrating 403 Forbidden errors during normal navigation.
*   **Idiomatic React**: Using `loader` functions for route guarding aligns with modern React Router best practices, avoiding the "flicker" associated with traditional protected route wrapper components.

### Negative
*   **Increased Complexity**: The frontend must now maintain and evaluate a local copy of the user's permissions.
*   **Cache Invalidation**: If an admin changes a user's role while they are logged in, the frontend's permission list will be stale until their next login or token refresh.