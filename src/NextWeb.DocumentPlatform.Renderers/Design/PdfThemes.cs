namespace NextWeb.DocumentPlatform.Renderers.Design;

/// <summary>Registered document themes — map renderers to these presets.</summary>
public static class PdfThemes
{
    public static readonly PdfTheme Voucher = new("#4338CA", "#EEF2FF", "voucher");
    public static readonly PdfTheme MaterialEntry = Voucher;
    public static readonly PdfTheme BillOfMaterial = new("#2563EB", "#DBEAFE", "bill-of-material");
    public static readonly PdfTheme StockTransferOut = new("#7C3AED", "#EDE9FE", "stock-transfer-out");
    public static readonly PdfTheme StockTransferIn = new("#059669", "#D1FAE5", "stock-transfer-in");
    public static readonly PdfTheme WorkOrder = new("#2563EB", "#DBEAFE", "work-order");
    public static readonly PdfTheme Reports = new("#0F766E", "#CCFBF1", "reports");
    public static readonly PdfTheme CostTemplate = new("#0D9488", "#CCFBF1", "cost-template");
    public static readonly PdfTheme WorkCenter = new("#6366F1", "#E0E7FF", "work-center");
}
