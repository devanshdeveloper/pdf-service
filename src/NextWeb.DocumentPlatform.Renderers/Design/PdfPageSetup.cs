using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace NextWeb.DocumentPlatform.Renderers.Design;

public static class PdfPageSetup
{
    public static void ConfigureA4(PageDescriptor page, PdfSemantics semantics)
    {
        page.Size(PageSizes.A4);
        page.Margin(PdfPrimitives.PageMarginCm, Unit.Centimetre);
        page.PageColor(semantics.SurfacePage);
        page.DefaultTextStyle(x =>
            x.FontSize(PdfPrimitives.FontSizeBody)
                .FontFamily(semantics.FontFamilyBody)
                .FontColor(semantics.TextPrimary));
    }
}
