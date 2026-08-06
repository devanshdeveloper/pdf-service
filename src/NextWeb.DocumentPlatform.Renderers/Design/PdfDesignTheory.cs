/*
 * PDF Design Theory — NextWeb Document Platform
 *
 * Principles (apply to every template):
 * 1. Print-first — sufficient contrast; status always includes text, not color alone.
 * 2. Typographic hierarchy — Montserrat for titles, Arial for body; one accent per document theme.
 * 3. Minimal chrome — hairline borders and light surfaces instead of heavy grids.
 * 4. Numeric integrity — only product/description columns wrap; amounts stay single-line and never
 *    use ellipsis (PdfTable.Amount); rate/amount columns use minimum widths from PdfPrimitives.
 * 5. Tight but readable layout — reduced margins/padding; whitespace separates sections.
 * 6. Theme accent — used for titles, table headers (primary docs), totals, and approved status.
 *
 * Layer model:
 *   Primitives  → raw tokens (colors, spacing, font sizes)
 *   Theme       → per-document accent pair
 *   Semantics   → role-based mapping (TextMuted, SurfaceCard, BorderDefault, …)
 *   Components  → reusable blocks (divider, status chip, section title)
 *   Table       → row/cell text rules shared by all line-item tables
 */

namespace NextWeb.DocumentPlatform.Renderers.Design;

/// <summary>Documents the design system; see file header for principles.</summary>
public static class PdfDesignTheory
{
    public const string Version = "1.0";
}
