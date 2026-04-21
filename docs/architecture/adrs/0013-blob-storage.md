# ADR 0013: Media Blob Storage Strategy

## Status

Superseded by [ADR 0021: Simplified Scope and DTO Pattern](./0021-simplified-scope.md)

## Context

The Task Management application initially considered supporting rich text content and media. However, to focus on the DTO pattern and clean architecture for the interview take-home, this feature was removed.

## Decision

We will implement an **Abstracted Blob Storage Architecture**:

1. **Interface**: Create an `IStorageService` interface in the backend to handle `UploadAsync` and `DeleteAsync` operations.
2. **MVP Implementation**: For the local DevContainer environment, we will implement a `LocalFileStorageService` that saves uploaded media to the `wwwroot/uploads` directory and serves them as static files. This keeps the local setup simple without requiring additional Docker containers (like MinIO).
3. **API Endpoint**: A `MediaController` will expose a `POST /api/media/upload` endpoint. The rich text editor on the frontend will intercept media drops/pastes, upload the file to this endpoint, and insert the returned URL into the rich text content.

## Consequences

- **Positive**: Prevents database bloat. The `IStorageService` abstraction ensures that transitioning to a true cloud blob storage provider (like AWS S3 or Azure Blob Storage) in a production environment requires only swapping the dependency injection implementation, with zero changes to the controllers or frontend.
- **Negative**: Requires configuring static file serving in the ASP.NET Core pipeline and managing local file paths during development.

