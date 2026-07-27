# QuestPDF - PageSettings

## Page break


**URL:** https://www.questpdf.com/api-reference/page-break.html

**Contents:**
- Page break ​

The Page Break feature allows you to control the layout of your document by forcing content to start on a new page. This is useful for separating sections, improving readability, and ensuring that specific elements appear on dedicated pages.

In the example below, we generate a programming dictionary where each term appears on its own page.

Unable to display PDF file. Download instead.

**Examples:**

Example 1 (swift):
```swift
Document
    .Create(document =>
    {
        document.Page(page =>
        {
            page.Size(300, 450);
            page.DefaultTextStyle(x => x.FontSize(20));
            page.Margin(25);

            page.Content()
                .PaddingTop(15)
                .Column(column =>
                {
                    var terms = new[]
                    {
                        ("Garbage Collection", "An automatic memory management feature in many programming languages that identifies and removes unused objects to free up memory, preventing memory leaks."),
                        ("Constructor", "A special method in object-oriented programming that is automatically called when an object is created. It initializes the object's properties and sets up any necessary resources."),
                        ("Dependency", "A software component or external library that a program relies on to function correctly. Dependencies can include third-party modules, frameworks, or system-level packages that provide additional functionality without requiring developers to write everything from scratch.")
                    };
                    
                    column.Item()
                        .Extend()
                        .AlignCenter().AlignMiddle()
                        .Text("Programming dictionary").FontSize(24).Bold();
                    
                    foreach (var term in terms)
                    {
                        column.Item().PageBreak();
                        column.Item().Element(c => GeneratePage(c, term.Item1, term.Item2));
                    }

                    static void GeneratePage(IContainer container, string term, string definition)
                    {
                        container.Text(text =>
                        {
                            text.Span(term).Bold().FontColor(Colors.Blue.Darken2);
                            text.Span($" - {definition}");
                        });
                    }
                });
        });
    })
    .GeneratePdf("page-break.pdf");
```

---



---

## Page Numbers


**URL:** https://www.questpdf.com/api-reference/text/page-numbers.html

**Contents:**
- Page Numbers ​
- Document related ​
- Section related ​
    - Available methods ​
    - Example ​
- Custom formatting ​

The following methods allow you to inject page numbers into any text container.

The following methods allow you to display page numbers relative to a specific section of the document.

Section defines part of the document that can be further referenced by name, e.g. for page numbering or linking.

First, define a section somewhere in the document:

Then, refer to this location position in your list of contents:

It is possible to format page numbers to a specified format, e.g. with leading zeros or as Roman numerals.

Please note that the formatting function accepts a nullable integer (int?). QuestPDF employs a two-pass rendering algorithm, meaning that page numbers are only determined during the second pass. During the first pass, your formatting method will receive null, indicating that the page number has not yet been determined. Please ensure that any text you return matches the expected output length.

**Examples:**

Example 1 (swift):
```swift
Document.Create(document =>
{
    document.Page(page =>
    {
        page.Size(PageSizes.A5);
        page.Margin(25);

        // content
        
        page.Footer()
            .PaddingTop(25)
            .AlignCenter()
            .Text(text =>
            {
                text.CurrentPageNumber();
                text.Span(" / ");
                text.TotalPages();
            });
    });
});
```

Example 2 (swift):
```swift
container
    .Section("customSection")
    // content of custom section
```

Example 3 (swift):
```swift
container
    .Text(text =>
    {
        // page number where section begins
        text.BeginPageNumberOfSection("customSection");
        
        // page number where section ends
        text.EndPageNumberOfSection("customSection");
        
        // page number relative to section beginning
        // at section beginning page, method returns 1
        text.PageNumberWithinSection("customSection");
        
        // how many pages section takes
        text.TotalPagesWithinSection("customSection");
    });
```

Example 4 (swift):
```swift
container
    .Text(text =>
    {
        text.CurrentPageNumber().Format(FormatWithLeadingZeros);
    });

// helper function
static string FormatWithLeadingZeros(int? pageNumber)
{
    const int expectedLength = 3;
    pageNumber ??= 1;
    return pageNumber.Value.ToString($"D{expectedLength}");
}
```

---



---

## Page


**URL:** https://www.questpdf.com/api-reference/page/basics.html

**Contents:**
- Page ​

This container allows you to define your page layout by configuring margins, watermarks, and different content sections such as header, footer, and the main content area. You can easily create pages of various sizes and orientations while controlling text styles and content direction.

Below is a minimal example showing how to create a simple document with a header, main content, and footer.

**Examples:**

Example 1 (swift):
```swift
Document.Create(document =>
{
    document.Page(page =>
    {
        page.Size(PageSizes.A4);
        page.Margin(2, Unit.Centimetre);
        page.DefaultTextStyle(x => x.FontSize(24));

        page.Header()
            .Text("Hello, World!")
            .FontSize(48).Bold();

        page.Content()
            .PaddingVertical(25)
            .Text(Placeholders.LoremIpsum())
            .Justify();

        page.Footer()
            .AlignCenter() 
            .Text(text =>
            {
                text.CurrentPageNumber();
                text.Span(" / ");
                text.TotalPages();
            });
    });
})
```

---



---

## Page Settings


**URL:** https://www.questpdf.com/api-reference/page/settings.html

**Contents:**
- Page Settings ​
- Page Color ​
- Page Size ​
  - Specific Page Size ​
  - Continuous Page Size ​
  - Flexible Page Size ​
  - Predefined Page Size ​
- Margin ​
- Default Text Style ​
- Content Direction ​

This section describes how to configure the page settings in your document.

You can set the background color of your document pages using the PageColor method. Colors can be specified using predefined constants, hexadecimal values, or named color variants:

Learn more about supported color formats and predefined color palettes in the Colors section.

QuestPDF offers multiple ways to define the dimensions of a page. You can set exact sizes in various units, choose from standard presets, or allow the library to adapt the page size dynamically based on your content.

Learn more about supported units in the Lenght unit types section.

Configures the exact dimensions of every page within the set.

Enables the continuous page size mode, allowing the page's height to adjust according to content while retaining a constant specified width.

This configuration is useful for output types like receipts, scrolls, or other cases where the length of the page can continuously expand.

Enables the flexible page size mode, where the output page's dimensions can vary based on its content. It is possible to specify the minimum and maximum dimensions for the page, or both.

Please note that with this setting, individual pages within the document may have different sizes.

For convenience, QuestPDF provides commonly used page size presets, including optional orientation:

Margins add empty space around the main layout (header, content, and footer). You can configure each side individually or use combined methods for convenience:

You can apply a default text style to every text element within a page. This is particularly helpful for setting consistent fonts, sizes, and colors across your document: Learn more

QuestPDF supports both left-to-right (LTR) and right-to-left (RTL) layouts to accommodate languages with different reading directions. This option applies a global content direction to the entire page set. Learn more

You can apply different settings to each page set in the document. This flexibility allows you to mix sizes, orientations, styles, or margins as needed:

**Examples:**

Example 1 (scala):
```scala
document.Page(page =>
{
    page.PageColor(Colors.White);
    // or
    page.PageColor("#F0F0F0");
    // or
    page.PageColor(Colors.Grey.Lighten3);
});
```

Example 2 (scala):
```scala
document.Page(page =>
{
    page.Size(595, 842); // in points
    // or
    page.Size(21, 29.7f, Unit.Centimeter);
    // or
    page.Size(PageSizes.A4);
});
```

Example 3 (scala):
```scala
document.Page(page =>
{
    page.ContinuousSize(215);
    // or
    page.ContinuousSize(76, Unit.Millimeter);
});
```

Example 4 (scala):
```scala
document.Page(page =>
{
    page.MinSize(400, 600);
    // and / or
    page.MaxSize(800, 1200);
    
    // also supports units and PageSizes
});
```

---



---

## Page Slots


**URL:** https://www.questpdf.com/api-reference/page/slots.html

**Contents:**
- Page Slots ​
- Main Slots ​
- Foreground Slot ​
- Background Slot ​

The Page container is a multi-child container that allows you to define the layout of the page. It provides several slots that can be used to add content to the page.

The main slots are Header, Content, and Footer.

Represents a layer drawn in front of the primary layer (header + content + footer), serving as a watermark. It is not affected by the Margin configuration and always occupy the entire page.

Represents a layer drawn behind the primary layer (header + content + footer). It is not affected by the Margin configuration and always occupy the entire page.

**Examples:**

Example 1 (swift):
```swift
.Page(page =>
{
    document.Page(page =>
    {
        page.Size(PageSizes.A4);
        page.Margin(2, Unit.Centimetre);
        page.DefaultTextStyle(x => x.FontSize(24));
    
        page.Header()
            .Background(Colors.Grey.Lighten1)
            .Height(125)
            .AlignCenter()
            .AlignMiddle()
            .Text("Header");
        
        page.Content()
            .Background(Colors.Grey.Lighten2)
            .AlignCenter()
            .AlignMiddle()
            .Text("Content");
        
        page.Footer()
            .Background(Colors.Grey.Lighten1)
            .Height(75)
            .AlignCenter()
            .AlignMiddle()
            .Text("Footer");
    });
});
```

Example 2 (swift):
```swift
document.Page(page =>
{
    page.Size(PageSizes.A4);
    page.Margin(2, Unit.Centimetre);
    page.DefaultTextStyle(x => x.FontSize(20));

    page.Header()
        .PaddingBottom(1, Unit.Centimetre)
        .Text("Report")
        .FontSize(30)
        .Bold();
    
    page.Content()
        .Text(Placeholders.Paragraphs())
        .ParagraphSpacing(1, Unit.Centimetre)
        .Justify();

    page.Foreground().Svg("Resources/draft-foreground.svg").FitArea();
});
```

Example 3 (swift):
```swift
document.Page(page =>
{ 
    page.Size(PageSizes.A4.Landscape());

    page.Background().Svg("Resources/certificate-background.svg").FitArea();

    page.Content() 
        .PaddingLeft(10, Unit.Centimetre)
        .PaddingRight(5 , Unit.Centimetre)
        .AlignMiddle()
        .Column(column =>
        {
            column.Item().Height(50).Svg("Resources/questpdf-logo.svg");
            
            column.Item().Height(50);
            
            column.Item().Text("CERTIFICATE").FontSize(64).ExtraBlack();
            
            column.Item().Height(25);
            
            column.Item()
                .Shrink().BorderBottom(1).Padding(10)
                .Text("Marcin Ziąbek").FontSize(32).Italic();
            
            column.Item().Height(10); 
            
            column.Item()
                .Text($"has successfully completed the course \"QuestPDF Basics\" on {DateTime.Now:dd MMM yyyy}.")
                .FontSize(20).Light();
        });
});
```

---



---

## Prevent page break


**URL:** https://www.questpdf.com/api-reference/prevent-page-break.html

**Contents:**
- Prevent page break ​
- Example ​
    - Without PreventPageBreak ​
    - With PreventPageBreak ​

Attempts to keep the container's content together on its first page of occurrence. If the content does not fit entirely on that page, it is moved to the next page. If it spans multiple pages, all subsequent pages are rendered as usual without restriction.

This method is useful for ensuring that content remains visually coherent and is not arbitrarily split.

Unable to display PDF file. Download instead.

Unable to display PDF file. Download instead.

**Examples:**

Example 1 (swift):
```swift
container.Column(column =>
{
    column.Item().Height(400).Background(Colors.Grey.Lighten3);
    column.Item().Height(30);

    column.Item()
        .PreventPageBreak()
        .Text(text =>
        {
            text.ParagraphSpacing(15);
            
            text.Span("Optimizing Content Placement").Bold().FontColor(Colors.Blue.Darken2).FontSize(24);
            text.Span("\n");
            text.Span("By carefully determining where to place a page break, you can avoid awkward text separations and maintain readability. Thoughtful formatting improves the overall user experience, making complex topics easier to digest.");
        });
});
```

---



---

## Customizing Header/Footer on the first page


**URL:** https://www.questpdf.com/examples/custom-header-on-first-page.html

**Contents:**
- Customizing Header/Footer on the first page ​

A common requirement in document design is to create a distinct header for the first page, with all subsequent pages sharing a standard header format. This can be easily accomplished using the ShowOnce and SkipOnce elements in QuestPDF.

The code above produces the following results:

Unable to display PDF file. Download instead.

**Examples:**

Example 1 (swift):
```swift
Document
   .Create(document =>
   {
       document.Page(page =>
       {
           page.Size(PageSizes.A5);
           page.Margin(30);
           page.DefaultTextStyle(x => x.FontSize(20));

           page.Header().Column(column =>
           {
               column.Item().ShowOnce().Background(Colors.Blue.Lighten2).Height(80);
               column.Item().SkipOnce().Background(Colors.Green.Lighten2).Height(60);
           });

           page.Content().PaddingVertical(20).Column(column =>
           {
               column.Spacing(20);

               foreach (var _ in Enumerable.Range(0, 20))
                   column.Item().Background(Colors.Grey.Lighten3).Height(40);
           });

           page.Footer().AlignCenter().Text(text =>
           {
               text.CurrentPageNumber();
               text.Span(" / ");
               text.TotalPages();
           });
       });
   })
   .GeneratePdf("custom-header-on-first-page.pdf");
```

---



---

