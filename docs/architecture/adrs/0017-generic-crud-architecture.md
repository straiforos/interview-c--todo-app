# ADR 0017: Generic CRUD Interfaces and Base REST Controller

## Status
Accepted

## Context
As the application grows, implementing repetitive CRUD (Create, Read, Update, Delete) logic for every entity leads to code duplication, inconsistent API patterns, and increased maintenance effort. We need a standardized way to handle basic data operations while maintaining flexibility for entity-specific logic. Additionally, we want to support advanced features like batch operations, partial updates (JSON Patch), and soft deletion across the system.

## Decision
We will implement a standardized, generic CRUD architecture:

1.  **Generic CRUD Interfaces**:
    *   `ICreateRepository<TEntity>`: Methods for `AddAsync` and `AddAllAsync`.
    *   `IReadRepository<TEntity>`: Methods for `FindByIdAsync` and `FindAllAsync`.
    *   `IUpdateRepository<TEntity>`: Methods for full updates and `JsonPatch` support.
    *   `IDeleteRepository<TEntity>`: Standardized `DeleteAsync` which will be implemented as a **Soft Delete** (setting an `IsDeleted` flag rather than removing the row).
    *   `ICrudRepository<TEntity>`: A consolidated interface inheriting from the above.

2.  **Base REST Controller**:
    *   A generic `BaseCrudController<TEntity, TDto, TCreateDto, TUpdateDto>` will be created.
    *   It will provide default implementations for standard REST actions (`GET`, `POST`, `PUT`, `PATCH`, `DELETE`).
    *   It will leverage AutoMapper for DTO/Entity conversion and the generic repository for data access.

3.  **Soft Delete Pattern**:
    *   Entities requiring soft delete will implement an `ISoftDeletable` interface.
    *   The `AppDbContext` will be configured with a Global Query Filter to automatically exclude soft-deleted records from all queries, unless explicitly requested.

## Consequences
- **Positive**: 
    - **Consistency**: Ensures all API endpoints follow the same REST patterns and status codes.
    - **Efficiency**: Dramatically reduces boilerplate code for new entities.
    - **Safety**: Soft delete prevents accidental permanent data loss.
    - **Flexibility**: JSON Patch allows for efficient partial updates without sending the entire object.
- **Negative**: Adds a layer of abstraction that can be harder to debug if not well-documented. Requires entities to adhere to specific interface structures.
