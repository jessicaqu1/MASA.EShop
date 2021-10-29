using Dapr.Client;
using MASA.EShop.Api.Caller;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace MASA.EShop.Web.Client.Services
{
    public class CatalogService : ServiceCaller
    {

        private readonly ILogger<CatalogService> _logger;

        private readonly string getCatalogItemsUrl;
        private readonly string getAllBrandsUrl;
        private readonly string getAllTypesUrl;
        private string party = "/api/v1/catalog/";

        public override string BaseAddress { get; set; }

        public CatalogService(
            HttpClient httpClient,
            DaprClient daprClient,
            IOptions<Settings> settings,
            ILogger<CatalogService> logger)
            : base(httpClient, daprClient)
        {
            _logger = logger;
            BaseAddress = settings.Value.CatalogUrl;
            _logger = logger;

            getCatalogItemsUrl = $"{party}items";
            getAllBrandsUrl = $"{party}brands";
            getAllTypesUrl = $"{party}types";
        }

        public async Task<CatalogData> GetCatalogItemsAsync(int pageIndex, int pageSize, int? brandId, int? typeId)
        {
            return await GetFromJsonAsync<CatalogData>(
            $"{getCatalogItemsUrl}?brandId={brandId}&typeId={typeId}&pageIndex={pageIndex}&pageSize={pageSize}") ?? new();
        }
        public async Task<IEnumerable<CatalogBrand>> GetBrandsAsync()
        {
            return await GetFromJsonAsync<IEnumerable<CatalogBrand>>(getAllBrandsUrl) ?? new List<CatalogBrand>();
        }

        public async Task<IEnumerable<CatalogType>> GetTypesAsync()
        {
            return await GetFromJsonAsync<IEnumerable<CatalogType>>(getAllTypesUrl) ?? new List<CatalogType>();
        }

        public async Task<CatalogItem> GetCatalogById(int Id)
        {
            return await GetFromJsonAsync<CatalogItem>($"{party}{Id}") ?? new();
        }
    }
}
