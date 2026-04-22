# Task Management App - Interview Take-Home

A Task Management application built with **.NET 10** and **React 19**. This project was developed as an interview take-home assignment to demonstrate advanced architectural patterns, bulletproof data isolation, and modern full-stack development practices.

## 📸 Application Preview

Here are some quick demonstrations of the application's core features:

### Role-Based Access Control (Admin vs ReadOnly)
![Admin vs ReadOnly Role](docs/assets/admin-readonly-role.gif)

### Login & ReadOnly Role
![Login & ReadOnly Role](docs/assets/login-readonly-role.gif)

### User Role Update View
![User Role Update View](docs/assets/user-role-update-view.gif)

### Internationalization (i18n)
![Internationalization](docs/assets/internationalization.gif)

## 🚀 Zero-Setup Development Environment

This project uses **VS Code Dev Containers** to provide a completely automated, zero-setup development environment. **You do not need to install .NET, Node.js, or PostgreSQL on your local machine.**

### How to run:
1. Clone the repository and open the folder in **VS Code**.
2. When prompted, click **"Reopen in Container"** (requires the Dev Containers extension and Docker).
3. The container will automatically:
   - Spin up a **PostgreSQL 18** database.
   - Install the **.NET 10 SDK** and **Node.js 24**.
   - Restore all backend and frontend dependencies.
   - Apply database migrations automatically on startup.

### Running the App (Inside the Dev Container)

**Backend (API):**
```bash
cd TaskManagement.Api
dotnet run
```
* API available at: `http://localhost:5000`
* Interactive API Docs (Scalar): `http://localhost:5000/scalar/v1`

**Frontend (Client):**
```bash
cd TaskManagement.Client
npm run dev
```
* App available at: `http://localhost:5173`

*(Note: Test credentials can be created via the Register page, or you can use the built-in Demo Role Swapper on the Profile page after registering).*

---

## 🏗️ Architecture & Technical Highlights

This project is driven by a series of **Architecture Decision Records (ADRs)** located in the `docs/architecture/adrs/` directory. Key highlights include:

### Backend (.NET 10 Web API)
- **PostgreSQL Row Level Security (RLS)**: Data isolation is enforced at the database engine level via EF Core Interceptors. Accidental data leaks are impossible as the DB rejects queries not matching the session's `app.current_user_id` or `app.user_role`.
- **Granular Roles & Permissions**: Implemented a robust RBAC system with `Admin`, `User`, and `ReadOnly` roles. Permissions are embedded in JWT claims and enforced across the stack.
- **Clean DTO Architecture**: Strict separation of concerns using AutoMapper. Internal EF Core entities are never exposed. We use lightweight `TaskSummaryDto` for lists and detailed `TaskDto` for single-item views.
- **Generic CRUD & SOA**: Leverages a generic repository/service pattern (`ICrudRepository`, `ICrudService`) to minimize boilerplate while maintaining a strict Service-Oriented Architecture.
- **Aspect-Oriented Programming (AOP)**: Uses Action Filters for declarative global exception handling (mapping exceptions to standard `ProblemDetails` responses).

### Frontend (React 19 + Vite)
- **Router-Driven Architecture**: Utilizes React Router v6 Data API (`createBrowserRouter`, `loader`). Data fetching and permission guards (`requirePermission`) happen *before* components render, eliminating layout shift and loading spinners.
- **RxJS State Management**: Business logic and API communication are encapsulated in singleton services (`TaskService`, `AuthService`). Components subscribe to Observable streams for a reactive, decoupled UI.
- **12-Factor App (Runtime Configuration)**: The frontend is built once and configured at runtime via a `config.json` file, adhering to the "Build once, deploy anywhere" principle.
- **Modern UI/UX**: Built with **Tailwind CSS** and **shadcn/ui** (Radix UI primitives) for an accessible, beautiful, and responsive design.
- **Internationalization (i18n)**: Fully integrated with **LinguiJS** for robust, macro-based translations.

---

## 📚 Documentation & Testing

- **Architecture Decision Records (ADRs)**: A complete history of technical decisions is documented using VitePress. You can view the docs locally by running:
  ```bash
  cd docs
  npm install
  npm run docs:dev
  ```
- **API Testing**: A **Bruno** collection is provided in the `/bruno` directory, featuring pre-configured environments and automatic JWT token injection for seamless API exploration.

## 📝 Assumptions & Trade-offs
- **Local Storage for JWT**: For this MVP demonstration, the JWT is stored in `localStorage`. In a strict production environment, `HttpOnly` cookies would be implemented to mitigate XSS risks.
- **Auto-Migrations**: The API auto-migrates the database on startup to ensure a seamless reviewer experience. In a real-world scenario, migrations would be applied via a CI/CD pipeline.

