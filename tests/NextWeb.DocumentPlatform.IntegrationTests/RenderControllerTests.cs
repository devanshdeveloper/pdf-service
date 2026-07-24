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
        await GeneratePdf("intra-state", "/api/v1/render/voucher/sale/gst-standard");
    }

    [Fact]
    public async Task Generate_InterState_IGST()
    {
        await GeneratePdf("inter-state", "/api/v1/render/voucher/sale/gst-standard");
    }

    [Fact]
    public async Task Generate_UnionTerritory_CGST_UTGST()
    {
        await GeneratePdf("utgst", "/api/v1/render/voucher/sale/gst-standard");
    }

    [Fact]
    public async Task Generate_DistinctConsignee_WithCosts()
    {
        await GeneratePdf("consignee-costs", "/api/v1/render/voucher/sale/gst-standard");
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
        var doc = JsonDocument.Parse(json);
        var responseNode = doc.RootElement.GetProperty("paths").GetProperty("/api/voucher/6a61be54cfc2809fe2131bfa/print").GetProperty("get").GetProperty("responses").GetProperty("200");
        
        var node = JsonNode.Parse(responseNode.GetRawText());
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

            // HSN injection
            var products = document["products"]?.AsArray();
            if (products != null && products.Count > 0)
            {
                var prod1 = products[0]?.AsObject();
                if (prod1 != null)
                {
                    var productInfo = prod1["product"]?.AsObject();
                    if (productInfo != null)
                    {
                        productInfo["hsn"] = "9403";
                    }
                }
            }

            var taxSummaryArray = new JsonArray();
            var taxObj = new JsonObject
            {
                ["hsn"] = "9403",
                ["taxableAmount"] = 10305,
                ["totalAmount"] = 1854
            };

            if (_configType == "inter-state")
            {
                taxObj["igstRate"] = 18;
                taxObj["igstAmount"] = 1854;
            }
            else if (_configType == "utgst")
            {
                taxObj["cgstRate"] = 9;
                taxObj["cgstAmount"] = 927;
                taxObj["utgstRate"] = 9;
                taxObj["utgstAmount"] = 927;
            }
            else // intra-state
            {
                taxObj["cgstRate"] = 9;
                taxObj["cgstAmount"] = 927;
                taxObj["sgstRate"] = 9;
                taxObj["sgstAmount"] = 927;
            }
            taxSummaryArray.Add(taxObj);

            // Add taxSummary at the root of data (as per API shape)
            data["taxSummary"] = taxSummaryArray;

            if (_configType == "consignee-costs")
            {
                var consignee = new JsonObject
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
                document["consignee"] = consignee;

                var costs = new JsonArray
                {
                    new JsonObject { ["name"] = "Shipping Charges", ["value"] = 500 },
                    new JsonObject { ["name"] = "Installation", ["value"] = 1000 }
                };
                document["cost"] = costs;
                document["grandTotal"] = 13659; // 10305 + 1854 + 1500
            }
        }

        return node?.ToJsonString() ?? "";
    }
}
