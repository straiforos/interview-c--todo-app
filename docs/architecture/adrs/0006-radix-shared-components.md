# ADR 0006: Radix UI and Shared Components

## Status
Accepted

## Context
The frontend requires a consistent, accessible, and highly reusable set of UI components (e.g., buttons, dialogs, forms, dropdowns). Building complex, accessible UI components from scratch is error-prone and time-consuming. We need a foundation that provides accessibility and behavior without imposing a rigid visual style, allowing us to implement our own design system using Tailwind CSS.

## Decision
We will use **Radix UI Primitives** as the foundation for our interactive UI elements. 
Furthermore, we will establish a **Shared Components** architecture (`src/components/ui`). All base UI elements (like `Button`, `Dialog`, `Input`) will be built as reusable components wrapping Radix primitives and styled with Tailwind CSS, managed via the **shadcn CLI**. Feature-specific components will compose these shared elements.

## Consequences
- **Positive**: Guarantees WAI-ARIA compliance, keyboard navigation, and focus management out-of-the-box. The shared component library ensures visual consistency and accelerates feature development.
- **Negative**: Adds dependencies for the specific Radix primitives we use. Developers need to familiarize themselves with Radix's composition patterns (e.g., `asChild`).
