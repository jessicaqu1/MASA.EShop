namespace MASA.EShop.Api.Open.Callers.Catalog;

public class CatalogCaller : ServiceCaller
{
    private readonly ILogger<CatalogCaller> _logger;

    private readonly string getCatalogItemsUrl;
    private readonly string getAllBrandsUrl;
    private readonly string getAllTypesUrl;
    private string party = "/api/v1/catalog/";

    public CatalogCaller(
        IServiceProvider serviceProvider,
        IOptions<Settings> settings,
        ILogger<CatalogCaller> logger)
        : base(serviceProvider)
    {
        _logger = logger;

        BaseAddress = settings.Value.CatalogUrl;

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

