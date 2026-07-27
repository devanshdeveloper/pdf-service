# QuestPDF - CompanionApp

## # Questpdf - Companion


**Pages:** 4

---



---

## Companion App: Downloads


**URL:** https://www.questpdf.com/companion/download.html

**Contents:**
- Companion App: Downloads ​

The QuestPDF Companion application is available for download on Windows, MacOS, and Linux.

The application version is not tightly coupled with the library version. The application is backward compatible with older library versions, but it may not support all features of the latest library version.

---



---

## Companion App


**URL:** https://www.questpdf.com/companion/usage.html

**Contents:**
- Companion App ​
- Introduction ​
- Installation ​
- Changes in your code ​
- How to use hot-reload ​
  - Visual Studio ​
  - JetBrains Rider ​
  - Terminal ​

The QuestPDF Companion application is a tool designed to simplify and speed up your development lifecycle. First, it shows a preview of your document. But the real magic starts with the hot-reload capability! It observes your code and updates the preview every time you change the implementation. Get real-time results without the need of code recompilation. Save time and enjoy the task!

Read more about features availble in the Companion App in the Features section.

The Companion App is available for download on Windows, MacOS, and Linux.

To access older versions of the Companion App, visit the Download section.

To preview your document, you need to slightly modify your code.

The QuestPDF Companion integration requires the library version 2024.10 or newer.

If you cannot update, please use the legacy QuestPDF Previewer application.

Start your application in the DEBUG mode with the 'Hot Reload on Save' flag enabled. On every file save, the document will be refreshed.

Start your application without debugger attached. To apply code changes, click on the Apply changes button displayed on the top bar, or use the Alt+F10 shortcut.

Start your application using the following command:

**Examples:**

Example 1 (elixir):
```elixir
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Companion;

// code in your main method
var document = Document.Create(container =>
{
    container.Page(page =>
    {
        // page content
    });
});

// instead of the standard way of generating a PDF file
document.GeneratePdf("hello.pdf");

// use the following invocation
document.ShowInCompanion();

// optionally, you can specify an HTTP port to communicate with the previewer host (default is 12500)
document.ShowInCompanion(12345);
```

Example 2 (unknown):
```unknown
dotnet watch
dotnet watch --project YourSampleProject
```

Example 3 (unknown):
```unknown
dotnet watch --project YourProjectWithTests test --filter "YourClassWithTests.TestMethodName"
```

---



---

## Companion App: Warning Messages


**URL:** https://www.questpdf.com/companion/warnings.html

**Contents:**
- Companion App: Warning Messages ​
- Complex document ​
- Hot-reload ​

Reason: This warning message is displayed when the document contains complex content. The hot-reload performance may be impacted.

Solution: Please consider using simpler and shorter content while working on the document's design.

Reason: This warning message is displayed when the content preview is refreshed using hot-reload. Modern dotnet hot-reload feature has certain limitations that may impact accuracy of a stack trace collection. As a result, any feature related to code navigation may not be as precise as expected.

Solution: If code navigation is crucial, please consider using the dotnet watch command instead of hot-reload: dotnet watch --no-hot-reload.

---



---

## Companion: Features


**URL:** https://www.questpdf.com/companion/features.html

**Contents:**
- Companion: Features ​
- Document hierarchy ​
- Document preview ​
- Magnifier ​
- Coordinate picker ​
- Size measurement ​
- Element selection ​
- Content searching ​
- Go to implementation ​
- Document links ​

The companion app provides a preview of the document. The preview is interactive and allows you to navigate the document, select elements, and measure distances.

Document hierarchy is a tree structure that represents the document content. The hierarchy is displayed in the left panel of the companion app. The hierarchy allows you to quickly navigate the document content and select elements.

The tree-structure uses a similar compact concept as C# Fluent API. Each node in the hierarchy represents an element in the document. Please note that certain API calls may produce more advanced hierarchy structures. The hierarchy is interactive, and you can expand and collapse nodes to navigate the document content.

The document preview section (on the right side of the screen) displays the document content. You can interact with the preview in many ways, such as moving the preview, zooming in and out, and measuring distances.

Use the magnifier feature (shortcut: key 1) to quickly see document's structure details without the need of zooming and adjusting the preview.

The coordinate picker feature (shortcut: key 2) allows you to pick the coordinates of the selected element. This feature is useful when you need to know the position of an element in the document.

This feature allows you to measure the size of visual elements in the document, as well as the distance between elements.

You can measure the size of the content vertically (shortcut: key 3) or horizontally (shortcut: key 4).

To select an element in the document, click on it in the document hierarchy section, or double-click on the content displayed in the document preview section. The selected element is highlighted in the preview.

Once the element is selected, you can review its details in the appropriate panel, such as configuration, position and size. If the element is visible on multiple pages, you can use arrows keys to navigate between all occurrences.

Quickly navigate the document content by searching for a specific phrase. To search for a phrase, press ctrl + F. The search bar appears at the top of the structure tree view. Enter the phrase you want to search for.

The selected search result is highlighted in both structure tree view and on the document's preview. You can navigate between the search results using the arrow keys (up and down).

The companion app allows you to quickly navigate to the implementation of the desired area in the code editor. To do this, hold the ctrl key and click on the desired area. The code editor will open with the implementation of the selected area.

The hot-reload feature may limit the accuracy of this feature. The first document load produces the most accurate results. Hot-reloaded documents provide less precise navigation.

The companion app allows you to open links in the document. To open a link, hold the alt key and click on the link.

There are two types of links in the document:

The companion app provides a detailed view of runtime exceptions that occur during document generation. The exception details are displayed in the error panel. You can review the exception message, stack trace, and the source code that caused the exception.

In case of layout issues, the companion app provides a set of tools to help you identify and resolve the problem.

If a document contains multiple layout issues, you can navigate between them using the arrow buttons (up and down).

Each element in the document structure view will be annotated with a color-coded dot with the following meaning.

When an element is selected, additional information about the layout issue is displayed in the element details panel. You can review the reason for the layout wrap or layout overflow. Use that hint and your knowledge about structure elements behavior to resolve the layout issue.

The companion app provides a set of customization options to adjust the appearance and behavior of the previewer, including:

---


---

