# Task Management App

A production-ready Task Management application built with **.NET 10** and **React 19**, demonstrating advanced architectural patterns, bulletproof data isolation, and real-time features.

## 🚀 Quick Start

### Prerequisites
- Docker
- VS Code with "Dev Containers" extension

### Setup
1. Clone the repository.
2. **Environment Setup**:
   - Copy the environment templates to create your local development configuration:
     ```bash
     cp TaskManagement.Api/.env.example TaskManagement.Api/.env.development
     cp TaskManagement.Client/.env.example TaskManagement.Client/.env.development
     ```
3. Open in VS Code.
3. Click **"Reopen in Container"** when prompted.
4. The DevContainer will automatically:
   - Spin up a **PostgreSQL 18** database.
   - Install .NET 10 SDK and Node.js 24.
   - Configure all necessary tools.

### Running the App
1. **Backend**:
   ```bash
   cd TaskManagement.Api
   dotnet run
   ```
   *The database will auto-migrate on startup.*
   *API available at: `http://localhost:5000`*
   *Interactive Docs (Scalar): `http://localhost:5000/scalar/v1`*

2. **Frontend**:
   ```bash
   cd TaskManagement.Client
   npm install
   npm run dev
   ```
   *App available at: `http://localhost:5173`*

## 🏗️ Architecture Highlights

### Backend
- **PostgreSQL Row Level Security (RLS)**: Data isolation is enforced at the database engine level. Accidental data leaks are impossible as the DB rejects queries not matching the session's `app.current_user_id`.
- **JWT Authentication**: Secure, stateless authentication using ASP.NET Core Identity.
- **SignalR**: Real-time WebSocket notifications when tasks are assigned.
- **Clean DTO Architecture**: Strict separation between EF entities and API contracts using AutoMapper and separate DTOs for each operation.
- **Abstracted Blob Storage**: Media uploads (images/videos) are handled via an `IStorageService` abstraction, making the app cloud-ready.

### Frontend
- **Service-Oriented Architecture (SOA)**: Business logic and API communication are encapsulated in singleton services.
- **RxJS State Management**: Services expose state via Observables, providing a reactive and highly decoupled UI.
- **Radix UI & Tailwind CSS**: Accessible, unstyled primitives paired with modern utility-first styling.
- **Rich Text Editor**: Support for embedded media and formatted task descriptions.

## 📝 Assumptions & Trade-offs
- **Local Storage for JWT**: For this MVP, the JWT is stored in `localStorage`. In a high-security production app, `HttpOnly` cookies would be preferred to mitigate XSS risks.
- **Local File Storage**: Media is stored in `wwwroot/uploads` for the MVP. The architecture is designed to swap this for AWS S3 or Azure Blob Storage by changing one line in `Program.cs`.
- **Auto-Migrations**: The app auto-migrates on startup in Development mode for a seamless reviewer experience. In production, migrations would be part of a CI/CD pipeline.

## 📈 Scalability & Future Work
- **Microservices**: The clear separation of the Task and Notification logic makes it easy to split these into microservices if needed.
- **Caching**: Implement Redis for distributed caching of frequent queries.
- **Search**: Integrate Elasticsearch for full-text search across rich text task content.
- **Testing**: Expand integration test coverage to include SignalR hubs and RLS policy edge cases.

## 🛠️ API Testing
A **Bruno** collection is provided in the `/bruno` directory. It includes pre-configured environments for automatic token injection after login.
