# ADR 0012: Real-time Notifications with SignalR

## Status

Accepted

## Context

The application allows users to assign tasks to other users. To provide a modern, responsive user experience, the assigned user should be notified immediately without needing to refresh the page or rely on inefficient HTTP polling.

## Decision

We will use **ASP.NET Core SignalR** to implement real-time push notifications over WebSockets.

1. **Backend**: A `NotificationHub` will manage connected clients. When a task is assigned, the `TaskService` will persist a `Notification` record to the database and broadcast the event to the assignee's specific user group via `IHubContext`.
2. **Frontend**: The frontend will use the `@microsoft/signalr` client. The connection logic will be encapsulated in an RxJS-based `NotificationService`, exposing a `BehaviorSubject` or `Observable` that React components can subscribe to for real-time UI updates (e.g., a toast notification or a badge counter).

## Consequences

- **Positive**: Provides a highly responsive, "production-quality" user experience. WebSockets are vastly more efficient than HTTP polling for real-time events.
- **Negative**: Introduces stateful connections to the backend, requiring connection lifecycle management (reconnections, authentication token passing over WebSockets) on the frontend.

