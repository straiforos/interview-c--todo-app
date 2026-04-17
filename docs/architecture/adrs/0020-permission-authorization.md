# ADR 0020: Dynamic Permission-Based Authorization

## Status
Accepted

## Context
While Row Level Security (RLS) handles data ownership at the database layer, the application requires a granular mechanism to control access to specific functional features (e.g., "Can this user upload media?", "Can this user delete tasks?"). Standard role-based assignments often lack the precision needed for complex domain requirements, where access rights may need to evolve independently of a user's organizational role. We need a system that supports real-time policy enforcement and scales without requiring frequent modifications to the identity provider or token structure.

## Decision
We will implement a **Dynamic Permission-Based Authorization** system:

1.  **Permission Model**: Decouple authorization from roles by defining granular permission strings (e.g., `Tasks.Delete`, `Media.Upload`). These strings are enumerated in an `AppPermission` enum for type-safety and discoverability.
2.  **Relational Schema**:
    *   `Permission` entity: A canonical registry of functional capabilities. The `Name` property is used as the unique natural key in the database to avoid the inflexibility of ordinal persistence.
    *   `RolePermission` entity: A many-to-many mapping that allows permissions to be grouped into logical roles.
3.  **Authorization Infrastructure**:
    *   `PermissionRequirement`: A custom `IAuthorizationRequirement` that encapsulates a permission key.
    *   `PermissionAuthorizationHandler`: A custom `AuthorizationHandler` that evaluates the user's effective permissions by querying the database/cache.
    *   `PermissionPolicyProvider`: A dynamic provider that automatically generates authorization policies for any requested permission string, eliminating manual policy registration.
4.  **Declarative Enforcement**: A custom `[HasPermission]` attribute to provide a clean, readable developer experience for securing endpoints.
5.  **Performance Optimization**: Implement a short-lived memory cache within the handler to balance real-time permission updates with system throughput.

## Consequences
- **Positive**: 
    - **Granularity**: Enables fine-grained control over every API endpoint and business operation.
    - **Flexibility**: Permissions can be reassigned to roles at runtime without code changes or user re-authentication.
    - **Security**: Keeps the JWT payload minimal, reducing the attack surface and preventing token bloat.
- **Negative**: 
    - **Latency**: Introduces a database or cache lookup during the authorization pipeline.
    - **Maintenance**: Requires managing a registry of permissions and their assignments to roles.
