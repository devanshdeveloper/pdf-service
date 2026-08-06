using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace NextWeb.DocumentPlatform.Renderers.Design;

/// <summary>Reusable visual blocks shared across PDF templates.</summary>
public static class PdfComponents
{
    public static void HorizontalDivider(IContainer container, PdfSemantics s) =>
        container.PaddingTop(PdfPrimitives.HeaderDividerGap)
            .LineHorizontal(PdfPrimitives.BorderHairline)
            .LineColor(s.BorderDefault);

    public static void SectionTitle(IContainer container, PdfSemantics s, string title) =>
        container.Text(title).FontSize(s.FontBody).SemiBold().FontColor(s.TextAccent);

    public static void SectionLabel(IContainer container, PdfSemantics s, string label) =>
        container.Text(label).FontSize(s.FontCaption).SemiBold().FontColor(s.TextMuted);

    public static void StatusChip(IContainer container, PdfSemantics s, string label, bool isPayment = false)
    {
        var (bg, fg) = ResolveStatusColors(s, label, isPayment);
        container
            .Background(bg)
            .PaddingVertical(PdfPrimitives.ChipPaddingVertical)
            .PaddingHorizontal(PdfPrimitives.ChipPaddingHorizontal)
            .Text(label)
            .FontSize(s.FontCaption)
            .SemiBold()
            .FontColor(fg);
    }

    public static (string Bg, string Fg) ResolveStatusColors(PdfSemantics s, string status, bool isPayment)
    {
        var normalized = status.Trim().ToLowerInvariant();

        if (isPayment)
        {
            if (normalized is "paid") return (PdfPrimitives.Green50, PdfPrimitives.Green800);
            if (normalized is "unpaid" or "overdue") return (PdfPrimitives.Amber50, PdfPrimitives.Amber800);
            if (normalized.Contains("partial")) return (PdfPrimitives.Orange50, PdfPrimitives.Orange800);
        }
        else
        {
            if (normalized is "approved") return (s.SurfaceAccent, s.TextAccent);
            if (normalized is "draft" or "pending") return (PdfPrimitives.Amber50, PdfPrimitives.Amber800);
            if (normalized is "rejected" or "cancelled") return (PdfPrimitives.Red50, PdfPrimitives.Red800);
        }

        return (s.SurfaceCard, s.TextMuted);
    }

    public static void SignatoryBlock(IContainer container, string businessName, PdfSemantics s)
    {
        container.Column(c =>
        {
            c.Item().Text($"For {businessName}").FontSize(s.FontCaption).SemiBold();
            c.Item().PaddingTop(PdfPrimitives.FooterSignatoryGap)
                .Text("Authorised Signatory")
                .FontSize(s.FontCaption)
                .SemiBold()
                .FontColor(s.TextMuted);
        });
    }
}
