# ADR 0023: Internationalization with LinguiJS

## Status
Accepted

## Context
As a production-ready application, supporting multiple languages (i18n) is a core requirement. We need a solution that is developer-friendly, type-safe, and has minimal impact on the application's performance and bundle size. Standard solutions like `react-i18next` often involve runtime overhead for parsing large JSON files and lack strong compile-time checks.

## Decision
We will implement **LinguiJS** for internationalization:

1.  **Macro-Based Extraction**: Use `@lingui/macro` to define translatable strings in the code. This allows for a clean developer experience using template literals (e.g., `t`Task Name``) and JSX components (e.g., `<Trans>`).
2.  **Build-Time Compilation**: Leverage the Lingui Vite plugin to compile message catalogs into optimized JavaScript modules at build time. This ensures zero runtime parsing overhead and smaller bundle sizes.
3.  **PO Format**: Use the industry-standard `.po` (Gettext) format for translation catalogs, which supports pluralization rules and rich metadata for translators.
4.  **SOA Integration**: Create an `i18n.service.ts` using **RxJS** to manage the active locale and dynamic loading of message catalogs, ensuring the UI remains reactive to language changes.
5.  **Type Safety**: Utilize Lingui's generated types to ensure that all message keys used in the application are valid and present in the catalogs.

## Consequences
- **Positive**: 
    - **Performance**: Compiled catalogs lead to a "snappy" experience with no runtime translation overhead.
    - **Developer Experience**: Automated extraction (`lingui extract`) eliminates manual synchronization of translation keys.
    - **Scalability**: The PO format and macro system are designed to handle complex pluralization and nesting as the app grows.
- **Negative**: 
    - **Tooling Dependency**: Requires a specific build-time setup (Vite plugin and Babel macros).
    - **Learning Curve**: Developers need to understand the macro-based workflow compared to traditional key-value lookups.
