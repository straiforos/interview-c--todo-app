# ADR 0004: Service-Oriented Architecture and RxJS

## Status
Accepted

## Context
The frontend needs a robust way to manage state, handle asynchronous data fetching, and clearly separate business logic from presentation components. While standard React hooks or libraries like TanStack Query are common, encapsulating logic in a Service-Oriented Architecture (SOA) provides superior decoupling and testability for a Task MVP.

## Decision
We will implement a **Service-Oriented Architecture (SOA)** on the frontend and use **RxJS** for state management. 
- **Services**: API communication and business logic will be abstracted into singleton service classes (e.g., `TodoService`, `AuthService`).
- **State Management**: Services will expose state via RxJS `BehaviorSubject` and `Observable` streams.
- **Components**: React components will remain purely presentational, subscribing to the RxJS observables provided by the services to receive data, loading, and error states.

## Consequences
- **Positive**: Highly decoupled architecture. Business logic is easily testable outside of the React component tree. RxJS provides extremely powerful tools for handling complex asynchronous data streams and race conditions.
- **Negative**: Introduces a steeper learning curve and a slightly more verbose setup compared to standard React hooks. Requires careful management of RxJS subscriptions to avoid memory leaks (though custom hooks like `useObservable` can mitigate this).
