using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using NextWeb.DocumentPlatform.Application;

namespace NextWeb.DocumentPlatform.IntegrationTests;

public class RenderControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public RenderControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    private async Task GeneratePdf(string configName, string endpoint)
    {
        var customFactory = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddTransient<IErpForwardingService>(sp => new ConfigurableMockErpForwardingService(configName));
            });
        });

        var client = customFactory.CreateClient();
        var response = await client.GetAsync(endpoint);

        if (!response.IsSuccessStatusCode)
        {
            var err = await response.Content.ReadAsStringAsync();
            throw new System.Exception($"API Error: {response.StatusCode} - {err}");
        }

        response.EnsureSuccessStatusCode(); 
        response.Content.Headers.ContentType?.MediaType.Should().Be("application/pdf");

        var pdfBytes = await response.Content.ReadAsByteArrayAsync();
        pdfBytes.Should().NotBeEmpty();
        
        var outputDir = @"c:\code\next-web-works\pdf-service\test-output";
        if (!Directory.Exists(outputDir))
            Directory.CreateDirectory(outputDir);
            
        var path = Path.Combine(outputDir, $"voucher-sale-gst-standard-{configName}.pdf");
        await File.WriteAllBytesAsync(path, pdfBytes);
    }

    [Fact]
    public async Task Generate_IntraState_CGST_SGST()
    {
        await GeneratePdf("intra-state", "/render/voucher/test-doc-id");
    }

    [Fact]
    public async Task Generate_InterState_IGST()
    {
        await GeneratePdf("inter-state", "/render/voucher/test-doc-id");
    }

    [Fact]
    public async Task Generate_UnionTerritory_CGST_UTGST()
    {
        await GeneratePdf("utgst", "/render/voucher/test-doc-id");
    }

    [Fact]
    public async Task Generate_DistinctConsignee_WithCosts()
    {
        await GeneratePdf("consignee-costs", "/render/voucher/test-doc-id");
    }

    [Fact]
    public async Task Generate_MaterialEntry_Standard()
    {
        var customFactory = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddTransient<IErpForwardingService, MaterialEntryMockErpForwardingService>();
            });
        });

        var client = customFactory.CreateClient();
        var response = await client.GetAsync("/render/material-entry/test-doc-id");

        if (!response.IsSuccessStatusCode)
        {
            var err = await response.Content.ReadAsStringAsync();
            throw new System.Exception($"API Error: {response.StatusCode} - {err}");
        }

        response.EnsureSuccessStatusCode();
        response.Content.Headers.ContentType?.MediaType.Should().Be("application/pdf");

        var pdfBytes = await response.Content.ReadAsByteArrayAsync();
        pdfBytes.Should().NotBeEmpty();

        var outputDir = @"c:\code\next-web-works\pdf-service\test-output";
        if (!Directory.Exists(outputDir))
            Directory.CreateDirectory(outputDir);

        var path = Path.Combine(outputDir, "material-entry-standard.pdf");
        await File.WriteAllBytesAsync(path, pdfBytes);
    }

    [Fact]
    public async Task Generate_StockTransferIn_Standard()
    {
        await GenerateFromMockFile(
            new StockTransferInMockErpForwardingService(),
            "/render/stock-transfer-in/test-doc-id",
            "stock-transfer-in-standard.pdf");
    }

    [Fact]
    public async Task Generate_StockTransferOut_Standard()
    {
        await GenerateFromMockFile(
            new StockTransferOutMockErpForwardingService(),
            "/render/stock-transfer-out/test-doc-id",
            "stock-transfer-out-standard.pdf");
    }

    [Fact]
    public async Task Generate_BillOfMaterial_Standard()
    {
        await GenerateFromMockFile(
            new BillOfMaterialMockErpForwardingService(),
            "/render/bill-of-material/test-doc-id",
            "bill-of-material-standard.pdf");
    }

    private async Task GenerateFromMockFile(IErpForwardingService mockService, string endpoint, string outputFileName)
    {
        var customFactory = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddTransient<IErpForwardingService>(_ => mockService);
            });
        });

        var client = customFactory.CreateClient();
        var response = await client.GetAsync(endpoint);

        if (!response.IsSuccessStatusCode)
        {
            var err = await response.Content.ReadAsStringAsync();
            throw new System.Exception($"API Error: {response.StatusCode} - {err}");
        }

        response.EnsureSuccessStatusCode();
        response.Content.Headers.ContentType?.MediaType.Should().Be("application/pdf");

        var pdfBytes = await response.Content.ReadAsByteArrayAsync();
        pdfBytes.Should().NotBeEmpty();

        var outputDir = @"c:\code\next-web-works\pdf-service\test-output";
        if (!Directory.Exists(outputDir))
            Directory.CreateDirectory(outputDir);

        var path = Path.Combine(outputDir, outputFileName);
        await File.WriteAllBytesAsync(path, pdfBytes);
    }
}

public class ConfigurableMockErpForwardingService : IErpForwardingService
{
    private readonly string _configType;

    public ConfigurableMockErpForwardingService(string configType)
    {
        _configType = configType;
    }

    public async Task<string> GetDocumentDataAsync(HttpRequestContext requestContext, CancellationToken cancellationToken)
    {
        var path = @"c:\code\next-web-works\pdf-service\context\QUEST PDF.json";
        var json = await File.ReadAllTextAsync(path, cancellationToken);
        var node = JsonNode.Parse(json);
        var data = node?["data"]?.AsObject();
        var document = data?["document"]?.AsObject();

        if (document != null && data != null)
        {
            document["amountInWords"] = "Ten Thousand Three Hundred Five Rupees Only";
            document["totalTaxAmountInWords"] = "One Thousand Eight Hundred Fifty Four Rupees Only";
            document["taxType"] = _configType switch
            {
                "intra-state" => "CGST_SGST",
                "inter-state" => "IGST",
                "utgst" => "CGST_UTGST",
                _ => "CGST_SGST"
            };

            var products = document["products"]?.AsArray();
            if (products != null && products.Count > 0)
            {
                var prod1 = products[0]?.AsObject();
                if (prod1 != null)
                {
                    prod1["hsn"] = "9403";
                    var productInfo = prod1["product"]?.AsObject();
                    if (productInfo != null)
                        productInfo["hsn"] = "9403";
                }
            }

            var computedTaxes = new JsonArray();
            if (_configType == "inter-state")
            {
                computedTaxes.Add(new JsonObject
                {
                    ["name"] = "IGST",
                    ["type"] = "percentage",
                    ["value"] = 18,
                    ["amount"] = 1854
                });
            }
            else if (_configType == "utgst")
            {
                computedTaxes.Add(new JsonObject
                {
                    ["name"] = "CGST",
                    ["type"] = "percentage",
                    ["value"] = 9,
                    ["amount"] = 927
                });
                computedTaxes.Add(new JsonObject
                {
                    ["name"] = "UTGST",
                    ["type"] = "percentage",
                    ["value"] = 9,
                    ["amount"] = 927
                });
            }
            else
            {
                computedTaxes.Add(new JsonObject
                {
                    ["name"] = "CGST",
                    ["type"] = "percentage",
                    ["value"] = 9,
                    ["amount"] = 927
                });
                computedTaxes.Add(new JsonObject
                {
                    ["name"] = "SGST",
                    ["type"] = "percentage",
                    ["value"] = 9,
                    ["amount"] = 927
                });
            }

            document["summary"] = new JsonArray
            {
                new JsonObject
                {
                    ["hsn"] = "9403",
                    ["taxableValue"] = 10305,
                    ["totalTaxAmount"] = 1854,
                    ["computedTaxes"] = computedTaxes
                }
            };

            document["totalTaxableAmount"] = 10305;
            document["totalTaxAmount"] = 1854;
            document["totalAmount"] = 12159;

            if (_configType == "consignee-costs")
            {
                document["consignee"] = new JsonObject
                {
                    ["businessName"] = "Reliance Retail",
                    ["gst"] = "27AAACR1234E1Z5",
                    ["shippingAddress"] = new JsonObject
                    {
                        ["streetAddress"] = "Maker Chambers IV",
                        ["city"] = "Mumbai",
                        ["state"] = "Maharashtra",
                        ["stateCode"] = "27",
                        ["postalCode"] = "400021"
                    }
                };

                document["cost"] = new JsonArray
                {
                    new JsonObject { ["name"] = "Shipping Charges", ["value"] = 500 },
                    new JsonObject { ["name"] = "Installation", ["value"] = 1000 }
                };
                document["totalAmount"] = 13659;
            }
        }

        return node?.ToJsonString() ?? "";
    }
}

public class MaterialEntryMockErpForwardingService : IErpForwardingService
{
    public async Task<string> GetDocumentDataAsync(HttpRequestContext requestContext, CancellationToken cancellationToken)
    {
        var path = @"c:\code\next-web-works\pdf-service\context\MaterialEntry.json";
        return await File.ReadAllTextAsync(path, cancellationToken);
    }
}

public class StockTransferInMockErpForwardingService : IErpForwardingService
{
    public async Task<string> GetDocumentDataAsync(HttpRequestContext requestContext, CancellationToken cancellationToken)
    {
        var path = @"c:\code\next-web-works\pdf-service\context\StockTransferIn.json";
        return await File.ReadAllTextAsync(path, cancellationToken);
    }
}

public class StockTransferOutMockErpForwardingService : IErpForwardingService
{
    public async Task<string> GetDocumentDataAsync(HttpRequestContext requestContext, CancellationToken cancellationToken)
    {
        var path = @"c:\code\next-web-works\pdf-service\context\StockTransferOut.json";
        return await File.ReadAllTextAsync(path, cancellationToken);
    }
}

public class BillOfMaterialMockErpForwardingService : IErpForwardingService
{
    public async Task<string> GetDocumentDataAsync(HttpRequestContext requestContext, CancellationToken cancellationToken)
    {
        var path = @"c:\code\next-web-works\pdf-service\context\BillOfMaterial.json";
        return await File.ReadAllTextAsync(path, cancellationToken);
    }
}
