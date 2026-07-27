# QuestPDF - Svg

## SVG Support


**URL:** https://www.questpdf.com/api-reference/image/svg.html

**Contents:**
- SVG Support ​
- Basic Usage ​
- Example ​
- Preloading ​
- Font Support ​
- Limitations ​

QuestPDF supports SVG images, allowing you to integrate scalable vector graphics just as you would with raster images. You can either load and parse an SVG image on demand or preload it to improve performance when the same image is used multiple times.

There are two ways to add an SVG image to your document:

SVG content supports the same scaling options as raster images. Learn more

For better performance, especially when reusing the same image, you can preload the SVG image. This ensures that the image is loaded and parsed only once:

If your SVG image contains text, please ensure that the font is available in your application:

The SVG module displays SVGs as images with high capabilities and compliance. Most SVG files are expected to render correctly, particularly those from popular design tools. However, there are some limitations to be aware of. If an SVG file does not render as expected after considering the following points, please file an issue.

Learn more on the Shopify page.

**Examples:**

Example 1 (gdscript):
```gdscript
// 1) with a text containing SVG content
var svgContent = File.ReadAllText("pdf-icon.svg");
container.Svg(svgContent);

// 2) with a file path
container.Svg("pdf-icon.svg")
```

Example 2 (unknown):
```unknown
container
  .Width(200)
  .Svg("pdf-icon.svg")
  .FitArea();
```

Example 3 (swift):
```swift
container.Column(column =>
{
    column.Item().Text("The classic PDF icon looks like this:").Bold();
    column.Item().Height(15);
    column.Item().Svg(svgContent);
});
```

Example 4 (swift):
```swift
// in global or static context
var image = SvgImage.FromFile("pdf-icon.svg");

Document
    .Create(document =>
    {
        document.Page(page =>
        {
            page.Size(PageSizes.A7.Landscape());
            page.Margin(25);
            
            page.Content()
                .Padding(25)
                .Svg(image))
                .FitArea();
        });
    })
    .GeneratePdfAndShow();
```

---



---

