namespace NextWeb.DocumentPlatform.Renderers.Design;

/// <summary>Per-document accent palette (the only colors that vary between PDF types).</summary>
public sealed record PdfTheme(string Accent, string AccentSurface, string Id)
{
    public static PdfTheme Create(string accent, string accentSurface, string id = "custom") =>
        new(accent, accentSurface, id);
}
