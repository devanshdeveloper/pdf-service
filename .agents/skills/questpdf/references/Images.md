# QuestPDF - Images

## Placeholder


**URL:** https://www.questpdf.com/api-reference/placeholder.html

**Contents:**
- Placeholder ​
  - Size ​
  - Example ​

The Placeholder element is a simple utility for prototyping and layout visualization. It helps structure document layouts by displaying either a provided text label or a default icon when no text is specified.

By default, Placeholder fills the designated space with an icon. If a text value is provided, it displays the specified text instead.

You can adjust the size of the Placeholder by chaining layout-modifying elements before its invocation.

**Examples:**

Example 1 (unknown):
```unknown
container
    .Width(200)
    .Height(100)
    .Placeholder("Sample text");
```

Example 2 (scala):
```scala
Document
    .Create(document =>
    {
        document.Page(page =>
        {
            page.Size(PageSizes.A5);
            page.DefaultTextStyle(x => x.FontSize(20));
            page.Margin(25);

            page.Header()
                .Height(100)
                .Placeholder("Header");
            
            page.Content()
                .PaddingVertical(25)
                .Placeholder();
            
            page.Footer()
                .Height(100)
                .Placeholder("Footer");
        });
    })
    .GeneratePdf("placeholder.pdf");
```

---



---

## # Questpdf - Image


**Pages:** 3

---



---

## Dynamic Images


**URL:** https://www.questpdf.com/api-reference/image/dynamic.html

**Contents:**
- Dynamic Images ​

QuestPDF provides flexible layouts, which means the optimal image resolution cannot always be determined in advance. To ensure the best clarity, especially when generating maps or charts, it's important to produce images at a specific resolution (or a multiple of that resolution for retina displays).

This dynamic image element behaves similarly to static images. However, instead of accepting a preloaded image, it expects a function that receives the available space and returns the image as a binary array.

**Examples:**

Example 1 (swift):
```swift
container
    .Column(column =>
    {
        column.Spacing(10);

        column.Item().Text(text =>
        {
            text.Span("The national flag of Poland").Bold();
            text.Span(" consists of two horizontal stripes of equal width, the upper one white and the lower one red.");
        });
        
        column.Item()
            .AspectRatio(80 / 50f)
            .Border(2)
            .Image(GenerateNationalFlagOfPoland);
    });

// using SkiaSharp for custom image generation
byte[]? GenerateNationalFlagOfPoland(GenerateDynamicImageDelegatePayload context)
{
    using var whitePaint = new SKPaint
    {
        Color = SKColors.White,
    };
                
    using var redPaint = new SKPaint
    {
        Color = SKColor.Parse("#BB0A30"),
    };

    using var bitmap = new SKBitmap(context.ImageSize.Width, context.ImageSize.Height);
    using var canvas = new SKCanvas(bitmap);
                
    canvas.DrawRect(0, 0, context.ImageSize.Width, context.ImageSize.Height / 2, whitePaint);
    canvas.DrawRect(0, context.ImageSize.Height / 2, context.ImageSize.Width, context.ImageSize.Height, redPaint);
    canvas.Flush();

    using var content = bitmap.Encode(SKEncodedImageFormat.Png, 100);
    return content.ToArray();
}
```

---



---

## Image


**URL:** https://www.questpdf.com/api-reference/image/basics.html

**Contents:**
- Image ​
- Usage ​
    - Example ​
- Image scaling ​
    - Fitting options ​
    - Example ​
- Limiting image size ​

Use this element to embed images into your document. By default, it preserves the image's original aspect ratio, ensuring that your visuals remain undistorted.

Supported image format: JPEG, PNG, BMP, WEBP.

There are several ways to add an image to your document:

Please note that there is a significant difference between image resolution (number of pixels vertically and horizontally) and its physical size described in points. Therefore, the resolution of an image is not used for determining its physical size on the document.

When working with the Image element, controlling how it adjusts to available space is crucial for achieving the desired layout. By default, the image scales to fill the full width of its container while maintaining its aspect ratio.

Please be careful. This component may try to enforce size constraints that are impossible to meet. For example, the container may require more space than is available, or may try to squeeze its child into less space than possible.

Such scenarios result in a layout exception.

The PDF standard uses points to describe size, where there are 72 points in 1 inch. Image uses pixels to describe content. However, pixel does not have any meaningful size. Only when you specify DPI (dots per inch), is it possible to determine a pixel's size. The QuestPDF library always scales an image, because determining physical image size based on its resolution does not make sense.

To force an image to take a specified area, you can use any of the constraining elements. The simplest ones are Width and Height, e.g.:

Please note that because the Image element uses a proper scaling setting by default, you do not need to use both Width and Height (the image aspect ratio is preserved).

**Examples:**

Example 1 (julia):
```julia
// 1) a binary array
byte[] imageData = File.ReadAllBytes("path/to/logo.png")
container.Image(imageData)

// 2) a fileName
container.Image("path/myFile.png")

// 3) a stream
using var stream = new FileStream("logo.png", FileMode.Open);
container.Image(stream);
```

Example 2 (swift):
```swift
.Grid(grid =>
{
    grid.Columns(2);
    grid.Spacing(10);
    
    grid.Item(2).Text("My photo gallery:").Bold();
    
    grid.Item().Image("photo-gallery-1.jpg");
    grid.Item().Image("photo-gallery-2.jpg");
    grid.Item().Image("photo-gallery-3.jpg");
    grid.Item().Image("photo-gallery-4.jpg");
});
```

Example 3 (swift):
```swift
.Column(column =>
{
    column.Item().PaddingBottom(5).Text("FitWidth").Bold();
    column.Item()
        .Width(200)
        .Height(150)
        .Border(4)
        .BorderColor(Colors.Red.Medium)
        .Image("photo.jpg")
        .FitWidth();

    column.Item().Height(15);

    column.Item().PaddingBottom(5).Text("FitHeight").Bold();
    column.Item()
        .Width(200)
        .Height(100)
        .Border(4)
        .BorderColor(Colors.Red.Medium)
        .Image("photo.jpg")
        .FitHeight();
    
    column.Item().Height(15);

    column.Item().PaddingBottom(5).Text("FitArea 1").Bold();
    column.Item()
        .Width(200)
        .Height(100)
        .Border(4)
        .BorderColor(Colors.Red.Medium)
        .Image("photo.jpg")
        .FitArea();
    
    column.Item().Height(15);
    
    column.Item().PaddingBottom(5).Text("FitArea 2").Bold();
    column.Item()
        .Width(200)
        .Height(150)
        .Border(4)
        .BorderColor(Colors.Red.Medium)
        .Image("photo.jpg")
        .FitArea();
    
    column.Item().Height(15);

    column.Item().PaddingBottom(5).Text("FitUnproportionally").Bold();
    column.Item()
        .Width(200)
        .Height(50)
        .Border(4)
        .BorderColor(Colors.Red.Medium)
        .Image("photo.jpg")
        .FitUnproportionally();
});
```

Example 4 (swift):
```swift
container
    .Width(1, Unit.Inch)
    .Image(ImageElement.Image)
```

---



---

## Shared Images


**URL:** https://www.questpdf.com/api-reference/image/shared.html

**Contents:**
- Shared Images ​
    - Example: inefficient image processing ​
    - Solution: shared image resources ​

When generating a PDF with multiple items that use the same image, processing the image repeatedly can negatively affect both performance and the final file size.

Consider the following scenario: you want to create a list of items, each displaying the same image. In the naive approach, for each item, the following steps occur:

Because these steps are repeated for every list item, the overall process becomes inefficient, and the PDF may end up including multiple copies of the same image.

Starting with the 2025.4.0 version, the library automatically detects when static assets (images loaded via local file paths) are used, and enhances performance by caching them to avoid redundant processing.

Below is an example where the image is loaded and processed for each item:

To avoid redundant processing, load the image once and reuse it across all items. This approach improves performance and reduces the final PDF file size:

**Examples:**

Example 1 (swift):
```swift
.Column(column =>
{
    column.Spacing(15);

    foreach (var i in Enumerable.Range(0, 5))
    {
        column.Item().Row(row =>
        {
            row.AutoItem().Width(24).Image("checkbox.png");
            row.RelativeItem().PaddingLeft(8).AlignMiddle().Text(Placeholders.Label()).FontSize(16);
        });
    }
});
```

Example 2 (swift):
```swift
.Column(column =>
{
    column.Spacing(15);

    var image = Image.FromFile("checkbox.png");
    
    foreach (var i in Enumerable.Range(0, 5))
    {
        column.Item().Row(row =>
        {
            row.AutoItem().Width(24).Image(image);
            row.RelativeItem().PaddingLeft(8).AlignMiddle().Text(Placeholders.Label()).FontSize(16);
        });
    }
});
```

---


---

