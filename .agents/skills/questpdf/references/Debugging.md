# QuestPDF - Debugging

## Debug area


**URL:** https://www.questpdf.com/api-reference/debug-area.html

**Contents:**
- Debug area ​
- API ​
- Example ​

The DebugArea element helps you visually debug document layouts by drawing a labeled box around its content. This aids in understanding spacing, alignment and pinpointing specific sections of the document during development.

For enhanced development and debugging experience, please consider using the QuestPDF Companion App.

You can specify text and color to better distinguish between various debug elements:

It is also possible to skip the color (it is red by default), and even the label:

Learn more about supported color formats and predefined color palettes in the Colors section.

**Examples:**

Example 1 (unknown):
```unknown
container
    .Debug("Grid example", Colors.Blue.Medium)
    // content
```

Example 2 (unknown):
```unknown
.Debug("Grid example")
.Debug()
```

Example 3 (swift):
```swift
container
    .Width(250)
    .Height(250)
    .Padding(25)
    .DebugArea("Grid example", Colors.Blue.Medium)
    .Grid(grid =>
    {
        grid.Columns(3);
        grid.Spacing(5);

        foreach (var _ in Enumerable.Range(0, 8))
            grid.Item().Height(50).Placeholder();
    });
```

---



---

## Debug pointer


**URL:** https://www.questpdf.com/api-reference/debug-pointer.html

**Contents:**
- Debug pointer ​
- Example ​

Inserts a virtual debug element visible in the document hierarchy tree in the QuestPDF Companion App, as well as in the enhanced debugging message provided by the DocumentLayoutException. It does not appear in the final PDF output.

For enhanced development and debugging experience, please consider using the QuestPDF Companion App.

Learn more about the DocumentLayoutException.

The code above throws an exception with the following element trace:

**Examples:**

Example 1 (swift):
```swift
container
    .Width(100)
    .DebugPointer("Product details section")
    .Width(150)
    .Column(column =>
    {
        column.Item().Text("Coffee Beans");
        column.Item().Text("$19.99");
    });
```

Example 2 (perl):
```perl
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

-> Product details section



To learn more, please analyse the document measurement of the problematic location: 

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


   🟡 Column
   ==========
   Available Space: (Width: 0,000, Height: 0,000)
   Space Plan: PartialRender (Width: 0,000, Height: 0,000)
   ----------


      🟡 TextBlock
      =============
      Available Space: (Width: 0,000, Height: 0,000)
      Space Plan: PartialRender (Width: 0,000, Height: 0,000)
      -------------
      Alignment: Start
      Content Direction: LeftToRight
      Line Clamp: -
      Line Clamp Ellipsis: -
      Paragraph Spacing: 0
      Paragraph First Line Indentation: 0
      Text: Coffee Beans


      ⚪️ TextBlock
      =============
      Alignment: Start
      Content Direction: LeftToRight
      Line Clamp: -
      Line Clamp Ellipsis: -
      Paragraph Spacing: 0
      Paragraph First Line Indentation: 0
      Text: $19.99


Legend: 
🚨 - Element that is likely the root cause of the layout issue based on library heuristics and prediction. 
🔴 - Element that cannot be drawn due to the provided layout constraints. This element likely causes the layout issue, or one of its descendant children is responsible for the problem. 
🟡 - Element that can be partially drawn on the page and will also be rendered on the consecutive page. In more complex layouts, this element may also cause issues or contain a child that is the actual root cause.
🟢 - Element that is successfully and completely drawn on the page.
⚪️ - Element that has not been drawn on the faulty page. Its children are omitted.
```

---



---

## Document previewer


**URL:** https://www.questpdf.com/document-previewer.html

**Contents:**
- Document previewer ​
- Introduction ​
- Installation ​
  - Changes in your code ​
- How to use hot-reload ​
  - Visual Studio ​
  - JetBrains Rider ​
  - Terminal ​

The QuestPDF Previewer is a tool designed to simplify and speed up your development lifecycle. First, it shows a preview of your document. But the real magic starts with the hot-reload capability! It observes your code and updates the preview every time you change the implementation. Get real-time results without the need of code recompilation. Save time and enjoy the task!

The hot-reload feature is available only in the .NET 6 environment and beyond.

The Previewer tool is available as a NuGet tool. Therefore, it is installed on your local development environment and does not change your project.

📁 To install the QuestPDF Previewer, please execute the following command on your PC:

🚀 Optional: you can start an independent previewer application:

🔁 To update the tool, please use:

To preview your document, you need to slightly modify your code.

Start your application in the DEBUG mode with the 'Hot Reload on Save' flag enabled. On every file save, the document will be refreshed.

Start your application without debugger attached. To apply code changes, click on the "Apply changes" button displayed on the top bar, or use the Alt+F10 shortcut.

Start your application using the following command:

**Examples:**

Example 1 (unknown):
```unknown
dotnet tool install QuestPDF.Previewer --global
```

Example 2 (unknown):
```unknown
questpdf-previewer

// specify HTTP port on which previewer will communicate (default is 12500)
questpdf-previewer 12345
```

Example 3 (sql):
```sql
dotnet tool update questpdf.previewer --global
```

Example 4 (unknown):
```unknown
dotnet tool uninstall questpdf.previewer --global
```

---



---

