---
name: questpdf
description: "Generate complex, data-driven PDF reports and invoices using QuestPDF."
risk: safe
date_added: "2026-07-24"
version_crawled: "2025.4.0"
project_version: "2026.7.1"
---

# QuestPDF

QuestPDF is a modern C# library for generating PDF documents. It uses a fluent API to build documents out of layout elements such as rows, columns, and tables, without relying on HTML-to-PDF conversion.

> [!WARNING]
> **Version Notice**: This skill was generated from the QuestPDF docs which indicate features up to version **2025.4.0**. The current project uses version **2026.7.1**. We have verified that the core syntax compiles safely against the project's dependency, with no breaking changes reported by QuestPDF between these versions. The patterns here are authoritative for `2026.7.1`.

## When to Use This Skill

Reach for this skill when you need to:
- Generate PDF invoices, vouchers, purchase orders, or complex enterprise reports.
- Implement bordered, multi-page, or gridded layouts.
- Conditionally render document sections.
- Embed fonts and render multi-currency symbols (e.g. ₹, $, €).

## Quick Start

```csharp
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

Document.Create(container =>
{
    container.Page(page =>
    {
        page.Size(PageSizes.A4);
        page.Margin(2, Unit.Centimetre);
        page.PageColor(Colors.White);
        page.DefaultTextStyle(x => x.FontSize(12));

        page.Header().Text("Hello PDF!").SemiBold().FontSize(24).FontColor(Colors.Blue.Darken2);
        
        page.Content().PaddingVertical(1, Unit.Centimetre).Column(col =>
        {
            col.Item().Text("This is a simple document.");
        });

        page.Footer().AlignCenter().Text(x => 
        {
            x.Span("Page ");
            x.CurrentPageNumber();
        });
    });
})
.GeneratePdf("output.pdf");
```

## Directory Map

- `references/`: Topic-by-topic markdown guides adapted from the official documentation.
  - **Layouts.md**: Alignment, Borders, Aspect Ratios, Columns, Padding, etc.
  - **Tables.md**: Multi-page tables, repeating headers, cell borders.
  - **Text.md**: Text styling, hyperlinks, paragraphs.
  - **Images.md**: Image rendering and placeholders.
  - **DynamicComponents.md**: IComponent implementations, injection.
  - **PageSettings.md**: Margins, Page sizes, Page breaks.
  - **Fonts.md**: Font embedding and text shaping.
  - **Debugging.md**, **Performance.md**, **CompanionApp.md**, **Licensing.md**, etc.
- `examples/`: Hand-picked, fully runnable examples verifying the syntax.
- `templates/`: Full end-to-end boilerplate for an Enterprise Invoice (`InvoiceTemplate.cs`).

