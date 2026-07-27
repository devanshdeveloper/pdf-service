# QuestPDF - Fonts

## Font management


**URL:** https://www.questpdf.com/api-reference/text/font-management.html

**Contents:**
- Font management ​
- Library default font ​
- System font registration ​
- Automatic local font registration ​
- Manual font registration ​
- Checking if all glyphs are available ​
- Removing the default Lato font ​

To ensure successful document generation, QuestPDF uses and includes the Lato font version 2.015 by default.

Lato is a sanserif typeface family designed in the Summer 2010 by Warsaw-based designer Łukasz Dziedzic (“Lato” means “Summer” in Polish).

It is available under the SIL Open Font License, Version 1.1.

You can download it from the Adobe Fonts website.

By default, QuestPDF loads all fonts available in the execution environment. This simplifies the development process, as your code can easily access all system fonts.

However, in most cloud deployments, few or no fonts are available, which may lead to unexpected results. To avoid this, you can disable environment font loading using the following setting:

During application startup, QuestPDF automatically loads all font files present in the deployment directory (as specified by the CopyToOutputDirectory property in the .csproj file). This allows you to include font files in your project without the need for manual registration.

If you prefer to manually specify directories for font discovery, use the following approach:

You can manually register custom fonts using the FontManager class. Please perform this operation only once, during application startup or initialization.

You can also register fonts under custom names to simplify usage within your documents:

If your document contains non-Latin characters or special symbols such as emojis, you may want to verify that all required glyphs are available in the selected font. If glyphs are missing in both the primary font and all registered fallback fonts, they will be replaced with placeholder characters.

To detect such issues, enable the following setting:

When enabled, the library will throw an exception if any glyphs are missing from the selected font.

QuestPDF includes the Lato font by default to ensure a seamless experience when generating PDFs. However, if you are using your own fonts and want to optimize your package size, you can safely remove Lato from the output.

To follow this approach, please add the following snippet to your .csproj file:

**Examples:**

Example 1 (unknown):
```unknown
// true by default
QuestPDF.Settings.UseEnvironmentFonts = false;
```

Example 2 (unknown):
```unknown
QuestPDF.Settings.FontDiscoveryPaths.Clear();

// adjust the path based on your project structure
QuestPDF.Settings.FontDiscoveryPaths.Add("resources/fonts");
```

Example 3 (julia):
```julia
using QuestPDF.Drawing;

// register font from a file
using var fontStream = File.OpenRead("NotoEmoji-Regular.ttf");
FontManager.RegisterFont(fontStream);

// register font from an embedded resource
// ensure the file is located in the YourApplication project under Resources/Fonts
FontManager.RegisterFontFromEmbeddedResource("YourApplication.Resources.Fonts.NotoEmoji-Regular.ttf");
```

Example 4 (elixir):
```elixir
// load the font at startup
using var fontStream = File.OpenRead("LibreBarcode39-Regular.ttf");
FontManager.RegisterFontWithCustomName("MyBarcodeFont", fontStream);

// use it during document generation
container
    .Text("*QuestPDF*")
    .FontFamily("MyBarcodeFont") // use your custom font name
    .FontSize(64);
```

---



---

