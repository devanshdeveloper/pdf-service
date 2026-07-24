---
name: document-engine-standards
description: Standing architecture and coding rules for the Document Rendering Platform
scope: workspace
---

# Document Rendering Platform — Standing Rules

## Architecture (must not be violated)
- Layered pipeline: Controllers → Application Layer → Document Engine →
  Template Resolver → Renderer Factory → QuestPDF Renderer → Reusable
  Components → Utilities → PDF.
- No hardcoded per-document branching (no `if(document=="Invoice")`).
  All document logic routes through the Document Registry + Template
  Resolver + Renderer Factory.
- The rendering service owns NO business logic. It only forwards
  authentication, headers, query/route params, and body to the
  upstream ERP API, receives JSON, and renders. Treat it as a proxy.

## Templates & Components
- Every document type may have many templates (e.g. Invoice: Modern,
  Classic, Thermal, GST Standard, Retail). Adding a template must
  require minimal code change — no redeploy-per-template patterns.
- Layout sections (Header, Company Info, Customer Info, Address Block,
  GST Block, Items Table, Summary, Bank Details, Terms, Notes, Footer,
  QR Code, Signature, Stamp) are independent, reusable QuestPDF
  components. Never duplicate layout code across templates.
- Maintain a Document Registry mapping Document Type → Supported
  Templates → Supported Versions → Renderer → Validation Rules →
  Configuration → Permissions.

## Configuration & Security
- Nothing hardcoded: base URLs, timeouts, retry counts, storage paths,
  fonts, themes, colors, margins, default template, logo path,
  secrets, connection strings all come from config/env.
- Support Bearer Token, API Key, Cookie, JWT, and be ready for OAuth.
  Never log passwords, tokens, secrets, or private keys.
- Never expose internal exceptions to API clients — return
  descriptive, sanitized error messages only.

## Reliability
- Exponential backoff with configurable retry count and timeout
  policy for upstream calls. No infinite retries.
- Structured logging with correlation IDs, execution time, document
  type, template, request ID, rendering duration.
- Expose /health, /ready, /version endpoints.

## Coding Standards
- Clean Architecture, SOLID, DI, async/await + CancellationTokens,
  nullable reference types, strong typing, XML docs, consistent
  naming. No large monolithic classes. No duplicated code.

## PDF Output Requirements
- Support A4/A5/Letter/Thermal/custom sizes, portrait & landscape,
  repeating headers/footers, page numbering, multi-page tables with
  automatic row splitting, QR/barcodes, watermarks, digital signature
  placeholders, conditional section visibility.
- When a reference/sample PDF is supplied, match its layout, spacing,
  alignment, typography hierarchy, and table structure as closely as
  possible (minor border/color differences are acceptable).

## Testing (required, not optional)
- Unit tests: component rendering, formatting, calculations, layout
  rules, template resolution.
- Integration tests: API proxying, auth forwarding, PDF generation,
  error handling.
- Regression tests: template changes must not silently alter
  previously-shipped layouts.
- Visual/snapshot tests where feasible.

## Verification Before Marking Work Done
- Always run the solution and generate at least one real sample PDF
  per touched template before reporting a task complete. If a browser
  or file-preview tool is available, use it to visually confirm the
  rendered PDF/HTML preview rather than only checking that the build
  succeeds.
