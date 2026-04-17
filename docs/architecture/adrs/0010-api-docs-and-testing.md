# ADR 0010: API Documentation and Testing Strategy

## Status
Accepted

## Context
Providing an effortless way for other developers to interact with the API is a hallmark of a production-ready project. While OpenAPI (Swagger) provides a great interactive UI, a dedicated API client collection with pre-configured environments (like automatic token injection) offers a superior testing experience for end-to-end workflows.

## Decision
We will provide two methods for API exploration and testing:
1. **OpenAPI (Swagger)**: We will configure .NET 10's OpenAPI support to generate a Swagger UI. This will be configured to accept JWT Bearer tokens so endpoints can be tested directly from the browser.
2. **Bruno Collection**: We will include a `bruno` directory in the root of the repository containing a [Bruno](https://www.usebruno.com/) API collection. Bruno is an open-source, plain-text API client. The collection will include all endpoints (Auth, Tasks) and a configured environment that automatically extracts the JWT from the Login response and applies it to subsequent requests.

## Consequences
- **Positive**: Bruno collections are stored as plain text, making them perfectly suited for Git version control (unlike Postman's opaque JSON exports). Developers have multiple, frictionless ways to test the API.
- **Negative**: Developers will need to install the Bruno VS Code extension or the desktop client to use the collection, though the Swagger UI remains available as a zero-install fallback.
