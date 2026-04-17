# ADR 0003: DTOs and AutoMapper

## Status
Accepted

## Context
Returning EF entities directly from controllers is a known anti-pattern that tightly couples the API contract to the database schema. The API contract must be decoupled from the database schema to ensure maintainability and security.

## Decision
We will implement a strict **DTO (Data Transfer Object)** architecture. 
- Controllers will only accept and return DTOs.
- **AutoMapper** will be used to map between DTOs and EF Core entities in the service layer.

## Consequences
- **Positive**: Prevents over-posting attacks, hides internal database structures, and allows the API contract to evolve independently of the database schema.
- **Negative**: Adds slight boilerplate (creating DTO classes and mapping profiles).
