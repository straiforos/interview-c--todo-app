# ADR 0021: Simplified Scope and DTO Pattern

## Status

Accepted

## Context

The project was initially designed with a broad set of features (Rich Text, Blob Storage, SignalR, etc.). To better align with the core requirements of an interview take-home assessment and to more effectively demonstrate the **DTO (Data Transfer Object) Pattern**, we have decided to simplify the application scope.

## Decision

We will simplify the domain model and focus on a clear distinction between "List" and "Detail" views to highlight DTO usage:

1. **Simplified Task Model**: The `TaskItem` entity is reduced to core properties: `Id`, `Title`, `Description`, `IsCompleted`, and audit fields (`CreatedAt`, `UpdatedAt`, `CreatorId`).
2. **DTO Differentiation**:
  - `TaskSummaryDto`: A lightweight DTO used for listing tasks, containing only `Id`, `Title`, and `IsCompleted`.
  - `TaskDto`: A detailed DTO used for viewing/editing a single task, containing all fields including audit metadata.
3. **Feature Removal**:
  - Removed **Rich Text Content** and **Blob Storage** (`MediaItem`, `IStorageService`) to reduce boilerplate and complexity.
  - Maintained **RLS (Row Level Security)** and **Permissions** as they demonstrate robust security practices.
4. **Frontend Implementation**: The UI will reflect this by showing a simple list (using `TaskSummaryDto`) and an "Edit/Detail" view that fetches the full `TaskDto`.

## Consequences

- **Positive**: 
  - **Clarity**: More clearly demonstrates the "Why" behind DTOs (performance, security, and separation of concerns).
  - **Focus**: Reduces "over-engineering" while maintaining high-quality architectural patterns (RLS, AOP, SOA).
  - **Maintainability**: Smaller codebase that is easier to review.
- **Negative**: 
  - **Feature Set**: Reduced functionality compared to the initial plan.

