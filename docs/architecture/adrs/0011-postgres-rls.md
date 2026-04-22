# ADR 0011: PostgreSQL Row Level Security (RLS)

## Status
Accepted

## Context
Data isolation is a critical requirement. While application-level filtering (e.g., EF Core Global Query Filters or manual `WHERE` clauses) is common, it is susceptible to developer error. If a developer forgets to apply the filter, data leaks across tenants/users. For a true "Production MVP," we want a bulletproof mechanism to ensure users can only access their own data or data assigned to them.

## Decision
We will implement **True PostgreSQL Row Level Security (RLS)**.
1. **Database Policies**: We will define RLS policies in PostgreSQL (via raw SQL in EF Core migrations) on the `Tasks` and `Notifications` tables. For example, a user can only `SELECT`, `UPDATE`, or `DELETE` a `Task` if their ID matches the `CreatorId`.
2. **Session Context**: We will use an EF Core `DbConnectionInterceptor`. Whenever EF Core opens a database connection, the interceptor will read the current user's ID from the `ICurrentUserService` and execute `SET LOCAL app.current_user_id = '...';`. The PostgreSQL RLS policies will read this session variable to enforce access.

## Consequences
- **Positive**: Bulletproof security at the database engine level. It is impossible for application code to accidentally leak data, even with raw SQL queries or forgotten `WHERE` clauses. Demonstrates advanced enterprise architecture.
- **Negative**: Adds complexity to the EF Core setup (interceptors) and requires writing raw SQL for the RLS policies in migrations, bypassing some of EF Core's abstractions.
