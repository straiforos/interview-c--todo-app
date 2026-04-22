# ADR 0007: Authentication and Authorization Strategy

## Status
Accepted

## Context
The application requires secure user authentication (AuthN) and strict data isolation (AuthZ) to ensure users can only access their own Task items. Aligning the .NET implementation with established security concepts (like `ClaimsPrincipal` and `Claims`) provides a clear mental model. Furthermore, to maintain strict control over API contracts (DTOs) and avoid returning internal structures, we need to avoid opaque built-in endpoints that don't allow customization of the response format.

## Decision
We will implement the following security architecture:

1. **ASP.NET Core Identity**: Used for underlying user management, database schema generation, and secure password hashing.
2. **JWT Bearer Authentication**: Used for stateless API security.
3. **Custom AuthController**: Instead of using the built-in `.MapIdentityApi()`, we will build custom login and registration endpoints. This ensures we have complete control over the input/output DTOs and error responses.
4. **Data Isolation via `ICurrentUserService`**: We will create an `ICurrentUserService` that wraps `IHttpContextAccessor` to extract the `UserId` claim from the authenticated `ClaimsPrincipal`. The `TaskService` will inject this service to filter all database queries, ensuring strict data ownership without trusting client-provided user IDs.

## Consequences
- **Positive**: Provides enterprise-grade security, strict data isolation, complete control over API contracts, and highly testable business logic (since `ICurrentUserService` can be mocked in unit tests).
- **Negative**: Requires writing custom boilerplate for the login and registration endpoints instead of relying on the faster, but less flexible, built-in minimal APIs.
