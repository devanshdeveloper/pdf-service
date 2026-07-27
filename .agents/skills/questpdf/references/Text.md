# QuestPDF - Text

## Hyperlink


**URL:** https://www.questpdf.com/api-reference/hyperlink.html

**Contents:**
- Hyperlink ​
  - Content ​
  - Inside text ​

The Hyperlink element creates a clickable area that redirects the user to a designated webpage.

Hyperlink can span any content, including text, images, or other elements.

Unable to display PDF file. Download instead.

Hyperlinks can also be placed inside text elements.

Unable to display PDF file. Download instead.

**Examples:**

Example 1 (swift):
```swift
.Column(column =>
{
    column.Spacing(25);
    
    column.Item()
        .Text("Clicking the NuGet logo will redirect you to the NuGet website.");

    column.Item()
        .Width(150)
        .Hyperlink("https://www.nuget.org/")
        .Svg("Resources/nuget-logo.svg");
});
```

Example 2 (swift):
```swift
container
    .Text(text =>
    {
        text.Span("Click ");
        text.Hyperlink("here", "https://www.nuget.org/").Underline().FontColor(Colors.Blue.Darken2);
        text.Span(" to visit the official NuGet website.");
    });
```

---



---

## Ordered and bullet lists


**URL:** https://www.questpdf.com/api-reference/lists.html

**Contents:**
- Ordered and bullet lists ​
- Unordered list ​
- Ordered list ​
- Nested lists ​

Lists are an essential part of structured documents, helping to present information in an organized and easy-to-read format. In QuestPDF, lists can be created using the Column and Row elements, allowing for both unordered and ordered lists with custom styling and nesting capabilities.

An unordered list is a list of items where the order does not matter. You can create an unordered list in QuestPDF by prepending each item with an image, such as a bullet point icon.

An ordered list is a list of items that follow a sequential order, typically numbered. In QuestPDF, you can create an ordered list by prepending each item with a number.

Nested lists allow structuring items hierarchically, creating sub-items under main list entries. In QuestPDF, nested lists can be implemented by adjusting item indentation levels or using recursive functions for dynamic list generation.

**Examples:**

Example 1 (swift):
```swift
container.Column(column =>
{
    column.Spacing(10);
    
    foreach (var i in Enumerable.Range(1, 7))
    {
        column.Item().Row(row =>
        {
            row.ConstantItem(26).Image("Resources/bulletpoint.png");
            row.ConstantItem(5);
            row.RelativeItem().Text(Placeholders.Label());
        });
    }
});
```

Example 2 (swift):
```swift
container.Column(column =>
{
    column.Spacing(10);
    
    foreach (var i in Enumerable.Range(1, 11))
    {
        column.Item().Row(row =>
        {
            row.ConstantItem(35).Text($"{i}.");
            row.RelativeItem().Text(Placeholders.Sentence());
        });
    }
});
```

Example 3 (json):
```json
container.Column(column =>
{
    const float nestingSize = 25;
    
    column.Spacing(10);
    
    column.Item()
        .Text("Algorithm: Checking if a Number is Prime")
        .FontSize(24).FontColor(Colors.Blue.Darken2);

    AddListItem(0, "1.", "Handle special cases");
    AddListItem(1, "a)", "If n is less than 2, return false (not prime).");
    AddListItem(1, "b)", "If n is 2, return true (prime).");
    
    AddListItem(0, "2.", "Check divisibility");
    AddListItem(1, "-", "Iterate through numbers from 2 to n - 1:");
    AddListItem(2, "-", "If n is divisible by any of these numbers, return false.");
    
    AddListItem(0, "3.", "Return true (if no divisors were found, n is prime).");

    void AddListItem(int nestingLevel, string bulletText, string text)
    {
        column.Item().Row(row =>
        {
            row.ConstantItem(nestingSize * nestingLevel);
            row.ConstantItem(nestingSize).Text(bulletText);
            row.RelativeItem().Text(text);
        });
    }
});
```

---



---

## Paragraph Style


**URL:** https://www.questpdf.com/api-reference/text/paragraph-style.html

**Contents:**
- Paragraph Style ​
- Text Alignment ​
- Default Text Style ​
- Paragraph Spacing ​
- First Line Indentation ​
- Clamp Line With Ellipsis ​

Determines how text is positioned horizontally within its container.

Available alignment options:

Applies a consistent style for the whole content within the Text element.

Adjusts the vertical gap between successive paragraphs (separated by line breaks), helping to visually separate blocks of text for improved readability.

Specifies the horizontal offset of the first line in a paragraph. Commonly used to visually separate paragraphs in a block of text.

Limits the number of visible lines in a paragraph, truncating overflow text with an ellipsis or by hiding it to maintain layout consistency.

It is also possible to customize the ellipsis:

**Examples:**

Example 1 (swift):
```swift
container
    .Text("Sample text")
    .AlignCenter();
    
// or

container
    .Text(text => 
    {
        text.AlignCenter();
        text.Span(Placeholders.Paragraph());
    });
```

Example 2 (swift):
```swift
.Column(column =>
{
    column.Spacing(20);
    
    column.Item()
        .Element(CellStyle)
        .Text("This is an example of left-aligned text, showcasing how the text starts from the left margin and continues naturally across the container.")
        .AlignLeft();

    column.Item()
        .Element(CellStyle)
        .Text("This text is centered within its container, creating a balanced look, especially for titles or headers.")
        .AlignCenter();

    column.Item()
        .Element(CellStyle)
        .Text("This example demonstrates right-aligned text, often used for dates, numbers, or aligning text to the right margin.")
        .AlignRight();

    column.Item()
        .Element(CellStyle)
        .Text("Justified text adjusts the spacing between words so that both the left and right edges of the text block are aligned, creating a clean, newspaper-like look.")
        .Justify();

    static IContainer CellStyle(IContainer container) 
        => container.Background(Colors.Grey.Lighten3).Padding(10);
});
```

Example 3 (swift):
```swift
.Text(text =>
{
    text.DefaultTextStyle(x => x.Light().LetterSpacing(-0.1f).WordSpacing(0.1f));

    text.Span("Changing typography settings helps creating ");
    text.Span("significant").LetterSpacing(0.2f).Black().BackgroundColor(Colors.Grey.Lighten2);
    text.Span(" visual contrast.");
});
```

Example 4 (swift):
```swift
container
    .Text(Placeholders.Paragraphs())
    .ParagraphFirstLineIndentation(40);

// or

container
    .Text(text => 
    {
        text.ParagraphSpacing(20);
        text.Span(Placeholders.Paragraphs());
    });
```

---



---

## Section


**URL:** https://www.questpdf.com/api-reference/section.html

**Contents:**
- Section ​
  - Example ​

A Section defines a named fragment of a document that can span multiple pages. It is useful for creating table of contents and document navigation.

A SectionLink creates a clickable area that allows users to navigate to a designated section. This enhances document usability by enabling quick access to relevant content.

It is also possible to display page numbers for each section, which is particularly useful when generating tables of contents or cross-referencing sections.

Unable to display PDF file. Download instead.

**Examples:**

Example 1 (swift):
```swift
Document
    .Create(document =>
    {
        document.Page(page =>
        {
            page.Size(PageSizes.A5.Landscape());
            page.DefaultTextStyle(x => x.FontSize(20));
            page.Margin(25);

            page.Content()
                .Column(column =>
                {
                    var terms = new[]
                    {
                        ("Bit", "The smallest unit of data in computing, representing either a 0 or a 1. Multiple bits are combined to form bytes, which are used to store larger data values."),
                        ("Byte", "A unit of digital information that consists of 8 bits. A byte is commonly used to store a single character of text, such as a letter or a number, in computer memory."),
                        ("Binary", "A number system that uses only two digits, 0 and 1, which are the fundamental building blocks of computer operations. Computers process and store all data in binary format, including text, images, and instructions."),
                        ("Array", "A data structure that stores a fixed-size sequence of elements, all of the same type, in a contiguous block of memory. Arrays allow quick access to elements using an index and are commonly used to manage collections of data.")
                    };

                    // title
                    column.Item().Extend().AlignMiddle().AlignCenter().Text("Programming Glossary").FontSize(32).Bold();
                    column.Item().PageBreak();
                    
                    // table of contents
                    column.Item().PaddingBottom(25).Text("Table of Contents").FontSize(24).Bold().Underline();
                    
                    foreach (var term in terms)
                    {
                        column.Item()
                            .PaddingBottom(10)
                            .SectionLink($"term-{term}")
                            .Text(text =>
                            {
                                text.Span("Term ");
                                text.Span(term.Item1).Bold();
                                text.Span(" on page ");
                                text.BeginPageNumberOfSection($"term-{term}");
                            });
                    }
                    
                    // content
                    foreach (var term in terms)
                    {
                        column.Item().PageBreak();
                        
                        column.Item()
                            .Section($"term-{term}")
                            .Text(text =>
                            {
                                text.Span(term.Item1).Bold().FontColor(Colors.Blue.Darken2);
                                text.Span(" - ");
                                text.Span(term.Item2);
                            });
                    }
                });
        });
    })
    .GeneratePdf("sections.pdf");
```

---



---

## # Questpdf - Text


**Pages:** 3

---



---

## Default text style


**URL:** https://www.questpdf.com/api-reference/default-text-style.html

**Contents:**
- Default text style ​
- API ​
- Example ​

Applies a default text style to all nested Text elements. Please note that this element extends and overrides existing styles with additional configuration.

Depending on your use-case, you can provide a TextStyle object or use a lambda expression:

Please note that this element extends existing styles with additional configuration. Those styles can be extended/overridden in later stages of the code.

**Examples:**

Example 1 (swift):
```swift
.DefaultTextStyle(x => x.Bold().Underline())
.DefaultTextStyle(TextStyle.Default.Bold().Underline())
```

Example 2 (swift):
```swift
container
    .Width(400)
    .Padding(25)
    .DefaultTextStyle(x => x.Bold().Underline())
    .Column(column =>
    { 
        column.Spacing(10);
        
        column.Item().Text("Inherited bold and underline");
        
        column.Item()
            .Text("Disabled underline, inherited bold and adjusted font color")
            .Underline(false).FontColor(Colors.Green.Darken2);

        column.Item()
            .DefaultTextStyle(x => x.DecorationWavy().FontColor(Colors.LightBlue.Darken3))
            .Text("Changed underline type and adjusted font color");
    });
```

---



---

## Text


**URL:** https://www.questpdf.com/api-reference/text/basics.html

**Contents:**
- Text ​
- Simple usage ​
- Customization ​
- Rich text formatting ​
- Typography pattern ​
- Hyperlinks ​

In most cases, text content can be added using the following shorthand. The text will inherit the default style.

The Text method returns a descriptor that allows further customization of the text style.

It is also possible to format specific parts of the text content using spans:

The typography pattern helps maintain consistent text styling across your documents.

Then, a predefined typography can be used in the following way:

Hyperlink is a clickable text that redirects the user to a specific webpage.

Unable to display PDF file. Download instead.

**Examples:**

Example 1 (swift):
```swift
container
    .Text("Hello, World!");
```

Example 2 (swift):
```swift
.Column(column =>
{
    column.Spacing(10);

    column.Item()
        .Element(CellStyle)
        .Text("Text with blue color")
        .FontColor(Colors.Blue.Darken1);

    column.Item()
        .Element(CellStyle)
        .Text("Bold and underlined text")
        .Bold()
        .Underline();

    column.Item()
        .Element(CellStyle)
        .Text("Centered small text")
        .FontSize(12)
        .AlignCenter();

    static IContainer CellStyle(IContainer container) =>
        container.Background(Colors.Grey.Lighten3).Padding(10);
});
```

Example 3 (swift):
```swift
container
    .Text(text =>
    {
        text.Span("The ");
        text.Span("chemical formula").Underline();
        text.Span(" of ");
        text.Span("sulfuric acid").BackgroundColor(Colors.Amber.Lighten3);
        text.Span(" is H");
        text.Span("2").Subscript();
        text.Span("SO");
        text.Span("4").Subscript();
        text.Span(".");
    });
```

Example 4 (swift):
```swift
public static class Typography
{
    public static TextStyle Title => TextStyle
        .Default
        .FontType("Helvetica")
        .FontColor(Colors.Black)
        .FontSize(20)
        .Bold();

    public static TextStyle Headline => TextStyle
        .Default
        .FontType("Helvetica")
        .FontColor(Colors.Blue.Medium)
        .FontSize(14);

    public static TextStyle Normal => TextStyle
        .Default
        .FontType("Helvetica")
        .FontColor("#000000")
        .FontSize(10)
        .LineHeight(1.25f)
        .AlignLeft();
}
```

---



---

## Text Style


**URL:** https://www.questpdf.com/api-reference/text/text-style.html

**Contents:**
- Text Style ​
- Font Size ​
- Font Family ​
- Font Fallback ​
- Font Color ​
- Background Color ​
- Font Weight ​
- Italic ​
- Decorations ​
  - Positions ​

Font size measures the height of text characters, determining how large or small the text appears.

It's worth noting that different fonts may render text with different visual sizes, even when assigned the same numerical font size.

A font family is a collection of related fonts that share a consistent design style but may vary in weight, style, or width.

Examples of font families include Arial, Times New Roman, and Calibri.

The Font Fallback option is a list of alternative fonts that are used when specific glyphs are unavailable in the primary font. This ensures that text is displayed correctly across different systems and environments.

A common example is the display of non-Latin characters, such as Arabic or Chinese, which may not be supported by all fonts.

It's also useful for displaying emojis, which are not universally supported by all fonts.

The font color determines the color applied to text characters, affecting their visual appearance.

It also influences the default color of text decorations, such as underlines.

Sets a solid background color for the text.

This color fills the area behind the text or other elements, enhancing contrast and providing visual emphasis.

Determines the thickness of the text characters, ranging from light to bold, to create visual hierarchy or emphasis.

Please note that not all fonts support every weight. If the specified weight isn't available, the library selects the closest available option.

QuestPDF does not currently support fonts with variable weights.

Renders text with an italic effect, where letters are slightly slanted to the right.

Commonly used for emphasis or to distinguish specific words.

Applies decorative lines on text. Commonly used to emphasize specific words or phrases.

It is also possible to customize the decoration position:

It is also possible to customize the decoration line style:

By default, the decoration line color is the same as the text color, and the decoration thickness is determined by the font. However, these properties can be customized.

Subscript displays text slightly below the baseline, often in a smaller size. Commonly used for chemical formulas or mathematical notations

Superscript displays text slightly above the baseline, often in a smaller size. Typically used for exponents, footnotes, or ordinal indicators

Adjusts the vertical spacing between lines of text, affecting readability and overall text layout. The added space is proportional to the text size.

Adjusts the horizontal spacing between characters in the text, affecting readability and overall visual style.

The adjustment is proportional to the text size.

Adjusts the horizontal spacing between words in the text, affecting readability and overall visual style. The adjustment is proportional to the text size.

Font features are a set of typographic features that can be applied to text to enhance its appearance. They are used to control various aspects of text rendering.

Font features are always encoded as 4-character long strings. For example, the ligatures feature is encode as liga, while the kernig feature as kern. For a list of available features, refer to the QuestPDF.Helpers.FontFeatures class.

Please note that fonts usually support only a subset of font features. If you try to enable a feature that is not supported by the font, it will be ignored. Moreover, some fonts have features enabled by default, and you may not see any difference when enabling them.

Let's analyze the StandardLigatures font feature, which replaces specific pairs of letters (such as 'fi' or 'fl') with a single, combined glyph to enhance aesthetics.

**Examples:**

Example 1 (swift):
```swift
.Column(column =>
{
    column.Spacing(10);

    column.Item()
        .Text("This is small text (16pt)")
        .FontSize(16);

    column.Item()
        .Text("This is medium text (24pt)")
        .FontSize(24);

    column.Item()
        .Text("This is large text (36pt)")
        .FontSize(36);
});
```

Example 2 (swift):
```swift
.Column(column =>
{
    column.Spacing(10);

    column.Item().Text("This is text with default font (Lato)");

    column.Item().Text("This is text with Times New Roman font")
        .FontFamily("Times New Roman");

    column.Item().Text("This is text with Courier New font")
        .FontFamily("Courier New");
});
```

Example 3 (swift):
```swift
container
    .Text("The Arabic word for programming is البرمجة.")
    .FontFamily("Lato", "Noto Sans Arabic");
```

Example 4 (swift):
```swift
container
    .Text("Popular emojis include 😊, 😂, ❤️, 👍, and 😎.")
    .FontFamily("Lato", "Noto Emoji");
```

---


---

