using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace NextWeb.DocumentPlatform.Renderers.Design;

/// <summary>
/// Table cell conventions: only description/product name may wrap; all other values stay on one line.
/// </summary>
public static class PdfTable
{
    public static void Description(IContainer container, PdfSemantics s, string? text, bool semiBold = false)
    {
        if (string.IsNullOrWhiteSpace(text))
            return;

        if (semiBold)
            container.Text(text).FontSize(s.FontCaption).SemiBold();
        else
            container.Text(text).FontSize(s.FontCaption);
    }

    public static void SingleLine(
        IContainer container,
        PdfSemantics s,
        string? text,
        bool alignRight = false,
        bool semiBold = false)
    {
        var display = string.IsNullOrWhiteSpace(text) ? "—" : text;
        var target = alignRight ? container.AlignRight() : container;

        if (semiBold)
            target.Text(display).FontSize(s.FontCaption).SemiBold().ClampLines(1);
        else
            target.Text(display).FontSize(s.FontCaption).ClampLines(1);
    }

    /// <summary>
    /// Currency in a table cell — full value, single line, right-aligned, never ellipsized.
    /// Uses full cell width (ExtendHorizontal) so AlignRight does not shrink the text box.
    /// </summary>
    public static void Amount(
        IContainer container,
        PdfSemantics s,
        string? formattedAmount,
        bool semiBold = false,
        float? fontSize = null)
    {
        var display = string.IsNullOrWhiteSpace(formattedAmount) ? "—" : formattedAmount;
        var size = fontSize ?? s.FontCaption;

        container.ExtendHorizontal().Text(text =>
        {
            text.AlignRight();
            var span = text.Span(display).FontSize(size);
            if (semiBold)
                span.SemiBold();
        });
    }

    public static void RowSingleLine(
        IContainer cell,
        PdfSemantics s,
        bool shaded,
        string? text,
        bool alignRight = false,
        bool semiBold = false)
    {
        SingleLine(RowCell(cell, s, shaded), s, text, alignRight, semiBold);
    }

    public static void RowDescription(
        IContainer cell,
        PdfSemantics s,
        bool shaded,
        string? text,
        bool semiBold = false)
    {
        Description(RowCell(cell, s, shaded), s, text, semiBold);
    }

    public static void FooterText(
        IContainer cell,
        PdfSemantics s,
        string text,
        bool alignRight = false,
        bool semiBold = false,
        float? fontSize = null)
    {
        var styled = FooterCell(cell, s);
        var target = alignRight ? styled.AlignRight() : styled;
        var size = fontSize ?? s.FontCaption;

        if (semiBold)
            target.Text(text).FontSize(size).SemiBold().ClampLines(1);
        else
            target.Text(text).FontSize(size).ClampLines(1);
    }

    /// <summary>Full-width amount cell for table rows (rate, amount, footer totals).</summary>
    public static void TableAmountCell(
        IContainer container,
        PdfSemantics s,
        bool shaded,
        string? formattedAmount,
        bool semiBold = false)
    {
        Amount(NumericRowCell(container, s, shaded), s, formattedAmount, semiBold);
    }

    public static void TableAmountFooterCell(
        IContainer container,
        PdfSemantics s,
        string? formattedAmount,
        bool semiBold = false)
    {
        Amount(NumericFooterCell(container, s), s, formattedAmount, semiBold);
    }

  /// <summary>Shaded row cell with minimal horizontal padding for numeric columns.</summary>
    public static IContainer NumericRowCell(IContainer container, PdfSemantics s, bool shaded) =>
        RowCell(container, s, shaded)
            .PaddingHorizontal(PdfPrimitives.NumericCellPaddingHorizontal);

    public static IContainer RowCell(IContainer container, PdfSemantics s, bool shaded)
    {
        var cell = shaded ? container.Background(s.TableRowShade) : container;
        return cell
            .PaddingVertical(PdfPrimitives.CellPaddingVertical)
            .PaddingHorizontal(PdfPrimitives.CellPaddingHorizontal);
    }

    public static IContainer HeaderCellPrimary(IContainer container, PdfSemantics s) =>
        container
            .Background(s.TableHeaderPrimary)
            .PaddingVertical(PdfPrimitives.HeaderCellPaddingVertical)
            .PaddingHorizontal(PdfPrimitives.CellPaddingHorizontal);

    public static IContainer HeaderCellNeutral(IContainer container, PdfSemantics s) =>
        container
            .Background(s.TableHeaderNeutral)
            .PaddingVertical(PdfPrimitives.HeaderCellPaddingVertical)
            .PaddingHorizontal(PdfPrimitives.CellPaddingHorizontal);

    public static IContainer FooterCell(IContainer container, PdfSemantics s) =>
        container
            .BorderTop(PdfPrimitives.BorderHairline)
            .BorderColor(s.BorderDefault)
            .PaddingVertical(PdfPrimitives.CellPaddingVertical + 1)
            .PaddingHorizontal(PdfPrimitives.CellPaddingHorizontal);

    public static IContainer NumericFooterCell(IContainer container, PdfSemantics s) =>
        FooterCell(container, s).PaddingHorizontal(PdfPrimitives.NumericCellPaddingHorizontal);
}
