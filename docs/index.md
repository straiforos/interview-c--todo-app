---
# VitePress configuration and home page
layout: home

hero:
  name: "Task App Docs"
  text: "Architecture & Developer Guide"
  tagline: "Production-ready .NET 10 & React 19 Task Management Application"
  actions:
    - theme: brand
      text: View Architecture
      link: /architecture/
    - theme: alt
      text: View ADRs
      link: /architecture/adrs/

features:
  - title: Clean Architecture
    details: DTOs, thin controllers, and service layers in .NET 10.
  - title: Modern Frontend
    details: RxJS-powered Service-Oriented Architecture, Radix UI, Tailwind CSS 4.2, and Vite 8.
  - title: Documented Decisions
    details: Architecture Decision Records (ADRs) explaining the 'why' behind the code.
---

## 📸 Application Preview

### Role-Based Access Control (Admin; Delete Permissions)
![Admin vs ReadOnly Role](./assets/admin-readonly-role.gif)

### Login & ReadOnly Role (View Only Permissions)
![Login & ReadOnly Role](./assets/login-readonly-role.gif)

### User Role Update View (Edit Permissions)
![User Role Update View](./assets/user-role-update-view.gif)

### Internationalization (i18n)
![Internationalization](./assets/internationalization.gif)

