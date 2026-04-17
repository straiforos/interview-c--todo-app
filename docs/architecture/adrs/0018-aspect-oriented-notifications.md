# ADR 0018: Aspect-Oriented Notifications

## Status
Accepted

## Context
In a complex application, business logic often becomes cluttered with "side-effect" logic like logging, auditing, or notifications. In our case, the `TaskService` was manually creating notifications whenever a task was assigned. This violates the Single Responsibility Principle and makes the service harder to maintain. We need a way to "cross-cut" these events without modifying the core business logic.

## Decision
We will implement an **Aspect-Oriented Notification System** using **EF Core SaveChanges Interceptors**:

1.  **Notification Interceptor**: We will create a `NotificationInterceptor` that inherits from `SaveChangesInterceptor`.
2.  **Intercepting Events**: The interceptor will override `SavingChangesAsync`. It will inspect the EF Core `ChangeTracker` for any `TaskItem` entities that are being created or updated.
3.  **Logic**:
    *   If a new `TaskItem` is added with an `AssigneeId`, the interceptor will automatically queue a `Notification` entity.
    *   If an existing `TaskItem` has its `AssigneeId` changed, the interceptor will queue a `Notification` for the new assignee.
4.  **Real-time Broadcast**: The interceptor will also handle the SignalR broadcast after the changes are successfully committed to the database (via `SavedChangesAsync`).

## Consequences
- **Positive**: 
    - **Clean Business Logic**: `TaskService` is now 100% focused on task management and is completely unaware of the notification system.
    - **Guaranteed Consistency**: Notifications are created automatically whenever the database state changes, regardless of which service or process made the change.
    - **Scalability**: This pattern can be easily extended to other entities (e.g., "Project assigned", "Comment added") by simply updating the interceptor.
- **Negative**: Moves logic into the "magic" of the database pipeline, which can be harder for new developers to discover if not properly documented.
