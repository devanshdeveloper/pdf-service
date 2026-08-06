namespace NextWeb.DocumentPlatform.Renderers.Design;

/// <summary>Raw design tokens — do not use directly in templates; prefer <see cref="PdfSemantics"/>.</summary>
public static class PdfPrimitives
{
    // Colors — neutrals
    public const string White = "#FFFFFF";
    public const string Black = "#000000";
    public const string Gray50 = "#F3F4F6";
    public const string Gray200 = "#E5E7EB";
    public const string Gray500 = "#6B7280";

    // Colors — status (print-safe pairs)
    public const string Green50 = "#DCFCE7";
    public const string Green800 = "#166534";
    public const string Amber50 = "#FEF3C7";
    public const string Amber800 = "#92400E";
    public const string Orange50 = "#FFEDD5";
    public const string Orange800 = "#9A3412";
    public const string Red50 = "#FEE2E2";
    public const string Red800 = "#991B1B";

    // Typography
    public const string FontBody = "Arial";
    public const string FontDisplay = "Montserrat";

    public const float FontSizeMicro = 7f;
    public const float FontSizeCaption = 8f;
    public const float FontSizeBody = 9f;
    public const float FontSizeSubheading = 10f;
    public const float FontSizeTitle = 11f;
    public const float FontSizeDisplaySm = 16f;
    public const float FontSizeDisplayMd = 18f;
    public const float FontSizeDisplayLg = 20f;
    public const float FontSizeTotal = 12f;

    public const float LineHeightBody = 1.3f;
    public const float LineHeightAddress = 1.35f;

    // Spacing (points)
    public const float SpaceXs = 2f;
    public const float SpaceSm = 4f;
    public const float SpaceMd = 8f;
    public const float SpaceLg = 12f;
    public const float SpaceXl = 16f;

    public const float PageMarginCm = 0.65f;
    public const float SectionGap = SpaceMd;
    public const float CardPadding = SpaceMd;
    public const float CellPaddingVertical = 3f;
    public const float CellPaddingHorizontal = 4f;
    public const float NumericCellPaddingHorizontal = 2f;
    public const float HeaderCellPaddingVertical = 4f;
    public const float FooterSignatoryGap = SpaceXl;
    public const float HeaderDividerGap = SpaceLg;
    public const float ChipPaddingVertical = 3f;
    public const float ChipPaddingHorizontal = 8f;
    public const float MetaColumnWidth = 200f;
    public const float TotalsBlockWidth = 260f;
    public const float TotalsValueWidth = 120f;

    // Line-item table — sized for INR ₹10,000,000.00 at 8pt on one line
    public const float TableColIndex = 16f;
    public const float TableColHsn = 34f;
    public const float TableColQty = 40f;
    public const float TableColUnit = 26f;
    public const float TableColRate = 78f;
    public const float TableColDisc = 28f;
    public const float TableColTaxPct = 28f;
    public const float TableColAmount = 86f;

    // Borders
    public const float BorderHairline = 1f;
}
