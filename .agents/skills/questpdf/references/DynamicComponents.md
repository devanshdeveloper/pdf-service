# QuestPDF - DynamicComponents

## Injecting custom content


**URL:** https://www.questpdf.com/api-reference/text/injecting-custom-content.html

**Contents:**
- Injecting custom content ​
- Image ​
- SVG ​
- Position ​
    - Example: ​

It is possible to inject custom content into the document, e.g. images.

The element must fit within one line and cannot span multiple pages.

The most common use-case is to inject images into the text.

Another common use-case is to inject SVG icons into the text.

The injected element can be positioned in relation to the text baseline or font edges.

**Examples:**

Example 1 (swift):
```swift
.Text(text =>
{
    text.Span("A unit test can either ");
    text.Element().PaddingBottom(-4).Height(24).Image("unit-test-completed-icon.png");
    text.Span(" pass").FontColor(Colors.Green.Medium);
    text.Span(" or ");
    text.Element().PaddingBottom(-4).Height(24).Image("unit-test-failed-icon.png");
    text.Span(" fail").FontColor(Colors.Red.Medium);
    text.Span(".");
});
```

Example 2 (swift):
```swift
.Text(text =>
{
    text.Span("To synchronize your email inbox, please click the ");
    text.Element().PaddingBottom(-4).Height(24).Svg("mail-synchronize-icon.svg");
    text.Span(" icon.");
});
```

Example 3 (swift):
```swift
.Text(text =>
{
    text.Span("This ");

    text.Element(TextInjectedElementAlignment.AboveBaseline)
        .Width(12).Height(12)
        .Background(Colors.Green.Medium);

    text.Span(" element is positioned above the baseline, while this ");

    text.Element(TextInjectedElementAlignment.BelowBaseline)
        .Width(12).Height(12)
        .Background(Colors.Blue.Medium);

    text.Span(" element is positioned below the baseline.");
});
```

---



---

## Maps


**URL:** https://www.questpdf.com/api-reference/maps.html

**Contents:**
- Maps ​
- Example ​

QuestPDF provides seamless integration with Mapbox Static Maps API, allowing you to embed high-quality, customizable maps into your PDF documents. This integration offers a reliable and efficient way to include geographical visualizations in your reports, documents, or any PDF output.

This section provides examples of how to integrate the Mapbox service with QuestPDF. This service is paid but provides a generous free tier, including commercial usage.

The code below presents a simple helper class that fetches a map image based on the provided coordinates, zoom level, and dimensions.

Please generate your own access token via your Mapbox account before deploying.

You can use the helper class implemented above to fetch a map image and embed it in your document.

Always fetch the map before starting PDF generation. The map retrieval is an asynchronous operation and should not be performed during document generation.

**Examples:**

Example 1 (csharp):
```csharp
static class MapboxStaticMapRenderer
{
    private const string MapboxBaseUrl = "https://api.mapbox.com/styles/v1/mapbox/streets-v12/static";
    private const string AccessToken = "<MAPBOX_TOKEN>";

    public static async Task<byte[]?> FetchStaticMapAsync(double longitude, double latitude, float zoom, int width, int height)
    {
        var longitudeString = longitude.ToString(System.Globalization.CultureInfo.InvariantCulture);
        var latitudeString = latitude.ToString(System.Globalization.CultureInfo.InvariantCulture);
        var url = $"{MapboxBaseUrl}/{longitudeString},{latitudeString},{zoom},0,0/{width}x{height}@2x?access_token={AccessToken}";

        using var client = new HttpClient();
        
        try
        {
            var response = await client.GetAsync(url);
            return await response.Content.ReadAsByteArrayAsync();
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}
```

Example 2 (swift):
```swift
var map = await MapboxStaticMapRenderer.FetchStaticMapAsync(19.9376052f, 50.0616087f, 10, 500, 400);

Document
    .Create(document =>
    {
        document.Page(page =>
        {
            page.ContinuousSize(550);
            page.Margin(25);

            page.Content()
                .Column(column =>
                {
                    column.Item().Text("Map of Kraków").FontSize(20).Bold();
                    column.Item().Text("Capital of Lesser Poland Voivodeship").FontSize(16).Light();
                    column.Item().Height(15);

                    column.Item()
                        .Background(Colors.Grey.Lighten3)
                        .ShowIf(map != null)
                        .Image(map);
                });
        });
    })
    .GeneratePdf("map.pdf");
```

---



---

## Accessibility


**URL:** https://www.questpdf.com/concepts/accessibility.html

**Contents:**
- Accessibility ​
- Introduction ​
  - Tagged PDF ​
  - PDF/A (Archival) ​
  - PDF/UA (Universal Accessibility) ​
- Compliance Tools ​
  - VeraPDF ​
  - PAC (PDF Accessibility Checker) ​
- Minimal Example ​
- Semantic Elements ​

For software developers, PDF accessibility means programmatically creating documents that everyone, including people with disabilities, can use effectively.

At its core, it's about ensuring your generated PDFs—like invoices, reports, or statements—work seamlessly with assistive technologies such as screen readers, braille displays, and navigation software.

An accessible PDF isn't defined by its visual appearance, but by the hidden logical structure you build into the file. This structure, which you create with your code, dictates the correct reading order, identifies headings, describes tables, and explains images. A screen reader cannot interpret a document based on visual layout alone; it relies entirely on the structural information you provide.

This is the foundation of accessibility. A tagged PDF contains hidden structural metadata (tags) that define the document's logical structure. This is very similar to semantic HTML elements (e.g., <h1>, <p>, <table>). These tags describe the meaning of your content, allowing assistive technologies to navigate and read the document correctly.

This standard (ISO 19005) is primarily focused on the long-term preservation of electronic documents, ensuring a file can be opened and viewed reliably many years in the future. While its main goal isn't accessibility, several of its levels (like PDF/A-2a and PDF/A-3a) require the document to also be a Tagged PDF, thus incorporating accessibility as part of the archival requirements.

This is the gold standard for PDF accessibility. PDF/UA (ISO 14289) is a formal standard that specifies exactly how a PDF must be structured to be considered fully accessible. Achieving PDF/UA-1 compliance ensures your document provides the best possible experience for all users. When you enable accessibility features in QuestPDF, this is the standard you are working to meet.

While QuestPDF handles the technical implementation of accessibility tags, it's crucial to validate your output. Generating a compliant document is a critical step, and several excellent tools are available to help you ensure your PDFs meet the necessary standards.

VeraPDF is an open-source, industry-supported tool designed specifically to validate PDF files against the PDF/A (Archival) and PDF/UA (Universal Accessibility) standards.

Developed with support from the PDF Association, VeraPDF performs a deep, technical analysis of a file's structure to confirm it fully conforms to the complex ISO specifications. It's the definitive tool for proving formal compliance, which is often a requirement for legal or archival purposes.

You can download VeraPDF from the official website. Once installed, you can use its command-line interface to check a document:

PAC (PDF Accessibility Checker) is a free Windows tool that focuses on the practical aspects of accessibility. While VeraPDF checks for strict conformance to the standard, PAC checks how usable the document is for people relying on assistive technologies.

This tool is invaluable for developers because it simulates how a screen reader will interpret your document. It provides clear, actionable reports that highlight issues like incorrect reading order, missing image descriptions, or improperly tagged tables. Using PAC helps you move beyond simple compliance to ensure you're delivering a genuinely good experience for all users.

You can download PAC from the official website.

Generating accessible PDF documents with QuestPDF is a straightforward process. Beyond just creating the visual layout, accessibility requires a few key considerations, all demonstrated in the example below.

First, you must apply a semantic structure to the content. This tells assistive technologies what is a header, paragraph, or image, creating a logical reading order. Second, it's essential to provide complete document metadata, such as the document's title and language. Finally, you need to enable the correct conformance settings to formally declare the document as PDF/A and PDF/UA compliant.

Unable to display PDF file. Download instead.

Semantic extension methods allow you to add logical structure to your PDF document, which is essential for accessibility and content extraction. By wrapping your layout elements (containers) with these methods, you are tagging the content according to its meaning, such as a heading, paragraph, or figure.

This "semantic tree" is used by assistive technologies, like screen readers, to navigate and understand the document's structure, making your content accessible to all users. It also improves content reflow and copy-paste behavior.

These methods define the high-level organization of your document, which is essential for accessibility and creating a logical flow.

Headings are fundamental for document navigation and outlining the content's structure. Using them correctly is crucial for accessibility and allowing a PDF reader to generate a navigable Table of Contents.

QuestPDF uses these semantic headers to automatically generate the document outline (often called a Table of Contents) in PDF readers. This allows end-users to quickly browse and jump to different parts of your document, significantly improving usability.

These methods are used to tag common block-level text elements, distinguishing them from other structural elements like headings or lists.

Use this set of methods to properly structure ordered or unordered lists. Correctly tagging list components is vital for screen readers to announce the list structure correctly.

Here is a practical example of how to build a semantically correct list:

Unable to display PDF file. Download instead.

Tables are complex structural elements, and conformance standards require detailed tagging. This process is essential to ensure the document is accessible, especially for users who rely on screen readers to navigate and understand the data relationships.

QuestPDF simplifies this by automatically tagging tables for accessibility when you use the SemanticTable helper method.

It is also possible to mark specific cells as headers. Horizontal headers (often called Row Headers) provide a title or description for the data presented in their respective rows. Tagging them is crucial for accessibility, as it allows screen readers to correctly associate data cells with their corresponding row titles.

For example, a screen reader can announce "Position: Senior Developer" rather than just "Senior Developer."

You can apply this tag using the AsSemanticHorizontalHeader method:

Unable to display PDF file. Download instead.

Use accessibility tags to identify navigational aids within your document, such as a Table of Contents (TOC) or an Index. Properly tagging these sections is crucial for assistive technologies, allowing them to understand the document's structure and provide effective navigation for users.

QuestPDF provides the following methods for tagging these specific elements:

The example below demonstrates how to build a fully functional and accessible Table of Contents. It generates a list of entries, and each entry links to a corresponding section later in the document. The page numbers for each entry are automatically resolved by referencing the target section.

Unable to display PDF file. Download instead.

These methods apply semantic meaning to a portion of text, often within a Text element's span.

This helps assistive technologies, like screen readers, understand the structure and type of content, even when it's mixed with other text.

Use these methods to identify non-text content.

Providing clear and descriptive alternative text for these elements is one of the most important aspects of creating an accessible document.

Unable to display PDF file. Download instead.

This method applies a language attribute to a container, specifying the natural language of its content (e.g., "en-US", "fr-FR", "es-ES").

This is crucial for accessibility, as it allows screen readers to switch to the correct pronunciation rules, ensuring the text is read clearly and accurately.

This method excludes a container and all its children from the PDF's semantic (accessibility) tree. This is essential for decorative elements—such as background shapes, ornamental borders, or layout helper lines—that add visual flair but provide no structural or informational value.

Ignoring these elements prevents screen readers from announcing confusing, non-essential content, leading to a much cleaner and more understandable experience for the user.

**Examples:**

Example 1 (unknown):
```unknown
verapdf document.pdf
```

Example 2 (swift):
```swift
Document
    .Create(document =>
    {
        document.Page(page =>
        {
            page.Size(PageSizes.A5);
            page.Margin(30);

            page.Header()
                .PaddingBottom(15)
                .SemanticHeader1()
                .Text("Accessibility Test Document")
                .FontColor(Colors.Blue.Darken3)
                .FontSize(24)
                .Bold();
            
            page.Content()
                .Column(column =>
                {
                    column.Spacing(20);
                    
                    column.Item()
                        .SemanticSection()
                        .Column(column =>
                        {
                            column.Item()
                                .PaddingBottom(10)
                                .SemanticHeader2()
                                .Text("Section with text content")
                                .FontColor(Colors.Blue.Darken1)
                                .FontSize(16);
                            
                            column.Item()
                                .Text(Placeholders.Paragraphs())
                                .FontSize(12)
                                .ParagraphSpacing(8);
                        });
                    
                    column.Item()
                        .PreventPageBreak()
                        .SemanticSection()
                        .Column(column =>
                        {
                            column.Item()
                                .PaddingBottom(10)
                                .SemanticHeader2()
                                .Text("Section with image")
                                .FontColor(Colors.Blue.Darken1)
                                .FontSize(16);
                            
                            column.Item()
                                .Width(250)
                                .SemanticImage("Image showing a laptop")
                                .Image("Resources/product.jpg");
                        });
                });
        });
    })
    .WithMetadata(new DocumentMetadata
    {
        Language = "en-US",
        Title = "Accessibility Test",
        Subject = "This document shows how easy it is to create accessible PDF documents with QuestPDF"
    })
    .WithSettings(new DocumentSettings
    {
        PDFA_Conformance = PDFA_Conformance.PDFA_3A,
        PDFUA_Conformance = PDFUA_Conformance.PDFUA_1
    })
    .GeneratePdf("accessibility-minimal-example.pdf");
```

Example 3 (swift):
```swift
.SemanticList()
.Column(listColumn =>
{
    listColumn.Spacing(10);
    
    foreach (var i in Enumerable.Range(2, 5))
    {
        listColumn.Item()
            .SemanticListItem()
            .Row(row =>
            {
                row.ConstantItem(20)
                    .SemanticListLabel()
                    .Text($"{i}.");

                row.RelativeItem()
                    .SemanticListItemBody()
                    .Text(Placeholders.Sentence());
            });
    }
});
```

Example 4 (scala):
```scala
.SemanticTable()
.Table(table =>
{
    // table content
});
```

---



---

## Code pattern: components


**URL:** https://www.questpdf.com/concepts/code-patterns/components.html

**Contents:**
- Code pattern: components ​
- Example: address component ​
    - Component definition ​
    - Component usage ​
- Complex example ​
    - Component definition ​
    - Component usage ​

Components in QuestPDF provide a powerful abstraction mechanism for creating reusable content across multiple document types.

By encapsulating specific content generation logic in standalone classes, you can significantly improve the modularity and maintainability of your PDF generation codebase. Components follow a clean separation of concerns principle, ensuring that your document structure remains organized and consistent across various implementations.

This example demonstrates how to create a reusable address component and integrate it into document structure.

By simply passing an Address object to the component, all formatting and layout concerns are delegated to the component itself.

A more advanced example involves a configurable section component that can hold multiple fields, each defined by a label and its own content. This approach provides flexibility for various data types and layout requirements while retaining an organized, maintainable structure.

The component exposes methods for adding text, images, and custom content fields.

Please note how easy it is to create a new section with multiple fields. The layout and styling are encapsulated within the component, ensuring consistency across different sections.

**Examples:**

Example 1 (swift):
```swift
public class Address
{
    public string CompanyName { get; set; }
    
    public string PostalCode { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
}

public class AddressComponent : IComponent
{
    private Address Address { get; }

    public AddressComponent(Address address)
    {
        Address = address;
    }
    
    public void Compose(IContainer container)
    {
        container.Column(column =>
        {
            column.Spacing(10);
            
            AddItem("Company name", Address.CompanyName);
            AddItem("Postal code", Address.PostalCode);
            AddItem("Country", Address.Country);
            AddItem("City", Address.City);
            AddItem("Street", Address.Street);
            
            void AddItem(string label, string value)
            {
                column.Item().Text(text =>
                {
                    text.Span($"{label}: ").Bold();
                    text.Span(value);
                });
            }
        });
    }
}
```

Example 2 (javascript):
```javascript
var address = new Address
{
    CompanyName = "Apple",
    PostalCode = "95014",
    Country = "United States",
    City = "Cupertino",
    Street = "One Apple Park Way"
};

Document
    .Create(document =>
    {
        document.Page(page =>
        {
            page.MinSize(new PageSize(0, 0));
            page.MaxSize(new PageSize(600, 1200));
            page.DefaultTextStyle(x => x.FontSize(20));
            page.Margin(25);

            page.Content()
                .Component(new AddressComponent(address));
        });
    })
    .GeneratePdf("report.pdf");
```

Example 3 (swift):
```swift
using QuestPDF.Infrastructure;

public class SectionComponent : IComponent
{
    private List<(string Label, IContainer Content)> Fields { get; set; } = [];

    public SectionComponent()
    {
        
    }
    
    public void Compose(IContainer container)
    {
        container
            .Border(1)
            .Column(column =>
            {
                foreach (var field in Fields)
                {
                    column.Item().Row(row =>
                    {
                        row.RelativeItem()
                            .Border(1)
                            .BorderColor(Colors.Grey.Medium)
                            .Background(Colors.Grey.Lighten3)
                            .Padding(10)
                            .Text(field.Label);

                        row.RelativeItem(2)
                            .Border(1)
                            .BorderColor(Colors.Grey.Medium)
                            .Padding(10)
                            .Element(field.Content);
                    });
                }
            });
    }

    public void Text(string label, string text)
    {
        Custom(label).Text(text);
    }
    
    public void Image(string label, string imagePath)
    {
        Custom(label).Image(imagePath);
    }
    
    public IContainer Custom(string label)
    {
        var content = IContainer.Empty;
        Fields.Add((label, content));
        return content;
    }
}
```

Example 4 (swift):
```swift
Document
    .Create(document =>
    {
        document.Page(page =>
        {
            page.MinSize(new PageSize(0, 0));
            page.MaxSize(new PageSize(600, 1200));
            page.DefaultTextStyle(x => x.FontSize(20));
            page.Margin(25);

            page.Content()
                .Column(column =>
                {
                    column.Item().Component(BuildSampleSection());
                    // more usages of the section component
                });
        });
    }
    .GeneratePdf("report.pdf");

IComponent BuildSampleSection()
{
    var section = new SectionComponent();

    section.Text("Product name", Placeholders.Label());
    section.Text("Description", Placeholders.Sentence());
    section.Text("Price", Placeholders.Price());
    section.Text("Date of production", Placeholders.ShortDate());
    section.Image("Photo of the product", "Resources/product.jpg");
    section.Custom("Status").Text("Accepted").FontColor(Colors.Green.Darken2).Bold();
    
    return section;
}
```

---



---

## Code pattern: dynamic components


**URL:** https://www.questpdf.com/concepts/code-patterns/dynamic-components.html

**Contents:**
- Code pattern: dynamic components ​
- Simple examples ​
    - Alternating side of page numbers ​
    - Progressbar ​
    - Usage ​
- Table with per-page subtotals ​
    - Model and state ​
    - Paging algorithm ​
    - Usage ​

Dynamic components provide a powerful way to generate conditional or varying content on each page of your PDF document. Unlike standard components that render once for the entire document, dynamic components' Compose method is invoked separately for each page where the component appears.

This page-specific rendering gives you access to crucial context information like the current page number, total page count, and available space. With this information, you can create sophisticated layouts that adapt to their position within the document.

This component places page numbers on alternating sides of the page - left for odd pages and right for even pages. It demonstrates how to use the page number information to conditionally format content.

This component creates a visual progress bar indicating how far the reader has advanced through the document.

The following example demonstrates how to incorporate both dynamic components into a document structure. The progress bar appears in the header, while the alternating page numbers display in the footer.

Unable to display PDF file. Download instead.

This more complex example demonstrates how to create a table that spans multiple pages and displays subtotals for each page. It uses component state to track which items have been shown across pages.

Important: Always treat state as read-only. Never modify existing state directly. Instead, create a new instance of your state struct with the updated values and assign it to the State property. QuestPDF may call the Compose method multiple times per page and may internally change the state.

First, let's define our data model and the state structure.

The following component implements a paging algorithm that:

Here is how you can integrate this component into a document that displays per-page subtotals.

Unable to display PDF file. Download instead.

**Examples:**

Example 1 (swift):
```swift
public class PageNumberSideComponent : IDynamicComponent
{
    public DynamicComponentComposeResult Compose(DynamicContext context)
    {
        var content = context.CreateElement(element =>
        {
            element
                .Element(x => context.PageNumber % 2 == 0 ? x.AlignRight() : x.AlignLeft())
                .Text(text =>
                {
                    text.Span("Page ");
                    text.CurrentPageNumber();
                });
        });

        return new DynamicComponentComposeResult
        {
            Content = content,
            HasMoreContent = false
        };
    }
}
```

Example 2 (swift):
```swift
public class PageProgressbarComponent : IDynamicComponent
{
    public DynamicComponentComposeResult Compose(DynamicContext context)
    {
        var content = context.CreateElement(element =>
        {
            var width = context.AvailableSize.Width * context.PageNumber / context.TotalPages;
                
            element
                .Background(Colors.Blue.Lighten3)
                .Height(5)
                .Width(width)
                .Background(Colors.Blue.Darken2);
        });

        return new DynamicComponentComposeResult
        {
            Content = content,
            HasMoreContent = false
        };
    }
}
```

Example 3 (swift):
```swift
Document
    .Create(document =>
    {
        document.Page(page =>
        {
            page.Size(PageSizes.A4);
            page.Margin(50);
            page.DefaultTextStyle(x => x.FontSize(20));

            page.Header().Column(column =>
            {
                column.Item()
                    .Text("MyBrick Set")
                    .FontSize(48).FontColor(Colors.Blue.Darken2).Bold();
                  
                column.Item()
                    .Text("Building Instruction")
                    .FontSize(24);
                
                column.Item().Height(15);
                
                column.Item().Dynamic(new PageProgressbarComponent());
            });
                
            page.Content().PaddingVertical(25).Column(column =>
            {
                column.Spacing(25);
                
                foreach (var i in Enumerable.Range(1, 30))
                {
                    column.Item()
                        .Background(Colors.Grey.Lighten3)
                        .Height(Random.Shared.Next(4, 8) * 25)
                        .AlignCenter()
                        .AlignMiddle()
                        .Text($"Step {i}");
                }
            });

            page.Footer().Dynamic(new PageNumberSideComponent());
        });
    })
    .GeneratePdf();
```

Example 4 (swift):
```swift
public class OrderItem
{
    public string ItemName { get; set; } = Placeholders.Label();
    public int Price { get; set; } = Placeholders.Random.Next(1, 11) * 10;
    public int Count { get; set; } = Placeholders.Random.Next(1, 11);
}

public struct OrdersTableWithPageSubtotalsComponentState
{
    public int ShownItemsCount { get; set; }
}
```

---



---

