# ADR 0009: Validation Strategy and DTO Design

## Status
Accepted

## Context
We need a robust way to validate incoming data. While some frameworks use Validation Groups (e.g., `@Validated` in Spring) to apply different rules to a single DTO depending on the context (Create vs. Update), the idiomatic .NET approach is to use separate DTOs for different operations to ensure strict API contracts.

## Decision
We will use **ASP.NET Core Data Annotations** (e.g., `[Required]`, `[MaxLength]`) combined with **Separate DTOs** (e.g., `CreateTaskDto`, `UpdateTaskDto`). 
- Each DTO will define its own validation rules via attributes directly on the properties.
- ASP.NET Core will automatically validate these DTOs and return a `400 Bad Request` with a `ProblemDetails` payload if validation fails.

## Consequences
- **Positive**: Provides the cleanest API contract. Swagger/OpenAPI documentation will accurately reflect exactly which fields are required for a POST versus a PUT request. It strictly adheres to the Single Responsibility Principle (by separating operations) and prevents over-posting attacks. Using built-in Data Annotations keeps the project dependency-free and is the most standard, out-of-the-box .NET approach.
- **Negative**: Data Annotations mix validation rules directly into the data container (DTO), which can make complex or conditional validation rules harder to implement and test compared to a dedicated validation library.
