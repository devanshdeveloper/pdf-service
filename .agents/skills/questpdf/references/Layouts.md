# QuestPDF - Layouts

## # Questpdf - Api


**Pages:** 58

---



---

## Alignment


**URL:** https://www.questpdf.com/api-reference/alignment.html

**Contents:**
- Alignment ​
- API ​
  - Horizontal ​
  - Vertical ​
- Example ​

The Alignment element controls the positioning of its child content within the available space. It offers both horizontal and vertical options that can be used independently or combined.

**Examples:**

Example 1 (swift):
```swift
container
    .Width(300)
    .Height(300)
    .AlignBottom()
    .AlignCenter()
    .Background(Colors.Grey.Lighten2)
    .Padding(10)
    .Text("Test");
```

---



---

## Aspect Ratio


**URL:** https://www.questpdf.com/api-reference/aspect-ratio.html

**Contents:**
- Aspect Ratio ​
- API ​
  - Fitting Options ​
- Example ​

Constrains its content to maintain a given width-to-height ratio.

Specify the aspect-ratio value either as a number or a division of two numbers:

Additionally, you can specify how the content should be adjusted to meet the aspect ratio:

Please be careful. This component may try to enforce size constraints that are impossible to meet. For example, the container may require more space than is available, or may try to squeeze its child into less space than possible.

Such scenarios result in a layout exception.

**Examples:**

Example 1 (elixir):
```elixir
.AspectRatio(0.5) // use a ratio
.AspectRatio(1f / 2f) // or division
```

Example 2 (unknown):
```unknown
.AspectRatio(0.5, AspectRatioOption.FitArea)
```

Example 3 (swift):
```swift
container
    .Width(300)
    .Height(300)
    .AspectRatio(3f/4f, AspectRatioOption.FitArea)
    .Background(Colors.Grey.Lighten2)
    .AlignCenter()
    .AlignMiddle()
    .Text("3:4 Content Area");
```

---



---

## Background


**URL:** https://www.questpdf.com/api-reference/background.html

**Contents:**
- Background ​
- Solid color ​
- Gradient ​
- Rounded Corners ​

Background can be used to enhance the visual appearance of your document by providing a solid color or a gradient effect.

Learn more about supported color formats and predefined color palettes in the Colors section.

Sets a solid background color behind its content.

Applies a linear gradient background to the container with the specified angle and colors.

The first argument is the angle in degrees, and the second argument is an array of colors that define the gradient.

Sets the corner radius for the background, creating rounded corners.

Read more about rounded corners.

**Examples:**

Example 1 (swift):
```swift
.Background("#00FF00")
.Background(Colors.Green.Lighten2)
```

Example 2 (swift):
```swift
using QuestPDF.Helpers;

var colors = new[]
{
    Colors.LightBlue.Darken4,
    Colors.LightBlue.Darken3,
    Colors.LightBlue.Darken2,
    Colors.LightBlue.Darken1,

    Colors.LightBlue.Medium,

    Colors.LightBlue.Lighten1,
    Colors.LightBlue.Lighten2,
    Colors.LightBlue.Lighten3,
    Colors.LightBlue.Lighten4,
    Colors.LightBlue.Lighten5,

    Colors.LightBlue.Accent1,
    Colors.LightBlue.Accent2,
    Colors.LightBlue.Accent3,
    Colors.LightBlue.Accent4,
};

container
    .Height(150)
    .Width(420)
    .Row(row =>
    {
        foreach (var color in colors)
            row.RelativeItem().Background(color);
    });
```

Example 3 (scala):
```scala
.Column(column =>
{
    column.Spacing(25);

    column.Item()
        .BackgroundLinearGradient(0, [Colors.Red.Lighten2, Colors.Blue.Lighten2])
        .AspectRatio(2);

    column.Item()
        .BackgroundLinearGradient(45, [Colors.Green.Lighten2, Colors.LightGreen.Lighten2, Colors.Yellow.Lighten2])
        .AspectRatio(2);
    
    column.Item()
        .BackgroundLinearGradient(90, [Colors.Yellow.Lighten2, Colors.Amber.Lighten2, Colors.Orange.Lighten2])
        .AspectRatio(2);
});
```

Example 4 (swift):
```swift
container
    .Background(Colors.Grey.Lighten2)
    .CornerRadius(25)
    .Padding(25)
    .Text("Content with rounded corners");
```

---



---

## Border


**URL:** https://www.questpdf.com/api-reference/border.html

**Contents:**
- Border ​
- Thickness ​
  - Consistent thickness ​
  - Various thickness ​
- Solid Color ​
- Gradient ​
- Alignment ​
- Examples ​
  - Rounded corners ​
  - Multiple borders ​

You can use borders to create visual separation between elements in your document. Borders can be applied to any element, including text, images, and containers.

Each method requires a thickness value as a parameter. Optionally, you can specify the unit value (default is Unit.Points).

Learn more about supported units in the Lenght unit types section.

In the vast majority of cases, borders are applied with a solid color.

Learn more about supported color formats and predefined color palettes in the Colors section.

Applies a linear gradient background to the border with the specified angle and colors.

The first argument is the angle in degrees, and the second argument is an array of colors that define the gradient.

You can control the alignment of the border relative to the container's boundaries using the following methods:

By default, the border is aligned to the middle of the container boundaries.

However, if the border has rounded corners, the alignment is set to inside by default.

Borders support rounded corners, which can be applied using the CornerRadius method.

Read more about rounded corners.

It is possible to apply multiple borders to the same content by separating each border instance with the Container method.

You can create advanced styles by combining borders with other properties, such as background color, padding, and text styles.

**Examples:**

Example 1 (swift):
```swift
container
    .Border(3, Colors.Blue.Darken4)
    .Background(Colors.Blue.Lighten5)
    .Padding(25) 
    .Text(text =>
    {
        text.DefaultTextStyle(x => x.FontColor(Colors.Blue.Darken4).FontSize(16));
        text.Span("TIP: ").Bold();
        text.Span("You can use borders to create visual separation between elements in your document. Borders can be applied to any element, including text, images, and containers.");
    });
```

Example 2 (unknown):
```unknown
container.Border(1);
container.Border(1, Unit.Millimeters);
```

Example 3 (swift):
```swift
.Row(row =>
{
    row.Spacing(25);
    
    row.RelativeItem()
        .Border(1, Colors.Black)
        .Padding(10)
        .AlignCenter()
        .Text("Thin");
    
    row.RelativeItem()
        .Border(3, Colors.Black)
        .Padding(10)
        .AlignCenter()
        .Text("Medium");
    
    row.RelativeItem()
        .Border(9, Colors.Black)
        .Padding(10)
        .AlignCenter()
        .Text("Bold");
});
```

Example 4 (swift):
```swift
container
    .BorderLeft(4)
    .BorderTop(6)
    .BorderRight(8) 
    .BorderBottom(10)
    .Padding(25)
    .Text("Sample text");
```

---



---

## Column


**URL:** https://www.questpdf.com/api-reference/column.html

**Contents:**
- Column ​
- Basic usage ​
- Spacing ​
- Custom spacing ​
- Uniform item width ​
    - Default behavior (consistent item width) ​
    - Effect with ShrinkVertical applied ​

The Column element arranges content vertically, stacking items one below another.

It supports paging functionality, allowing content to flow naturally across multiple pages when needed. When required, child items are split across pages, ensuring that the content is not cut off.

The Column element uses a lambda function to define its content. Inside the lambda, you can add multiple items using the Item method.

You can adjust the vertical spacing between items using the Spacing method.

Optionally, you can specify the unit value (default is Unit.Points).

Learn more about supported units in the Lenght unit types section.

You can adjust the spacing between items individually by adding an empty item with a specific height.

By default, all items in a Column match the width of the widest item. This ensures consistent visual alignment, but sometimes it can result in unwanted visual stretching.

To disable this behavior, use the ShrinkHorizontal API:

**Examples:**

Example 1 (swift):
```swift
container
    .Width(250)
    .Padding(25)
    .Column(column =>
    {
        column.Item().Background(Colors.Grey.Medium).Height(50);
        column.Item().Background(Colors.Grey.Lighten1).Height(75);
        column.Item().Background(Colors.Grey.Lighten2).Height(100);
    });
```

Example 2 (swift):
```swift
container
    .Width(250)
    .Padding(25)
    .Column(column =>
    {
        column.Spacing(25);
        
        column.Item().Background(Colors.Grey.Medium).Height(50);
        column.Item().Background(Colors.Grey.Lighten1).Height(75);
        column.Item().Background(Colors.Grey.Lighten2).Height(100);
    });
```

Example 3 (unknown):
```unknown
column.Spacing(5, Unit.Millimeters);
```

Example 4 (scala):
```scala
.Column(column =>
{
    column.Item().Background(Colors.Grey.Darken1).Height(50);
    column.Item().Height(10);
    column.Item().Background(Colors.Grey.Medium).Height(50);
    column.Item().Height(20);
    column.Item().Background(Colors.Grey.Lighten1).Height(50);
    column.Item().Height(30);
    column.Item().Background(Colors.Grey.Lighten2).Height(50);
});
```

---



---

## Complex Graphics


**URL:** https://www.questpdf.com/api-reference/complex-graphics.html

**Contents:**
- Complex Graphics ​
- Rounded Rectangle ​
- Dotted Line ​

QuestPDF supports various built-in drawing capabilities, but there may be times when you need to include more sophisticated or custom graphics in your PDF. One powerful way to achieve this is by embedding SVG content dynamically. This allows you to draw custom shapes, gradients, and other visual elements that go beyond available functionalities.

This example shows how to draw a custom rectangle behind the text. It fills the available space, has rounded corners, and a gradient fill.

This example creates structure similar to a table of contents with dotted lines connecting the page numbers to the titles.

**Examples:**

Example 1 (jsx):
```jsx
.Layers(layers =>
{
    layers.Layer().Svg(size =>
    {
        return $"""
                <svg width="{size.Width}" height="{size.Height}" xmlns="http://www.w3.org/2000/svg">
                    <defs>
                      <linearGradient id="backgroundGradient" x1="0%" y1="0%" x2="100%" y2="100%">
                        <stop stop-color="#00E5FF" offset="0%"/>
                        <stop stop-color="#2979FF" offset="100%"/>
                      </linearGradient>
                    </defs>
                
                    <rect x="0" y="0" width="{size.Width}" height="{size.Height}" rx="{size.Height / 2}" ry="{size.Height / 2}" fill="url(#backgroundGradient)" />
                </svg>
                """;
    });

    layers.PrimaryLayer()
        .PaddingVertical(10)
        .PaddingHorizontal(20)
        .Text("QuestPDF")
        .FontColor(Colors.White)
        .FontSize(32)
        .ExtraBlack();
});
```

Example 2 (swift):
```swift
.Column(column =>
{
    column.Spacing(5);
    
    foreach (var i in Enumerable.Range(1, 5))
    {
        var pageNumber = i * 7 + 4;
        
        column.Item().Row(row =>
        {
            row.AutoItem().Text($"{i}.");
            row.ConstantItem(10);
            row.AutoItem().Text(Placeholders.Label());

            row.RelativeItem().PaddingHorizontal(3).OffsetY(20).Height(2).Svg(size =>
            {
                return $"""
                        <svg width="{size.Width}" height="{size.Height}" xmlns="http://www.w3.org/2000/svg">
                            <line x1="0" y1="0" x2="{size.Width}" y2="0" fill="none" stroke="black" stroke-width="2" stroke-dasharray="2 6" />
                        </svg>
                        """;
            });

            row.AutoItem().Text($"{pageNumber}");
        });
    }
});
```

---



---

## Content Direction


**URL:** https://www.questpdf.com/api-reference/content-direction.html

**Contents:**
- Content Direction ​
- API ​
- Overriding content direction ​
- Impact On Content ​

The ContentDirection element controls the flow direction of content in your document, supporting both left-to-right (LTR) and right-to-left (RTL) layouts. This is essential for proper text alignment and content organization when working with different languages.

It is also possible to override the content direction for specific elements:

This element impacts several key aspects:

**Examples:**

Example 1 (unknown):
```unknown
container
    .ContentFromRightToLeft()
    // content in right-to-left direction
```

Example 2 (scala):
```scala
.ContentFromRightToLeft()
.Column(column => 
{
    column
        .Item() 
        // content with inherited RTL content direction
        
    column
        .Item()
        .ContentFromLeftToRight() 
        // content with overridden LTR content direction     
});
```

Example 3 (scala):
```scala
.ContentFromRightToLeft() // LTR or RTL mode
.Row(row =>
{
    row.Spacing(5);
    
    row.AutoItem().Height(50).Width(50).Background(Colors.Red.Lighten1);
    row.AutoItem().Height(50).Width(50).Background(Colors.Green.Lighten1);
    row.AutoItem().Height(50).Width(75).Background(Colors.Blue.Lighten1);
});
```

---



---

## Decoration


**URL:** https://www.questpdf.com/api-reference/decoration.html

**Contents:**
- Decoration ​
- API ​
- Example ​

Divides the container's space into three distinct sections: before, content, and after.

The before section is rendered above the main content, while the after section is rendered below it. If the main content spans across multiple pages, both the before and after sections are consistently rendered on every page.

A typical use-case for this method is to render a table that spans multiple pages, with a consistent caption or header on each page.

**Examples:**

Example 1 (swift):
```swift
container
    .Background(Colors.Grey.Lighten3)
    .Padding(15)
    .Decoration(decoration =>
    {
        decoration
            .Before()
            .DefaultTextStyle(x => x.Bold())
            .Column(column =>
            {
                column.Item().ShowOnce().Text("Customer Instructions:");
                column.Item().SkipOnce().Text("Customer Instructions [continued]:");
            });

        decoration
            .Content()
            .PaddingTop(10)
            .Text("Please wrap the item in elegant gift paper and include a small blank card for a personal message. If possible, remove any price tags or invoices from the package. Make sure the wrapping is secure but easy to open without damaging the contents.");
    });
```

---



---

## Ensure space


**URL:** https://www.questpdf.com/api-reference/ensure-space.html

**Contents:**
- Ensure space ​
- Example ​
    - Without EnsureSpace ​
    - With EnsureSpace ​

Ensures that the container's content occupies at least a specified minimum height on its first page of occurrence.

This method is particularly useful for structured elements like tables, where rendering only a small fragment at the bottom of a page could negatively impact readability. By ensuring a minimum height, you can prevent undesired content fragmentation.

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
        .EnsureSpace(100)
        .Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.ConstantColumn(40);
                columns.RelativeColumn();
            });

            foreach (var i in Enumerable.Range(1, 12))
            {
                table.Cell().Text($"{i}.");
                table.Cell().ShowEntire().Text(Placeholders.Sentence());
            }
        });
});
```

---



---

## Height


**URL:** https://www.questpdf.com/api-reference/height.html

**Contents:**
- Height ​
- Example ​

Use this element to control the veertical size of its content.

The following example demonstrates a container with a fixed height of 100 pt and a width of 200 pt.

Please be careful. This component may try to enforce size constraints that are impossible to meet. For example, the container may require more space than is available, or may try to squeeze its child into less space than possible.

Such scenarios result in a layout exception.

**Examples:**

Example 1 (swift):
```swift
container
    .Width(300)
    .Padding(25)
    .Height(100)
    .AspectRatio(2f, AspectRatioOption.FitHeight)
    .Background(Colors.Grey.Lighten1);
```

---



---

## Inlined


**URL:** https://www.questpdf.com/api-reference/inlined.html

**Contents:**
- Inlined ​
    - Helper method ​
    - Usage ​
- Spacing ​
- Horizontal alignment ​
- Baseline alignment ​

The Inlined component arranges elements sequentially in a line, automatically wrapping to the next line when needed. This layout is particularly useful when you need to display a collection of elements horizontally with consistent spacing and alignment options.

The following helper method generates sample blocks with random sizes and colors to demonstrate the Inlined component's capabilities:

**Examples:**

Example 1 (json):
```json
void RandomBlock(IContainer container)
{
    container
        .Width(Random.Shared.Next(1, 4) * 25)
        .Height(Random.Shared.Next(1, 4) * 25)
        .Border(1)
        .BorderColor(Colors.Grey.Darken2)
        .Background(Placeholders.BackgroundColor());
}
```

Example 2 (swift):
```swift
.Background(Colors.Grey.Lighten3)
.Padding(25)
.Border(1)
.Background(Colors.White)
.Inlined(inlined =>
{
    inlined.Spacing(25);
    inlined.BaselineMiddle();
    inlined.AlignCenter();
    
    foreach (var _ in Enumerable.Range(0, 15))
        inlined.Item().Element(RandomBlock);
});
```

---



---

## Layers


**URL:** https://www.questpdf.com/api-reference/layers.html

**Contents:**
- Layers ​
  - Example ​

The Layers element adds content either underneath (as a background) or on top of (as a watermark) the main content.

The main layer supports paging, can span multiple pages, and determines the container's target length. Additional layers can also span multiple pages and are repeated on each one.

Exactly one PrimaryLayer must be defined.

The order of code execution determines the drawing order:

A common use-case for this element is to add background content behind the main content.

**Examples:**

Example 1 (swift):
```swift
.Column(column =>
{
    column.Item().PaddingBottom(15).Text("Proposed Business Card Design:").Bold();
    
    column.Item()
        .AspectRatio(4 / 3f)
        .Layers(layers =>
        {
            layers.Layer().Image("Resources/card-background.jpg").FitUnproportionally();

            layers.PrimaryLayer()
                .OffsetY(75)
                .Column(innerColumn =>
                {
                    innerColumn.Item()
                        .AlignCenter()
                        .Text("Horizon Ventures")
                        .Bold().FontSize(32).FontColor(Colors.Blue.Darken2);

                    innerColumn.Item().AlignCenter().Text("Your journey begins here");
                });
        });
});
```

---



---

## Lazy


**URL:** https://www.questpdf.com/api-reference/lazy.html

**Contents:**
- Lazy ​
- Available Approaches ​
- Example ​
  - Normal Approach ​
  - Lazy Approach ​
  - LazyWithCache Approach ​
  - Observed results ​

When generating large PDF documents with thousands of pages, memory consumption becomes a critical concern. QuestPDF provides specialized elements to optimize memory usage by deferring content creation until it is actually needed. This reduces the lifetime of objects, allowing for more efficient garbage collection and lowering the risk of out-of-memory errors.

There are two primary approaches to optimize memory usage when generating large documents:

The Lazy element defers the construction of document elements until they are required for rendering. This means that instead of preloading and storing all content in memory at once, only the necessary elements are created dynamically when needed.

The Lazy element achieves this by providing a delegate function which is executed later during the document generation process.

The LazyWithCache element introduces an additional performance benefit: previously rendered sections are cached, reducing the recomputation overhead when revisiting pages. However, this may lead to higher native memory usage due to caching mechanisms.

This example uses a simple component generating a list of numbers from a specified range. It simulates a typical text-heavy content generation scenario.

This approach does not use any optimization techniques and generates the entire document at once. It is typically used for small documents or when memory usage is not a concern.

This approach uses the Lazy element to defer the creation of content until it is needed.

This approach uses the LazyWithCache element to defer the creation of content and cache previously rendered sections.

Please analyze the following results to understand the performance benefits of each approach:

Understanding when and how to use these elements is key to improving both document generation speed and resource management.

**Examples:**

Example 1 (swift):
```swift
class SimpleComponent : IComponent
{
    public required int Start { get; init; }
    public required int End { get; init; }
    
    public void Compose(IContainer container)
    {
        container.Decoration(decoration =>
        {
            decoration.Before()
                .Text($"Numbers from {Start} to {End}")
                .FontSize(20).Bold().FontColor(Colors.Blue.Darken2);
        
            decoration.Content().Column(column =>
            {
                foreach (var i in Enumerable.Range(Start, End - Start + 1))
                    column.Item().Text($"Number {i}").FontSize(10);
            });
        });
    }
}
```

Example 2 (swift):
```swift
Document
    .Create(document =>
    {
        document.Page(page =>
        {
            page.Margin(10);

            page.Content().Column(column =>
            {
                const int sectionSize = 1000;
                
                foreach (var i in Enumerable.Range(0, 1000))
                {
                    column.Item().Component(new SimpleComponent
                    {
                        Start = i * sectionSize,
                        End = i * sectionSize + sectionSize - 1
                    });
                }
            });
        });
    })
    .GeneratePdf("lazy-disabled.pdf");
```

Example 3 (swift):
```swift
Document
    .Create(document =>
    {
        document.Page(page =>
        {
            page.Margin(10);

            page.Content().Column(column =>
            {
                const int sectionSize = 1000;

                foreach (var i in Enumerable.Range(0, 1000))
                {
                    var start = i * sectionSize;
                    var end = start + sectionSize - 1;

                    column.Item().Lazy(c =>
                    {
                        c.Component(new SimpleComponent
                        {
                            Start = start,
                            End = end
                        });
                    });
                }
            });
        });
    })
    .GeneratePdf("lazy-enabled.pdf");
```

Example 4 (swift):
```swift
Document
    .Create(document =>
    {
        document.Page(page =>
        {
            page.Margin(10);

            page.Content().Column(column =>
            {
                const int sectionSize = 1000;

                foreach (var i in Enumerable.Range(0, 1000))
                {
                    var start = i * sectionSize;
                    var end = start + sectionSize - 1;

                    column.Item().LazyWithCache(c =>
                    {
                        c.Component(new SimpleComponent
                        {
                            Start = start,
                            End = end
                        });
                    });
                }
            });
        });
    })
    .GeneratePdf("lazy-enabled-with-cache.pdf");
```

---



---

## Line


**URL:** https://www.questpdf.com/api-reference/line.html

**Contents:**
- Line ​
- Vertical ​
- Horizontal ​
- Thickness ​
- Solid Color ​
- Gradient ​
- Dash Pattern ​
- Complex Example ​

The Line component allows you to render simple yet customizable vertical and horizontal lines within your layout.

These lines can serve as visual dividers, helping to structure and improve the readability of your content. You can specify the thickness of the line and optionally customize its color.

Renders a vertical line with a specified thickness.

Renders a horizontal line with a specified thickness.

It is possible to modify how pronounced the line appears by adjusting its thickness.

Specifies the color for the line.

Applies a linear gradient to a line using the specified colors.

Configures a dashed pattern for the line.

For example, a pattern of [2, 3] creates a dash of 2 units followed by a gap of 3 units.

The length of the pattern array must be even.

It is possible to combine multiple options to create a more complex line style.

**Examples:**

Example 1 (swift):
```swift
container
    .Row(row =>
    {
        row.AutoItem().Text("Text on the left");
        
        row.AutoItem()
            .PaddingHorizontal(15)
            .LineVertical(3)
            .LineColor(Colors.Blue.Medium); // optional
        
        row.AutoItem().Text("Text on the right");
    });
```

Example 2 (swift):
```swift
container
    .Column(column =>
    {
        column.Item().Text("Text above the line");
        
        column.Item()
            .PaddingVertical(10)
            .LineHorizontal(2)
            .LineColor(Colors.Blue.Medium); // optional
        
        column.Item().Text("Text below the line");
    });
```

Example 3 (swift):
```swift
container
    .Column(column =>
    {
        column.Spacing(20);

        foreach (var thickness in new[] { 1, 2, 4, 8 })
        {
            column.Item()
                .Width(200)
                .LineHorizontal(thickness);
        }
    });
```

Example 4 (swift):
```swift
container
    .Column(column =>
    {
        var colors = new[]
        {
            Colors.Red.Medium,
            Colors.Green.Medium,
            Colors.Blue.Medium,
        };
        
        column.Spacing(20);

        foreach (var color in colors)
        {
            column.Item()
                .Width(200)
                .LineHorizontal(5)
                .LineColor(color);
        }
    });
```

---



---

## Multi Column Layout


**URL:** https://www.questpdf.com/api-reference/multi-column.html

**Contents:**
- Multi Column Layout ​
- Example ​
- Spacer ​
- Balance height ​
    - BalanceHeight disabled ​
    - BalanceHeight enabled ​

A multi-column layout arranges content into vertical columns, similar to newspaper or magazine formatting. This approach optimizes horizontal space and enhances readability, especially for wide containers or screens.

Multi-column layouts require significant computational resources, which may impact performance.

The Content() method provides access to the container where your primary content will be distributed across multiple columns. This container serves as the main content area for your multi-column layout and supports all available layout elements.

The Columns() method defines the number of vertical columns in your layout. This setting establishes the basic structure of the grid layout.

The Spacing() method configures the horizontal space between adjacent columns. This setting affects the visual presentation of your column arrangement. Positive values increase separation between columns, while negative values may cause overlap (though this is rarely desirable).

Use the Spacer approach to create a visual break between content sections, improving readability and aesthetics. The container's dimensions are determined by the height of the columns and the configured spacing. It supports all available layout elements.

The BalanceHeight() method controls how content is distributed across columns. This feature helps create a more aesthetically pleasing and professional layout by ensuring columns have similar heights.

The layout occupies the entire vertical space, often leaving the last column shorter or empty if there is less content to fill it.

The layout engine distributes elements so that each column ends up with approximately the same height.

**Examples:**

Example 1 (swift):
```swift
container.MultiColumn(multiColumn =>
{
    multiColumn.Columns(3);
    multiColumn.Spacing(25);

    multiColumn
        .Content()
        .Column(column =>
        {
            column.Spacing(15);

            foreach (var sectionId in Enumerable.Range(0, 3))
            {
                foreach (var textId in Enumerable.Range(0, 3))
                    column.Item().Text(Placeholders.Paragraph()).Justify();

                column.Item().AspectRatio(21 / 9f).Image(Placeholders.Image);
            }
        });
});
```

Example 2 (swift):
```swift
container.MultiColumn(multiColumn =>
{
    multiColumn.Columns(2);
    multiColumn.Spacing(50);

    multiColumn
        .Spacer()
        .AlignCenter()
        .LineVertical(2)
        .LineColor(Colors.Grey.Medium);
    
    multiColumn
        .Content()
        .Column(column =>
        {
            column.Spacing(15);

            foreach (var textId in Enumerable.Range(0, 5))
                column.Item().Text(Placeholders.Paragraph()).Justify();
        });
});
```

Example 3 (swift):
```swift
container.MultiColumn(multiColumn =>
{
    multiColumn.Spacing(30);
    multiColumn.BalanceHeight();

    multiColumn
        .Content()
        .Column(column =>
        {
            column.Spacing(15);
            
            foreach (var textId in Enumerable.Range(0, 8))
                column.Item().Text(Placeholders.Paragraph()).Justify();
        });
});
```

---



---

## Offset


**URL:** https://www.questpdf.com/api-reference/offset.html

**Contents:**
- Offset ​

This container allows you to precisely position content by moving it horizontally and vertically relative to its original position, independent of layout constraints.

When you apply offset, the element maintains its original size constraints while shifting its visual position.

**Examples:**

Example 1 (swift):
```swift
container
    .Padding(50)
    .Background(Colors.Blue.Lighten3)
    .OffsetX(25)
    .OffsetY(25)
    .Border(4)
    .BorderColor(Colors.Blue.Darken2)
    .Padding(50)
    .Text("Moved content")
    .FontSize(25);
```

---



---

## Padding


**URL:** https://www.questpdf.com/api-reference/padding.html

**Contents:**
- Padding ​
- Negative padding ​
- API ​

For positive values, the Padding element adds empty space around its content.

For negative values, it pushes content beyond the edges, increasing available space (similar to negative HTML margins).

**Examples:**

Example 1 (swift):
```swift
container
    .Width(250)
    .PaddingVertical(10)
    .PaddingLeft(20)
    .PaddingRight(40)
    .Background(Colors.Grey.Lighten2)
    .Text("Sample text");
```

Example 2 (swift):
```swift
container
    .Width(250)
    .Padding(50)
    .Background(Colors.Grey.Lighten2)
    .PaddingHorizontal(-25)
    .Text("Sample text with negative padding");
```

---



---

## Repeat


**URL:** https://www.questpdf.com/api-reference/repeat.html

**Contents:**
- Repeat ​
  - Example ​
  - Without the Repeat element ​
  - With the Repeat element ​

When designing a document, you may need certain elements—such as headers, footers, labels, or key terms—to be visible on every page where applicable. The Repeat element is designed to fulfill this requirement by rendering the specified content multiple times across different pages, rather than just once.

Please note that the term "Variable" is repeated across multiple pages.

**Examples:**

Example 1 (swift):
```swift
container
    .Decoration(decoration =>
    {
        var terms = new[]
        {
            ("Algorithm", "A precise set of instructions that defines a process for solving a specific problem or performing a computation. Algorithms are the foundation of programming and are used to optimize tasks efficiently."),
            ("Bug", "An error, flaw, or unintended behavior in a program that causes it to produce incorrect or unexpected results. Debugging is the process of identifying, analyzing, and fixing these issues to improve software reliability."),
            ("Variable", "A named storage location in memory that holds a value, which can be modified during program execution. Variables make code dynamic and flexible by allowing data manipulation and retrieval."),
            ("Compilation", "The process of transforming human-readable source code into machine code (binary instructions) that a computer can execute. This process is performed by a compiler and often includes syntax checks, optimizations, and linking dependencies.")
        };
        
        decoration.Before().Text("Terms and their definitions:").Bold();
        
        decoration.Content().PaddingTop(15).Column(column =>
        {
            foreach (var term in terms)
            {
                column.Item().Row(row =>
                {
                    row.RelativeItem(2)
                        .Border(1)
                        .Background(Colors.Grey.Lighten3)
                        .Padding(15)
                        .Repeat()
                        .Text(term.Item1);
                
                    row.RelativeItem(3)
                        .Border(1)
                        .Padding(15)
                        .Text(term.Item2);
                });
            }
        });
    });
```

---



---

## Rotate


**URL:** https://www.questpdf.com/api-reference/rotate.html

**Contents:**
- Rotate ​
- Constrained ​
- Free ​

Constrained rotation enables you to rotate an element by exactly 90 degrees, either clockwise or counterclockwise, while maintaining the content within the same space and size constraints.

When applying rotation, be aware that it changes the dimensional behavior of your elements. What was previously considered width may become height and vice versa. This affects how other properties like alignment and padding work on the rotated element.

Rotates its content clockwise by a given angle.

**Examples:**

Example 1 (swift):
```swift
container.Row(row =>
{
    row.AutoItem()
        .RotateLeft()
        .AlignCenter()
        .Text("Definition")
        .Bold().FontColor(Colors.Blue.Darken2);
    
    row.AutoItem()
        .PaddingHorizontal(15)
        .LineVertical(2).LineColor(Colors.Blue.Medium);
    
    row.RelativeItem()
        .Background(Colors.Blue.Lighten5)
        .Padding(15)
        .Text(text =>
        {
            text.Span("A variable").Bold();
            text.Span(" is a named storage location in memory that holds a value which can be modified during program execution.");
        });
});
```

Example 2 (swift):
```swift
container
    .Background(Colors.Grey.Lighten2)
    .Padding(25)
    .Row(row =>
    {
        row.Spacing(25);
        
        AddIcon(0);
        AddIcon(30);
        AddIcon(45);
        AddIcon(80);

        void AddIcon(float angle)
        {
            const float itemSize = 100;
            
            row.AutoItem()
                .Width(itemSize)
                .AspectRatio(1)
                
                .OffsetX(itemSize / 2)
                .OffsetY(itemSize / 2)
                
                .Rotate(angle)
                
                .OffsetX(-itemSize / 2)
                .OffsetY(-itemSize / 2)
                
                .Svg("Resources/compass.svg");
        }
    });
```

---



---

## Rounded Corners


**URL:** https://www.questpdf.com/api-reference/rounded-corners.md

**Contents:**
- Consistent Corner Radius
- Various Corner Radius
- Image Example
- Complex Example

Rounded corners can be applied to containers to create visually appealing designs.

This feature allows you to specify the radius of the corners, giving a softer look to the edges of the container.

In the vast majority of cases, you will want to apply the same corner radius to all corners of a container.

![example](/api-reference/rounded-corners-consistent.webp)

It is also possible to apply different corner radii to each corner of a container, allowing for more complex designs.

![example](/api-reference/rounded-corners-various.webp)

Rounded corners can also be applied to images, enhancing their appearance in documents.

![example](/api-reference/rounded-corners-image.webp)

Rounded corners can be used in more complex layouts, such as tables, to create a polished look.

![example](/api-reference/rounded-corners-complex.webp)

**Examples:**

Example 1 (text):
```text
container
    .Border(1, Colors.Black)
    .Background(Colors.Grey.Lighten3)
    .CornerRadius(25)
    .Padding(25)
    .Text("Container with consistently rounded corners");
```

Example 2 (text):
```text
container
    .Border(1, Colors.Black)
    .Background(Colors.Grey.Lighten3)
    .CornerRadiusTopLeft(5)
    .CornerRadiusTopRight(10)
    .CornerRadiusBottomRight(20)
    .CornerRadiusBottomLeft(40)
    .Padding(25)
    .Text("Container with rounded corners");
```

Example 3 (text):
```text
container
    .CornerRadius(25)
    .Image("Resources/landscape.jpg");
```

Example 4 (text):
```text
container
    .Border(1, Colors.Black)
    .CornerRadius(15)
    .Table(table =>
    {
        table.ColumnsDefinition(columns =>
        {
            columns.ConstantColumn(100);
            columns.RelativeColumn();
            columns.ConstantColumn(150);
        });
        
        table.Header(header =>
        {
            header.Cell().Element(Style).Text("Index");
            header.Cell().Element(Style).Text("Label");
            header.Cell().Element(Style).Text("Price");

            IContainer Style(IContainer container)
            {
                return container
                    .Border(1, Colors.Grey.Darken2)
                    .Background(Colors.Grey.Lighten3)
                    .PaddingVertical(10)
                    .PaddingHorizontal(15)
                    .DefaultTextStyle(x => x.Bold());
            }
        });

        foreach (var index in Enumerable.Range(1, 5))
        {
            table.Cell().Element(Style).Text(index.ToString());
            table.Cell().Element(Style).Text(Placeholders.Label());
            table.Cell().Element(Style).Text(Placeholders.Price());
            
            IContainer Style(IContainer container)
            {
                return container
                    .Border(1, Colors.Grey.Darken2)
                    .PaddingVertical(10)
                    .PaddingHorizontal(15);
            }
        }
    });
```

---



---

## Rounded Corners


**URL:** https://www.questpdf.com/api-reference/rounded-corners.html

**Contents:**
- Rounded Corners ​
- Consistent Corner Radius ​
- Various Corner Radius ​
- Image Example ​
- Complex Example ​

Rounded corners can be applied to containers to create visually appealing designs.

This feature allows you to specify the radius of the corners, giving a softer look to the edges of the container.

In the vast majority of cases, you will want to apply the same corner radius to all corners of a container.

It is also possible to apply different corner radii to each corner of a container, allowing for more complex designs.

Rounded corners can also be applied to images, enhancing their appearance in documents.

Rounded corners can be used in more complex layouts, such as tables, to create a polished look.

**Examples:**

Example 1 (swift):
```swift
container
    .Border(1, Colors.Black)
    .Background(Colors.Grey.Lighten3)
    .CornerRadius(25)
    .Padding(25)
    .Text("Container with consistently rounded corners");
```

Example 2 (swift):
```swift
container
    .Border(1, Colors.Black)
    .Background(Colors.Grey.Lighten3)
    .CornerRadiusTopLeft(5)
    .CornerRadiusTopRight(10)
    .CornerRadiusBottomRight(20)
    .CornerRadiusBottomLeft(40)
    .Padding(25)
    .Text("Container with rounded corners");
```

Example 3 (swift):
```swift
container
    .CornerRadius(25)
    .Image("Resources/landscape.jpg");
```

Example 4 (swift):
```swift
container
    .Border(1, Colors.Black)
    .CornerRadius(15)
    .Table(table =>
    {
        table.ColumnsDefinition(columns =>
        {
            columns.ConstantColumn(100);
            columns.RelativeColumn();
            columns.ConstantColumn(150);
        });
        
        table.Header(header =>
        {
            header.Cell().Element(Style).Text("Index");
            header.Cell().Element(Style).Text("Label");
            header.Cell().Element(Style).Text("Price");

            IContainer Style(IContainer container)
            {
                return container
                    .Border(1, Colors.Grey.Darken2)
                    .Background(Colors.Grey.Lighten3)
                    .PaddingVertical(10)
                    .PaddingHorizontal(15)
                    .DefaultTextStyle(x => x.Bold());
            }
        });

        foreach (var index in Enumerable.Range(1, 5))
        {
            table.Cell().Element(Style).Text(index.ToString());
            table.Cell().Element(Style).Text(Placeholders.Label());
            table.Cell().Element(Style).Text(Placeholders.Price());
            
            IContainer Style(IContainer container)
            {
                return container
                    .Border(1, Colors.Grey.Darken2)
                    .PaddingVertical(10)
                    .PaddingHorizontal(15);
            }
        }
    });
```

---



---

## Row


**URL:** https://www.questpdf.com/api-reference/row.html

**Contents:**
- Row ​
- Item Types ​
- Basic usage ​
- Spacing ​
- Custom spacing ​
- Uniform item height ​
    - Default behavior (consistent item height) ​
    - Effect with ShrinkVertical applied ​

Draws a collection of elements horizontally.

It supports paging functionality, allowing content to flow naturally across multiple pages when needed. When required, child items are split across pages, ensuring that the content is not cut off.

For a row element with a width of 100 points that has three items (a relative item of size 1, a relative item of size 5, and a constant item of size 10 points), the items will occupy sizes of 15 points, 75 points, and 10 points respectively.

For ConstantItem, you can optionally specify the unit value (default is Unit.Points).

Learn more about supported units in the Lenght unit types section.

The Row element uses a lambda function to define its content. Inside the lambda, you can add multiple items using the Item method.

You can adjust the horizontal spacing between items using the Spacing method.

Optionally, you can specify the unit value (default is Unit.Points).

Learn more about supported units in the Lenght unit types section.

You can adjust the spacing between items individually by adding an empty ConstantItem with a specific width.

By default, all items in a Row match the height of the tallest item. This ensures consistent visual alignment, but sometimes it can result in unwanted visual stretching.

To disable this behavior, use the ShrinkVertical API:

**Examples:**

Example 1 (unknown):
```unknown
row.ConstantItem(5, Unit.Centimetre);
```

Example 2 (swift):
```swift
container
    .Padding(25)
    .Width(325)
    .Row(row =>
    {
        row.ConstantItem(100)
            .Background(Colors.Grey.Medium)
            .Padding(10)
            .Text("100pt");

        row.RelativeItem()
            .Background(Colors.Grey.Lighten1)
            .Padding(10)
            .Text("75pt");

        row.RelativeItem(2)
            .Background(Colors.Grey.Lighten2)
            .Padding(10)
            .Text("150pt");
    });
```

Example 3 (swift):
```swift
container
    .Padding(25)
    .Width(220)
    .Height(50)
    .Row(row =>
    {
        row.Spacing(10);

        row.RelativeItem(2).Background(Colors.Grey.Darken1);
        row.RelativeItem(3).Background(Colors.Grey.Medium);
        row.RelativeItem(5).Background(Colors.Grey.Lighten1);
    });
```

Example 4 (unknown):
```unknown
row.Spacing(5, Unit.Millimeters);
```

---



---

## Scale


**URL:** https://www.questpdf.com/api-reference/scale.html

**Contents:**
- Scale ​
  - Scaling Factor ​
  - Interaction with Other Elements ​
- Example ​

Values greater than one enlarge the content, while values less than one reduce it.

Although this adjustment modifies the space available to its inner content, some elements might use their own strategies to fill that space.

For example, an Image with the setting may retain its size, but its quality could vary based on the DPI setting. In contrast, text will not only appear smaller or bigger; but also a different number of words may fit each line.

Please note that all content inside the container is scaled proportionally: including text, images, padding, etc.

**Examples:**

Example 1 (unknown):
```unknown
container
    .Scale(1.5f)
    // enlarged content by 50%
```

Example 2 (swift):
```swift
container
    .Width(300)
    .Column(column =>
    {
        var scales = new[] { 0.75f, 1f, 1.25f, 1.5f };

        foreach (var scale in scales)
        {
            column
                .Item()
                .Border(1)
                .Scale(scale)
                .Padding(10)
                .Text($"Content scale: {scale}")
                .FontSize(20);
        }
    });
```

---



---

## Scale to fit


**URL:** https://www.questpdf.com/api-reference/scale-to-fit.html

**Contents:**
- Scale to fit ​

This container dynamically adjusts its content to fit within the available space by proportionally scaling it down if necessary.

By attempting to shrink its child elements, it prevents common layout issues such as infinite layout exceptions. It is particularly useful when your content generally fits within the available space but occasionally needs slight adjustments to maintain a consistent appearance.

This container determines the optimal scale value through multiple iterations. For complex content, this may introduce a significant performance overhead.

This component scales the available space, not the content directly. As a result, you may still encounter situations where content doesn't fit properly, especially when a child element enforces a specific aspect ratio or has other fixed dimensional constraints.

**Examples:**

Example 1 (swift):
```swift
container.Column(column =>
{
    const string text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.";

    foreach (var i in Enumerable.Range(4, 5))
    {
        column
            .Item()
            .Shrink()
            .Border(1)
            .Padding(15)
            .Width(i * 50) // sizes from 200x100 to 450x175
            .Height(i * 25)
            .ScaleToFit()
            .Text(text);
    }
});
```

---



---

## Shadow


**URL:** https://www.questpdf.com/api-reference/shadow.html

**Contents:**
- Shadow ​
- Blur ​
- Spread ​
- Offset X ​
- Offset Y ​
- Color ​
- Without Blur (Fast) ​

Shadows can enhance the visual depth and separation of elements in a document.

Gets or sets the blur radius of the shadow in pixels. Higher values produce a more diffused shadow with softer edges.

A value of 0 results in a sharp, unblurred shadow.

Values different from 0 may significantly impact performance and enlarge the output file size. Use with caution, especially in large documents or when rendering complex shadows.

Gets or sets the spread radius of the shadow in pixels.

Positive values cause the shadow to expand, negative values cause it to contract.

Gets or sets the horizontal offset of the shadow in pixels.

Positive values move the shadow to the right, negative values move it to the left.

Gets or sets the vertical offset of the shadow in pixels.

Positive values move the shadow downward, negative values move it upward.

Gets or sets the color of the shadow.

Shadows can be applied without any blur effect for a sharper appearance.

This approach is faster and results in significantly smaller file sizes, making it suitable for performance-sensitive applications.

**Examples:**

Example 1 (swift):
```swift
container
    .Border(1, Colors.Black)
    .Shadow(new BoxShadowStyle
    {
        Color = Colors.Grey.Medium, 
        Blur = 5, 
        Spread = 5,
        OffsetX = 5, 
        OffsetY = 5
    })
    .Background(Colors.White)
    .Padding(15)
    .Text("Important content");
```

Example 2 (swift):
```swift
.Row(row =>
{
    row.Spacing(50);

    foreach (var blur in new[] { 5, 10, 20 })
    {
        row.ConstantItem(100)
            .AspectRatio(1)
            .Shadow(new BoxShadowStyle
            {
                Color = Colors.Grey.Darken1,
                Blur = blur
            })
            .Background(Colors.White);
    }
});
```

Example 3 (swift):
```swift
.Row(row =>
{
    row.Spacing(50);

    foreach (var spread in new[] { 0, 5, 10 })
    {
        row.ConstantItem(100)
            .AspectRatio(1)
            .Shadow(new BoxShadowStyle
            {
                Color = Colors.Grey.Darken1,
                Blur = 5,
                Spread = spread
            })
            .Background(Colors.White);
    }
});
```

Example 4 (swift):
```swift
.Row(row =>
{
    row.Spacing(50);
    
    foreach (var offsetX in new[] { -10, 0, 10 })
    {
        row.ConstantItem(100)
            .AspectRatio(1)
            .Shadow(new BoxShadowStyle
            {
                Color = Colors.Grey.Darken1,
                Blur = 10,
                OffsetX = offsetX
            })
            .Background(Colors.White);
    }
});
```

---



---

## Show entire


**URL:** https://www.questpdf.com/api-reference/show-entire.html

**Contents:**
- Show entire ​
  - Example ​
  - Without the ShowEntire element ​
  - With the ShowEntire element ​

The ShowEntire element is designed to ensure that specific content remains on a single page, preventing it from being split across multiple pages.

While many elements within the library naturally support paging, allowing content to flow seamlessly across pages, ShowEntire enforces strict page constraints to maintain visual cohesiveness. This can be particularly useful when presenting structured data, tables, or definitions that should remain uninterrupted for clarity and readability.

The ShowEntire element imposes strict space constraints, which may lead to a DocumentLayoutException if the content exceeds the page's capacity. Ensure that the enclosed content fits within a single page to avoid errors.

Please consider using a less-strict alternative, EnsureSpace, if you want to maintain visual consistency without enforcing a hard page constraint.

The following example demonstrates how to use the ShowEntire element to create a glossary where each term and its definition remain together on the same page:

**Examples:**

Example 1 (swift):
```swift
container
    .Decoration(decoration =>
    {
        var terms = new[]
        {
            ("Function", "A reusable block of code designed to perform a specific task. Functions take input parameters, process them, and return results, making code modular, readable, and maintainable. They are an essential component of all programming languages."),
            ("Recursion", "A programming technique where a function calls itself in order to solve a problem by breaking it down into smaller, similar subproblems. Recursion is often used for complex algorithms, such as searching, sorting, and tree traversal."),
            ("Framework", "A pre-built collection of code, tools, and best practices that provides a structured foundation for developing software. Frameworks simplify development by handling common functionalities, such as database access, user authentication, and UI rendering."),
            ("Package", "A self-contained collection of code, typically consisting of functions, classes, and modules, that provides specific functionality. Packages help organize large projects and allow developers to reuse and distribute their code easily."),
        };
        
        decoration.Before().Text("Terms and their definitions:").FontSize(24).Bold().Underline();
        
        decoration.Content().PaddingTop(15).Column(column =>
        {
            column.Spacing(15);
            
            foreach (var term in terms)
            {
                column.Item()
                    .ShowEntire()
                    .Text(text =>
                    {
                        text.Span(term.Item1).Bold().FontColor(Colors.Blue.Darken2);
                        text.Span($" - {term.Item2}");
                    });
            }
        });
    });
```

---



---

## Show if


**URL:** https://www.questpdf.com/api-reference/show-if.html

**Contents:**
- Show if ​

The ShowIf element provides a simple way to conditionally display or hide content without breaking the fluent API chain. This is particularly useful when you need to show or hide sections of your document based on runtime conditions.

**Examples:**

Example 1 (swift):
```swift
var condition = numberOfElements > 5;

// c# if-statement approach
.Row(row =>
{
    row.RelativeItem().Text("One");

    var secondColumn = row.RelativeItem();

    if (condition)
        secondColumn.Text("Two");
});

// equivalent fluent approach
.Row(row =>
{
    row.RelativeItem().Text("One");
    
    row.RelativeItem().ShowIf(condition).Text("Two");
});
```

---



---

## Show once


**URL:** https://www.questpdf.com/api-reference/show-once.html

**Contents:**
- Show once ​
  - Example ​

The ShowOnce element provides fine-grained control over content rendering across multiple pages.

By default, all elements are fully rendered once and do not repeat. However, in specific contexts such as headers, footers, or decorative elements positioned before and after content slots, elements may be repeated on every page. To prevent this automatic repetition, use the ShowOnce element.

Combine this element with SkipOnce to achieve more complex behaviors, e.g.:

The following example demonstrates how to use ShowOnce to create a professional invoice header that shows different content on the first page compared to subsequent pages:

**Examples:**

Example 1 (swift):
```swift
container
    .Decoration(decoration =>
    {
        decoration.Before().Column(column =>
        {
            column.Item()
                .ShowOnce()
                .Row(row =>
                {
                    row.ConstantItem(80).AspectRatio(4 / 3f).Placeholder();
                    row.ConstantItem(10);
                    row.RelativeItem()
                        .AlignMiddle()
                        .Column(innerColumn =>
                        {
                            innerColumn.Item().Text("Invoice #1234").FontSize(24).Bold();
                            innerColumn.Item().Text($"Generated on {DateTime.Now:d}").FontSize(16).Light();
                        });
                });
            
            column.Item()
                .SkipOnce()
                .Text("Invoice #1234").FontSize(24).Bold();
        });
        
        // generate dummy content
        decoration.Content()
            .PaddingTop(15)
            .ExtendHorizontal()
            .Column(column =>
            {
                column.Spacing(10);
                
                foreach (var i in Enumerable.Range(1, 15))
                {
                    column.Item()
                        .Height(30)
                        .Background(Colors.Grey.Lighten3)
                        .AlignCenter()
                        .AlignMiddle()
                        .Text($"{i}");
                }
            });
    });
```

---



---

## Shrink


**URL:** https://www.questpdf.com/api-reference/shrink.html

**Contents:**
- Shrink ​

Renders its content in the most compact size achievable. Ideal for situations where the parent element provides more space than necessary.

---



---

## SkiaSharp Integration


**URL:** https://www.questpdf.com/api-reference/skiasharp-integration.html

**Contents:**
- SkiaSharp Integration ​
- Helper Script ​
- SVG ​
- Rasterization ​

QuestPDF supports multiple ways to integrate dynamic complex graphics into PDF documents.

The preferred method is to generate an SVG image, as it ensures scalability and lightweight nature. However, in cases where this approach is not sufficient, you can use the SkiaSharp library to create custom graphics and effects.

This section provides examples of how to integrate the SkiaSharp library with QuestPDF. This library is available under the "MIT" license.

We extend our thanks to the authors and maintainers of that project for their contributions to the open-source community.

Please note that the SkiaSharp library is not included in the QuestPDF package. You need to install it separately via the NuGet package manager.

If you are planning to host your application on a Linux server, please install the following nuget package: SkiaSharp.NativeAssets.Linux.NoDependencies

To simplify the integration of SkiaSharp with QuestPDF, you'll need to add a helper class to your project. This class provides two extension methods that bridge QuestPDF's container system with SkiaSharp's canvas-based drawing approach.

Copy the following code into your project to enable SkiaSharp integration:

The SkiaSharpSvgCanvas method allows you to use SkiaSharp's drawing capabilities while maintaining the benefits of vector graphics. This approach is ideal for most custom graphics as it preserves sharp edges and details at any scale and keeps document file sizes smaller compared to rasterized images.

The following example demonstrates how to create a vector-based clock graphic using SkiaSharp.

While vector graphics are preferred for most cases, some visual effects and complex operations in SkiaSharp can only be properly rendered through rasterization. The SkiaSharpRasterizedCanvas method allows you to use the full range of SkiaSharp's capabilities when you need effects that aren't supported by SVG.

The following example demonstrates how to draw an image with rounded corners and apply a drop shadow effect.

**Examples:**

Example 1 (csharp):
```csharp
using System.Text;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using SkiaSharp;

namespace QuestPDF.SkiaSharpIntegration;

public static class SkiaSharpHelpers
{
    public static void SkiaSharpSvgCanvas(this IContainer container, Action<SKCanvas, Size> drawOnCanvas)
    {
        container.Svg(size =>
        {
            using var stream = new MemoryStream();

            using (var canvas = SKSvgCanvas.Create(new SKRect(0, 0, size.Width, size.Height), stream))
                drawOnCanvas(canvas, size);
            
            var svgData = stream.ToArray();
            return Encoding.UTF8.GetString(svgData);
        });
    }
    
    public static void SkiaSharpRasterizedCanvas(this IContainer container, Action<SKCanvas, ImageSize> drawOnCanvas)
    {
        container.Image(payload =>
        {
            using var bitmap = new SKBitmap(payload.ImageSize.Width, payload.ImageSize.Height);

            using (var canvas = new SKCanvas(bitmap))
            {
                canvas.Scale(payload.ImageSize.Width / payload.AvailableSpace.Width, payload.ImageSize.Height / payload.AvailableSpace.Height);
                drawOnCanvas(canvas, new ImageSize((int)payload.AvailableSpace.Width, (int)payload.AvailableSpace.Height));
            }
        
            return bitmap.Encode(SKEncodedImageFormat.Png, 100).ToArray();
        });
    }
}
```

Example 2 (swift):
```swift
Document.Create(document =>
{
    document.Page(page =>
    {
        page.Size(350, 350);
        page.Margin(25);

        page.Content()
            .Width(300)
            .Height(300)
            .SkiaSharpSvgCanvas((canvas, size) =>
            {
                var centerX = size.Width / 2;
                var centerY = size.Height / 2;
                var radius = Math.Min(centerX, centerY);

                // draw clock face
                using var facePaint = new SKPaint
                {
                    Color = new SKColor(Colors.Blue.Lighten4)
                };

                canvas.DrawCircle(centerX, centerY, radius, facePaint);

                // draw clock ticks
                using var tickPaint = new SKPaint
                {
                    Color = new SKColor(Colors.Blue.Darken4), 
                    StrokeWidth = 4, 
                    StrokeCap = SKStrokeCap.Round
                };

                canvas.Save();
                canvas.Translate(centerX, centerY);

                foreach (var i in Enumerable.Range(0, 12))
                {
                    canvas.DrawLine(new SKPoint(0, radius * 0.85f), new SKPoint(0, radius * 0.95f), tickPaint);
                    canvas.RotateDegrees(30);
                }

                canvas.Restore();

                // draw clock hands
                using var hourHandPaint = new SKPaint
                {
                    Color = new SKColor(Colors.Blue.Darken4),
                    StrokeWidth = 8,
                    StrokeCap = SKStrokeCap.Round
                };

                using var minuteHandPaint = new SKPaint
                {
                    Color = new SKColor(Colors.Blue.Darken2),
                    StrokeWidth = 4,
                    StrokeCap = SKStrokeCap.Round
                };

                canvas.Translate(centerX, centerY);

                canvas.Save();
                canvas.RotateDegrees(6 * DateTime.Now.Minute);
                canvas.DrawLine(new SKPoint(0, 0), new SKPoint(0, -radius * 0.7f), minuteHandPaint);
                canvas.Restore();
                
                canvas.Save();
                canvas.RotateDegrees(30 * DateTime.Now.Hour + DateTime.Now.Minute / 2);
                canvas.DrawLine(new SKPoint(0, 0), new SKPoint(0, -radius * 0.5f), hourHandPaint);
                canvas.Restore();
            });
    });
})
.GeneratePdf("clock.pdf");
```

Example 3 (swift):
```swift
Document.Create(document =>
{
    document.Page(page =>
    {
        page.Size(new PageSize(500, 400));
        page.DefaultTextStyle(x => x.FontSize(20));

        page.Content()
            .Padding(25)
            .SkiaSharpRasterizedCanvas((canvas, size) =>
            {
                // add padding to properly display the shadow effect
                const float padding = 25;
                canvas.Translate(padding, padding);
                
                // load image and scale canvas space
                using var bitmap = SKBitmap.Decode("Resources/landscape.jpg");
                
                var targetBitmapSize = new SKSize(size.Width - 2 * padding, size.Height - 2 * padding);
                var scale = Math.Min(targetBitmapSize.Width / bitmap.Width, targetBitmapSize.Height / bitmap.Height);
                canvas.Scale(scale);

                var drawingArea = new SKRoundRect(new SKRect(0, 0, bitmap.Width, bitmap.Height), 32, 32);
                
                // draw drop shadow
                using var dropShadowFilter = SKImageFilter.CreateDropShadow(8, 8, 16, 16, SKColors.Black);
                using var paint = new SKPaint
                {
                    ImageFilter = dropShadowFilter
                };

                canvas.DrawRoundRect(drawingArea, paint);
                
                // draw image
                canvas.ClipRoundRect(drawingArea, antialias: true);
                canvas.DrawBitmap(bitmap, SKPoint.Empty);
            });
    });
})
.GeneratePdf("rasterized-effect.pdf");
```

---



---

## Skip once


**URL:** https://www.questpdf.com/api-reference/skip-once.html

**Contents:**
- Skip once ​
  - Example ​

If the container spans multiple pages, its content is omitted on the first page and then displayed on the second and subsequent pages.

A common use-case for this element is when displaying a consistent header across pages but needing to conditionally show/hide specific fragments on the first page.

Combine this element with SkipOnce to achieve more complex behaviors, e.g.:

In this example, the SkipOnce and ShowOnce elements are combined to ensure that if a glossary term spans multiple pages, the header displays "Continued" on the second and subsequent pages.

**Examples:**

Example 1 (swift):
```swift
container
    .Column(column =>
    {
        var terms = new[]
        {
            ("Repository", "A centralized storage location for source code and related files, typically managed using version control systems like Git. Repositories allow multiple developers to collaborate on projects, track changes, and maintain version history."),
            ("Version Control", "A system that tracks changes to code over time, enabling developers to collaborate efficiently, revert to previous versions, and maintain a structured development workflow. Popular version control tools include Git, Mercurial, and Subversion."),
            ("Abstraction", "A programming concept that hides complex implementation details and exposes only the necessary parts. Abstraction helps simplify code and allows developers to focus on high-level design rather than low-level implementation details."),
            ("Namespace", "A container that groups related identifiers, such as variables, functions, and classes, to prevent naming conflicts in a program. Namespaces are commonly used in large projects to organize code efficiently."),
        };
        
        column.Spacing(15);
        
        foreach (var term in terms)
        {
            column.Item().Decoration(decoration =>
            {
                decoration.Before()
                    .DefaultTextStyle(x => x.FontSize(24).Bold().FontColor(Colors.Blue.Darken2))
                    .Column(innerColumn =>
                    {
                        innerColumn.Item().ShowOnce().Text(term.Item1);
                        
                        innerColumn.Item().SkipOnce().Text(text =>
                        {
                            text.Span(term.Item1);
                            text.Span(" (continued)").Light().Italic();
                        });
                    });

                decoration.Content().Text(term.Item2);
            });
        }
    });
```

---



---

## Stop paging


**URL:** https://www.questpdf.com/api-reference/stop-paging.html

**Contents:**
- Stop paging ​
- Example ​
  - Without StopPaging ​
  - With StopPaging ​

Renders the element exclusively on the first page. Any portion of the element that doesn't fit is omitted.

If your goal is to limit the content to a specific area, please consider using other approaches: Text Clamp Lines and Scale to Fit.

Unable to display PDF file. Download instead.

Unable to display PDF file. Download instead.

**Examples:**

Example 1 (swift):
```swift
const string bookDescription = "\"Master Modern C# Development\" is a comprehensive guide that takes you from the basics to advanced concepts in C# programming. Perfect for beginners and intermediate developers looking to enhance their skills with practical examples and real-world applications. Covering object-oriented programming, LINQ, asynchronous programming, and the latest .NET features, this book provides step-by-step explanations to help you write clean, efficient, and scalable code. Whether you're building desktop, web, or cloud applications, this resource equips you with the knowledge and best practices to become a confident C# developer.";

container
    .Width(400)
    .Height(300)
    .StopPaging()
    .Decoration(decoration =>
    {
        decoration.Before().Text("Book description:").Bold();
        decoration.Content().Text(bookDescription);
    });
```

---



---

## Style inheritance


**URL:** https://www.questpdf.com/api-reference/text/style-inheritance.html

**Contents:**
- Style inheritance ​

The DefaultTextStyle API allows you to define text styles that are automatically inherited by all child elements unless explicitly overridden.

This hierarchical approach simplifies style management, ensuring consistency across document sections while enabling easy local customizations.

It reduces repetitive code, enhances maintainability, and provides flexibility to adjust styles at different levels of the document structure.

**Examples:**

Example 1 (swift):
```swift
.DefaultTextStyle(style => style.FontSize(20))
.Column(column =>
{
    column.Spacing(10);
    
    column.Item().Text("Products").ExtraBold().Underline().DecorationThickness(2);
    
    column.Item().Text("Comments: " + Placeholders.Sentence());
    
    column.Item()
        .DefaultTextStyle(style => style.FontSize(14))
        .Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.ConstantColumn(30);
                columns.RelativeColumn(1);
                columns.RelativeColumn(2);
            });
    
            table.Header(header =>
            {
                header.Cell().Element(Style).Text("ID");
                header.Cell().Element(Style).Text("Name");
                header.Cell().Element(Style).Text("Description");

                IContainer Style(IContainer container)
                {
                    return container
                        .Background(Colors.Grey.Lighten3)
                        .BorderBottom(1)
                        .PaddingHorizontal(5)
                        .PaddingVertical(10)
                        .DefaultTextStyle(x => x.Bold().FontColor(Colors.Blue.Medium));
                }
            });

            foreach (var i in Enumerable.Range(0, 5))
            {
                table.Cell().Element(Style).Text(i.ToString()).Bold();
                table.Cell().Element(Style).Text(Placeholders.Label());
                table.Cell().Element(Style).Text(Placeholders.Sentence());
            }
        
            IContainer Style(IContainer container) => container.container.Padding(5);
        });
});
```

---



---

## Unconstrained


**URL:** https://www.questpdf.com/api-reference/unconstrained.html

**Contents:**
- Unconstrained ​

The Unconstrained container creates a space where content can go beyond the limits set by parent elements. This helps when you need elements that overlap or extend past their container.

When you use Unconstrained, the container doesn't take up space in the layout. It removes any size limits from parent elements, making content appear to "float" compared to other elements. You can pair this with translation to place elements exactly where you want them.

**Examples:**

Example 1 (swift):
```swift
container
    .Width(400)
    .Height(350)
    .Padding(25)
    .PaddingLeft(50) 
    .Column(column =>
    {
        column.Item().Width(300).Height(150).Background(Colors.Blue.Lighten3);

        column
            .Item()
            .Unconstrained()
            .OffsetX(-50)
            .OffsetY(-50)
            .Width(100)
            .Height(100)
            .Background(Colors.Blue.Darken2);

        column.Item().Width(300).Height(150).Background(Colors.Blue.Lighten2);
    });
```

---



---

## Width


**URL:** https://www.questpdf.com/api-reference/width.html

**Contents:**
- Width ​
- Example ​

Use this element to control the horizontal size of its content.

The following example shows how text content adjusts to the specified width constraints.

Please be careful. This component may try to enforce size constraints that are impossible to meet. For example, the container may require more space than is available, or may try to squeeze its child into less space than possible.

Such scenarios result in a layout exception.

**Examples:**

Example 1 (swift):
```swift
container
    .Width(300)
    .Padding(25)
    .Column(column =>
    {
        column.Spacing(25);
        
        column.Item()
            .MinWidth(200)
            .Background(Colors.Grey.Lighten3)
            .Text("Lorem ipsum");
        
        column.Item()
            .MaxWidth(100)
            .Background(Colors.Grey.Lighten3)
            .Text("dolor sit amet");
    });
```

---



---

## Z-Index


**URL:** https://www.questpdf.com/api-reference/zindex.html

**Contents:**
- Z-Index ​
- Example ​
    - Without Z-Index Element (Default Behavior) ​
    - With Z-Index 1 Element (Correct Implementation) ​
    - With Z-Index -1 Element (Incorrect Implementation) ​

By default, the library draws content in the order it is defined, which may not always be the desired behavior. This element allows you to alter the rendering order, ensuring that the content is displayed in the correct sequence.

The default z-index is 0, unless a different value is inherited from a parent container. Higher values are rendered above lower values.

The following example shows how to use the ZIndex element to create visually appealing pricing tables.

**Examples:**

Example 1 (swift):
```swift
container
    .PaddingVertical(15)
    .Border(2)
    .Row(row =>
    {
        row.RelativeItem()
            .Background(Colors.Grey.Lighten3)
            .Element(c => AddPricingItem(c, "Community", "Free"));
        
        row.RelativeItem()
            .ZIndex(1) // -1 or 0 or 1
            .Padding(-15)
            .Border(1)
            .Background(Colors.Grey.Lighten1)
            .PaddingTop(15)
            .Element(c => AddPricingItem(c, "Professional", "$699"));
        
        row.RelativeItem()
            .Background(Colors.Grey.Lighten3)
            .Element(c => AddPricingItem(c, "Enterprise", "$1999")); 

        void AddPricingItem(IContainer container, string name, string formattedPrice)
        {
            container
                .Padding(25)
                .Column(column =>
                {
                    column.Item().AlignCenter().Text(name).FontSize(24).Black();
                    column.Item().AlignCenter().Text(formattedPrice).FontSize(20).SemiBold();
                    
                    column.Item().PaddingHorizontal(-25).PaddingVertical(10).LineHorizontal(1);
                    
                    foreach (var i in Enumerable.Range(1, 4))
                    {
                        column.Item()
                            .PaddingTop(10)
                            .AlignCenter()
                            .Text(Placeholders.Label())
                            .FontSize(16)
                            .Light();
                    }
                });
        }
    });
```

---


---

## # Questpdf - Concepts


**Pages:** 19

---



---

## Code pattern: capture content position


**URL:** https://www.questpdf.com/concepts/code-patterns/capture-content-position.html

**Contents:**
- Code pattern: capture content position ​
- Example ​
  - Capturing position ​
  - Generating dependent content ​

When generating PDF documents, you sometimes need to create elements that depend on the position of other content already placed in the document.

QuestPDF provides the CaptureContentPosition API to address these scenarios elegantly.

This feature observes the rendering process of your content and captures its precise position and size on each page. You can then use this captured positional data in a Dynamic component to build and position other elements exactly where you need them, creating sophisticated layout relationships between different parts of your document.

When using the GetContentCapturedPositions method, keep in mind that it may return an empty or incomplete array depending on the current document rendering phase. It is expected behavior, as the document generation process requires two rendering passes.

Your implementation should handle these cases gracefully, as shown in the example above.

The following example demonstrates how to implement a demo of proofreading functionality. It highlights incorrect words in red with strikethrough formatting and adds corrected versions in green. Finally, it places an icon beside each correction for easy identification.

To implement this feature, we need to capture two types of positions: the position of the entire text container as a reference point, and the specific positions of each mistake that needs an icon.

The dynamic component below uses the captured positions to generate and place correction icons. Notice how we retrieve both the container position and the positions of each mistake marker to calculate the proper placement of each icon.

**Examples:**

Example 1 (swift):
```swift
Document
.Create(document =>
{
    document.Page(page =>
    {
        page.ContinuousSize(575);
        page.DefaultTextStyle(x => x.FontSize(20));
        page.Margin(25);

        page.Content()
            .Background(Colors.White)
            .Row(row =>
            {
                row.Spacing(25);

                row.ConstantItem(0).Dynamic(new DynamicTextSpanPositionCapture());

                row.RelativeItem().CaptureContentPosition("container").Text(text =>
                {
                    text.Justify();
                    
                    var mistakeTextStyle = TextStyle.Default
                        .FontColor(Colors.Red.Darken3)
                        .BackgroundColor(Colors.Red.Lighten4)
                        .Strikethrough()
                        .DecorationThickness(2);
                    
                    var correctionTextStyle = TextStyle.Default
                        .FontColor(Colors.Green.Darken3)
                        .BackgroundColor(Colors.Green.Lighten4);

                    text.Span("Proofreading").Bold().Underline().DecorationThickness(2);
                    text.Span(" technical documentation is a critical quality assurance step that ensures clarity, accuracy, and consistency across all written content. It involves more than just checking for grammar and ");
                    text.Span("spilling").Style(mistakeTextStyle);
                    text.Span("spelling").Style(correctionTextStyle);
                    text.Element(TextInjectedElementAlignment.Middle).CaptureContentPosition("mistake");
                    text.Span(" errors—it also includes verifying terminology, code syntax, formatting standards, and logical flow. A common best practice is to have the content reviewed by both a subject matter ");
                    text.Span("export").Style(mistakeTextStyle);
                    text.Span("expert").Style(correctionTextStyle);
                    text.Element(TextInjectedElementAlignment.Middle).CaptureContentPosition("mistake");
                    text.Span(" and a language specialist, ensuring that the material is technically sound while also being accessible to the intended audience.");
                });
            });
    });
})
.GeneratePdf("file.pdf");
```

Example 2 (swift):
```swift
public class DynamicTextSpanPositionCapture : IDynamicComponent
{
    public DynamicComponentComposeResult Compose(DynamicContext context)
    {
        var containerLocation = context.GetContentCapturedPositions("container").FirstOrDefault(x => x.PageNumber == context.PageNumber);
        var mistakeLocations = context.GetContentCapturedPositions("mistake").Where(x => x.PageNumber == context.PageNumber).ToList();
        
        if (containerLocation == null || mistakeLocations.Count == 0)
        {
            return new DynamicComponentComposeResult
            {
                Content = context.CreateElement(_ => { }),
                HasMoreContent = false
            };
        }

        var content = context.CreateElement(container =>
        {
            container.Layers(layers =>
            {
                layers.PrimaryLayer();

                foreach (var mistakeLocation in mistakeLocations)
                {
                    layers
                        .Layer()
                        .Unconstrained() 
                        .OffsetY(mistakeLocation.Y - containerLocation.Y)
                        .OffsetX(-12)
                        .OffsetY(-12)
                        .Width(24)
                        .Svg("Resources/proofreading.svg");
                }
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

---



---

## Code pattern: content styling


**URL:** https://www.questpdf.com/concepts/code-patterns/content-styling.html

**Contents:**
- Code pattern: content styling ​

When designing PDF documents, many elements often share similar styles. Instead of repeating styling code for each element, encapsulating the styling logic into reusable functions can improve maintainability and readability.

By defining local functions, you can ensure consistency across multiple elements while reducing redundancy. Below is an example demonstrating this approach:

**Examples:**

Example 1 (swift):
```swift
container.Table(table =>
{
    table.ColumnsDefinition(columns =>
    {
        columns.ConstantColumn(50);
        columns.RelativeColumn(1);
        columns.RelativeColumn(2);
    });
    
    table.Header(header =>
    {
        header.Cell().Element(Style).Text("#");
        header.Cell().Element(Style).Text("Product Name");
        header.Cell().Element(Style).Text("Description");

        IContainer Style(IContainer container)
        {
            return container
                .Background(Colors.Blue.Lighten5)
                .Padding(10)
                .DefaultTextStyle(TextStyle.Default.FontColor(Colors.Blue.Darken4).Bold());
        }
    });

    foreach (var i in Enumerable.Range(1, 5))
    {
        table.Cell().Element(Style).Text(i.ToString());
        table.Cell().Element(Style).Text(Placeholders.Label());
        table.Cell().Element(Style).Text(Placeholders.Sentence());
    }

    IContainer Style(IContainer container)
    { 
        return container
            .BorderTop(2)
            .BorderColor(Colors.Blue.Lighten3)
            .Padding(10);
    }
});
```

---



---

## Code pattern: document code structure


**URL:** https://www.questpdf.com/concepts/code-patterns/document-structure.html

**Contents:**
- Code pattern: document code structure ​

Organizing your code effectively is crucial for maintainability and readability. A recommended approach is to encapsulate your document generation within a single class, while breaking down the document structure into well-named private methods.

This pattern allows you to separate the overall document structure from the implementation details of individual sections, making your code more modular and easier to maintain as your documents grow in complexity.

Unable to display PDF file. Download instead.

**Examples:**

Example 1 (swift):
```swift
public class MyReport
{
    public byte[] GenerateReport()
    {
        return Document
            .Create(document =>
            {
                document.Page(page =>
                {
                    page.Size(PageSizes.A5);
                    page.DefaultTextStyle(x => x.FontSize(20));
                    page.Margin(25);

                    page.Content()
                        .PaddingBottom(15)
                        .Column(column =>
                        {
                            column.Item().Element(ReportTitle);
                            column.Item().PageBreak();
                            column.Item().Element(RedSection);
                            column.Item().PageBreak();
                            column.Item().Element(GreenSection);
                            column.Item().PageBreak();
                            column.Item().Element(BlueSection);
                        });

                    page.Footer().AlignCenter().Text(text => text.CurrentPageNumber());
                });
            })
            .GeneratePdf();
    }

    private void ReportTitle(IContainer container)
    {
        container.Extend()
            .AlignCenter()
            .AlignMiddle()
            .Text("Multi-section report")
            .FontSize(48)
            .Bold();
    }
    
    // dumb implementation of different document sections
    
    private void RedSection(IContainer container)
    {
        container.Grid(grid =>
        {
            grid.Columns(3);
            grid.Spacing(15);
            
            grid.Item(3 ).Text("Red section")
                .FontColor(Colors.Red.Darken2).FontSize(32).Bold();

            grid.Item(3).Text(Placeholders.Paragraph()).Light();

            foreach (var i in Enumerable.Range(0, 6))
                grid.Item().AspectRatio(4 / 3f).Background(Colors.Red.Lighten4);
        });
    }
    
    private void GreenSection(IContainer container)
    {
        container.Grid(grid =>
        {
            grid.Columns(3);
            grid.Spacing(15);
            
            grid.Item(3).Text("Green section")
                .FontColor(Colors.Green.Darken2).FontSize(32).Bold();

            grid.Item(3).Text(Placeholders.Paragraph()).Light();

            foreach (var i in Enumerable.Range(0, 12))
                grid.Item().AspectRatio(4 / 3f).Background(Colors.Green.Lighten4);
        });
    }
    
    private void BlueSection(IContainer container)
    {
        container.Grid(grid =>
        {
            grid.Columns(3);
            grid.Spacing(15);
            
            grid.Item(3).Text("Blue section")
                .FontColor(Colors.Blue.Darken2).FontSize(32).Bold();

            grid.Item(3).Text(Placeholders.Paragraph()).Light();

            foreach (var i in Enumerable.Range(0, 18))
                grid.Item().AspectRatio(4 / 3f).Background(Colors.Blue.Lighten4);
        });
    }
}
```

---



---

## Code pattern: extension methods


**URL:** https://www.questpdf.com/concepts/code-patterns/extension-methods.html

**Contents:**
- Code pattern: extension methods ​
    - Defining extension methods ​
    - Using extension methods ​

When you find yourself implementing the same styling or content patterns repeatedly across different documents, extension methods offer the perfect solution. They allow you to define your styling once and reuse it throughout your codebase, ensuring visual consistency while reducing duplication.

**Examples:**

Example 1 (swift):
```swift
public static class TableExtensions
{
    private static IContainer TableCellStyle(this IContainer container, string backgroundColor)
    {
        return container
            .Border(1)
            .BorderColor(Colors.Black)
            .Background(backgroundColor)
            .Padding(10);
    }
    
    public static void TableLabelCell(this IContainer container, string text)
    {
        container
            .TableCellStyle(Colors.Grey.Lighten3)
            .Text(text)
            .Bold();
    }
    
    public static IContainer TableValueCell(this IContainer container)
    {
        return container.TableCellStyle(Colors.Transparent);
    }
}
```

Example 2 (swift):
```swift
container
    .Border(1)
    .Table(table =>
    {
        table.ColumnsDefinition(columns =>
        {
            columns.RelativeColumn(2);
            columns.RelativeColumn(3);
            columns.RelativeColumn(2);
            columns.RelativeColumn(3);
        });
        
        table.Cell().TableLabelCell("Product name");
        table.Cell().TableValueCell().Text(Placeholders.Label());
        
        table.Cell().TableLabelCell("Description");
        table.Cell().TableValueCell().Text(Placeholders.Sentence());
        
        table.Cell().TableLabelCell("Price");
        table.Cell().TableValueCell().Text(Placeholders.Price());
        
        table.Cell().TableLabelCell("Date of production");
        table.Cell().TableValueCell().Text(Placeholders.ShortDate());
        
        table.Cell().ColumnSpan(2).TableLabelCell("Photo of the product");
        table.Cell().ColumnSpan(2).TableValueCell().AspectRatio(16 / 9f).Image(Placeholders.Image);
    });
```

---



---

## Code pattern: local helpers


**URL:** https://www.questpdf.com/concepts/code-patterns/local-helpers.html

**Contents:**
- Code pattern: local helpers ​

When building complex document layouts, you might find yourself repeating similar code structures. C# local functions offer an elegant solution to this challenge, allowing you to encapsulate reusable layout logic directly within your document generation method.

Local functions help maintain clean, readable code by defining specialized helper methods exactly where they're needed. This approach keeps related code together, improving both readability and maintainability without polluting your class with single-use methods.

**Examples:**

Example 1 (swift):
```swift
container.Column(column =>
{
    column.Spacing(15);

    column.Item().Text("Business details:").FontSize(24).Bold().FontColor(Colors.Blue.Darken2);
    
    AddContactItem("Resources/Icons/phone.svg", Placeholders.PhoneNumber());
    AddContactItem("Resources/Icons/email.svg", Placeholders.Email());
    AddContactItem("Resources/Icons/web.svg", Placeholders.WebpageUrl());

    void AddContactItem(string iconPath, string label)
    {
        column.Item().Row(row =>
        {
            row.ConstantItem(32).AspectRatio(1).Svg(iconPath);
            row.ConstantItem(15);
            row.AutoItem().AlignMiddle().Text(label);
        });
    }
});
```

---



---

## Colors


**URL:** https://www.questpdf.com/concepts/colors.html

**Contents:**
- Colors ​
- Color definitions ​
  - HEX Colors ​
  - Alpha channel ​
  - Shorthand HEX ​
- Examples ​
- Material Design colors ​

QuestPDF supports multiple color formats.

A hexadecimal color is specified with: #RRGGBB, where the RR (red), GG (green) and BB (blue) hexadecimal integers specify the components of the color. All values range from 00 to FF, and are case-insensitive.

To specify an alpha channel, add two more hexadecimal digits in front of the color code: #AARRGGBB where AA is the alpha channel. The alpha channel defines the transparency of a color and ranges from 00 (fully transparent) to FF (fully opaque).

You can use shorthand HEX codes with 3 or 4 digits. The library will automatically expand them to the full 6 or 8-digit format. For example, #123 will be expanded to #112233 and #89AB to #8899AABB.

You can also omit the hash sign (#) at the beginning of the color code.

Please be aware that in some software the alpha channel is specified at the end of the color code, e.g. #RRGGBBAA.

For your convenience, QuestPDF provides a list of colors from the Google Material Design palette.

**Examples:**

Example 1 (swift):
```swift
using QuestPDF.Helpers;

container
    .Padding(20)
    .Border(1)
    .BorderColor("#03A9F4")
    .Background(Colors.LightBlue.Lighten5)
    .Padding(20)
    .Text("Blue text")
    .Bold()
    .FontColor(Colors.LightBlue.Darken4)
    .Underline()
    .DecorationWavy()
    .DecorationColor(0xFF0000);
```

---



---

## Document metadata


**URL:** https://www.questpdf.com/concepts/document-metadata.html

**Contents:**
- Document metadata ​

It is possible to include additional information about the PDF document. This metadata is stored in the PDF file and can be viewed in the document properties.

**Examples:**

Example 1 (swift):
```swift
Document
    .Create(document =>
    {
        document.Page(page =>
        {
            page.Content().Text("Your invoice content");
        });
    })
    .WithMetadata(new DocumentMetadata
    {
        Title = "Invoice",
        Author = "John Doe",
        Subject = "Invoice for services",
        Keywords = "invoice, services, payment",
        Creator = "MyApplication",
        Producer = "PdfRpt",
        Language = "en-US",
        CreationDate = DateTimeOffset.Now,
        ModifiedDate = DateTimeOffset.Now
    })
    .GeneratePdf("document.pdf");
```

---



---

## Document Settings


**URL:** https://www.questpdf.com/concepts/document-settings.html

**Contents:**
- Document Settings ​

QuestPDF provides comprehensive control over the document generation process through the DocumentSettings class. These settings allow you to fine-tune various aspects of your PDF output, including compliance standards, compression, image quality, and content direction.

**Examples:**

Example 1 (swift):
```swift
Document
    .Create(document =>
    {
        document.Page(page =>
        {
            page.Content().Text("Your document content");
        });
    })
    .WithSettings(new DocumentSettings
    {
        PDFA_Conformance = PDFA_Conformance.PDFA_3B,
        PDFUA_Conformance = PDFUA_Conformance.None,
        CompressDocument = true,
        ImageCompressionQuality = ImageCompressionQuality.High,
        ImageRasterDpi = 288,
        ContentDirection = ContentDirection.LeftToRight
    })
    .GeneratePdf("document.pdf");
```

---



---

## Exceptions


**URL:** https://www.questpdf.com/concepts/common-exceptions.html

**Contents:**
- Exceptions ​
- DocumentComposeException ​
    - Possible Causes: ​
    - Resolution: ​
- DocumentDrawingException ​
- DocumentLayoutException ​
  - Enhanced Debugging Context ​
  - Example ​

While developing with QuestPDF, you might encounter issues during the PDF rendering process. Understanding the potential sources of these exceptions, their root causes, and the appropriate fixes is crucial. QuestPDF categorizes exceptions into three distinct groups:

For enhanced development and debugging experience, please consider using the QuestPDF Companion App.

This exception arises during the document composition phase, where you use the Fluent API to assemble various elements and create the final layout. Common tasks in this phase include working with your input data, applying conditions, iterating through loops, and calling additional methods.

This exception occurs during the document generation phase, when the rendering engine converts the layout tree into drawing commands. Unlike DocumentComposeException, this type typically stems from internal issues or problems with custom components.

If you encounter this exception, it could indicate a bug in the QuestPDF library. Please reach out to our support team with the error details, and we’ll work to resolve it promptly.

If you are using Dynamic Components, all exceptions thrown there are going to bubble up as this type of exception. In such case, please review the implementation of your dynamic components.

This exception can be challenging to resolve as it occurs with valid document trees that impose constraints impossible to satisfy.

For instance, attempting to draw a rectangle larger than the available page space triggers the rendering engine to wrap the content, hoping sufficient space will be available on the next page.

When the QuestPDF.Settings.EnableDebugging is set to true, or the debugger is attached, the library provides additional information to help you diagnose and resolve layout issues.

The code below contains conflicting size constraints.

And generates the following exception:

**Examples:**

Example 1 (swift):
```swift
.Padding(10)
.Width(100)
.Background(Colors.Grey.Lighten3)
.DebugPointer("Example debug pointer")
.Column(x =>
{
    x.Item().Text("Test");
    x.Item().Width(150); // requires 150pt width where only 100pt is available
});
```

Example 2 (yaml):
```yaml
The provided document content contains conflicting size constraints. For example, some elements may require more space than is available. 

The layout issue is likely present in the following part of the document: 

-> Document

-> Page

-> Page

-> Content

-> Content

-> In method:   content
   Called from: Render
   Source path: /Users/marcinziabek/RiderProjects/QuestPDF/Source/QuestPDF.Examples/Engine/RenderingTest.cs
   Line number: 100

-> Example debug pointer



To learn more, please analyse the document measurement of the problematic location: 

🔴 Column
==========
Available Space: (Width: 100,000, Height: 340,000)
Space Plan: Wrap
Wrap Reason: The available space is not sufficient for even partially rendering a single item.
----------


   ⚪️ TextBlock
   =============
   Alignment: Start
   Content Direction: LeftToRight
   Line Clamp: -
   Line Clamp Ellipsis: -
   Paragraph Spacing: 0
   Paragraph First Line Indentation: 0
   Text: Test


   🚨 Constrained 🚨
   ==================
   Available Space: (Width: 100,000, Height: 340,000)
   Space Plan: Wrap
   Wrap Reason: The available horizontal space is less than the minimum width.
   ------------------
   Content Direction: LeftToRight
   Min Width: 150
   Max Width: 150
   Min Height: -
   Max Height: -
   Enforce Size When Empty: False


      🟢 Empty
      =========
      Available Space: (Width: 0,000, Height: 0,000)
      Space Plan: FullRender (Width: 0,000, Height: 0,000)
      ---------


Legend: 
🚨 - Element that is likely the root cause of the layout issue based on library heuristics and prediction. 
🔴 - Element that cannot be drawn due to the provided layout constraints. This element likely causes the layout issue, or one of its descendant children is responsible for the problem. 
🟡 - Element that can be partially drawn on the page and will also be rendered on the consecutive page. In more complex layouts, this element may also cause issues or contain a child that is the actual root cause.
🟢 - Element that is successfully and completely drawn on the page.
⚪️ - Element that has not been drawn on the faulty page. Its children are omitted.
```

---



---

## Execution order


**URL:** https://www.questpdf.com/concepts/code-patterns/execution-order.html

**Contents:**
- Execution order ​

QuestPDF uses a fluent API with method chaining to define your document's structure and appearance. The execution order of these chained methods is strict, meaning that rearranging them may lead to different visual outcomes.

**Examples:**

Example 1 (swift):
```swift
container.Column(column =>
{
    column.Spacing(25);

    column.Item()
        .Border(1)
        .Background(Colors.Blue.Lighten4)
        .Padding(15)
        .Text("border → background → padding");
    
    column.Item()
        .Border(1)
        .Padding(15)
        .Background(Colors.Blue.Lighten4)
        .Text("border → padding → background");

    column.Item()
        .Background(Colors.Blue.Lighten4)
        .Padding(15)
        .Border(1)
        .Text("background → padding → border");
    
    column.Item()
        .Padding(15)
        .Border(1)
        .Background(Colors.Blue.Lighten4)
        .Text("padding → border → background");
});
```

---



---

## Generating output


**URL:** https://www.questpdf.com/concepts/generating-output.html

**Contents:**
- Generating output ​
- Generating PDF files ​
- Generating XPS files ​
- Generating SVG files ​
- Generating images ​

The primary goal of the QuestPDF library is to generate PDF files. However, it also supports other output formats such as XPS, SVG and images.

Please be aware that certain features may not be available on formats other than PDF.

There are several overloads for generating PDF files:

The library also supports generating XPS files:

Please note that generating XPS files is only supported on Windows operating systems.

The library also supports generating SVG files. Each page is represented as a separate SVG file.

The library also supports generating images. Each page is represented as a separate image file.

Optionally, you can provide additional generation settings:

**Examples:**

Example 1 (swift):
```swift
var document = Document.Create(document =>
{
    document.Page(page =>
    {
        page.Content().Text("Your invoice content");
    });
});

// generate PDF and save it to a file
document.GeneratePdf("document.pdf");

// generate PDF and return it as a byte array
var byteArray = document.GeneratePdf();

// generate PDF and save it to a stream
using var stream = new FileStream("document.pdf", FileMode.Create);
document.GeneratePdf(stream);
```

Example 2 (julia):
```julia
// generate XPS and save it to a file
document.GenerateXps("document.xps");

// generate XPS and return it as a byte array
var byteArray = document.GenerateXps();

// generate XPS and save it to a stream
using var stream = new FileStream("document.xps", FileMode.Create);
document.GenerateXps(stream);
```

Example 3 (typescript):
```typescript
ICollection<string> svgFiles = document.GenerateSvgFiles();
```

Example 4 (typescript):
```typescript
// generate images and return them as byte arrays
IEnumerable<byte[]> imagesAsByteArrays = document.GenerateImages();

// save images to files
document.GenerateImages(imageIndex => $"image{imageIndex}.png");
```

---



---

## Length unit types


**URL:** https://www.questpdf.com/concepts/length-unit-types.html

**Contents:**
- Length unit types ​
- Available units ​
- Example ​

Following the PDF specification, QuestPDF uses points as its default measurement unit. Most Fluent API methods accept an optional unit parameter for specifying alternative measurement units.

Unit types can be optionally specified in most of length-related API methods. As an example, the following code snippets are equivalent:

**Examples:**

Example 1 (swift):
```swift
using QuestPDF.Infrastructure;

.Padding(72)
.Padding(1, Unit.Inch)
.Padding(1/12f, Unit.Feet)
.Padding(1000, Unit.Mill)
```

---



---

## Merging documents


**URL:** https://www.questpdf.com/concepts/merging-documents.html

**Contents:**
- Merging documents ​
- Generating sample document ​
- Original page numbers ​
- Continuous page numbers ​

QuestPDF makes it easy to generate multiple PDF documents and then combine them into one. You can merge documents while either preserving each document’s original page numbers or creating a continuous page numbering sequence throughout the merged output.

QuestPDF supports two merging strategies regarding page numbering. Choose the one that best suits your needs.

This feature can be used only while generating PDF documents. To merge existing files, please use the Document Operations feature.

Before merging, you need to create individual documents. The sample code below defines a helper method that creates a report with a header, content area, and a footer displaying page numbers.

Documents maintain their own page numbers upon merging, without continuity between them. As a result, APIs related to page numbers reflect individual documents, not the cumulative count. All documents are simply be merged together.

Example: Merging a two-page document with a three-page document results in a sequence: 1, 2, 1, 2, 3.

Unable to display PDF file. Download instead.

Consolidates the content from every document, creating a continuous seamless one. Page number APIs return a consecutive numbering for this unified document.

Merging a two-page document with a three-page document results in a sequence: 1, 2, 3, 4, 5.

Unable to display PDF file. Download instead.

**Examples:**

Example 1 (swift):
```swift
static Document GenerateReport(string title, int itemsCount)
{
    return Document.Create(document =>
    {
        document.Page(page =>
        {
            page.Size(PageSizes.A5);
            page.Margin(0.5f, Unit.Inch);
            
            page.Header()
                .Text(title)
                .Bold()
                .FontSize(24)
                .FontColor(Colors.Blue.Accent2);
            
            page.Content()
                .PaddingVertical(20)
                .Column(column =>
                {
                    column.Spacing(10);

                    foreach (var i in Enumerable.Range(0, itemsCount))
                    {
                        column
                            .Item()
                            .Width(200)
                            .Height(50)
                            .Background(Colors.Grey.Lighten3)
                            .AlignMiddle()
                            .AlignCenter()
                            .Text($"Item {i}")
                            .FontSize(16);
                    }
                });
            
            page.Footer()
                .AlignCenter()
                .PaddingVertical(20)
                .Text(text =>
                {
                    text.DefaultTextStyle(TextStyle.Default.FontSize(16));
                    
                    text.CurrentPageNumber();
                    text.Span(" / ");
                    text.TotalPages();
                });
        });
    });
}
```

Example 2 (swift):
```swift
Document
    .Merge(
        GenerateReport("Short Document 1", 5),
        GenerateReport("Medium Document 2", 10),
        GenerateReport("Long Document 3", 15))
    .UseOriginalPageNumbers()
    .GeneratePdf("merged.pdf");
```

Example 3 (swift):
```swift
Document
    .Merge(
        GenerateReport("Short Document 1", 5),
        GenerateReport("Medium Document 2", 10),
        GenerateReport("Long Document 3", 15))
    .UseContinuousPageNumbers()
    .GeneratePdf("merged.pdf");
```

---



---

## PDF Document Operations


**URL:** https://www.questpdf.com/concepts/document-operations.html

**Contents:**
- PDF Document Operations ​
- Loading Documents ​
- Page Selection ​
- Page Range Format ​
- Document Linearization ​
- Merging Documents ​
- Overlays and Underlays ​
  - Configuration Options ​
- Document Encryption ​
  - Base Encryption Settings ​

The Document Operations API provides functionality for performing various operations on PDF documents, including loading, merging, overlaying, underlaying, selecting specific pages, adding attachments, and applying encryption settings.

Features presented in this sections are created using the qpdf library, available under the "Apache-2.0" license. We extend our thanks to the authors of qpdf for their contributions to the open-source community.

The code of qpdf library has been extended by QuestPDF to support important PDF/A-3b compliance requirements as well ZUGFeRD metadata extension.

Features presented in this section are available starting from the 2024.12.0 version of the library.

The LoadFile method loads a PDF file for processing, enabling operations such as merging, overlaying or underlaying content, selecting pages, adding attachments, and encrypting.

The TakePages method selects specific pages from the current document based on the provided page selector, marking them for further operations.

Linearization creates web-optimized output files. Linearized files are structured to allow compliant PDF readers to begin displaying content before the entire file is downloaded. Normally, a PDF reader requires the entire file to be present to render content, as essential cross-reference data typically appears at the file's end.

The MergeFile method merges pages from the specified PDF file into the current document, according to the provided page selection.

Simple example of merging two documents:

Example of merging multiple documents at once:

Advanced example where two documents are merged with specific page selections:

A simple overlay example:

More complex overlay example with specific page selections:

40-bit encryption example:

128-bit encryption example:

256-bit encryption example:

Removes any existing encryption from the current PDF document, effectively making it accessible without a password or encryption restrictions.

It is also possible to remove security restrictions associated with digitally signed PDF files.

A simple attachment example:

The PDF/A-3b standard requires that all attachments provide more information about their relationship to the document. The Relationship property is used to specify the attachment's relationship to the document content.

The ExtendMetadata method extends the current document's XMP metadata by adding content within the rdf:Description tag. This allows for adding additional descriptive metadata to the PDF, which is useful for compliance standards like PDF/A or for industry-specific metadata (e.g., ZUGFeRD).

Please ensure that the document is PDF/A-3b compliant before extending its metadata.

**Examples:**

Example 1 (gdscript):
```gdscript
// Load an unprotected document and save
DocumentOperation
    .LoadFile("input.pdf")
    .Save("output.pdf");

// Load a password-protected document and save
var operation = DocumentOperation
    .LoadFile("protected.pdf", "password123")
    .Save("unprotected-output.pdf");
```

Example 2 (sql):
```sql
// Select specific pages
DocumentOperation
    .LoadFile("input.pdf")
    .TakePages("1,3,5-10")
    .Save("selected-pages.pdf");
```

Example 3 (unknown):
```unknown
DocumentOperation
    .LoadFile("input.pdf")
    .Linearize()
    .Save("web-optimized.pdf");
```

Example 4 (unknown):
```unknown
DocumentOperation
    .LoadFile("document1.pdf")
    .MergeFile("document2.pdf")
    .Save("merged.pdf");
```

---



---

## Prototyping


**URL:** https://www.questpdf.com/concepts/prototyping.html

**Contents:**
- Prototyping ​
- Text ​
    - Example ​
- Colors ​
    - BackgroundColor example ​
    - Color example ​
- Image ​

Placeholders in QuestPDF let you quickly generate random text, numbers, colors, and images. They are useful for prototyping document layouts or creating test data when real information is not yet available.

This guide outlines how to use each type of placeholder.

QuestPDF provides a range of text placeholders that cover common scenarios:

QuestPDF can produce random colors based on the Material Design palette, returning them as a string in the #RRGGBB format.

The image Placeholders.Image method generates a soft color gradient. It returns a byte array in JPEG format and can be embedded directly in QuestPDF elements.

Use these placeholders to simulate images in your layout, ensuring you can test image placement, sizing, and alignment before real images become available.

**Examples:**

Example 1 (swift):
```swift
using QuestPDF.Helpers;

Placeholders.LoremIpsum();
Placeholders.Label();
Placeholders.Sentence();
Placeholders.Question();
Placeholders.Paragraph();
Placeholders.Paragraphs();

Placeholders.Email();
Placeholders.Name();
Placeholders.PhoneNumber();
Placeholders.WebpageUrl();

Placeholders.Time();
Placeholders.ShortDate();
Placeholders.LongDate();
Placeholders.DateTime();

Placeholders.Integer();
Placeholders.Decimal();
Placeholders.Percent();
```

Example 2 (swift):
```swift
.Column(column =>
{
    column.Spacing(15);

    AddItem("Name", Placeholders.Name());
    AddItem("Email", Placeholders.Email());
    AddItem("Phone", Placeholders.PhoneNumber());
    AddItem("Date", Placeholders.ShortDate());
    AddItem("Time", Placeholders.Time());
    
    void AddItem(string label, string value)
    {
        column.Item().Text(text =>
        {
            text.Span($"{label}: ").Bold();
            text.Span(value);
        });
    }
});
```

Example 3 (unknown):
```unknown
// bright color (lighten-2)
Placeholders.BackgroundColor();

// medium intensity color
Placeholders.Color();
```

Example 4 (swift):
```swift
.Grid(grid =>
{
    grid.Columns(5);
    grid.Spacing(5);

    foreach (var _ in Enumerable.Range(0, 25))
    {
        grid.Item()
            .Height(50)
            .Width(50)
            .Background(Placeholders.BackgroundColor());
    }
});
```

---



---

## Settings


**URL:** https://www.questpdf.com/concepts/global-settings.html

**Contents:**
- Settings ​
- Caching ​
- Debugging ​
- Checking Font Glyph Availability ​
- Using System Fonts ​
- Font Discovery Paths ​

QuestPDF provides several configurable settings to fine-tune the document generation process. These settings are accessible via the static QuestPDF.Settings class.

This flag generates additional document elements to cache layout calculation results. In the vast majority of cases, this significantly improves performance, while slightly increasing memory consumption.

This flag generates additional document elements to improve layout debugging experience.

When the provided content contains size constraints impossible to meet, the library generates an enhanced exception message with additional location and layout measurement details.

This flag enables checking the font glyph availability. If your text contains glyphs that are not present in the specified font:

Enabling this flag may slightly decrease document generation performance. However, it provides hints that used fonts are not sufficient to produce correct results.

Decides whether the application should use the fonts available in the environment:

This property is useful when you want to control the fonts used by your application, especially in cases where the environment might not have the necessary fonts installed.

Specifies the collection of paths where the library will automatically search for font files to register.

By default, this collection contains the application files path. You can add additional paths to this collection to include more directories for automatic font registration.

**Examples:**

Example 1 (unknown):
```unknown
// enabled by default
QuestPDF.Settings.EnableCaching = true;
```

Example 2 (unknown):
```unknown
// by default, enabled only when debugger is attached
QuestPDF.Settings.EnableDebugging = false;
```

Example 3 (unknown):
```unknown
// by default, enabled only when debugger is attached
QuestPDF.Settings.CheckIfAllTextGlyphsAreAvailable = false;
```

Example 4 (unknown):
```unknown
// enabled by default
QuestPDF.Settings.UseEnvironmentFonts = true;
```

---


---

## # Questpdf Documentation Index




---

## Categories


### Api
**File:** `api.md`
**Pages:** 58

### Companion
**File:** `companion.md`
**Pages:** 4

### Concepts
**File:** `concepts.md`
**Pages:** 19

### Image
**File:** `image.md`
**Pages:** 3

### Other
**File:** `other.md`
**Pages:** 12

### Table
**File:** `table.md`
**Pages:** 2

### Text
**File:** `text.md`
**Pages:** 3

### Tutorials
**File:** `tutorials.md`
**Pages:** 1


---

## # Questpdf - Other


**Pages:** 12

---



---

## Acknowledgements


**URL:** https://www.questpdf.com/acknowledgements.html

**Contents:**
- Acknowledgements ​

Thank you for developing a fantastic graphics library for the .NET platform.

Special thanks to James Jackson-South for so openly sharing his experience and know-how regarding the licensing opportunities for open-source projects.

Link to the official webpage

Copyright (c) 2015-2016 Xamarin, Inc. Copyright (c) 2017-2018 Microsoft Corporation.

Link to the repository webpage Link to the license

Copyright (c) 2024 Google BSD 3-Clause "New" or "Revised" License

Link to the repository webpage Link to the license

Copyright (c) 2015-2016 Xamarin, Inc. Copyright (c) 2017-2018 Microsoft Corporation.

Link to the repository webpage Link to the license

Copyright (c) 2013-present, Yuxi (Evan) You

Link to the repository webpage Link to the license

Copyright (c) 2018-present, Yuxi (Evan) You

Link to the repository webpage Link to the license

Copyright (c) 2021 Charlie Poole, Rob Prouse

Link to the repository webpage Link to the license

Copyright (c) 2021 Dennis Doomen

Link to the repository webpage Link to the license

Copyright (c) .NET Foundation and Contributors

Link to the repository webpage

---



---

## Creating ZUGFeRD-Compliant PDF Documents


**URL:** https://www.questpdf.com/examples/zugferd.html

**Contents:**
- Creating ZUGFeRD-Compliant PDF Documents ​
- Introduction ​
- Document Creation ​

ZUGFeRD (Zentraler User Guide des Forums elektronische Rechnung Deutschland) is a German standard for electronic invoicing that combines PDF documents with embedded XML data. It allows for both human-readable PDF invoices and machine-readable structured data in a single file, enabling automated processing while maintaining traditional PDF workflow compatibility.

A ZUGFeRD-compliant PDF document must meet the following requirements:

ZUGFeRD comes in different versions with varying requirements. This documentation covers ZUGFeRD 2.1, which is based on the UN/CEFACT Cross Industry Invoice (CII) standard.

When implementing ZUGFeRD support, ensure you're using the correct version for your needs and that all components (XML schema, metadata, and PDF/A version) align with that version.

The validation process can vary across different tools, often producing different results.

At QuestPDF, as part of our CI/CD pipeline, we use automated document validation with veraPDF (to verify PDF/A-3b compliance) and the Mustang Project (to verify ZUGFeRD compliance). Both tools are open-source and free to use.

In addition, we periodically perform manual validation using the Adobe Acrobat Pro Preflight tool to further ensure compliance.

Here's a complete example showing how to create a ZUGFeRD-compliant PDF document:

Find the full example here: ZUGFeRD Example

**Examples:**

Example 1 (swift):
```swift
Document
    .Create(document =>
    {
        document.Page(page =>
        {
            page.Content().Text("Your invoice content");
        });
    })
    .WithMetadata(new DocumentMetadata
    {
        Title = "Conformance Test: ZUGFeRD",
        Author = "SampleCompany",
        Subject = "ZUGFeRD Test Document",
        Language = "en-US"
    })
    .WithSettings(new DocumentSettings { PdfA = true }) // PDF/A-3b
    .GeneratePdf("invoice-bbb.pdf");

DocumentOperation
    .LoadFile("invoice.pdf")
    .AddAttachment(new DocumentOperation.DocumentAttachment
    {
        Key = "factur-zugferd",
        FilePath = "resource-factur-x.xml",
        AttachmentName = "factur-x.xml",
        MimeType = "text/xml",
        Description = "Factur-X Invoice",
        Relationship = DocumentOperation.DocumentAttachmentRelationship.Source,
        CreationDate = DateTime.UtcNow,
        ModificationDate = DateTime.UtcNow
    })
    .ExtendMetadata(File.ReadAllText("resource-zugferd-metadata.xml"))
    .Save("zugferd-invoice.pdf");
```

---



---

## Integration with ASP.NET


**URL:** https://www.questpdf.com/examples/aspnet-integration.html

**Contents:**
- Integration with ASP.NET ​
- License configuration ​
- Generating PDF files in controller endpoints ​

Configure your license in either the Startup.cs or Program.cs file depending on your project configuration. This code should be executed only once, when the application starts or during its initialization step.

Learn more about the licensing and related configuration here.

This section demonstrates how to generate and return a PDF file in an ASP.NET controller endpoint using QuestPDF. The example below creates a simple PDF document and sends it as a response when the endpoint is accessed.

**Examples:**

Example 1 (unknown):
```unknown
// please kindly ensure what license is appropriate for your project
QuestPDF.Settings.License = LicenseType.Community;
```

Example 2 (swift):
```swift
[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    [HttpGet(Name = "GeneratePdf")]
    public IResult GeneratePdf()
    {
        // use any method to create a document, e.g.: injected service
        var document = CreateDocument();
        
        // generate PDF file and return it as a response
        var pdf = document.GeneratePdf();
        return Results.File(pdf, "application/pdf", "hello-world.pdf");
    }

    QuestPDF.Infrastructure.IDocument CreateDocument()
    {
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(20));

                page.Header()
                    .Text("Hello PDF!")
                    .SemiBold().FontSize(36).FontColor(Colors.Blue.Medium);

                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Column(x =>
                    {
                        x.Spacing(20);

                        x.Item().Text(Placeholders.LoremIpsum());
                        x.Item().Image(Placeholders.Image(200, 100));
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Page ");
                        x.CurrentPageNumber();
                    });
            });
        });
    }
}
```

---



---

## Pricing


**URL:** https://www.questpdf.com/pricing.html

**Contents:**
- Perpetual Licensing

Full feature set — same core product as paid tiers

Commercial use allowed

Unlimited developers, projects, servers, and deployments

Royalty-free redistribution inside your own apps

No registration, license keys, or watermarks

Source-available — full source code on GitHub

Community support via GitHub

From package install to first PDF in minutes

Perpetual license for entire company

Unlimited developers, projects, servers, and deployments

One year of feature updates, fixes, and security patches included

Royalty-free redistribution — SaaS, desktop, on-prem; end users need no license

Runs fully offline and air-gapped

Price Lock — your renewal price never increases while you renew continuously

30-day money-back guarantee

Renews annually at today's price — cancel anytime, keep your version forever

Everything in Professional

One license covers all your affiliates

Next-business-day dedicated, priority email support

Priority handling for business-critical issues with off-schedule releases

12-month notice before any general end-of-life, with critical fixes and migration assistance throughout

Order Forms, purchase orders, direct invoicing, supplier onboarding, and multi-year terms

Buy online, through resellers, or request a quote

---



---

## QuestPDF


**URL:** https://www.questpdf.com/contact.html

**Contents:**
  - Community Support

Encountering an issue or have a feature idea? Join our community on GitHub. It is the best place to report bugs, discuss improvements, and share your insights with other developers.

---



---

## Quick start


**URL:** https://www.questpdf.com/quick-start.html

**Contents:**
- Quick start ​
- Installation ​
- Implementation ​
- License ​
- Are you ready for more? ​

QuestPDF is a modern C# library for PDF generation that provides a dedicated layout engine optimized specifically for creating PDF documents. Its component-based architecture lets you compose simple elements (such as text, images, tables, and grids) into sophisticated layouts through an intuitive, declarative API. Because it's pure C#, you get full access to familiar programming constructs, strong typing, and seamless IDE support.

QuestPDF is available as a NuGet package. You can install it through your IDE by searching for phrase QuestPDF. If you are not familiar how to do that, please refer to the following guides:

Or use the following command in your terminal:

QuestPDF's minimal API makes it incredibly easy to create and prototype PDF documents. Here's a simple example that demonstrates its intuitive syntax:

This code generates a PDF document with the following layout:

SUSTAINABLE AND FAIR LICENSE

By offering free access to most users and premium licenses for larger organizations, the project maintains its commitment to excellence while ensuring sustainable, long-term development for all.

The library is free to use for any individual or business with less than 1 million USD annual gross revenue, or operates as a non-profit organization, or is a FOSS project.

More details can be found on the QuestPDF Pricing and QuestPDF License pages.

For learning and evaluation, you can use the QuestPDF Evaluation license type.

QuestPDF's Fluent API scales seamlessly with your document complexity. To explore its full potential, check out the In-Depth Invoice Tutorial, where you'll learn to create a professional invoice in less than 200 lines of code.

**Examples:**

Example 1 (go):
```go
dotnet add package QuestPDF
```

Example 2 (swift):
```swift
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

// TODO: set your license here:
// QuestPDF.Settings.License = LicenseType.Evaluation;

Document.Create(container =>
{
    container.Page(page =>
    {
        page.Size(PageSizes.A4);
        page.Margin(2, Unit.Centimetre);
        page.PageColor(Colors.White);
        page.DefaultTextStyle(x => x.FontSize(20));
        
        page.Header()
            .Text("Hello PDF!")
            .SemiBold().FontSize(36).FontColor(Colors.Blue.Medium);
        
        page.Content()
            .PaddingVertical(1, Unit.Centimetre)
            .Column(x =>
            {
                x.Spacing(20);
                
                x.Item().Text(Placeholders.LoremIpsum());
                x.Item().Image(Placeholders.Image(200, 100));
            });
        
        page.Footer()
            .AlignCenter()
            .Text(x =>
            {
                x.Span("Page ");
                x.CurrentPageNumber();
            });
    });
})
.GeneratePdf("hello.pdf");
```

---



---

## Roadmap


**URL:** https://www.questpdf.com/roadmap.html

**Contents:**
- Roadmap ​
- In progress ​
- Up next ​
- Future ​
- Recently delivered ​

QuestPDF is built to be a dependable, long-term foundation for generating PDF documents in code. This page outlines where the library is heading, including the capabilities we are actively building and our strategic direction over the coming releases.

This roadmap is a living document. Because we prioritize quality over rigid deadlines, we do not attach fixed dates to these items. It reflects our current intent rather than a binding delivery schedule.

What we're actively building right now.

Support for more platforms and languages. Foundational work is underway to bring the QuestPDF API to runtimes and languages beyond .NET, so more teams can rely on the same document engine regardless of their technology stack.

Native AOT compilation support. Full compatibility with .NET Native AOT. AOT compilation delivers faster startup times, smaller self-contained deployments, and a lower memory footprint — increasingly important for serverless functions, containerized services, and high-density cloud workloads. This removes a key adoption barrier for teams standardizing on AOT-first architectures.

Introductory video and learning materials. A concise video walkthrough of QuestPDF fundamentals, from your first document to real-world layouts. The goal is to shorten the path from evaluation to productive use — particularly for developers and teams adopting the library for the first time.

Confirmed direction for upcoming releases. These items are planned and prioritized; exact timing depends on scope and dependencies.

Sample gallery with ready-to-use code. A curated gallery of copy-and-paste code samples covering the most common document types — invoices, reports, certificates, and more — with complete, working implementations. Less boilerplate, faster implementation, and a proven starting point instead of a blank page.

New layout elements, options, and enhancements. Ongoing expansion of the layout engine with new elements, richer configuration options, and refinements to existing components. A broader, more expressive set of building blocks means fewer custom workarounds and more document designs that can be described directly and cleanly in code.

Expanded and improved documentation. Continued investment in documentation: broader coverage, clearer explanations, more end-to-end examples, and deeper guidance for advanced scenarios. Strong documentation lowers onboarding cost and reduces day-to-day friction for every team using QuestPDF.

Future development plans include:

Built-in PDF validation with veraPDF. A first-class, built-in way to validate your generated documents against conformance standards using veraPDF — giving teams a straightforward, automated path to verify compliance directly within their own build and QA pipelines.

Increased test coverage. Ongoing expansion of the automated test suite across more layouts, edge cases, and rendering scenarios. Higher coverage translates directly into greater stability and predictability from release to release — a core reason teams trust QuestPDF in production-critical systems.

PDF/A-4 and PDF/UA-2 conformance. Support for the latest archival (PDF/A-4) and accessibility (PDF/UA-2) standards, building directly on the existing PDF/A-2, PDF/A-3, and PDF/UA-1 support. Essential for regulated industries, the public sector, and any organization with long-term archival or accessibility obligations.

Content translation support. Tooling to streamline generating the same document across multiple languages, making QuestPDF easier to adopt for teams serving international audiences and multi-market operations.

PDF signing with X.509 certificates. Built-in support for digitally signing documents using X509Certificate certificates. Digital signatures provide authenticity and tamper-evidence — a common requirement for contracts, invoices, and official documents across finance, legal, and government.

Further performance and resource-efficiency improvements. Continued, deliberate investigation into generation speed alongside CPU and memory usage, with the goal of pushing throughput higher and resource consumption lower. This matters most for high-volume, latency-sensitive, and cost-conscious workloads running at scale.

Basic AcroForm support. Programmatic creation of interactive form fields — text inputs, checkboxes, and similar controls — along with the ability to read submitted values back from existing form documents. This opens up fillable PDFs for use cases such as applications, surveys, and onboarding paperwork, and enables automated data capture from completed forms.

PDF content inspection. Reading and inspecting the contents of existing PDF files — extracting text and examining document structure programmatically. This extends QuestPDF beyond document creation into analysis, supporting scenarios such as content extraction, verification, and post-processing of documents your systems receive.

QuestPDF is under active, continuous development. A selection of recent milestones:

Enterprise-ready licensing and documentation. Substantially revised legal documents to better support enterprise procurement and compliance requirements.

Windows ARM64 native support. Native execution on win-arm64 environments.

Companion App. A visual companion for development: live document preview, layout-problem debugging, navigation from rendered output straight to the originating code, and content inspection — making it fast and intuitive to understand and fix exactly what your document is doing.

Tagged PDF and semantic structure. Automatic semantic tagging of document content, the foundation for accessible, machine-readable PDFs.

PDF/UA-1, PDF/A-2, and PDF/A-3 conformance. Support for accessibility and archival conformance standards across the PDF/A-2 and PDF/A-3 conformance levels and PDF/UA-1.

Automated conformance and e-invoice validation. QuestPDF's own output is continuously validated against conformance standards using veraPDF, and against ZUGFeRD / Factur-X requirements using the Mustang project.

Advanced graphics capabilities. Native support for linear gradients, rounded corners, customizable dash patterns for lines, and a dedicated shadow element with configurable blur, color, offset, and spread. Documents gain a polished, modern visual finish directly from code, with no external tooling required.

Document operations API. A dedicated API for working with existing PDF files: merge and split documents, apply password protection, add overlays and underlays (for watermarks, stationery, or background templates), attach external files, manage metadata, and embed e-invoice data.

Custom text and graphics engine. A custom Skia-based native layer replacing the previous SkiaSharp dependency, making QuestPDF a self-contained library with a rendering stack we control and update on a predictable cadence (currently tracking Skia M149). The same engine powers advanced typography (complex text shaping, right-to-left and bidirectional scripts, and automatic font subsetting), native SVG rendering, and document compression that meaningfully reduces output file size.

---


---

## # Questpdf - Tutorials


**Pages:** 1

---



---

## In-Depth Invoice Tutorial


**URL:** https://www.questpdf.com/invoice-tutorial.html

**Contents:**
- In-Depth Invoice Tutorial ​
- Suggested architecture ​
- Document models layer ​
- Data source layer ​
- Template layer ​
  - Basic page structure ​
  - Implementing header and footer ​
  - Content implementation ​
  - Table generation ​
  - Address component ​

QuestPDF is a modern .NET library for PDF document generation that emphasizes clean architecture and developer productivity. In this tutorial, we'll build a professional invoice document while exploring the core concepts that make QuestPDF powerful and intuitive to use.

By the end, you'll have a fully functional, paginated invoice generator that looks like this:

Before starting this tutorial, please familiarize yourself with the Quick Start tutorial. It will guide you through the installation process and provide a basic understanding of the library's architecture.

You can download, review, and compile the complete example from this GitHub repository.

QuestPDF recommends a clear three-layer architecture for both maintainability and clarity:

Document Models - define the raw data that appears in your PDF, such as invoice details or report content. These classes remain free of business logic and focus solely on representing structured information.

Data Source - handle asynchronous data fetching, transformations, and calculations. Here, you perform database queries, map domain entities to the document models, and load external resources (Images) to prepare all the information needed to render the document.

Template - use C# features (such as loops, conditional logic, helper methods) and QuestPDF Fluent API to design the visual layout and appearance of your document.

First, let's define the data structure for our invoice. These models capture all the information we need to display:

Next, implement a class that retrieves and prepares your invoice data. In a real application, this might query a database, download images from storage, or call an external API.

For this tutorial, we'll use a sample data generator:

With data ready, focus on how it should appear in the final PDF. QuestPDF’s layout engine uses a fluent API to define pages, headers, footers, and content sections.

As the first step, we’ll implement a single page with a simple header, content area, and footer. The class below implements the IDocument interface and uses the Compose method to define the document’s structure.

Each fluent API call creates a container with its own style, size, alignment constraints and layout behavior — making their order important. While most elements are simple containers holding a single child, some advanced elements offer multiple slots to accommodate more complex layouts.

Then, use the following code to generate the document:

This initial scaffolding sets up basic sections. You’ll refine them in the following steps.

Implement header and footer of the document using the most common QuestPDF visual, positional, and layout components.

The code also uses local methods to define the header and content sections. This approach produces cleaner code and makes it easier to maintain and understand.

Please hover your cursor over the code to see the explanation of various API calls.

The code above generates the following output:

Define general structure of the primary content. Please note that you can freely use C# features such as conditions and loops.

Here's the result generated by the code snippet above:

Table is one of the most flexible and powerful elements in QuestPDF.

Begin by defining the number, position, and size of your columns. After that, add cells which can be either auto-arranged by the layout engine or explicitly placed at specific rows and columns. You can even have cells span multiple columns or rows.

Note the use of the CellStyle local function, which applies consistent styling to cells in a single, reusable manner.

To prevent duplication and improve maintainability, move recurring sections into reusable components. For example, addresses often appear multiple times with the same layout. By implementing IComponent, you can pass arguments and reuse this logic throughout your project.

This approach is similar to extracting code into methods, but it goes further. Components reside in their own classes and files, making it simple to provide arguments and fully encapsulate their layout logic.

The code below demonstrates how to integrate and use the newly created component:

For learning and evaluation, you can use the free QuestPDF Evaluation license.

SUSTAINABLE AND FAIR LICENSE

By offering free access to most users and premium licenses for larger organizations, the project maintains its commitment to excellence while ensuring sustainable, long-term development for all.

The library is free to use for any individual or business with less than 1 million USD annual gross revenue, or operates as a non-profit organization, or is a FOSS project.

More details can be found on the QuestPDF Pricing and QuestPDF License pages.

**Examples:**

Example 1 (csharp):
```csharp
public class InvoiceModel
{
    public int InvoiceNumber { get; set; }
    public DateTime IssueDate { get; set; }
    public DateTime DueDate { get; set; }

    public Address SellerAddress { get; set; }
    public Address CustomerAddress { get; set; }

    public List<OrderItem> Items { get; set; }
    public string Comments { get; set; }
}

public class OrderItem
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}

public class Address
{
    public string CompanyName { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public object Email { get; set; }
    public string Phone { get; set; }
}
```

Example 2 (dart):
```dart
using QuestPDF.Helpers;

public static class InvoiceDocumentDataSource
{
    private static Random Random = new Random();

    public static InvoiceModel GetInvoiceDetails()
    {
        var items = Enumerable
            .Range(1, 8)
            .Select(i => GenerateRandomOrderItem())
            .ToList();

        return new InvoiceModel
        {
            InvoiceNumber = Random.Next(1_000, 10_000),
            IssueDate = DateTime.Now,
            DueDate = DateTime.Now + TimeSpan.FromDays(14),

            SellerAddress = GenerateRandomAddress(),
            CustomerAddress = GenerateRandomAddress(),

            Items = items,
            Comments = Placeholders.Paragraph()
        };
    }

    private static OrderItem GenerateRandomOrderItem()
    {
        return new OrderItem
        {
            Name = Placeholders.Label(),
            Price = (decimal) Math.Round(Random.NextDouble() * 100, 2),
            Quantity = Random.Next(1, 10)
        };
    }

    private static Address GenerateRandomAddress()
    {
        return new Address
        {
            CompanyName = Placeholders.Name(),
            Street = Placeholders.Label(),
            City = Placeholders.Label(),
            State = Placeholders.Label(),
            Email = Placeholders.Email(),
            Phone = Placeholders.PhoneNumber()
        };
    }
}
```

Example 3 (swift):
```swift
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

public class InvoiceDocument : IDocument
{
    public InvoiceModel Model { get; }

    public InvoiceDocument(InvoiceModel model)
    {
        Model = model;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
    public DocumentSettings GetSettings() => DocumentSettings.Default;

    public void Compose(IDocumentContainer container)
    {
        container
            .Page(page =>
            {
                page.Margin(50);
            
                page.Header().Height(100).Background(Colors.Grey.Lighten1);
                page.Content().Background(Colors.Grey.Lighten3);
                page.Footer().Height(50).Background(Colors.Grey.Lighten1);
            });
    }
}
```

Example 4 (csharp):
```csharp
using System.IO;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

static void Main(string[] args)
{
    // TODO: set your license here:
    // QuestPDF.Settings.License = LicenseType.Evaluation;

    var model = InvoiceDocumentDataSource.GetInvoiceDetails();
    var document = new InvoiceDocument(model);
    document.GeneratePdfAndShow();
    
    // document.GeneratePdf("invoice.pdf");
}
```

---


---

