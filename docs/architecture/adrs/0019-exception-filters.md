# ADR 0019: Exception Handling via Action Filters

## Status

Accepted

## Context

In a clean architecture, controllers should ideally focus on the "happy path" of request processing. Business logic failures (e.g., resource not found, unauthorized access) often result in exceptions being thrown from the service layer. While global middleware can handle these, sometimes we want more granular control similar to Spring Boot's `@ExceptionHandler` at the controller or base controller level to translate specific exceptions into appropriate REST status codes (404, 403, etc.).

## Decision

We will implement an **Exception Handling Action Filter** to provide granular, declarative error handling:

1. `**ApiExceptionFilterAttribute`**: We will create a custom `ExceptionFilterAttribute`.
2. **Exception Mapping**: The filter will contain a dictionary or switch statement that maps specific exception types to `IActionResult` results (e.g., `NotFoundException` -> `NotFoundResult`).
3. **Application**: We will apply this attribute to our `BaseCrudController`. This ensures that all inheriting controllers (Tasks) automatically benefit from this "aspect" without needing `try-catch` blocks in their actions.

## Consequences

- **Positive**: 
  - **Happy Path Controllers**: Controller actions remain extremely clean and readable.
  - **Granular Control**: Allows for different error handling strategies for different controllers if needed, while still providing a robust base implementation.
  - **Consistency**: Ensures that specific business exceptions always result in the same HTTP status codes across the entire API.
- **Negative**: Requires defining custom exception types in the service layer to trigger the specific mappings.

