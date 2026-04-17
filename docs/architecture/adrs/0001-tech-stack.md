# ADR 0001: Use .NET 10 Web API and React 19

## Status

Accepted

## Context

The project requires building a Todo application to demonstrate production-ready coding practices. The chosen stack is .NET Core and React. We want to ensure we are using the absolute latest stable versions available in 2026.

## Decision

We will use **.NET 10 Web API** (the latest LTS version) for the backend and **React 19.2** with **Vite 8** for the frontend.

## Consequences

- **Positive**: Aligns with the requested tech stack using the absolute latest stable versions available in 2026. .NET 10 provides excellent performance and modern C# features. React 19.2 and Vite 8 offer a fast and modern frontend build experience.
- **Negative**: Requires managing two separate build pipelines and running two development servers (which will be mitigated by a DevContainer setup).

