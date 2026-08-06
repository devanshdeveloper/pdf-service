namespace NextWeb.DocumentPlatform.Renderers.Design;

/// <summary>Semantic tokens — role-based names mapped from primitives + theme.</summary>
public sealed class PdfSemantics
{
    public PdfSemantics(PdfTheme theme) => Theme = theme;

    public PdfTheme Theme { get; }

    // Text
    public string TextPrimary => PdfPrimitives.Black;
    public string TextMuted => PdfPrimitives.Gray500;
    public string TextOnAccent => PdfPrimitives.White;
    public string TextAccent => Theme.Accent;

    // Surfaces
    public string SurfacePage => PdfPrimitives.White;
    public string SurfaceCard => PdfPrimitives.Gray50;
    public string SurfaceAccent => Theme.AccentSurface;

    // Borders
    public string BorderDefault => PdfPrimitives.Gray200;
    public string BorderAccent => Theme.Accent;

    // Table
    public string TableHeaderPrimary => Theme.Accent;
    public string TableHeaderNeutral => PdfPrimitives.Gray50;
    public string TableRowShade => PdfPrimitives.Gray50;

    // Typography shortcuts
    public float FontCaption => PdfPrimitives.FontSizeCaption;
    public float FontBody => PdfPrimitives.FontSizeBody;
    public float FontTitle => PdfPrimitives.FontSizeTitle;
    public float FontDisplaySm => PdfPrimitives.FontSizeDisplaySm;
    public float FontDisplayMd => PdfPrimitives.FontSizeDisplayMd;
    public float FontDisplayLg => PdfPrimitives.FontSizeDisplayLg;
    public float FontTotal => PdfPrimitives.FontSizeTotal;
    public string FontFamilyBody => PdfPrimitives.FontBody;
    public string FontFamilyDisplay => PdfPrimitives.FontDisplay;
}
