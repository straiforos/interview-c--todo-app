# ADR 0022: Router-Driven Frontend Architecture with RxJS Resolvers

## Status

Accepted

## Context

As the application grows, managing state solely within components leads to "prop drilling" and complex lifecycle management. We need a robust way to handle data fetching, state synchronization with the URL, and performance optimization to ensure a "snappy" user experience.

## Decision

We will implement a **Router-Driven Architecture** using `react-router` v7, integrated with our **Service-Oriented Architecture (SOA)** and **RxJS**:

1. **URL-Driven State**: The URL will be the "source of truth" for the application state (e.g., `/tasks/:id` for detail view). This ensures deep-linking and browser history work out-of-the-box.
2. **RxJS Resolvers**: We will implement a "Resolver" pattern using RxJS. Before a route renders, a service-based resolver will:
  - Initiate the data fetch via an RxJS `Observable`.
  - Emit the data into a `BehaviorSubject` within the service.
  - The router's `loader` function will await the first emission or use the current value if already present.
3. **Memoization & Caching**:
  - **Service-Level Caching**: RxJS `shareReplay(1)` will be used in services to memoize API responses, preventing redundant network requests when navigating back and forth.
  - **Component-Level Memoization**: Use `React.memo`, `useMemo`, and `useCallback` to prevent unnecessary re-renders of expensive shared components.
4. **Shared Components Library**: Standardize UI using a `shared` directory containing Radix UI primitives styled with Tailwind CSS 4.2. These components will be pure and highly optimized.
5. **Declarative Loading/Error States**: Use React Router's `Navigation` state and `ErrorBoundary` components to handle transition states globally, keeping individual components focused on the "happy path."

## Consequences

- **Positive**: 
  - **Predictability**: URL-driven state makes the app's behavior highly predictable and easy to debug.
  - **Performance**: Memoization and RxJS caching ensure a "snappy" feel by eliminating redundant fetches and renders.
  - **Separation of Concerns**: Components focus on presentation, while services and routers handle data orchestration.
- **Negative**: 
  - **Complexity**: Requires a deeper understanding of RxJS and React Router's data loading APIs.
  - **Boilerplate**: Slightly more setup for each route compared to simple `useEffect` fetches.

